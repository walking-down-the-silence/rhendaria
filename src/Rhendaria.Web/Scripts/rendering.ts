/// <reference path="../node_modules/@types/pixi.js/index.d.ts" />

/**
 * game view setup and initialization
 **/
async function loadGameView(nickname: string) {
    let url = `http://localhost:59023/api/player/${nickname}`;
    return fetch(url, { method: "GET" })
        .then(result => result.json())
        .catch(error => console.log(error));
}

function resizeRenderingViewport(renderer: PIXI.WebGLRenderer | PIXI.CanvasRenderer) {
    const container = document.getElementById("game-field");
    console.log(container.offsetWidth, container.offsetHeight);
    renderer.resize(container.offsetWidth, container.offsetHeight);
}

const container = document.getElementById("game-field");

let game: Game = null;

let app = (async function () {
    const app = new PIXI.Application({
        antialias: true,
        autoResize: true,
        resolution: devicePixelRatio
    });
    app.stage.interactive = true;
    app.view.addEventListener("mousemove", e => {
        // TODO: get relative coordinates
        //let mouse = {
        //    position: Vector.create(e.clientX - container.offsetLeft, e.clientY - container.offsetTop)
        //};
    });
    resizeRenderingViewport(app.renderer);

    const container = document.getElementById("game-field");
    container.appendChild(app.view);

    game = await loadGameView("justmegaara")
        .then(raw => Game.fromRaw(raw))
        .then(game => game.changeViewport(container.offsetWidth, container.offsetHeight));
    game.sprites
        .concat(game.player.sprite)
        .forEach(sprite => app.stage.addChild(sprite.view));


    let gameChannel = new GameChannel();
    await gameChannel.setupCommunicationChannel();

    gameChannel.onUpdatePosition((nickname: any, position: any) => {
        console.log(position);
        game.updatePosition(nickname, position);
    });

    // event subscriptions
    app.ticker.add(function () { /* move sprites here */ });
    window.addEventListener("resize", () => resizeRenderingViewport(app.renderer));

    return app;
})();
