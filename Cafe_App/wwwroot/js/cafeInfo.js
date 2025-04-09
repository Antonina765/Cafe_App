function openBookingModal() {
    document.getElementById('bookingModal').style.display = 'block';
}
function closeBookingModal() {
    document.getElementById('bookingModal').style.display = 'none';
}

// Для закрытия модального окна при клике вне его содержимого (опционально)
window.onclick = function(event) {
    var modal = document.getElementById('bookingModal');
    if (event.target == modal) {
        closeBookingModal();
    }
}