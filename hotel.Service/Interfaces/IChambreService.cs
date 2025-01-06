using hotel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel.Service.Interfaces
{
    public interface IChambreService
    {
        IEnumerable<Chambre> GetChambres();
        Chambre GetChambreById(int id);
        void AddChambre(Chambre chambre);
        void UpdateChambre(int id, Chambre chambre);
        void DeleteChambre(int id);
        bool NumeroExiste(string numero);
        bool ChambreExists(int chambreId);
    }
}

