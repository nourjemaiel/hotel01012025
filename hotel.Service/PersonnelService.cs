using hotel.BLL.Contract;
using hotel.Entities;
using hotel.Service.Interfaces;

namespace hotel.Service
{
    public class PersonnelService: IPersonnelService
    {
        private IGenericBLL<Personnel> _personnelBLL;
        public PersonnelService(IGenericBLL<Personnel> personnelBll)
        {
            _personnelBLL = personnelBll;
        }
        public IEnumerable<Personnel> GetPersonnels()
        {
            return _personnelBLL.GetMany();

        }

        public Personnel GetPersonnelById(int id) // Implementation
        {
            return _personnelBLL.GetById(id);
        }

        public bool EmailExiste(string email)
        {
            return _personnelBLL.GetMany().Any(p => p.Email == email);
        }


        public void AddPersonnel(Personnel personnel) // Implementation
        {
            _personnelBLL.Add(personnel);
            _personnelBLL.Submit();
        }

        public void UpdatePersonnel(int id, Personnel personnel)
        {
            var existingPersonnel = _personnelBLL.GetById(id);
            if (existingPersonnel == null)
            {
                throw new Exception($"Personnel with ID {id} not found.");
            }

            // Mettre à jour les propriétés du personnel
            existingPersonnel.Nom = personnel.Nom;
            existingPersonnel.Prenom = personnel.Prenom;
            existingPersonnel.Email = personnel.Email;
            existingPersonnel.Motdepasse = personnel.Motdepasse;

            _personnelBLL.Update(existingPersonnel);
            _personnelBLL.Submit();
        }

        public void DeletePersonnel(int id)
        {
            var personnel = _personnelBLL.GetById(id);
            if (personnel == null)
            {
                throw new Exception($"Personnel with ID {id} not found.");
            }
            _personnelBLL.Delete(personnel);
            _personnelBLL.Submit();
        }


        public Personnel Authentifier(string email, string motdepasse)
        {
            var personnel = _personnelBLL.GetMany().FirstOrDefault(p => p.Email == email);
            if (personnel == null)
            {
                throw new UnauthorizedAccessException("Email ou mot de passe invalide.");
            }

            // Si les mots de passe sont stockés en texte brut, il est préférable de les hacher en production.
            if (personnel.Motdepasse != motdepasse)
            {
                throw new UnauthorizedAccessException("Email ou mot de passe invalide.");
            }

            return personnel; // Si l'authentification est réussie, retourner le personnel
        }



    }
}