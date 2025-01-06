using hotel.Entities;
using hotel.Service;
using hotel.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[Controller]")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IChambreService _chambreService;
        private readonly IClientService _clientService;

        public ReservationController(IReservationService reservationService, IChambreService chambreService, IClientService clientService)
        {
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
            _chambreService = chambreService ?? throw new ArgumentNullException(nameof(chambreService));
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        }


        [HttpGet]
        [Route("GetReservations")]
        public IActionResult GetReservations()
        {
            var reservations = _reservationService.GetReservations();
            return Ok(reservations);
        }

        [HttpGet]
        [Route("GetReservationById/{id}")]
        public IActionResult GetReservationById(int id)
        {
            try
            {
                var reservation = _reservationService.GetReservationById(id);
                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddReservation")]
        public IActionResult AddReservation([FromBody] Reservation reservation)
        {
            if (reservation == null)
            {
                return BadRequest("Reservation data is null.");
            }

            // Vérifier que la date de départ n'est pas avant la date d'arrivée
            if (reservation.Datedepart < reservation.Datearrivee)
            {
                return BadRequest("La date de départ ne peut pas être avant la date d'arrivée.");
            }

            // Vérifier que le type de paiement est valide
            if (reservation.Typepaiement != "especes" && reservation.Typepaiement != "par carte")
            {
                return BadRequest("Le type de paiement doit être 'especes' ou 'par carte'.");
            }

            // Vérifier que le client existe
            var clientExists = _clientService.ClientExists(reservation.ClientId);
            if (!clientExists)
            {
                return BadRequest("Le client spécifié n'existe pas.");
            }

            // Vérifier que la chambre existe
            var chambreExists = _chambreService.ChambreExists(reservation.ChambreId);
            if (!chambreExists)
            {
                return BadRequest("La chambre spécifiée n'existe pas.");
            }

            // Récupérer le tarif journalier de la chambre
            var chambre = _chambreService.GetChambreById(reservation.ChambreId);
            if (chambre == null)
            {
                return BadRequest("Chambre non trouvée.");
            }

            // Calculer le montant total (tarif journalier * nombre de jours)
            var diffDays = (reservation.Datedepart - reservation.Datearrivee).Days;
            if (diffDays <= 0)
            {
                return BadRequest("La période de réservation est invalide.");
            }

            var montantTotal = chambre.Tarifjournalier * diffDays;
            reservation.Montanttotal = montantTotal;

            // Ajouter la réservation
            _reservationService.AddReservation(reservation);
            return Ok("Réservation ajoutée avec succès.");
        }



        [HttpPut]
        [Route("UpdateReservation/{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] Reservation reservation)
        {
            if (reservation == null)
            {
                return BadRequest("Reservation data is null.");
            }

            // Vérifier si la date de départ est après la date d'arrivée
            if (reservation.Datedepart < reservation.Datearrivee)
            {
                return BadRequest("La date de départ ne peut pas être avant la date d'arrivée.");
            }

            // Vérifier si le type de paiement est valide
            if (reservation.Typepaiement != "especes" && reservation.Typepaiement != "par carte")
            {
                return BadRequest("Le type de paiement doit être 'especes' ou 'par carte'.");
            }

            // Vérifier si le client existe dans la base de données
            var clientExists = _clientService.ClientExists(reservation.ClientId);
            if (!clientExists)
            {
                return BadRequest($"Le client avec l'ID {reservation.ClientId} n'existe pas.");
            }

            // Vérifier si la chambre existe dans la base de données
            var chambreExists = _chambreService.ChambreExists(reservation.ChambreId);
            if (!chambreExists)
            {
                return BadRequest($"La chambre avec l'ID {reservation.ChambreId} n'existe pas.");
            }

            // Calculer le montant total en fonction du tarif journalier de la chambre et de la période de la réservation
            var chambre = _chambreService.GetChambreById(reservation.ChambreId);
            var tarifJournalier = chambre.Tarifjournalier;
            var dureeSejour = (reservation.Datedepart - reservation.Datearrivee).Days;

            // Si la durée du séjour est négative ou nulle, retourner une erreur
            if (dureeSejour <= 0)
            {
                return BadRequest("La durée de la réservation est invalide.");
            }

            var montantTotal = tarifJournalier * dureeSejour * reservation.Nombrepersonnes;

            // Mettre à jour les informations de la réservation
            var existingReservation = _reservationService.GetReservationById(id);
            if (existingReservation == null)
            {
                return NotFound($"Réservation avec ID {id} non trouvée.");
            }

            // Passer à la méthode de service avec le bon paramètre
            existingReservation.Datearrivee = reservation.Datearrivee;
            existingReservation.Datedepart = reservation.Datedepart;
            existingReservation.Nombrepersonnes = reservation.Nombrepersonnes;
            existingReservation.Typepaiement = reservation.Typepaiement;
            existingReservation.Montanttotal = montantTotal;
            existingReservation.ChambreId = reservation.ChambreId;
            existingReservation.ClientId = reservation.ClientId;

            // Mettre à jour la réservation dans la base de données via le service
            _reservationService.UpdateReservation(id, existingReservation);  // Pass both 'id' and 'existingReservation' here

            return Ok("Réservation mise à jour avec succès.");
        }



        [HttpDelete]
        [Route("DeleteReservation/{id}")]
        public IActionResult DeleteReservation(int id)
        {
            try
            {
                _reservationService.DeleteReservation(id);
                return Ok($"Reservation with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

