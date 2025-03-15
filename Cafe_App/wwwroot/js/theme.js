$(document).ready(function () {
    // Обработчик клика по кнопкам переключения темы
    $(".theme-toggle").click(function () {
        // Считываем выбранную тему через data-атрибут
        var theme = $(this).data("theme");
        console.log("Selected theme:", theme);
        eraseCookie("theme");
        setCookie("theme", theme);
        // Перезагружаем страницу для применения новоустановленной темы
        location.reload();
    });

    // Функция установки cookie
    function setCookie(name, value) {
        document.cookie = name + "=" + (value || "") + "; path=/";
    }

    // Функция чтения cookie
    function getCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(";");
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i].trim();
            if (c.indexOf(nameEQ) === 0)
                return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    // Функция для удаления cookie
    function eraseCookie(name) {
        document.cookie = name + "=; path=/; expires=Thu, 01 Jan 1970 00:00:01 GMT;";
    }

    // При загрузке страницы читаем cookie "theme" и обновляем ссылку на группы стилей
    var currentTheme = getCookie("theme");
    if (currentTheme) {
        document.documentElement.setAttribute("data-theme", currentTheme);
        if (currentTheme === "dark") {
            $('#themeStylesheet').attr('href', window.themeUrls.dark);
        } else {
            $('#themeStylesheet').attr('href', window.themeUrls.light);
        }
    } else {
        $('#themeStylesheet').attr('href', window.themeUrls.light);
    }
});
