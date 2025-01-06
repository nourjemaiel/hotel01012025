document.getElementById("addChambreForm").addEventListener("submit", async (event) => {
    event.preventDefault();

    const numero = document.getElementById("numero").value;
    const type = document.getElementById("type").value;
    const capacite = document.getElementById("capacite").value;
    const tarifjournalier = document.getElementById("tarifjournalier").value;

    // Construct the chambre object to send
    const chambre = { numero, type, capacite, tarifjournalier };

    try {
        const response = await fetch('/api/Chambre/AddChambre', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(chambre),
        });

        const messageElement = document.getElementById("message");

        if (response.ok) {
            messageElement.textContent = "Chambre ajoutée avec succès.";
            messageElement.style.color = "green";
            // Redirect to chambres list page after successful addition
            window.location.href = "chambres-list.html";
        } else if (response.status === 400) {
            const error = await response.text();
            messageElement.textContent = error;
            messageElement.style.color = "red";
        } else {
            messageElement.textContent = "Erreur lors de l'ajout de la chambre.";
            messageElement.style.color = "red";
        }
    } catch (error) {
        console.error("Erreur:", error);
        document.getElementById("message").textContent = "Une erreur est survenue.";
        document.getElementById("message").style.color = "red";
    }
});
