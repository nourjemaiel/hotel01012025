// Récupérer l'ID du personnel depuis l'URL
const urlParams = new URLSearchParams(window.location.search);
const personnelId = urlParams.get('id');

// Vérifier si l'ID existe
if (!personnelId) {
    alert("Aucun ID de personnel fourni.");
    window.location.href = 'personnel-list.html';
}

// Charger les informations du personnel
fetch(`api/Personnel/GetPersonnelById/${personnelId}`)
    .then(response => {
        if (!response.ok) {
            throw new Error("Erreur lors de la récupération des informations du personnel.");
        }
        return response.json();
    })
    .then(personnel => {
        // Remplir les champs du formulaire avec les données du personnel
        document.getElementById('nom').value = personnel.nom;
        document.getElementById('prenom').value = personnel.prenom;
        document.getElementById('email').value = personnel.email;
        document.getElementById('motdepasse').value = personnel.motdepasse;
    })
    .catch(error => {
        console.error('Erreur:', error);
        alert("Impossible de charger les informations du personnel.");
        window.location.href = 'personnel-list.html';
    });

// Gestion de la soumission du formulaire
document.getElementById('editPersonnelForm').addEventListener('submit', function (event) {
    event.preventDefault();

    const nom = document.getElementById('nom').value;
    const prenom = document.getElementById('prenom').value;
    const email = document.getElementById('email').value;
    const motdepasse = document.getElementById('motdepasse').value;

    const personnelData = {
        nom: nom,
        prenom: prenom,
        email: email,
        motdepasse: motdepasse
    };

    fetch(`api/Personnel/UpdatePersonnel/${personnelId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(personnelData)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Erreur lors de la mise à jour du personnel.");
            }
            return response.text();
        })
        .then(data => {
            alert("Personnel modifié avec succès !");
            window.location.href = 'personnel-list.html'; // Redirige vers la liste des personnels
        })
        .catch(error => {
            console.error('Erreur:', error);
            alert("Une erreur est survenue lors de la modification du personnel.");
        });
});
