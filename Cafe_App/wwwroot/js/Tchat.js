(function () {
    // Функция для добавления нового сообщения в область чата.
    function appendMessage(message) {
        var messageBlock = $(".message.template").clone();
        messageBlock.removeClass("template");
        messageBlock.text(message);
        $(".messages").append(messageBlock);
        // Автоматическая прокрутка к последнему сообщению.
        $(".chat-messages").scrollTop($(".chat-messages")[0].scrollHeight);
    }

    // Функция для реализации long polling.
    function longPoll() {
        $.ajax({
            url: '/api/chat/poll',
            type: 'GET',
            timeout: 30000, // Максимальное ожидание 30 секунд.
            success: function (message) {
                // Если получено новое сообщение, выводим его.
                if (message) {
                    appendMessage(message);
                }
                // После успешного завершения запускаем новый запрос.
                longPoll();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                // При ошибке или таймауте пробуем снова через секунду.
                setTimeout(longPoll, 1000);
            }
        });
    }

    // Функция для отправки нового сообщения.
    function sendMessage() {
        var message = $(".new-message").val().trim();
        if (!message) return;

        var userName = $(".user-name").val();

        $.ajax({
            url: '/api/chat/send',
            type: 'POST',
            data: {
                message: message,
                userName: userName
            },
            success: function () {
                $(".new-message").val("");
            },
            error: function (err) {
                console.error("Ошибка при отправке сообщения:", err);
            }
        });
    }

    // Инициализация после загрузки документа.
    $(document).ready(function () {
        // Подгружаем историю сообщений (опционально).
        $.get("/api/chat/last")
            .done(function (messages) {
                messages.forEach(function (msg) {
                    appendMessage(msg);
                });
            })
            .fail(function (err) {
                console.error("Ошибка при получении истории:", err);
            });

        // Запускаем long polling.
        longPoll();

        // Обработчик клика по кнопке отправки.
        $(".send-new-message").click(function () {
            sendMessage();
        });

        // Отправка сообщения по нажатию Enter.
        $(".new-message").on("keyup", function (e) {
            if (e.which === 13) {
                sendMessage();
            }
        });
    });
})();
