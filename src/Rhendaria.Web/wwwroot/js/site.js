// set up the stage element
window.app = (function () {
    const app = new PIXI.Application({
        antialias: true,
        autoResize: true,
        resolution: devicePixelRatio
    });

    const container = document.getElementById("game-field");

    app.stage.interactive = true;
    console.log(`${container.offsetWidth}, ${container.offsetHeight}`);
    app.renderer.resize(container.clientWidth, container.clientHeight);
    app.view.addEventListener("mousemove", e => {
        // TODO: get relative coordinates
        window.mouse.position = new Offset(e.clientX, e.clientY);
    });

    container.appendChild(app.view);

    return app;
})();

// set up a sprite for player in form of a circle
window.game = (function () {
    const state = {
        player: new Player("justmegaara", new Offset(window.innerWidth / 2, window.innerHeight / 2)),
        sprites: [
            new Player("test player 1", createRandomPosition(window.innerWidth, window.innerHeight)),
            new Player("test player 2", createRandomPosition(window.innerWidth, window.innerHeight)),
            new Player("test player 3", createRandomPosition(window.innerWidth, window.innerHeight)),
            new Player("test player 4", createRandomPosition(window.innerWidth, window.innerHeight))
        ]
    };

    state.sprites.forEach(player => window.app.stage.addChild(player.sprite));
    window.app.stage.addChild(state.player.sprite);
    window.app.ticker.add(moveWorldRelativeToPlayer);

    return state;
})();

window.mouse = {
    position: new Offset(0, 0)
}

// event subscriptions
window.addEventListener("resize", () => {
    app.renderer.resize(window.innerWidth, window.innerHeight);
});

function moveWorldRelativeToPlayer() {
    window.game.sprites.forEach(player => {
        const target = window.mouse.position;
        const negative = target.negate();
        player.move(negative);
    });
}

function createRandomPosition(widthMax, heightMax) {
    const x = Math.floor(Math.random() * widthMax);
    const y = Math.floor(Math.random() * heightMax);
    return new Offset(x, y);
}
