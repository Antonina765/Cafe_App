// Функция открытия модального окна для обновления названия
function showTitleModal(cafeId, currentTitle) {
    document.getElementById("titleCafeId").value = cafeId;
    document.getElementById("newTitle").value = currentTitle;
    document.getElementById("titleModal").style.display = "block";
}

// Функция закрытия окна обновления названия
function closeTitleModal() {
    document.getElementById("titleModal").style.display = "none";
}

// Функция открытия модального окна для обновления картинки
function showImageModal(cafeId) {
    document.getElementById("imageCafeId").value = cafeId;
    // Очищаем поля формы, если это нужно
    document.getElementById("imageFile").value = "";
    document.getElementById("imageUrl").value = "";
    document.getElementById("imageModal").style.display = "block";
}

// Функция закрытия окна обновления картинки
function closeImageModal() {
    document.getElementById("imageModal").style.display = "none";
}

// Закрываем модальное окно при клике вне его контента
window.onclick = function(event) {
    if (event.target.classList.contains("modal")) {
        event.target.style.display = "none";
    }
};


// Пример функции для получения более подробной информации (если требуется)
function moreInfo(cafeId) {
    window.location.href = '/Cafe/Details?id=' + cafeId;
}
