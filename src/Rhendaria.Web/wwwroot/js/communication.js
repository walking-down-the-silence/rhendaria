/// <reference types="@aspnet/signalr" />
/**
 * game communication with backend via websockets
 **/
var GameChannel = /** @class */ (function () {
    function GameChannel() {
    }
    GameChannel.prototype.setupCommunicationChannel = function () {
        this.connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
        return this.connection.start().catch(function (err) { return console.error(err); });
    };
    GameChannel.prototype.teardownCommunicationChannel = function () {
        return this.connection.stop();
    };
    GameChannel.prototype.movePlayer = function (nickname, position) {
        return this.connection.invoke("MovePlayer", nickname, position).catch(function (err) { return console.error(err); });
    };
    GameChannel.prototype.onUpdatePosition = function (handleUpdatePosition) {
        this.connection.on("UpdatePosition", handleUpdatePosition);
    };
    return GameChannel;
}());
//# sourceMappingURL=communication.js.map