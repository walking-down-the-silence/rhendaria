class Vector {
    private constructor(
        public readonly x: number,
        public readonly y: number) {
    }

    static create(x: number, y: number) {
        return new Vector(x, y);
    }

    add(vector: Vector) {
        let x = vector.x + this.x;
        let y = vector.y + this.y;
        return new Vector(x, y);
    }

    subtract(vector: Vector) {
        let x = vector.x - this.x;
        let y = vector.y - this.y;
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

    static create(size: Vector) {
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

class Player {
    private constructor(
        public readonly nickname: string,
        public readonly sprite: Sprite) {
    }

    static create(nickname: string, position: Vector) {
        return new Player(nickname, Sprite.create(position));
    }

    translate(zone: Zone, viewport: Viewport) {
        let playerToScreenOffset = viewport.getOffsetRelativeTo(zone, this.sprite.position);
        return (player: Player) => {
            let position = player.sprite.position
                .subtract(zone.box.topLeft)
                .subtract(playerToScreenOffset);
            return Player.create(player.nickname, position);
        }
    }
}
