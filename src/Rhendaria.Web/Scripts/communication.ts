/// <reference types="@aspnet/signalr" />

/**
 * game communication with backend via websockets
 */
class GameChannel {
    private connection: signalR.HubConnection;

    setupCommunicationChannel() {
        this.connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
        this.connection.on("UpdatePosition", this.handleUpdatePosition);
        return this.connection.start().catch((err) => console.error(err));
    }

    teardownCommunicationChannel() {
        return this.connection.stop();
    }

    movePlayer(nickname: string, position: Vector) {
        return this.connection.invoke("MovePlayer", nickname, position).catch(err => console.error(err));
    }

    private handleUpdatePosition(nickname: any, message: any) {
        // TODO: update player position
        console.log(nickname, message);
    }
}