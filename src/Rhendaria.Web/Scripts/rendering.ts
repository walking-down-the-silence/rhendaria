/// <reference path="../node_modules/@types/pixi.js/index.d.ts" />

/**
 * game view setup and initialization
 **/
async function loadGameView(nickname: string) {
    const url = `http://localhost:59023/api/player/${nickname}`;
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

    const container = document.getElementById("game-field");
    const response = await loadGameView("justmegaara");
    game = Game.fromRaw(response);
    game.changeViewport(container.offsetWidth, container.offsetHeight);
    game.sprites.forEach(sprite => game.updatePosition(sprite.nickname, sprite.actual));
    game.sprites.forEach(sprite => app.stage.addChild(sprite.view));

    const gameChannel = new GameChannel();
    await gameChannel.setupCommunicationChannel();
    gameChannel.onUpdatePosition((nickname: any, event: any) => {
        const actual = Vector.fromRaw(event.position);
        game.updatePosition(nickname, actual);
        console.log(game);
    });

    app.view.addEventListener("mousemove", event => {
        // TODO: get relative coordinates
        const x = event.clientX - container.offsetLeft;
        const y = event.clientY - container.offsetTop;
        let mouse = { position: Vector.create(x, y) };
    });
    app.view.addEventListener("mouseup", event => {
        const x = event.clientX - container.offsetLeft;
        const y = event.clientY - container.offsetTop;
        const vector = Vector.create(x, y);
        gameChannel.movePlayer(game.player.nickname, vector);
    });
    app.ticker.add(() => { /* move sprites here */ });
    resizeRenderingViewport(app.renderer);
    
    container.appendChild(app.view);

    // event subscriptions
    window.addEventListener("resize", () => resizeRenderingViewport(app.renderer));

    return app;
})();
