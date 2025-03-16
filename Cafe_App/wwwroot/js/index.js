// Функции для показа/скрытия модального окна обновления названия
function openUpdateTitleModal(cafeId, currentTitle) {
    $('#titleCafeId').val(cafeId);
    $('#newTitle').val(currentTitle);
    $('#updateTitleModal').fadeIn();
}

function closeUpdateTitleModal() {
    $('#updateTitleModal').fadeOut();
}

// Функции для показа/скрытия модального окна обновления картинки
function openUpdateImageModal(cafeId) {
    $('#imageCafeId').val(cafeId);
    $('#imageFile').val('');
    $('#imageUrl').val('');
    $('#updateImageModal').fadeIn();
}

function closeUpdateImageModal() {
    $('#updateImageModal').fadeOut();
}

// Обработка формы обновления названия (AJAX)
$('#updateTitleForm').submit(function (e) {
    e.preventDefault();

    var cafeId = $('#titleCafeId').val();
    var newTitle = $('#newTitle').val();
    console.log("Отправляем обновление: cafeId=", cafeId, "newTitle=", newTitle);
    
    $.ajax({
        url: '/Cafe/UpdateTitle', // Замените на актуальный URL вашего контроллера
        type: 'POST',
        data: { id: cafeId, newTitle: newTitle },
        success: function () {
            // Обновляем название в DOM для соответствующего кафе
            $('div.cafe-container').each(function () {
                var container = $(this);
                if (container.data('cafeid') == cafeId) {
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

// Обработка формы обновления картинки (AJAX)
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
        url: '/Cafe/UpdateImage', // Замените на актуальный URL вашего контроллера
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function () {
            // Обновляем источник картинки в DOM для выбранного кафе
            $('div.cafe-container').each(function () {
                var container = $(this);
                if (container.data('cafeid') === cafeId) {
                    // Если при обновлении введён URL, используем его; иначе можно сохранять предыдущий источник
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

// Метод удаления (оставляем без изменений)
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
