function updateCafeImage(cafeId) {
    const url = `/Cafe/UpdateImage/${cafeId}`;
    window.location.href = url;
}

function updateCafeTitle(cafeId) {
    const url = `/Cafe/UpdateTitle/${cafeId}`;
    window.location.href = url;
}

function deleteCafe(cafeId) {
    if (confirm("Are you sure you want to delete this cafe?")) {
        const url = `/Cafe/Remove/${cafeId}`;
        fetch(url, { method: 'POST' })
            .then(response => {
                if (response.ok) {
                    alert("Cafe deleted successfully");
                    location.reload();
                } else {
                    alert("Failed to delete cafe");
                }
            });
    }
}
