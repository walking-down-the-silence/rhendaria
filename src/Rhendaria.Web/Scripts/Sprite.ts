import { Vector2D as Vector } from "./Geometry/Vector2D";

export class Sprite {
    private constructor(
        public readonly nickname: string,
        public readonly color: number,
        // Actual, real position.
        public readonly actual: Vector,
        // Position relative to main player. For main player is (width/2, height/2).
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