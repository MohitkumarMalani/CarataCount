using CaratCount.Entities;
using CaratCount.Models;

namespace CaratCount.Interface
{
    public interface IClientManager
    {
        Task<Client?> GetClientByIdAsync(string id, string userId);

        List<Client>? GetClientsByUserId(string userId);
       
        Task AddClientAsync(ClientViewModel clientViewModel, string userId);

        Task UpdateClientAsync(ClientViewModel clientViewModel, string userId);

        Task DeleteClientAsync(Client client);
    }
}
