using hotel.BLL.Contract;
using hotel.Entities;
using hotel.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel.Service
{
    public class ClientService : IClientService
    {
        private readonly IGenericBLL<Client> _clientBLL;

        public ClientService(IGenericBLL<Client> clientBLL)
        {
            _clientBLL = clientBLL;
        }

        public IEnumerable<Client> GetClients()
        {
            return _clientBLL.GetMany();
        }

        public Client GetClientById(int id)
        {
            var client = _clientBLL.GetById(id);
            if (client == null)
            {
                throw new Exception($"Client with ID {id} not found.");
            }
            return client;
        }

        public bool ClientExists(int clientId)
        {
            var client = _clientBLL.GetById(clientId); // Ensure GetById does not return null
            return client != null; ; // Assuming GetById method
        }


        public bool EmailExiste(string email)
        {
            return _clientBLL.GetMany().Any(p => p.Email == email);
        }

        public void AddClient(Client client)
        {
            _clientBLL.Add(client);
            _clientBLL.Submit();
        }

        public void UpdateClient(int id, Client client)
        {
            var existingClient = _clientBLL.GetById(id);
            if (existingClient == null)
            {
                throw new Exception($"Client with ID {id} not found.");
            }

            existingClient.Nom = client.Nom;
            existingClient.Prenom = client.Prenom;
            existingClient.Email = client.Email;
            existingClient.Telephone = client.Telephone;

            _clientBLL.Update(existingClient);
            _clientBLL.Submit();
        }

        public void DeleteClient(int id)
        {
            var client = _clientBLL.GetById(id);
            if (client == null)
            {
                throw new Exception($"Client with ID {id} not found.");
            }
            _clientBLL.Delete(client);
            _clientBLL.Submit();
        }
    }
}


