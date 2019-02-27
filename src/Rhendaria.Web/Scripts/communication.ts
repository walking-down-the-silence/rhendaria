/// <reference types="@aspnet/signalr" />

/**
 * game communication with backend via websockets
 **/
class GameChannel {
    private connection: signalR.HubConnection;
    private game: Game;

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

    setGame(game: Game) {
        this.game = game;
        console.log(game, this.game);
    }

    private handleUpdatePosition(nickname: any, position: any) {
        // TODO: update player position
        console.log(nickname, position);
        this.game = this.game.updatePosition(nickname, position);
    }
}