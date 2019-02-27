/// <reference path="../node_modules/@types/pixi.js/index.d.ts" />

/**
 * game view setup and initialization
 * */
let gameOptions = {
    fullWidth: 0,
    fullHeight: 0
};

let mouse = {
    position: null
};

let app = (async function () {
    const container = document.getElementById("game-field");
    gameOptions = {
        fullWidth: container.offsetWidth,
        fullHeight: container.offsetHeight
    };

    const app = new PIXI.Application({
        antialias: true,
        autoResize: true,
        resolution: devicePixelRatio
    });
    app.stage.interactive = true;
    app.renderer.resize(gameOptions.fullWidth, gameOptions.fullHeight);
    app.view.addEventListener("mousemove", e => {
        // TODO: get relative coordinates
        mouse = {
            position: Vector.create(e.clientX - container.offsetLeft, e.clientY - container.offsetTop)
        };
    });

    container.appendChild(app.view);

    // set up a sprite for player in form of a circle
    const centerX = gameOptions.fullWidth / 2;
    const centerY = gameOptions.fullHeight / 2;

    //let game = await loadGame();
    //game.sprites.forEach(sprite => app.stage.addChild(sprite));
    //app.stage.addChild(game.player.sprite);

    // event subscriptions
    app.ticker.add(function () { /* move sprites here */ });
    window.addEventListener("resize", () => app.renderer.resize(window.innerWidth, window.innerHeight));

    return app;
})();
