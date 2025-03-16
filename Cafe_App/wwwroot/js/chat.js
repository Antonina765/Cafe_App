$(document).ready(function () {
    const baseUrl = `https://localhost:7292`;

    const authorId = $(".user-id").val() - 0;
    const authorName = $(".user-name").val();

    const hub = new signalR.HubConnectionBuilder()
        .withUrl(baseUrl + "/hub/chat")
        .build();

    init();

    hub.on("newMessageAdded", createNewMessage);

    $(".send-new-message").click(sendMessage);

    $(".new-message").on("keyup", function (e) {
        if (e.which == 13) {
            // 13 is a code of Enter key
            sendMessage();
        }
    });

    function sendMessage() {
        const message = $(".new-message").val();
        hub.invoke("addNewMessage", authorId, authorName, message);
        $(".new-message").val("");
    }

    hub.start().then(function () {
        // When connection with server is alive
        hub.invoke("userEnteredToChat", authorId, authorName);
    });

    //TODO refactor the method. Get data from new minimal api server
    function init() {
        const url = "/api/ApiChat/GetLastMessages";
        $.get(url).then(function (messages) {
            messages.forEach((message) => createNewMessage(message));
        });

        setTimeout(() => {
            $(".joke").show();
        }, 3 * 1000);
    }

    function createNewMessage(message) {
        const messageBlock = $(".message.template").clone();
        messageBlock.removeClass("template");
        messageBlock.text(message);
        $(".messages").append(messageBlock);
    }
});
