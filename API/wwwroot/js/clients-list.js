// Fonction pour récupérer la liste des clients depuis l'API
async function fetchClients() {
    console.log("Fetching clients..."); // Debugging line
    try {
        const response = await fetch('/api/Client/GetClients');
        console.log(response); // Check the response object
        if (response.ok) {
            const clients = await response.json();
            console.log(clients); // Log the clients data
            const tbody = document.getElementById('clientTableBody');
            tbody.innerHTML = ''; // Effacer les anciennes lignes avant d'ajouter de nouvelles données

            // Ajouter chaque client à la table
            clients.forEach(client => {
                const row = document.createElement('tr');
                row.setAttribute('data-id', client.id); // Ajouter un attribut pour l'ID

                row.innerHTML = `
                        <td>${client.id}</td>
                        <td>${client.nom}</td>
                        <td>${client.prenom}</td>
                        <td>${client.email}</td>
                        <td>${client.telephone}</td>
                        <td>
                            <a href="client-edit.html?id=${client.id}">Modifier</a>
                            <a href="#" class="delete" data-id="${client.id}">Supprimer</a>
                        </td>
                    `;

                tbody.appendChild(row);
            });

            // Ajouter les événements de suppression après l'ajout des lignes
            const deleteButtons = document.querySelectorAll('.delete');
            deleteButtons.forEach(button => {
                button.addEventListener('click', function (event) {
                    event.preventDefault(); // Prevent the default link behavior
                    const clientId = button.getAttribute('data-id');
                    if (confirm('Êtes-vous sûr de vouloir supprimer ce client ?')) {
                        deleteClient(clientId);
                    }
                });
            });
        } else {
            console.error('Erreur lors de la récupération des clients');
        }
    } catch (error) {
        console.error('Erreur:', error);
    }
}

// Fonction pour supprimer un client
async function deleteClient(id) {
    try {
        const response = await fetch(`/api/Client/DeleteClient/${id}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            alert('Client supprimé avec succès!');
            // Recharger la liste après suppression
            fetchClients();
        } else {
            console.error('Erreur lors de la suppression du client');
            alert('Une erreur est survenue lors de la suppression du client.');
        }
    } catch (error) {
        console.error('Erreur:', error);
        alert('Une erreur est survenue lors de la suppression du client.');
    }
}

// Charger les clients lors du chargement de la page
window.onload = fetchClients;

