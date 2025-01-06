document.addEventListener("DOMContentLoaded", async () => {
    // Get chambre ID from the query parameters
    const urlParams = new URLSearchParams(window.location.search);
    const chambreId = urlParams.get("id");

    if (!chambreId) {
        alert("Chambre ID manquant.");
        return;
    }

    // Fetch chambre data to pre-populate the form
    const response = await fetch(`/api/Chambre/GetChambreById/${chambreId}`);
    if (response.ok) {
        const chambre = await response.json();
        document.getElementById("numero").value = chambre.numero;
        document.getElementById("type").value = chambre.type;
        document.getElementById("capacite").value = chambre.capacite;
        document.getElementById("tarifjournalier").value = chambre.tarifjournalier;
    } else {
        alert("Erreur lors de la récupération des informations de la chambre.");
    }

    // Handle form submission
    document.getElementById("editChambreForm").addEventListener("submit", async (event) => {
        event.preventDefault();

        const numero = document.getElementById("numero").value;
        const type = document.getElementById("type").value;
        const capacite = document.getElementById("capacite").value;
        const tarifjournalier = document.getElementById("tarifjournalier").value;

        const chambre = { numero, type, capacite: parseInt(capacite), tarifjournalier: parseFloat(tarifjournalier) };

        try {
            const response = await fetch(`/api/Chambre/UpdateChambre/${chambreId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(chambre),
            });

            const messageElement = document.getElementById("message");

            if (response.ok) {
                messageElement.textContent = "Chambre mise à jour avec succès.";
                messageElement.style.color = "green";
                window.location.href = "chambres-list.html"; // Redirect to chambres list page
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

