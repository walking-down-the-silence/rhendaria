window.gameCommunication = (function () {
    let connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

    // event subscriptions from backend server
    connection.on("UpdatePosition", function (nickname, message) {
        // TODO: update player position
        console.log(nickname, message);
    });
    
    connection.start().catch((err) => console.error(err));

    // action invocators for frontend client
    function movePlayer(nickname, position) {
        connection.invoke("MovePlayer", nickname, position).catch(err => console.error(err));
    }

    return {
        movePlayer
    };
})();