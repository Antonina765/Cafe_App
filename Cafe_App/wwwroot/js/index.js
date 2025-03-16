$(document).ready(function () {
    // Функции управления модальным окном обновления названия
    function openUpdateTitleModal(cafeId, currentTitle) {
        $('#titleCafeId').val(cafeId);
        $('#newTitle').val(currentTitle);
        $('#updateTitleModal').fadeIn();
    }

    function closeUpdateTitleModal() {
        $('#updateTitleModal').fadeOut();
    }

    // Функции управления модальным окном обновления картинки
    function openUpdateImageModal(cafeId) {
        $('#imageCafeId').val(cafeId);
        $('#imageFile').val('');
        $('#imageUrl').val('');
        $('#updateImageModal').fadeIn();
    }

    function closeUpdateImageModal() {
        $('#updateImageModal').fadeOut();
    }

    // Экспортируем функции в глобальную область, чтобы их можно было вызвать из HTML (onclick)
    window.openUpdateTitleModal = openUpdateTitleModal;
    window.closeUpdateTitleModal = closeUpdateTitleModal;
    window.openUpdateImageModal = openUpdateImageModal;
    window.closeUpdateImageModal = closeUpdateImageModal;

    // Обработка формы обновления названия через AJAX
    $('#updateTitleForm').submit(function (e) {
        e.preventDefault();

        var cafeId = $('#titleCafeId').val();
        var newTitle = $('#newTitle').val();
        console.log("Отправляем обновление: cafeId =", cafeId, "newTitle =", newTitle);

        $.ajax({
            url: '/Cafe/UpdateTitle',
            type: 'POST',
            data: {
                id: cafeId,
                newTitle: newTitle
            },
            success: function () {
                // Обновляем название в DOM для соответствующего кафе
                $('div.cafe-container').each(function () {
                    var container = $(this);
                    if (container.data('cafeid') == cafeId) { // использование нестрогого сравнения
                        container.find('.cafe-title').text(newTitle);
                    }
                });
                closeUpdateTitleModal();
            },
            error: function (xhr) {
                alert('Ошибка: ' + xhr.responseText);
            }
        });
    });

    // Обработка формы обновления картинки через AJAX
    $('#updateImageForm').submit(function (e) {
        e.preventDefault();

        var cafeId = $('#imageCafeId').val();
        var formData = new FormData();
        formData.append('id', cafeId);

        var fileInput = $('#imageFile')[0];
        if (fileInput.files.length > 0) {
            formData.append('imageFile', fileInput.files[0]);
        }

        var imageUrl = $('#imageUrl').val();
        formData.append('url', imageUrl);

        $.ajax({
            url: '/Cafe/UpdateImage',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function () {
                // Обновляем источник картинки в DOM для выбранного кафе
                $('div.cafe-container').each(function () {
                    var container = $(this);
                    if (container.data('cafeid') == cafeId) {
                        // Если введён URL – обновляем картинку им, иначе оставляем прежний источник
                        var newSrc = imageUrl ? imageUrl : container.find('.cafe-image').attr('src');
                        container.find('.cafe-image').attr('src', newSrc);
                    }
                });
                closeUpdateImageModal();
            },
            error: function (xhr) {
                alert('Ошибка: ' + xhr.responseText);
            }
        });
    });
});

// Функция удаления кафе (вызывается через onclick из HTML)
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
            })
            .catch(error => {
                console.error("Ошибка при удалении кафе:", error);
                alert("Произошла ошибка при удалении кафе.");
            });
    }
}
