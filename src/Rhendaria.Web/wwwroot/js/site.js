window.app = (function () {
    const container = document.getElementById("game-field");
    window.gameOptions = {
        fullWidth: container.offsetWidth,
        fullHeight: container.offsetHeight
    };

    const app = new PIXI.Application({
        antialias: true,
        autoResize: true,
        resolution: devicePixelRatio
    });
    app.stage.interactive = true;
    app.renderer.resize(window.gameOptions.fullWidth, window.gameOptions.fullHeight);
    app.view.addEventListener("mousemove", e => {
        // TODO: get relative coordinates
        window.mouse = {
            position: new Offset(e.clientX - container.offsetLeft, e.clientY - container.offsetTop)
        };
    });

    container.appendChild(app.view);

    return app;
})();

// set up a sprite for player in form of a circle
window.game = (function () {
    const centerX = window.gameOptions.fullWidth / 2;
    const centerY = window.gameOptions.fullHeight / 2;

    const state = {
        player: new User("justmegaara", new Offset(centerX, centerY)),
        sprites: [
            new User("test player 1", new Offset(12, 3), 0xFF0000)
        ]
    };

    state.sprites.forEach(player => window.app.stage.addChild(player.sprite));
    window.app.stage.addChild(state.player.sprite);

    // event subscriptions
    window.app.ticker.add(function () { /* move sprites here */ });
    window.addEventListener("resize", () => app.renderer.resize(window.innerWidth, window.innerHeight));

    return state;
})();

