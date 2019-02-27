/// <reference path="../node_modules/@types/pixi.js/index.d.ts" />

/**
 * game view setup and initialization
 * */
async function initializeGame(nickname: string) {
    let url = `http://localhost:59023/api/player/${nickname}`;
    let response = await fetch(url, { method: "GET" })
        .then(result => result.json())
        .catch(error => console.log(error));

    let zone = Zone.fromRaw(response.zone);
    let viewport = Viewport.create(12, 8);
    let player = Player.create(Sprite.fromRaw(response.player));
    let sprites = response.sprites ? response.sprites.map(sprite => Sprite.fromRaw(sprite)) : [];

    let game = Game.create(zone, viewport, player, sprites);
    console.log(this.game);
    return game;
}

let app = (async function () {
    const container = document.getElementById("game-field");
    let gameOptions = {
        fullWidth: container.offsetWidth,
        fullHeight: container.offsetHeight
    };

    let gameChannel = new GameChannel();
    await gameChannel.setupCommunicationChannel();

    let game = await initializeGame("aaaaa");

    const app = new PIXI.Application({
        antialias: true,
        autoResize: true,
        resolution: devicePixelRatio
    });
    app.stage.interactive = true;
    app.renderer.resize(gameOptions.fullWidth, gameOptions.fullHeight);
    app.view.addEventListener("mousemove", e => {
        // TODO: get relative coordinates
        let mouse = {
            position: Vector.create(e.clientX - container.offsetLeft, e.clientY - container.offsetTop)
        };
    });

    container.appendChild(app.view);
    //game.sprites
    //    .concat(game.player.sprite)
    //    .forEach(sprite => app.stage.addChild(sprite.view));

    // set up a sprite for player in form of a circle
    const centerX = gameOptions.fullWidth / 2;
    const centerY = gameOptions.fullHeight / 2;

    // event subscriptions
    app.ticker.add(function () { /* move sprites here */ });
    window.addEventListener("resize", () => app.renderer.resize(window.innerWidth, window.innerHeight));

    return app;
})();
