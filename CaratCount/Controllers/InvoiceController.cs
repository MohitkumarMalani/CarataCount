using System.Text.Json;
using CaratCount.Data;
using CaratCount.Entities;
using CaratCount.Interface;
using CaratCount.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace CaratCount.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInvoiceManager _invoiceManager;
        private readonly IClientManager _clientManager;
        private readonly IDiamondPacketManager _diamondPacketManager;
        private readonly ApplicationDbContext _context;

        public InvoiceController(
            UserManager<ApplicationUser> userManager,
            IInvoiceManager invoiceManager,
            IClientManager clientManager,
            IDiamondPacketManager diamondPacketManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _invoiceManager = invoiceManager;
            _clientManager = clientManager;
            _diamondPacketManager = diamondPacketManager;
            _context = context;
        }

        // GET: /invoice
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            List<Invoice>? invoices = _invoiceManager.GetInvoicesByUserId(user.Id);

            ViewBag.PageName = "Invoice";

            return View(invoices);
        }

        // GET: /invoice/generate-invoice/{id}
        [HttpGet("/invoice/generate-invoice/{id}")]
        [Authorize]
        public async Task<IActionResult> GenerateInvoice(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }


            Invoice? invoice = await _invoiceManager.GetInvoiceByIdAsync(id, user.Id);

            if (invoice == null)
            {
                return NotFound();
            }

            ApplicationUser? userInfo = await _context.Users.Include(u => u.GstInDetail)
                        .ThenInclude(g => g.Address)
                    .FirstOrDefaultAsync(u => u.Id == user.Id);

            var clientInfo = await _clientManager.GetClientByIdAsync(invoice.ClientId.ToString(), user.Id);

            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = new PdfWriter(memoryStream);
            PdfDocument pdf = new PdfDocument(writer);

            try
            {
                Document document = new Document(pdf);

                document.Add(new Paragraph("From:").SetBold());
                document.Add(new Paragraph(
                  $" {userInfo.GstInDetail.TradeName},\n " +
                    $"{userInfo.GstInDetail.Address.CombineAddressFields()}"));

                document.Add(new Paragraph("To:").SetMarginTop(10).SetBold());
                document.Add(new Paragraph(
                    $"{clientInfo.GstInDetail.TradeName},\n " +
                    $"{clientInfo.GstInDetail.Address.CombineAddressFields()}"));

                document.Add(new Paragraph($"Invoice ID: {invoice.Id}")
                    .SetTextAlignment(TextAlignment.RIGHT).SetMarginTop(10));
                document.Add(new Paragraph($"Description: {invoice.Description}")
                    .SetTextAlignment(TextAlignment.RIGHT));
                document.Add(new Paragraph($"Issue Date: {invoice.IssueDate?.ToString("MM/dd/yyyy")}")
                    .SetTextAlignment(TextAlignment.RIGHT));
                document.Add(new Paragraph($"Due Date: {invoice.DueDate?.ToString("MM/dd/yyyy")}")
                    .SetTextAlignment(TextAlignment.RIGHT));

                Table table = new Table(new float[] { 3, 1, 1, 1 }).SetMarginTop(10);
                table.SetWidth(UnitValue.CreatePercentValue(100));
                table.AddHeaderCell("Item Id");
                table.AddHeaderCell("Diamond Packet ID");
                table.AddHeaderCell("Cost");
                foreach (var item in invoice.InvoiceItems)
                {
                    table.AddCell(new Cell().Add(new Paragraph(item.Id.ToString())));
                    table.AddCell(new Cell().Add(new Paragraph(item.DiamondPacket.Id.ToString())));
                    table.AddCell(new Cell().Add(new Paragraph(item.ClientCharge.ToString("C"))));
                }
                //Add the Table to the PDF Document
                document.Add(table);


                document.Add(new Paragraph($"Total Cost: ${invoice.TotalAmount}")
                    .SetTextAlignment(TextAlignment.RIGHT));

                // Close the document
                document.Close();


                var clonedMemoryStream = new MemoryStream(memoryStream.ToArray());

                // Return the cloned memory stream
                return File(clonedMemoryStream, "application/pdf", "invoice.pdf");
            }
            finally
            {
                // Dispose of PdfDocument and PdfWriter
                pdf.Close();
                writer.Close();
            }
        }
        // GET: /invoice/get-diamond-packets
        [HttpGet("/invoice/get-diamond-packets")]
        [Authorize]
        public async Task<IActionResult> GetDiamondPackets([FromQuery] string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return BadRequest("Client ID is required.");
            }

            var diamondPackets = await _diamondPacketManager.GetDiamondPacketsByClientIdAsync(clientId);

            if (diamondPackets == null)
            {
                return NotFound("No diamond packets found for the specified client.");
            }

            string jsonString = JsonSerializer.Serialize(diamondPackets);

            return Ok(jsonString);
        }

        // GET: /invoice/add
        [HttpGet]
        [Authorize()]
        public async Task<IActionResult> Add()
        {

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            InvoiceViewModel? invoiceViewModel = new() { UserId = user.Id };

            List<Client>? clients = _clientManager.GetClientsByUserId(user.Id);
            invoiceViewModel.Clients = clients;

            List<DiamondPacket>? diamondPackets = await _diamondPacketManager.GetDiamondPacketsByClientIdAsync(clients.First().Id.ToString());

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (DiamondPacket diamondPacket in diamondPackets)
            {
                var item = new SelectListItem
                {
                    Value = diamondPacket.Id.ToString(),
                    Text = diamondPacket.Id.ToString(),
                };

                items.Add(item);
            }

            MultiSelectList diamondPacketSelectList = new MultiSelectList(items.OrderBy(i => i.Text), "Value", "Text");
            invoiceViewModel.DiamondPackets = diamondPacketSelectList;
            ViewBag.PageName = "Invoice";
            ViewBag.PageAction = "Add";
            return View("Edit", invoiceViewModel);
        }

        // GET: /invoice/edit/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            Invoice? invoice = await _invoiceManager.GetInvoiceByIdAsync(id, user.Id);

            if (invoice == null)
            {
                return NotFound();
            }

            InvoiceViewModel invoiceViewModel = new()
            {
                InvoiceId = invoice.Id,
                ClientId = invoice.ClientId,
                Description = invoice.Description,
                PaymentStatus = invoice.PaymentStatus,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                UserId = user.Id,
            };


            ViewBag.PageName = "Invoice";
            ViewBag.PageAction = "Edit";

            return View(invoiceViewModel);
        }

        // GET: /invoice/details/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            Invoice? invoice = await _invoiceManager.GetInvoiceByIdAsync(id, user.Id);

            if (invoice == null) return NotFound();

            ViewBag.PageName = "Invoice";

            return View(invoice);
        }

        // POST: /invoice/add
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(InvoiceViewModel invoiceViewModel)
        {
            try
            {
                ApplicationUser? user = await _userManager.GetUserAsync(User);

                if (ModelState.IsValid)
                {

                    if (user == null || user.Id != invoiceViewModel.UserId)
                    {
                        return NotFound();
                    }

                    Client? client = await _clientManager.GetClientByIdAsync(invoiceViewModel.ClientId.ToString(), user.Id);
                    if (client == null) { return NotFound(); }

                    await _invoiceManager.AddInvoiceAsync(invoiceViewModel);

                    TempData["ToastMessage"] = "Invoice added successfully.";
                    TempData["ToastStatus"] = ToastStatus.Success;
                    TempData.Keep();

                    return RedirectToAction("Index", "Invoice");
                }

                List<Client>? clients = _clientManager.GetClientsByUserId(user.Id);
                invoiceViewModel.Clients = clients;

                List<DiamondPacket>? diamondPackets = await _diamondPacketManager.GetDiamondPacketsByClientIdAsync(invoiceViewModel.ClientId.ToString());

                List<SelectListItem> items = new List<SelectListItem>();

                foreach (DiamondPacket diamondPacket in diamondPackets)
                {
                    var item = new SelectListItem
                    {
                        Value = diamondPacket.Id.ToString(),
                        Text = diamondPacket.Id.ToString(),
                    };

                    items.Add(item);
                }

                MultiSelectList diamondPacketSelectList = new MultiSelectList(items.OrderBy(i => i.Text), "Value", "Text");
                invoiceViewModel.DiamondPackets = diamondPacketSelectList;

            }
            catch
            {
                TempData["ToastMessage"] = "Something went wrong! Please try again.";
                TempData["ToastStatus"] = ToastStatus.Danger;
                TempData.Keep();

                return RedirectToAction("Add");
            }

            TempData["ToastMessage"] = "Something went wrong! Please try again.";
            TempData["ToastStatus"] = ToastStatus.Danger;


            ViewBag.PageName = "Invoice";
            ViewBag.PageAction = "Add";
            return View("Edit", invoiceViewModel);

        }

        // POST: /invoice/edit/{id}
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, InvoiceViewModel invoiceViewModel)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            ViewBag.PageName = "Invoice";
            ViewBag.PageAction = "Edit";
            TempData["ToastMessage"] = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))); ;
            if (ModelState.IsValid)
            {
                try
                {

                    if (invoiceViewModel.UserId != user.Id)
                    {
                        return NotFound();
                    }


                    await _invoiceManager.UpdateInvoiceAsync(invoiceViewModel);

                    TempData["ToastMessage"] = "Invoice updated successfully.";
                    TempData["ToastStatus"] = ToastStatus.Success;
                    TempData.Keep();

                    return RedirectToAction("Index", "Invoice");
                }
                catch
                {

                    TempData["ToastMessage"] = "Something went wrong! Please try again.";
                    TempData["ToastStatus"] = ToastStatus.Danger;
                    TempData.Keep();

                    return View(invoiceViewModel);
                }
            }


            return View(invoiceViewModel);
        }

    }
}
