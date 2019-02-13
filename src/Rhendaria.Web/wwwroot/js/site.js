// set up the stage element
window.gameOptions = {
    fullWidth: 0,
    fullHeight: 0
};

Object.defineProperty(window.gameOptions,
    "centerX",
    {
        get: () => window.gameOptions.fullWidth / 2
    });
Object.defineProperty(window.gameOptions,
    "centerY",
    {
        get: () => window.gameOptions.fullHeight / 2
    });

window.app = (function () {
    const app = new PIXI.Application({
        antialias: true,
        autoResize: true,
        resolution: devicePixelRatio
    });

    const gameOptions = window.gameOptions;
    const container = document.getElementById("game-field");

    app.stage.interactive = true;
    
    gameOptions.fullWidth = container.offsetWidth;
    gameOptions.fullHeight = container.offsetHeight;

    app.renderer.resize(gameOptions.fullWidth, gameOptions.fullHeight);
    app.view.addEventListener("mousemove", e => {
        // TODO: get relative coordinates
        window.mouse.position = new Offset(e.clientX - container.offsetLeft, e.clientY - container.offsetTop);
    });

    container.appendChild(app.view);

    return app;
})();

// set up a sprite for player in form of a circle
window.game = (function () {
    const gameOptions = window.gameOptions;
    const positionFactory = () => createRandomPosition(gameOptions.fullWidth, gameOptions.fullHeight);

    const state = {
        player: new Player("justmegaara", new Offset(gameOptions.centerX, gameOptions.centerY)),
        sprites: [
            new Player("test player 1", positionFactory(), 0xFF0000),
            //new Player("test player 2", positionFactory()),
            //new Player("test player 3", positionFactory()),
            //new Player("test player 4", positionFactory())
        ]
    };

    state.sprites.forEach(player => window.app.stage.addChild(player.sprite));
    window.app.stage.addChild(state.player.sprite);
    window.app.ticker.add(moveWorldRelativeToPlayer);

    return state;
})();

window.mouse = {
    position: new Offset(0, 0)
};

// event subscriptions
window.addEventListener("resize", () => {
    app.renderer.resize(window.innerWidth, window.innerHeight);
});

function moveWorldRelativeToPlayer() {
    const target = new Offset(window.mouse.position._x - window.gameOptions.centerX,
    window.mouse.position._y - window.gameOptions.centerY);
    const negative = target.negate();
    //console.log(negative);
    window.game.sprites.forEach(player => {
        player.move(negative);
    });
}

function createRandomPosition(widthMax, heightMax) {
    const x = Math.floor(Math.random() * widthMax);
    const y = Math.floor(Math.random() * heightMax);
    return new Offset(x, y);
}
