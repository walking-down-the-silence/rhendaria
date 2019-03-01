/// <reference path="../node_modules/@types/pixi.js/index.d.ts" />

/**
 * game view setup and initialization
 **/
async function loadGameView(nickname: string) {
    const url = `http://localhost:54016/api/player/${nickname}`;
    return fetch(url, { method: "GET" })
        .then(result => result.json())
        .catch(error => console.log(error));
}

function resizeRenderingViewport(renderer: PIXI.WebGLRenderer | PIXI.CanvasRenderer) {
    const container = document.getElementById("game-field");
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
    app.ticker.add(() => { /* move sprites here */ });
    resizeRenderingViewport(app.renderer);

    const container = document.getElementById("game-field");
    container.appendChild(app.view);

    const response = await loadGameView("justmegaara");
    game = Game.fromRaw(response);
    game.changeViewport(container.offsetWidth, container.offsetHeight);
    game.sprites.forEach(sprite => game.updatePosition(sprite.nickname, sprite.actual));
    game.sprites.forEach(sprite => app.stage.addChild(sprite.view));

    const gameChannel = new GameChannel();
    await gameChannel.setupCommunicationChannel();
    gameChannel.onUpdatePosition((nickname: any, position: any) => {
        const actual = Vector.fromRaw(position);
        game.updatePosition(nickname, actual);
        console.log(game);
    });

    // event subscriptions
    window.addEventListener("resize", () => resizeRenderingViewport(app.renderer));

    return app;
})();
