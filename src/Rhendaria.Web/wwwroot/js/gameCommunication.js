window.gameCommunication = (function() {
    let connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();


    connection.on("ReceiveMessage", function (user, message) {
        console.log(user + ':' + 'message');
    });

    connection.start().catch(function (err) {
        return console.error(err.toString());
    });

    function invoke(user,message) {
        connection.invoke("SendMessage", user, message).catch(err => console.error(err));
    }

    return {
        invoke
    };
})();