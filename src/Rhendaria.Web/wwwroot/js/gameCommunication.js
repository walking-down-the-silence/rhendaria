window.gameCommunication = (function() {
    let connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

    connection.start().catch(function (err) {
        return console.error(err.toString());
    });

    function move(user, direction) {
        connection.invoke("Move", user, direction).catch(err => console.error(err));
    }

    function setOnMove(callback) {
        connection.on("OnMove",callback);
    }

    return {
        move,
        setOnMove
    };
})();