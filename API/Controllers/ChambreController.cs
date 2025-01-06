using hotel.Entities;
using hotel.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[Controller]")]
    public class ChambreController : Controller
    {
        private readonly IChambreService _chambreService;

        public ChambreController(IChambreService chambreService)
        {
            _chambreService = chambreService;
        }

        [HttpGet]
        [Route("GetChambres")]
        public IActionResult GetChambres()
        {
            var chambres = _chambreService.GetChambres();
            return Ok(chambres);
        }

        [HttpGet]
        [Route("GetChambreById/{id}")]
        public IActionResult GetChambreById(int id)
        {
            try
            {
                var chambre = _chambreService.GetChambreById(id);
                return Ok(chambre);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddChambre")]
        public IActionResult AddChambre([FromBody] Chambre chambre)
        {
            if (chambre == null)
            {
                return BadRequest("Chambre data is null.");
            }

            // Validate type
            if (chambre.Type != "single" && chambre.Type != "double" && chambre.Type != "suite")
            {
                return BadRequest("Le type de chambre doit être 'single', 'double' ou 'suite'.");
            }

            // Validate unique numero
            if (_chambreService.NumeroExiste(chambre.Numero))
            {
                return BadRequest("Le numéro de chambre existe déjà.");
            }

            _chambreService.AddChambre(chambre);
            return Ok("Chambre added successfully.");
        }



        [HttpPut]
        [Route("UpdateChambre/{id}")]
        public IActionResult UpdateChambre(int id, [FromBody] Chambre chambre)
        {
            if (chambre == null)
            {
                return BadRequest("Chambre data is null.");
            }

            // Check if the chambre number already exists (assuming Numero is unique)
            var existingChambre = _chambreService.GetChambres().FirstOrDefault(p => p.Numero == chambre.Numero && p.Id != id);
            if (existingChambre != null)
            {
                return BadRequest("Ce numéro existe déjà.");
            }

            // Validate Capacite (must be between 1 and 4)
            if (chambre.Capacite < 1 || chambre.Capacite > 4)
            {
                return BadRequest("La capacité doit être entre 1 et 4.");
            }

            // Validate Type (must be either 'single', 'double', or 'suite')
            if (chambre.Type != "single" && chambre.Type != "double" && chambre.Type != "suite")
            {
                return BadRequest("Le type de chambre doit être 'single', 'double', ou 'suite'.");
            }

            try
            {
                _chambreService.UpdateChambre(id, chambre);
                return Ok("Chambre mise à jour avec succès.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpDelete]
        [Route("DeleteChambre/{id}")]
        public IActionResult DeleteChambre(int id)
        {
            try
            {
                _chambreService.DeleteChambre(id);
                return Ok($"Chambre with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

