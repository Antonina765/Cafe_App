// Превью по URL
function previewByUrl() {
    const imageUrl = document.getElementById("imageSrc").value;
    const imagePreview = document.getElementById("imagePreview");

    if (imageUrl) {
        imagePreview.innerHTML = `<img src="${imageUrl}" alt="Image Preview" style="max-width: 300px; max-height: 200px;" />`;
    } else {
        imagePreview.innerHTML = `<p>No image selected.</p>`;
    }
}

// Превью загружаемого файла
function previewByFile() {
    const fileInput = document.getElementById("imageFile");
    const imagePreview = document.getElementById("imagePreview");

    // Проверяем, что файл загружен
    if (fileInput.files && fileInput.files[0]) {
        const reader = new FileReader();

        reader.onload = function (e) {
            imagePreview.innerHTML = `<img src="${e.target.result}" alt="Image Preview" style="max-width: 300px; max-height: 200px;" />`;
        };

        reader.readAsDataURL(fileInput.files[0]); // Читаем файл как Data URL
    } else {
        imagePreview.innerHTML = `<p>No image selected.</p>`;
    }
}
