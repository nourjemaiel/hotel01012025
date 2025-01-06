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
    public class ChambreService : IChambreService
    {
        private readonly IGenericBLL<Chambre> _chambreBLL;

        public ChambreService(IGenericBLL<Chambre> chambreBLL)
        {
            _chambreBLL = chambreBLL;
        }

        public IEnumerable<Chambre> GetChambres()
        {
            return _chambreBLL.GetMany();
        }

        public Chambre GetChambreById(int id)
        {
            var chambre = _chambreBLL.GetById(id);
            if (chambre == null)
            {
                throw new Exception($"Chambre with ID {id} not found.");
            }
            return chambre;
        }

        public bool NumeroExiste(string numero)
        {
            return _chambreBLL.GetMany().Any(c => c.Numero == numero);
        }

        public bool ChambreExists(int chambreId)
        {
            return _chambreBLL.GetById(chambreId) != null; // Assuming GetById method
        }




        public void AddChambre(Chambre chambre)
        {
            _chambreBLL.Add(chambre);
            _chambreBLL.Submit();
        }

        public void UpdateChambre(int id, Chambre chambre)
        {
            var existingChambre = _chambreBLL.GetById(id);
            if (existingChambre == null)
            {
                throw new Exception($"Chambre with ID {id} not found.");
            }

            existingChambre.Numero = chambre.Numero;
            existingChambre.Type = chambre.Type;
            existingChambre.Capacite = chambre.Capacite;
            existingChambre.Tarifjournalier = chambre.Tarifjournalier;

            _chambreBLL.Update(existingChambre);
            _chambreBLL.Submit();
        }

        public void DeleteChambre(int id)
        {
            var chambre = _chambreBLL.GetById(id);
            if (chambre == null)
            {
                throw new Exception($"Chambre with ID {id} not found.");
            }
            _chambreBLL.Delete(chambre);
            _chambreBLL.Submit();
        }
    }
}

