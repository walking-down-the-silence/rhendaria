/// <reference types="@aspnet/signalr" />

/**
 * game communication with backend via websockets
 */
let gameCommunication = (function () {
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

async function loadGame() {
    let response = await fetch("pavlo.hodysh", { method: "GET" })
        .then(result => result.json())
        .catch(error => console.log(error));

    var zone = Zone.create(
        Vector.create(response.zone.topLeftX, response.zone.topLeftY),
        Vector.create(response.zone.bottomRightX, response.zone.bottomRightY));
    var viewport = Viewport.create(12, 8);
    var sprites = response.sprites.map(sprite => {
        let position = Vector.create(sprite.positionX, sprite.positionY);
        return Sprite.create(sprite.nickname, position);
    });
    let player = Player.create(
        Sprite.create(
            response.player.nickname,
            Vector.create(
                response.player.positionX,
                response.player.positionY)));
    let game = Game.create(zone, viewport, player, sprites);

    return game;
};