// Fonction pour récupérer la liste des réservations depuis l'API
async function fetchReservations() {
    console.log("Fetching reservations..."); // Debugging line
    try {
        // Fetch reservations
        const response = await fetch('/api/Reservation/GetReservations');
        if (response.ok) {
            const reservations = await response.json();
            console.log(reservations); // Log the reservations data

            const tbody = document.getElementById('reservationTableBody');
            tbody.innerHTML = ''; // Effacer les anciennes lignes avant d'ajouter de nouvelles données

            // Fetch each client and chambre data individually for each reservation
            for (const reservation of reservations) {
                // Fetch client and chambre details based on their IDs
                const clientResponse = await fetch(`/api/Client/GetClientById/${reservation.clientId}`);
                const chambreResponse = await fetch(`/api/Chambre/GetChambreById/${reservation.chambreId}`);

                if (clientResponse.ok && chambreResponse.ok) {


                    const client = await clientResponse.json();
                    const chambre = await chambreResponse.json();

                    const row = document.createElement('tr');
                    row.setAttribute('data-id', reservation.id); // Ajouter un attribut pour l'ID

                    row.innerHTML = `
                        <td>${reservation.id}</td>
                        <td>${new Date(reservation.datearrivee).toLocaleDateString()}</td>
                        <td>${new Date(reservation.datedepart).toLocaleDateString()}</td>
                        <td>${reservation.nombrepersonnes}</td>
                        <td>${reservation.typepaiement}</td>
                        <td>${reservation.montanttotal.toFixed(2)} €</td>
                        <td>${chambre ? chambre.numero : 'N/A'}</td>
                        <td>${client ? `${client.nom} ${client.prenom}` : 'N/A'}</td>
                        <td>
                            <a href="reservation-edit.html?id=${reservation.id}">Modifier</a>
                            <a href="#" class="delete" data-id="${reservation.id}">Supprimer</a>
                        </td>
                    `;

                    tbody.appendChild(row);
                } else {
                    console.error(`Erreur lors de la récupération des données pour la réservation ID ${reservation.id}`);
                }
            }

            // Ajouter les événements de suppression après l'ajout des lignes
            const deleteButtons = document.querySelectorAll('.delete');
            deleteButtons.forEach(button => {
                button.addEventListener('click', function (event) {
                    event.preventDefault(); // Prevent the default link behavior
                    const reservationId = button.getAttribute('data-id');
                    if (confirm('Êtes-vous sûr de vouloir supprimer cette réservation ?')) {
                        deleteReservation(reservationId);
                    }
                });
            });
        } else {
            console.error('Erreur lors de la récupération des réservations');
        }
    } catch (error) {
        console.error('Erreur:', error);
    }
}


// Fonction pour supprimer une réservation
async function deleteReservation(id) {
    try {
        const response = await fetch(`/api/Reservation/DeleteReservation/${id}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            alert('Réservation supprimée avec succès!');
            // Recharger la liste après suppression
            fetchReservations();
        } else {
            console.error('Erreur lors de la suppression de la réservation');
            alert('Une erreur est survenue lors de la suppression de la réservation.');
        }
    } catch (error) {
        console.error('Erreur:', error);
        alert('Une erreur est survenue lors de la suppression de la réservation.');
    }
}

// Charger les réservations lors du chargement de la page
window.onload = fetchReservations;

