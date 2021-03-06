/// <reference path="../node_modules/@types/pixi.js/index.d.ts" />

/**
 * game view setup and initialization
 **/

import { Game } from "./game";
import { GameChannel } from "./GameChannel";
import { Vector } from "./Vector";

async function loadGameView(nickname: string) {
    const url = `api/player/${nickname}`;
    return fetch(url, { method: "GET" })
        .then(result => result.json())
        .catch(error => console.log(error));
}

function parseNickname() {
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

function resizeRenderingViewport(renderer: PIXI.WebGLRenderer | PIXI.CanvasRenderer) {
    const container = document.getElementById("game-field");
    renderer.resize(container.offsetWidth, container.offsetHeight);
}

let game: Game = null;

let app = (async function () {
    const app = new PIXI.Application({
        antialias: true,
        autoResize: true,
        resolution: devicePixelRatio
    });
    app.stage.interactive = true;

    const nickname = parseNickname();
    const gameField = document.getElementById("game-field");
    const response = await loadGameView(nickname);

    game = Game.fromRaw(response);
    game.changeViewport(gameField.offsetWidth, gameField.offsetHeight);
    app.stage.addChild(game.viewport.backSprite);
    game.sprites.forEach(sprite => game.updatePosition(sprite.nickname, sprite.actual));
    game.sprites.forEach(sprite => app.stage.addChild(sprite.view));

    const gameChannel = new GameChannel();
    await gameChannel.setupCommunicationChannel();
    gameChannel.onUpdatePosition((nickname: any, event: any) => {
        const actual = Vector.fromRaw(event.position);
        game.updatePosition(nickname, actual);
    });

    // event subscriptions
    window.addEventListener("resize", () => resizeRenderingViewport(app.renderer));
    app.view.addEventListener("mousedown", event => {
        const player = game.findPlayerSprite();
        const fieldOffset = Vector.create(gameField.offsetLeft, gameField.offsetTop);
        const mouseOffset = Vector.create(event.clientX, event.clientY);
        const offset = mouseOffset
            .subtract(player.relative)
            .subtract(fieldOffset);
        const direction = player.actual.add(offset);
        gameChannel.movePlayer(player.nickname, direction);
    });

    resizeRenderingViewport(app.renderer);
    gameField.appendChild(app.view);

    return app;
})();
