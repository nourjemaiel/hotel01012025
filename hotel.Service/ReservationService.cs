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
    public class ReservationService : IReservationService
    {
        private readonly IGenericBLL<Reservation> _reservationBLL;

        public ReservationService(IGenericBLL<Reservation> reservationBLL)
        {
            _reservationBLL = reservationBLL;
        }

        public IEnumerable<Reservation> GetReservations()
        {
            return _reservationBLL.GetMany();
        }

        public Reservation GetReservationById(int id)
        {
            var reservation = _reservationBLL.GetById(id);
            if (reservation == null)
            {
                throw new Exception($"Reservation with ID {id} not found.");
            }
            return reservation;
        }

        public void AddReservation(Reservation reservation)
        {
            _reservationBLL.Add(reservation);
            _reservationBLL.Submit();
        }

        public void UpdateReservation(int id, Reservation reservation)
        {
            var existingReservation = _reservationBLL.GetById(id); // Use 'id' to fetch the reservation
            if (existingReservation == null)
            {
                throw new Exception($"Réservation avec ID {id} non trouvée.");
            }

            // Mettre à jour les champs de la réservation
            existingReservation.Datearrivee = reservation.Datearrivee;
            existingReservation.Datedepart = reservation.Datedepart;
            existingReservation.Nombrepersonnes = reservation.Nombrepersonnes;
            existingReservation.Typepaiement = reservation.Typepaiement;
            existingReservation.Montanttotal = reservation.Montanttotal;
            existingReservation.ChambreId = reservation.ChambreId;
            existingReservation.ClientId = reservation.ClientId;

            _reservationBLL.Update(existingReservation);
            _reservationBLL.Submit();
        }



        public void DeleteReservation(int id)
        {
            var reservation = _reservationBLL.GetById(id);
            if (reservation == null)
            {
                throw new Exception($"Reservation with ID {id} not found.");
            }
            _reservationBLL.Delete(reservation);
            _reservationBLL.Submit();
        }
    }
}

