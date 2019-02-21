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
        public readonly position: Vector) {
    }

    static create(position: Vector) {
        return new Sprite(position);
    }
}

class User {
    private constructor(
        public readonly nickname: string,
        public readonly sprite: Sprite) {
    }

    static create(nickname: string, position: Vector) {
        return new User(nickname, Sprite.create(position));
    }

    translate(zone: Zone, viewport: Viewport) {
        let playerToScreenOffset = viewport.getOffsetRelativeTo(zone, this.sprite.position);
        console.log(playerToScreenOffset);
        return (player: User) => {
            let position = player.sprite.position
                .subtract(zone.box.topLeft)
                .subtract(playerToScreenOffset);
            return User.create(player.nickname, position);
        }
    }
}

(function () {
    var zone = Zone.create(Vector.create(12, 8), Vector.create(24, 16));
    var viewport = Viewport.create(12, 8);
    var players = [
        User.create("player1", Vector.create(16, 11)),
        User.create("player2", Vector.create(18, 9)),
        User.create("player3", Vector.create(11, 11)),
        User.create("player4", Vector.create(18, 18))
    ];
    console.log(players);
    let main = players[0];
    let translateRelativeTo = main.translate(zone, viewport);
    let tranlated = players.map(player => translateRelativeTo(player));
    console.log(tranlated);
})();