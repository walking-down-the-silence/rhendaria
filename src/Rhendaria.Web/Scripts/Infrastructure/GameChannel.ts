/**
 * game communication with backend via websockets
 **/
import { Vector2D as Vector } from "../Geometry/Vector2D";
import * as signalR from "@aspnet/signalr";

export class GameChannel {
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