/// <reference path="../node_modules/@types/pixi.js/index.d.ts" />

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
}

class Sprite {
    private constructor(
        public readonly nickname: string,
        public readonly color: number,
        public readonly actual: Vector,
        public readonly relative: Vector = null,
        public readonly view: PIXI.Graphics = null) {

        if (view) {
            this.view = view;
            this.view.x = actual.x;
            this.view.y = actual.y;
        }
        else {
            this.view = new PIXI.Graphics();
            this.view.beginFill(color, 1);
            this.view.drawCircle(actual.x, actual.y, 50);
            this.view.endFill();
        }
    }

    static create(nickname: string, color: number, position: Vector) {
        return new Sprite(nickname, color, position);
    }

    static fromRaw(raw: any) {
        if (raw) {
            let position = Vector.create(raw.position.x, raw.position.y);
            return new Sprite(raw.nickname, raw.color, position);
        }
        return null;
    }

    setActualPosition(actual: Vector) {
        return new Sprite(this.nickname, this.color, actual, this.relative, this.view);
    }

    setRelativePosition(relative: Vector) {
        return new Sprite(this.nickname, this.color, this.actual, relative, this.view);
    }
}

class Player {
    private constructor(
        public readonly sprite: Sprite) {
    }

    static create(sprite: Sprite) {
        return new Player(sprite);
    }
}

class Game {
    private constructor(
        public zone: Zone,
        public viewport: Viewport,
        public player: Player,
        public sprites: Sprite[]) {
    }

    static create(zone: Zone, viewport: Viewport, player: Player, sprites: Sprite[]) {
        return new Game(zone, viewport, player, sprites);
    }

    static fromRaw(raw: any) {
        let viewport = Viewport.create(0, 0);
        let zone = Zone.fromRaw(raw.zone);
        let player = Player.create(Sprite.fromRaw(raw.player));
        let sprites = raw.sprites ? raw.sprites.map(sprite => Sprite.fromRaw(sprite)) : [];
        return new Game(zone, viewport, player, sprites);
    }

    changeViewport(width: number, height: number) {
        let viewport = Viewport.create(width, height);
        this.viewport = viewport;
        return this;
    }

    updatePosition(nickname: string, position: Vector) {
        let sprites = this.sprites.map(sprite => {
            return sprite.nickname !== nickname
                ? sprite
                : this.updateSpritePosition(sprite, position);
        });
        this.sprites = sprites;
        return this;
    }

    private updateSpritePosition(sprite: Sprite, actual: Vector) {
        let offset = this.player.sprite.actual.subtract(actual);
        let relative = this.viewport.size.divide(2).subtract(offset);
        return sprite.setActualPosition(actual).setRelativePosition(relative);
    }
}