/**
 * game view setup and initialization
 **/

import { Game } from "./Game";
import { GameChannel } from "./Infrastructure/GameChannel";
import { Vector2D as Vector } from "./Geometry/Vector2D";
import { getGameInfo } from "./Infrastructure/Api/getGameInfo";


class GameApplication {
    async main() {
        const initResult = await this.preInit();
       
        game = Game.fromGameMeta(initResult.gameMeta);
        game.changeViewport(initResult.$gameField.offsetWidth, initResult.$gameField.offsetHeight);

        const gameChannel = new GameChannel();
        await gameChannel.setupCommunicationChannel();
        gameChannel.onUpdatePosition((nickname: any, event: any) => {
            const actual = Vector.fromRaw(event.position);
            game.updatePosition(nickname, actual);
        });

        // Events from User.
        window.addEventListener("resize", () => this.resizeRenderingViewport(initResult.pixi.renderer));

        initResult.pixi.view.addEventListener("mousedown", event => {
            const player = game.findPlayerSprite();
            const fieldOffset = Vector.create(initResult.$gameField.offsetLeft, initResult.$gameField.offsetTop);
            const mouseOffset = Vector.create(event.clientX, event.clientY);
            const offset = mouseOffset
                .subtract(player.relative)
                .subtract(fieldOffset);
            const direction = player.actual.add(offset);
            gameChannel.movePlayer(player.nickname, direction);
        });

        this.resizeRenderingViewport(initResult.pixi.renderer);
        initResult.$gameField.appendChild(initResult.pixi.view);
    }

    async preInit() {
        const app = new PIXI.Application({
            antialias: true,
            autoResize: true,
            resolution: devicePixelRatio
        });
        app.stage.interactive = true;

        const nickname = this.getNicknameFromUrl();
        const $gameField = document.getElementById("game-field");

        const response = await getGameInfo(nickname);

        return {
            pixi: app,
            nickname,
            $gameField,
            gameMeta : response
        };
    }
    
    resizeRenderingViewport(renderer: PIXI.WebGLRenderer | PIXI.CanvasRenderer) {
        const container = document.getElementById("game-field");
        renderer.resize(container.offsetWidth, container.offsetHeight);
    }   

    getNicknameFromUrl() {
        // if the query string is NULL
        const queryString = window.location.search.substring(1);
        const queries = queryString.split("&");
        let value = "";

        queries.forEach((indexQuery: string) => {
            var indexPair = indexQuery.split("=");

            var queryKey = decodeURIComponent(indexPair[0]);
            var queryValue = decodeURIComponent(indexPair.length > 1 ? indexPair[1] : "");

            if (queryKey === "nickname") {
                value = queryValue;
            }
        });

        return value;
    }
}

// entry point.
new GameApplication().main();



let game: Game = null;
