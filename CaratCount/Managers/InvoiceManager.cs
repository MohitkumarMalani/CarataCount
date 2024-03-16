using CaratCount.Data;
using CaratCount.Entities;
using CaratCount.Interface;
using CaratCount.Migrations;
using CaratCount.Models;
using Microsoft.EntityFrameworkCore;

namespace CaratCount.Managers
{
    public class InvoiceManager : IInvoiceManager
    {
        private readonly ApplicationDbContext _context;

        public InvoiceManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(string id, string userId)
        {
            Guid invoiceId;

            if (!Guid.TryParse(id, out invoiceId) || string.IsNullOrEmpty(value: userId)) return null;


            Invoice? invoice = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .Include(i => i.Client)
                .FirstOrDefaultAsync(i => i.Id == invoiceId && i.UserId == userId);

            return invoice;
        }

        public List<Invoice>? GetInvoicesByUserId(string userId)
        {
            List<Invoice>? invoices = _context.Invoices
               .Include(i => i.InvoiceItems)
               .Include(i => i.Client)
               .Where(c => c.UserId == userId)
               .ToList();
            return invoices ?? new List<Invoice>();
        }

        public async Task AddInvoiceAsync(InvoiceViewModel invoiceViewModel)
        {
            try
            {
                Invoice invoice = new()
                {
                    UserId = invoiceViewModel.UserId,
                    ClientId = invoiceViewModel.ClientId,
                    Description = invoiceViewModel.Description,
                    PaymentStatus = invoiceViewModel.PaymentStatus,
                    IssueDate = invoiceViewModel.IssueDate,
                    DueDate = invoiceViewModel.DueDate,
                };

                _context.Invoices.Add(invoice);

                decimal totalAmount = 0;
                foreach (var diamondPacketId in invoiceViewModel.DiamondPacketIds)
                {
                    DiamondPacket? diamondPacket = await _context.DiamondPackets
                        .Include(d => d.DiamondPacketProcesses)
                        .ThenInclude(d => d.ProcessPrice)
                        .FirstOrDefaultAsync(
                        d => d.Id == diamondPacketId
                        );

                    if (diamondPacket == null) { continue; }

                    ICollection<DiamondPacketProcess>? diamondPacketProcesses = diamondPacket.DiamondPacketProcesses;

                    decimal totalUserCost = 0;
                    decimal totalClientCharge = 0;

                    foreach (var process in diamondPacketProcesses)
                    {
                        ProcessPrice? processPrice = process.ProcessPrice;
                        totalUserCost += processPrice.UserCost * diamondPacket.CaratWeight;
                        totalClientCharge += processPrice.ClientCharge * diamondPacket.CaratWeight;
                      
                    }
                    
                    totalAmount += totalClientCharge;

                    InvoiceItem invoiceItem = new()
                    {
                        UserCost = totalUserCost,
                        ClientCharge = totalClientCharge,
                        DiamondPacketId = diamondPacketId,
                        InvoiceId = invoice.Id
                    };

                    await AddInvoiceItemAsync(invoiceItem);
                }



                invoice.TotalAmount = totalAmount;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task UpdateInvoiceAsync(InvoiceViewModel invoiceViewModel)
        {
            try
            {
                Invoice? invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == invoiceViewModel.InvoiceId);

              
                if (invoice == null)
                {
                    throw new Exception("Invoice not found");
                }

                invoice.Description = invoiceViewModel.Description;
                invoice.PaymentStatus = invoiceViewModel.PaymentStatus;
                invoice.IssueDate = invoiceViewModel.IssueDate;
                invoice.DueDate = invoiceViewModel.DueDate;

                _context.Invoices.Update(invoice);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<InvoiceItem?> GetInvoiceItemByIdAsync(string id)
        {
            Guid invoiceItemId;

            if (!Guid.TryParse(id, out invoiceItemId)) return null;


            InvoiceItem? invoiceItem = await _context.InvoiceItems.Include(i => i.DiamondPacket)
                .FirstOrDefaultAsync(i => i.Id == invoiceItemId);

            return invoiceItem;
        }

        public async Task<InvoiceItem?> GetInvoiceItemByInvoiceIdAsync(string id)
        {
            Guid invoiceId;

            if (!Guid.TryParse(id, out invoiceId)) return null;


            InvoiceItem? invoiceItem = await _context.InvoiceItems
                .Include(i => i.DiamondPacket)
                .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

            return invoiceItem;
        }

        public async Task<List<InvoiceItem>?> GetInvoiceItemsByInvoiceIdAsync(string id)
        {
            Guid invoiceId;

            if (!Guid.TryParse(id, out invoiceId)) return null;

            List<InvoiceItem> invoices = await _context.InvoiceItems
                .Where(i => i.InvoiceId == invoiceId)
                .ToListAsync();

            return invoices ?? new List<InvoiceItem>();
        }

        public async Task AddInvoiceItemAsync(InvoiceItem invoiceItem)
        {
            try
            {
                _context.InvoiceItems.Add(invoiceItem);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateInvoiceItemAsync(InvoiceItem invoiceItem)
        {
            try
            {
                _context.InvoiceItems.Update(invoiceItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
