document.getElementById('addPersonnelForm').addEventListener('submit', function (event) {
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

    fetch('api/Personnel/AddPersonnel', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(personnelData)
    })
        .then(response => {
            if (response.ok) {
                return response.text();
            } else {
                return response.text().then(errorText => {
                    throw new Error(errorText);
                });
            }
        })
        .then(data => {
            alert("Personnel ajouté avec succès!");
            window.location.href = 'personnel-list.html';
        })
        .catch(error => {
            if (error.message.includes("Cet email est déjà utilisé")) {
                alert("L'email est déjà utilisé par un autre personnel.");
            } else {
                alert("Une erreur est survenue lors de l'ajout du personnel : " + error.message);
            }
        });
});



