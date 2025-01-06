document.addEventListener("DOMContentLoaded", async () => {
    // Get client ID from the query parameters
    const urlParams = new URLSearchParams(window.location.search);
    const clientId = urlParams.get("id");

    if (!clientId) {
        alert("Client ID manquant.");
        return;
    }

    // Fetch client data to pre-populate the form
    const response = await fetch(`/api/Client/GetClientById/${clientId}`);
    if (response.ok) {
        const client = await response.json();
        document.getElementById("nom").value = client.nom;
        document.getElementById("prenom").value = client.prenom;
        document.getElementById("email").value = client.email;
        document.getElementById("telephone").value = client.telephone;
    } else {
        alert("Erreur lors de la récupération des informations du client.");
    }

    // Handle form submission
    document.getElementById("editClientForm").addEventListener("submit", async (event) => {
        event.preventDefault();

        const nom = document.getElementById("nom").value;
        const prenom = document.getElementById("prenom").value;
        const email = document.getElementById("email").value;
        const telephone = document.getElementById("telephone").value;

        const client = { nom, prenom, email, telephone };

        try {
            const response = await fetch(`/api/Client/UpdateClient/${clientId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(client),
            });

            const messageElement = document.getElementById("message");

            if (response.ok) {
                messageElement.textContent = "Client mis à jour avec succès.";
                messageElement.style.color = "green";
                window.location.href = "clients-list.html"; // Redirect to clients list page
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
