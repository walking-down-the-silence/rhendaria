class Direction {
    constructor(x, y) {
        this._x = Math.sign(x);
        this._y = Math.sign(y);
    }

    get x() { return this._x; }

    get y() { return this._y; }

    toString() {
        return "x: " + this._x + "; y: " + this._y;
    }
}

class Offset {
    constructor(x, y) {
        this._x = x;
        this._y = y;
    }

    get x() { return this._x; }

    get y() { return this._y; }

    toString() {
        return "x: " + this._x + "; y: " + this._y;
    }

    negate() {
        return new Offset(-this._x, -this._y);
    }

    getDirection(targetPosition) {
        const xDiff = targetPosition.x - this._x;
        const yDiff = targetPosition.y - this._y;
        return new Direction(xDiff, yDiff);
    }
}

class Player {
    constructor(username, position) {
        this._username = username;

        const circle = new PIXI.Graphics();

        circle.beginFill(0xFFBB0B, 1);
        circle.drawCircle(position.x, position.y, 50);
        circle.endFill();

        this._sprite = circle;
    }

    get sprite() {
        return this._sprite;
    }

    get position() {
        return new Offset(this._sprite.x, this._sprite.y);
    }

    move(target) {
        const velocity = 1;
        const current = this.position;
        const direction = current.getDirection(target);

        this._sprite.x = this._sprite.x + velocity * direction.x;
        this._sprite.y = this._sprite.y + velocity * direction.y;
    }
}
