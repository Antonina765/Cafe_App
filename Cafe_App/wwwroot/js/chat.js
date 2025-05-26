$(document).ready(function () {
    const baseUrl = `http://localhost:5131`;

    const authorId = $(".user-id").val() - 0;
    const authorName = $(".user-name").val();

    // Создание подключения к SignalR с автоматическим переподключением
    const hub = new signalR.HubConnectionBuilder()
        .withUrl(baseUrl + "/hub/chatMainPage")
        .withAutomaticReconnect()  // Автопереподключение при разрыве соединения
        .build();

    // Логирование событий соединения
    hub.onclose(() => console.warn("Соединение с хабом потеряно!"));
    hub.onreconnecting(() => console.warn("Переподключение к SignalR..."));
    hub.onreconnected(() => console.log("Соединение с SignalR восстановлено!"));

    init(); // Инициализация чата

    console.log("Чат загружен!");

    hub.on("newMessageAdded", createNewMessage);

    $(".send-new-message").click(sendMessage);

    $(".new-message").on("keyup", function (e) {
        if (e.which == 13) {
            sendMessage();
        }
    });

    async function sendMessage() {
        const message = $(".new-message").val();
        if (!message.trim()) return; // Игнорируем пустые сообщения

        if (hub.state !== signalR.HubConnectionState.Connected) {
            console.warn("⏳ Соединение не установлено, жду...");
            try {
                await hub.start(); // Пробуем подключиться перед отправкой
                console.log("Соединение установлено!");
            } catch (err) {
                console.error("Ошибка подключения к SignalR:", err);
                return;
            }
        }

        hub.invoke("AddNewMessage", message)
            .catch(err => console.error("Ошибка при отправке сообщения:", err));

        $(".new-message").val(""); // Очищаем поле ввода после отправки
    }

    async function startHubConnection() {
        try {
            await hub.start();
            console.log("Connected to SignalR hub.");
            hub.invoke("userEnteredToChat", authorId, authorName);
        } catch (err) {
            console.error("Ошибка подключения к SignalR:", err);
            setTimeout(startHubConnection, 5000); // Пытаемся снова через 5 сек
        }
    }

    startHubConnection(); // Запускаем подключение

    function init() {
        const url = "/api/ApiChat/GetLastMessages";
        $.get(url)
            .then(function (messages) {
                messages.forEach((message) => createNewMessage(message));
            })
            .catch(err => console.error("Ошибка при загрузке сообщений:", err));

        setTimeout(() => {
            $(".joke").show();
        }, 3000);
    }

    function createNewMessage(message) {
        const messageBlock = $(".message.template").clone();
        messageBlock.removeClass("template");
        messageBlock.text(message);
        $(".messages").append(messageBlock);
    }

    // Инициализация состояния переключателя из localStorage
    var toggleState = localStorage.getItem("toggleState");
    if (toggleState === "true") {
        $("#refreshSwitch").prop("checked", true);
        $("#switch-label").text("TCP чат");
    } else {
        $("#refreshSwitch").prop("checked", false);
        $("#switch-label").text("Чат");
    }

    // При изменении состояния переключателя сохраняем новое значение
    $("#refreshSwitch").change(function(){
        if ($(this).is(":checked")) {
            localStorage.setItem("toggleState", "true");
        } else {
            localStorage.setItem("toggleState", "false");
        }
        window.location.href = window.location.href; // страница перезагружается и включается другой чат
    });
});