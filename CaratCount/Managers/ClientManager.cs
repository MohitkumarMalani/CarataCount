using CaratCount.Data;
using CaratCount.Entities;
using CaratCount.Interface;
using CaratCount.Migrations;
using CaratCount.Models;
using Microsoft.EntityFrameworkCore;

namespace CaratCount.Managers
{
    public class ClientManager : IClientManager
    {
        private readonly ApplicationDbContext _context;

        public ClientManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Client?> GetClientByIdAsync(string id, string userId)
        {
            Guid clientId;

            if (!Guid.TryParse(id, out clientId) || string.IsNullOrEmpty(userId)) return null;


            Client? client =  await _context.Clients
                .Include(c => c.GstInDetail)
                .ThenInclude(g => g.Address)
                .FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);

            return client;
        }

        public List<Client>? GetClientsByUserId(string userId)
        {
            List<Client>? clients = _context.Clients
                .Include(c => c.GstInDetail)
                .ThenInclude(g => g.Address)
                .Where(c => c.UserId == userId)
                .ToList();
            return clients ?? new List<Client>();
        }


        public async Task AddClientAsync(ClientViewModel clientViewModel, string userId)
        {
            try
            {
                Address? address = new ()
                {
                    BuildingName = clientViewModel.BuildingName,
                    StreetName = clientViewModel.StreetName,
                    FloorNumber = clientViewModel.FloorNumber,
                    UnitNumber = clientViewModel.UnitNumber,
                    Locality = clientViewModel.Locality,
                    City = clientViewModel.City,
                    District = clientViewModel.District,
                    State = clientViewModel.State,
                    Country = clientViewModel.Country,
                    PostalCode = clientViewModel.PostalCode
                };


                 _context.Addresses.Add(address);

                GstInDetail? gstInDetail = new ()
                {
                    UserId = userId,
                    AddressId = address.Id,
                    GstInNo = clientViewModel.GstInNo,
                    LegalName = clientViewModel.LegalName,
                    TradeName = clientViewModel.TradeName,
                    Address = address
                };

                _context.GstInDetails.Add(gstInDetail);

                Client? client = new()
                {
                    UserId = userId,
                    GstInDetailId = gstInDetail.Id,
                    Name = clientViewModel.Name,
                    Email = clientViewModel.Email,
                    PhoneNumber = clientViewModel.PhoneNumber,
                    GstInDetail = gstInDetail,

                };

                _context.Clients.Add(client);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateClientAsync(ClientViewModel clientViewModel, string userId)
        {
            try
            {
                
                Client? client = await _context.Clients
                    .Include(c => c.GstInDetail)
                    .ThenInclude(g => g.Address)
                     .FirstOrDefaultAsync(c => c.Id == clientViewModel.ClientId && c.UserId == userId);
                 

                if (client != null)
                {
                    client.Name = clientViewModel.Name;
                    client.Email = clientViewModel.Email;
                    client.PhoneNumber = clientViewModel.PhoneNumber;

                    client.GstInDetail.GstInNo = clientViewModel.GstInNo;
                    client.GstInDetail.LegalName = clientViewModel.LegalName;
                    client.GstInDetail.TradeName = clientViewModel.TradeName;

                    client.GstInDetail.Address.BuildingName = clientViewModel.BuildingName;
                    client.GstInDetail.Address.StreetName = clientViewModel.StreetName;
                    client.GstInDetail.Address.FloorNumber = clientViewModel.FloorNumber;
                    client.GstInDetail.Address.UnitNumber = clientViewModel.UnitNumber;
                    client.GstInDetail.Address.Locality = clientViewModel.Locality;
                    client.GstInDetail.Address.City = clientViewModel.City;
                    client.GstInDetail.Address.District = clientViewModel.District;
                    client.GstInDetail.Address.State = clientViewModel.State;
                    client.GstInDetail.Address.Country = clientViewModel.Country;
                    client.GstInDetail.Address.PostalCode = clientViewModel.PostalCode;


                    await _context.SaveChangesAsync();
                }
                else
                {
                  
                    throw new Exception("Client not found");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task DeleteClientAsync(Client client)
        {
            try
            {
                if (client == null)
                {
                    throw new ArgumentNullException(nameof(client));
                }

                _context.Clients.Remove(client);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
