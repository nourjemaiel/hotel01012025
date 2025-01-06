using hotel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel.Service.Interfaces
{
    public interface IPersonnelService
    {
        IEnumerable<Personnel> GetPersonnels();
        Personnel GetPersonnelById(int id);
        void AddPersonnel(Personnel personnel);
        void UpdatePersonnel(int id, Personnel personnel);
        void DeletePersonnel(int id);
        Personnel Authentifier(string email, string motdepasse);
        bool EmailExiste(string email);

    }
}
