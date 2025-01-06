using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using hotel.Service.Interfaces;
using hotel.Entities;
using hotel.Service;
using hotel.DTO;

namespace API.Controllers
{
    [Route("api/[Controller]")]
    public class PersonnelController : Controller
    {
        IPersonnelService service;

        public PersonnelController(IPersonnelService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("GetPersonnels")]

        public IActionResult GetPersonnels()
        {
            var listPersonnels = service.GetPersonnels();
            if (listPersonnels != null)
            {
                return new OkObjectResult(listPersonnels);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("GetPersonnelById/{id}")]
        public IActionResult GetPersonnelById(int id)
        {
            var personnel = service.GetPersonnelById(id);
            if (personnel != null)
            {
                return new OkObjectResult(personnel);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("AddPersonnel")]
        public IActionResult AddPersonnel([FromBody] Personnel personnel)
        {
            if (ModelState.IsValid)
            {
                // Vérifiez si l'email existe déjà
                if (service.EmailExiste(personnel.Email))
                {
                    return BadRequest("Cet email est déjà utilisé par un autre personnel.");
                }
                service.AddPersonnel(personnel);
                return Ok("Personnel ajouté avec succès.");
            }
            return BadRequest("Les données envoyées ne sont pas valides.");
        }

        [HttpPut]
        [Route("UpdatePersonnel/{id}")]
        public IActionResult UpdatePersonnel(int id, [FromBody] Personnel personnel)
        {
            if (personnel == null)
            {
                return BadRequest("Données du personnel manquantes.");
            }

            // Vérifiez si l'email existe déjà pour un autre personnel
            var existingPersonnel = service.GetPersonnels().FirstOrDefault(p => p.Email == personnel.Email && p.Id != id);
            if (existingPersonnel != null)
            {
                return BadRequest("Cet email est déjà utilisé par un autre personnel.");
            }

            try
            {
                service.UpdatePersonnel(id, personnel);
                return Ok("Personnel mis à jour avec succès.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpDelete]
        [Route("DeletePersonnel/{id}")]
        public IActionResult DeletePersonnel(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            try
            {
                service.DeletePersonnel(id);
                return Ok($"Personnel with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] DemandeConnexion demandeConnexion)
        {
            try
            {
                var personnel = service.Authentifier(demandeConnexion.Email, demandeConnexion.Motdepasse);
                return Ok(new { message = "Connexion réussie", personnel = personnel });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }





    }
}
