document.addEventListener("DOMContentLoaded", async () => {
    // Récupérer les listes de clients et de chambres
    const clientsResponse = await fetch("/api/Client/GetClients");
    const clients = await clientsResponse.json();

    const chambresResponse = await fetch("/api/Chambre/GetChambres");
    const chambres = await chambresResponse.json();

    // Remplir les listes déroulantes pour le client et la chambre
    const clientSelect = document.getElementById("client");
    clients.forEach(client => {
        const option = document.createElement("option");
        option.value = client.id;
        option.textContent = `${client.nom} ${client.prenom}`;
        clientSelect.appendChild(option);
    });

    const chambreSelect = document.getElementById("chambre");
    chambres.forEach(chambre => {
        const option = document.createElement("option");
        option.value = chambre.id;
        option.textContent = chambre.numero;
        chambreSelect.appendChild(option);
    });

    // Gérer la soumission du formulaire
    document.getElementById("addReservationForm").addEventListener("submit", async (event) => {
        event.preventDefault();

        // Récupérer les valeurs du formulaire
        const clientId = document.getElementById("client").value;
        const chambreId = document.getElementById("chambre").value;
        const dateArrivee = document.getElementById("dateArrivee").value;
        const dateDepart = document.getElementById("dateDepart").value;
        const typePaiement = document.getElementById("typePaiement").value;
        const nombrePersonnes = document.getElementById("nombrePersonnes").value;

        // Créer l'objet de réservation
        const reservation = {
            clientId,
            chambreId,
            dateArrivee,
            dateDepart,
            typePaiement,
            nombrePersonnes,
            montanttotal: 0 // Le montant sera calculé côté serveur
        };

        try {
            // Soumettre la réservation au serveur
            const response = await fetch("/api/Reservation/AddReservation", {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(reservation),
            });

            const messageElement = document.getElementById("message");

            if (response.ok) {
                messageElement.textContent = "Réservation ajoutée avec succès.";
                messageElement.style.color = "green";
                window.location.href = "reservations-list.html"; // Rediriger vers la liste des réservations
            } else {
                const errorText = await response.text();
                messageElement.textContent = errorText;
                messageElement.style.color = "red";
            }
        } catch (error) {
            console.error("Erreur:", error);
        }
    });
});
