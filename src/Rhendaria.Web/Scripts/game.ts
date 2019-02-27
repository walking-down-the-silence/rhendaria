import * as PIXI from "pixi.js"
import * as signalR from "@aspnet/signalr"

class Vector {
    private constructor(
        public readonly x: number,
        public readonly y: number) {
    }

    static create(x: number, y: number) {
        return new Vector(x, y);
    }

    add(vector: Vector) {
        let x = this.x + vector.x;
        let y = this.y + vector.y;
        return new Vector(x, y);
    }

    subtract(vector: Vector) {
        let x = this.x - vector.x;
        let y = this.y - vector.y;
        return new Vector(x, y);
    }

    multiply(scale: number) {
        let x = this.x * scale;
        let y = this.y * scale;
        return new Vector(x, y);
    }

    divide(scale: number) {
        let x = this.x / scale;
        let y = this.y / scale;
        return new Vector(x, y);
    }
}

class Rectangle {
    private constructor(
        public readonly topLeft: Vector,
        public readonly bottomRight: Vector) {
    }

    static create(topLeft: Vector, bottomRight: Vector) {
        return new Rectangle(topLeft, bottomRight);
    }
}

class Zone {
    private constructor(
        public readonly box: Rectangle) {
    }

    static create(topLeft: Vector, bottomRight: Vector) {
        let box = Rectangle.create(topLeft, bottomRight);
        return new Zone(box);
    }
}

class Viewport {
    private constructor(
        public readonly size: Vector) {
    }

    static create(width: number, height: number) {
        let size = Vector.create(width, height);
        return new Viewport(size);
    }

    getOffsetRelativeTo(zone: Zone, position: Vector) {
        let viewportCenter = this.size.divide(2);
        return position
            .subtract(zone.box.topLeft)
            .subtract(viewportCenter);
    }
}

class Sprite {
    private constructor(
        public readonly nickname: string,
        public readonly color: string,
        public readonly position: Vector) {
    }

    static create(nickname: string, position: Vector) {
        return new Sprite(nickname, "", position);
    }

    setPosition(position: Vector) {
        return new Sprite(this.nickname, this.color, position);
    }
}

class Player {
    private constructor(
        public readonly sprite: Sprite) {
    }

    static create(sprite: Sprite) {
        return new Player(sprite);
    }

    translate(zone: Zone, viewport: Viewport) {
        let playerToScreenOffset = viewport.getOffsetRelativeTo(zone, this.sprite.position);
        return (sprite: Sprite) => {
            let position = sprite.position
                .subtract(zone.box.topLeft)
                .subtract(playerToScreenOffset);
            return Sprite.create(sprite.nickname, position);
        }
    }
}

class Game {
    private constructor(
        public readonly zone: Zone,
        public readonly viewport: Viewport,
        public readonly player: Player,
        public readonly sprites: Sprite[]) {
    }

    static create(zone: Zone, viewport: Viewport, player: Player, sprites: Sprite[]) {
        return new Game(zone, viewport, player, sprites);
    }

    updatePosition(nickname: string, position: Vector) {
        let translateRelativeTo = this.player.translate(this.zone, this.viewport);
        let translated = this.sprites.map(sprite =>
            sprite.nickname === nickname
                ? translateRelativeTo(sprite.setPosition(position))
                : translateRelativeTo(sprite));
        return new Game(this.zone, this.viewport, this.player, translated);
    }
}



///
/// game communication with backend via websockets
///
let gameCommunication = (function () {
    let connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

    // event subscriptions from backend server
    connection.on("UpdatePosition", function (nickname, message) {
        // TODO: update player position
        console.log(nickname, message);
    });

    connection.start().catch((err) => console.error(err));

    // action invocators for frontend client
    function movePlayer(nickname, position) {
        connection.invoke("MovePlayer", nickname, position).catch(err => console.error(err));
    }

    return {
        movePlayer
    };
})();

async function loadGame() {
    let response = await fetch("pavlo.hodysh", { method: "GET" })
        .then(result => result.json())
        .catch(error => console.log(error));

    var zone = Zone.create(
        Vector.create(response.zone.topLeftX, response.zone.topLeftY),
        Vector.create(response.zone.bottomRightX, response.zone.bottomRightY));
    var viewport = Viewport.create(12, 8);
    var sprites = response.sprites.map(sprite => {
        let position = Vector.create(sprite.positionX, sprite.positionY);
        return Sprite.create(sprite.nickname, position);
    });
    let player = Player.create(
        Sprite.create(
            response.player.nickname,
            Vector.create(
                response.player.positionX,
                response.player.positionY)));
    let game = Game.create(zone, viewport, player, sprites);

    return game;
};



///
/// game view setup and initialization
///
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
