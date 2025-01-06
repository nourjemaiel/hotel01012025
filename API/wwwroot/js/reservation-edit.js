$(document).ready(function () {
    const reservationId = new URLSearchParams(window.location.search).get('id');

    function fetchClientsAndChambres(callback) {
        let clientsLoaded = false;
        let chambresLoaded = false;

        $.get('/api/Client/GetClients', function (clients) {
            clients.forEach(client => {
                $('#clientSelect').append(new Option(`${client.nom} ${client.prenom}`, client.id));
            });
            clientsLoaded = true;
            if (clientsLoaded && chambresLoaded) callback();
        }).fail(function () {
            alert("Erreur lors du chargement des clients.");
        });

        $.get('/api/Chambre/GetChambres', function (chambres) {
            chambres.forEach(chambre => {
                $('#chambreSelect').append(new Option(chambre.numero, chambre.id));
            });
            chambresLoaded = true;
            if (clientsLoaded && chambresLoaded) callback();
        }).fail(function () {
            alert("Erreur lors du chargement des chambres.");
        });
    }

    // Fetch reservation data using the GetReservationById endpoint
    function fetchReservation() {
        if (!reservationId) {
            alert("Aucun identifiant de réservation fourni.");
            return;
        }

        // Fetch data from the controller using the reservation ID
        $.get(`/api/Reservation/GetReservationById/${reservationId}`, function (reservation) {
            console.log("Reservation data received:", reservation); // For debugging

            // Check if data is returned correctly
            if (!reservation || !reservation.datearrivee || !reservation.datedepart) {
                alert("Les données de réservation sont incomplètes ou introuvables.");
                return;
            }

            // Pre-populate the form fields
            $('#dateArrivee').val(reservation.datearrivee.split('T')[0]);
            $('#dateDepart').val(reservation.datedepart.split('T')[0]);
            $('#nombrePersonnes').val(reservation.nombrepersonnes);
            $('#typePaiement').val(reservation.typepaiement);
            $('#clientSelect').val(reservation.clientId);
            $('#chambreSelect').val(reservation.chambreId);
        }).fail(function () {
            alert("Erreur lors du chargement de la réservation.");
        });
    }

    // Call the function when the page loads
    fetchClientsAndChambres(fetchReservation);

    $('#reservationForm').submit(function (e) {
        e.preventDefault();

        const updatedReservation = {
            datearrivee: $('#dateArrivee').val(),
            datedepart: $('#dateDepart').val(),
            nombrepersonnes: parseInt($('#nombrePersonnes').val()),
            typepaiement: $('#typePaiement').val(),
            clientId: parseInt($('#clientSelect').val()),
            chambreId: parseInt($('#chambreSelect').val())
        };

        $.ajax({
            url: `/api/Reservation/UpdateReservation/${reservationId}`,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(updatedReservation),
            success: function () {
                window.location.href = 'reservations-list.html';
            },
            error: function (xhr) {
                alert("Erreur lors de la mise à jour : " + xhr.responseText);
            }
        });
    });
});

