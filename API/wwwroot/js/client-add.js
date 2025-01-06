document.getElementById('addClientForm').addEventListener('submit', function (event) {
    event.preventDefault();

    const nom = document.getElementById('nom').value;
    const prenom = document.getElementById('prenom').value;
    const email = document.getElementById('email').value;
    const telephone = document.getElementById('telephone').value;

    const clientData = {
        nom: nom,
        prenom: prenom,
        email: email,
        telephone: telephone
    };

    fetch('/api/Client/AddClient', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(clientData)
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
            alert("Client ajouté avec succès !");
            window.location.href = 'clients-list.html'; // Redirect to client list
        })
        .catch(error => {
            if (error.message.includes("Cet email est déjà utilisé")) {
                alert("L'email est déjà utilisé par un autre client.");
            } else if (error.message.includes("Le numéro de téléphone doit contenir uniquement des chiffres")) {
                alert("Le numéro de téléphone doit contenir uniquement des chiffres.");
            } else {
                alert("Une erreur est survenue lors de l'ajout du client : " + error.message);
            }
        });
});


