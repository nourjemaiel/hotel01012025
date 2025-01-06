using hotel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel.Service.Interfaces
{
    public interface IClientService
    {
        IEnumerable<Client> GetClients();
        Client GetClientById(int id);
        void AddClient(Client client);
        void UpdateClient(int id, Client client);
        void DeleteClient(int id);
        bool EmailExiste(string email);
        bool ClientExists(int clientId);
    }
}
