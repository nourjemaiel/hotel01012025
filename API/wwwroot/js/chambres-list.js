// Fonction pour récupérer la liste des chambres depuis l'API
async function fetchChambres() {
    console.log("Fetching chambres..."); // Debugging line
    try {
        const response = await fetch('/api/Chambre/GetChambres');
        console.log(response); // Check the response object
        if (response.ok) {
            const chambres = await response.json();
            console.log(chambres); // Log the chambres data
            const tbody = document.getElementById('chambreTableBody');
            tbody.innerHTML = ''; // Effacer les anciennes lignes avant d'ajouter de nouvelles données

            // Ajouter chaque chambre à la table
            chambres.forEach(chambre => {
                const row = document.createElement('tr');
                row.setAttribute('data-id', chambre.id); // Ajouter un attribut pour l'ID

                row.innerHTML = `
                        <td>${chambre.numero}</td>
                        <td>${chambre.type}</td>
                        <td>${chambre.capacite}</td>
                        <td>${chambre.tarifjournalier}</td>
                        <td>
                            <a href="chambre-edit.html?id=${chambre.id}">Modifier</a>
                            <a href="#" class="delete" data-id="${chambre.id}">Supprimer</a>
                        </td>
                    `;

                tbody.appendChild(row);
            });

            // Ajouter les événements de suppression après l'ajout des lignes
            const deleteButtons = document.querySelectorAll('.delete');
            deleteButtons.forEach(button => {
                button.addEventListener('click', function (event) {
                    event.preventDefault(); // Prevent the default link behavior
                    const chambreId = button.getAttribute('data-id');
                    if (confirm('Êtes-vous sûr de vouloir supprimer cette chambre ?')) {
                        deleteChambre(chambreId);
                    }
                });
            });
        } else {
            console.error('Erreur lors de la récupération des chambres');
        }
    } catch (error) {
        console.error('Erreur:', error);
    }
}

// Fonction pour supprimer une chambre
async function deleteChambre(id) {
    try {
        const response = await fetch(`/api/Chambre/DeleteChambre/${id}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            alert('Chambre supprimée avec succès!');
            // Recharger la liste après suppression
            fetchChambres();
        } else {
            console.error('Erreur lors de la suppression de la chambre');
            alert('Une erreur est survenue lors de la suppression de la chambre.');
        }
    } catch (error) {
        console.error('Erreur:', error);
        alert('Une erreur est survenue lors de la suppression de la chambre.');
    }
}

// Charger les chambres lors du chargement de la page
window.onload = fetchChambres;


