using CaratCount.Entities;
using CaratCount.Models;

namespace CaratCount.Interface
{
    public interface IInvoiceManager
    {
        Task<Invoice?> GetInvoiceByIdAsync(string id, string userId);
        List<Invoice>? GetInvoicesByUserId(string userId);
        Task AddInvoiceAsync(InvoiceViewModel invoiceViewModel);
        Task UpdateInvoiceAsync(InvoiceViewModel invoiceViewModel);
        Task<InvoiceItem?> GetInvoiceItemByIdAsync(string id);
        Task<InvoiceItem?> GetInvoiceItemByInvoiceIdAsync(string id);
        Task<List<InvoiceItem>>? GetInvoiceItemsByInvoiceIdAsync(string id);
        Task AddInvoiceItemAsync(InvoiceItem invoiceItem);
        Task UpdateInvoiceItemAsync(InvoiceItem invoiceItem);
    }
}
