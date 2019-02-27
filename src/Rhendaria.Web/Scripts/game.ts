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

    static fromRaw(raw: any) {
        if (raw) {
            let topLeft = Vector.create(raw.box.topLeft.x, raw.box.topLeft.y);
            let bottomRight = Vector.create(raw.box.bottomRight.x, raw.box.bottomRight.y);
            let rectangle = Rectangle.create(topLeft, bottomRight);
            return new Zone(rectangle);
        }
        return null;
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

    static fromRaw(raw: any) {
        if (raw) {
            let position = Vector.create(raw.position.x, raw.position.y);
            return new Sprite(raw.nickname, raw.color, position);
        }
        return null;
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