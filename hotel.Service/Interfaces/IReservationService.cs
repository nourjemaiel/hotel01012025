using hotel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel.Service.Interfaces
{
    public interface IReservationService
    {
        IEnumerable<Reservation> GetReservations();
        Reservation GetReservationById(int id);
        void AddReservation(Reservation reservation);
        void UpdateReservation(int id, Reservation reservation);
        void DeleteReservation(int id);
    }
}

