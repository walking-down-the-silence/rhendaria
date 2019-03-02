/// <reference types="@aspnet/signalr" />

/**
 * game communication with backend via websockets
 **/
class GameChannel {
    private connection: signalR.HubConnection;

    setupCommunicationChannel() {
        this.connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
        return this.connection.start().catch((err) => console.error(err));
    }

    teardownCommunicationChannel() {
        return this.connection.stop();
    }

    movePlayer(nickname: string, direction: Vector) {
        return this.connection.invoke("MovePlayer", nickname, direction).catch(err => console.error(err));
    }

    onUpdatePosition(handleUpdatePosition: (nickname: any, position: any) => void) {
        this.connection.on("UpdatePosition", handleUpdatePosition);
    }
}