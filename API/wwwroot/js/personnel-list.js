// Fonction pour récupérer la liste des personnels depuis l'API
async function fetchPersonnels() {
    try {
        const response = await fetch('/api/Personnel/GetPersonnels');
        if (response.ok) {
            const personnels = await response.json();
            const tbody = document.getElementById('personnelTableBody');
            tbody.innerHTML = ''; // Effacer les anciennes lignes avant d'ajouter de nouvelles données

            // Ajouter chaque personnel à la table
            personnels.forEach(personnel => {
                const row = document.createElement('tr');
                row.setAttribute('data-id', personnel.id); // Ajouter un attribut pour l'ID

                row.innerHTML = `
                        <td>${personnel.id}</td>
                        <td>${personnel.nom}</td>
                        <td>${personnel.prenom}</td>
                        <td>${personnel.email}</td>
                        <td>
                            <a href="personnel-edit.html?id=${personnel.id}">Modifier</a>
                            <a href="#" class="delete" data-id="${personnel.id}">Supprimer</a>
                        </td>
                    `;

                tbody.appendChild(row);
            });

            // Ajouter les événements de suppression après l'ajout des lignes
            const deleteButtons = document.querySelectorAll('.delete');
            deleteButtons.forEach(button => {
                button.addEventListener('click', function (event) {
                    event.preventDefault(); // Prevent the default link behavior
                    const personnelId = button.getAttribute('data-id');
                    if (confirm('Êtes-vous sûr de vouloir supprimer ce personnel ?')) {
                        deletePersonnel(personnelId);
                    }
                });
            });
        } else {
            console.error('Erreur lors de la récupération des personnels');
        }
    } catch (error) {
        console.error('Erreur:', error);
    }
}

// Fonction pour supprimer un personnel
async function deletePersonnel(id) {
    try {
        const response = await fetch(`/api/Personnel/DeletePersonnel/${id}`, {
            method: 'DELETE',
        });

        if (response.ok) {
            alert('Personnel supprimé avec succès.');
            const row = document.querySelector(`tr[data-id='${id}']`);
            if (row) {
                row.remove(); // Retirer la ligne du tableau
            }
        } else {
            alert("Une erreur est survenue lors de la suppression du personnel.");
        }
    } catch (error) {
        console.error('Erreur:', error);
        alert("Une erreur est survenue lors de la suppression du personnel.");
    }
}

// Charger les personnels lors du chargement de la page
window.onload = fetchPersonnels;



