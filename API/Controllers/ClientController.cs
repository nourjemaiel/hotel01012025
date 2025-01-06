using hotel.Entities;
using hotel.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[Controller]")]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        [Route("GetClients")]
        public IActionResult GetClients()
        {
            var clients = _clientService.GetClients();
            return Ok(clients);
        }

        [HttpGet]
        [Route("GetClientById/{id}")]
        public IActionResult GetClientById(int id)
        {
            try
            {
                var client = _clientService.GetClientById(id);
                return Ok(client);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddClient")]
        public IActionResult AddClient([FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest("Client data is null.");
            }
            if (_clientService.EmailExiste(client.Email))
            {
                return BadRequest("Cet email est déjà utilisé par un autre personnel.");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(client.Telephone, @"^\d+$"))
            {
                return BadRequest("Le numéro de téléphone doit contenir uniquement des chiffres.");
            }

            _clientService.AddClient(client);
            return Ok("Client added successfully.");
        }

        [HttpPut]
        [Route("UpdateClient/{id}")]
        public IActionResult UpdateClient(int id, [FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest("Client data is null.");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(client.Telephone, @"^\d+$"))
            {
                return BadRequest("Le numéro de téléphone doit contenir uniquement des chiffres.");
            }

            // Vérifiez si l'email existe déjà pour un autre personnel
            var existingClient = _clientService.GetClients().FirstOrDefault(p => p.Email == client.Email && p.Id != id);
            if (existingClient != null)
            {
                return BadRequest("Cet email est déjà utilisé par un autre personnel.");
            }

            try
            {
                _clientService.UpdateClient(id, client);
                return Ok("Client updated successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpDelete]
        [Route("DeleteClient/{id}")]
        public IActionResult DeleteClient(int id)
        {
            try
            {
                _clientService.DeleteClient(id);
                return Ok($"Client with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

