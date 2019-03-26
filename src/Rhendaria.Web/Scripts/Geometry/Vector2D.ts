import { Point2D } from "./Point2D";
import { IPoint2D } from "./IPoint2D";

export class Vector2D {
    private constructor(
        public readonly x: number,
        public readonly y: number) {
    }

    static fromLine(start: IPoint2D, end: IPoint2D) {
        return new Vector2D(end.x - start.x, end.y - start.y);
    }

    static create(x: number, y: number) {
        return new Vector2D(x, y);
    }

    static fromRaw(raw: any) {
        return new Vector2D(raw.x, raw.y);
    }

    add(vector: Vector2D) {
        const x = this.x + vector.x;
        const y = this.y + vector.y;
        return new Vector2D(x, y);
    }

    subtract(vector: Vector2D) {
        const x = this.x - vector.x;
        const y = this.y - vector.y;
        return new Vector2D(x, y);
    }

    scale(scale: number) {
        const x = this.x * scale;
        const y = this.y * scale;
        return new Vector2D(x, y);
    }

    shrink(scale: number) {
        const x = this.x / scale;
        const y = this.y / scale;
        return new Vector2D(x, y);
    }

    toPoint(startX?: number, startY?: number) {
        let centerX = 0;
        let centerY = 0;

        if (startX !== undefined) {
            centerX = startX;
        }

        if (startY !== undefined) {
            centerY = startY;
        }

        return new Point2D(this.x + centerX, this.y + centerY);
    }
}