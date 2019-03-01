/// <reference path="../node_modules/@types/pixi.js/index.d.ts" />

class Vector {
    private constructor(
        public readonly x: number,
        public readonly y: number) {
    }

    static create(x: number, y: number) {
        return new Vector(x, y);
    }

    static fromRaw(raw: any) {
        return new Vector(raw.x, raw.y);
    }

    add(vector: Vector) {
        const x = this.x + vector.x;
        const y = this.y + vector.y;
        return new Vector(x, y);
    }

    subtract(vector: Vector) {
        const x = this.x - vector.x;
        const y = this.y - vector.y;
        return new Vector(x, y);
    }

    scale(scale: number) {
        const x = this.x * scale;
        const y = this.y * scale;
        return new Vector(x, y);
    }

    shrink(scale: number) {
        const x = this.x / scale;
        const y = this.y / scale;
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
        const box = Rectangle.create(topLeft, bottomRight);
        return new Zone(box);
    }

    static fromRaw(raw: any) {
        if (raw) {
            const topLeft = Vector.create(raw.box.topLeft.x, raw.box.topLeft.y);
            const bottomRight = Vector.create(raw.box.bottomRight.x, raw.box.bottomRight.y);
            const rectangle = Rectangle.create(topLeft, bottomRight);
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
        const size = Vector.create(width, height);
        return new Viewport(size);
    }
}

class Sprite {
    private constructor(
        public readonly nickname: string,
        public readonly color: number,
        public readonly actual: Vector,
        public readonly relative: Vector,
        public readonly view: PIXI.Graphics = null) {

        if (view) {
            this.view = view;
            this.view.x = relative.x;
            this.view.y = relative.y;
        }
        else {
            this.view = new PIXI.Graphics();
            this.view.beginFill(color, 1);
            this.view.drawCircle(relative.x, relative.y, 50);
            this.view.endFill();
        }
    }

    static create(nickname: string, color: number, position: Vector) {
        const relative = Vector.create(0, 0);
        return new Sprite(nickname, color, position, relative);
    }

    static fromRaw(raw: any) {
        if (raw) {
            const position = Vector.create(raw.position.x, raw.position.y);
            const relative = Vector.create(0, 0);
            return new Sprite(raw.nickname, raw.color, position, relative);
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
        public readonly nickname: string) {
    }

    static create(nickname: string) {
        return new Player(nickname);
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
        const viewport = Viewport.create(0, 0);
        const zone = Zone.fromRaw(raw.zone);
        const player = Player.create(raw.player.nickname);
        const sprites = raw.sprites ? raw.sprites.map(Sprite.fromRaw) : [];
        return new Game(zone, viewport, player, sprites);
    }

    changeViewport(width: number, height: number) {
        this.viewport = Viewport.create(width, height);
        return this;
    }

    updatePosition(nickname: string, position: Vector) {
        // first update the current player actual position
        this.sprites = this.sprites.map(sprite => sprite.nickname === nickname
            ? this.updateSpriteActual(sprite, position)
            : sprite);
        // then update all the other sprite relative positions
        this.sprites = this.sprites.map(sprite => sprite.nickname !== nickname
            ? this.updateSpriteRelative(sprite, position)
            : sprite);
        return this;
    }

    private findPlayersSprite() {
        return this.sprites.find(sprite => sprite.nickname === this.player.nickname);
    }

    private updateSpriteActual(sprite: Sprite, actual: Vector) {
        // update actual for current player
        const player = sprite.setActualPosition(actual);
        // relative for current player is always in the center
        // TODO: is this check needed for all players except current?
        const relative = this.viewport.size.shrink(2);
        return player.setRelativePosition(relative);
    }

    private updateSpriteRelative(sprite: Sprite, actual: Vector) {
        // find and set relative for other that current player
        const player = this.findPlayersSprite();
        const offset = player.actual.subtract(sprite.actual);
        // relative for non current player is actually relevant to current
        const relative = this.viewport.size.shrink(2).subtract(offset);
        return sprite.setRelativePosition(relative);
    }
}