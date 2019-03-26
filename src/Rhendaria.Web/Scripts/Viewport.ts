import { Vector2D as Vector } from "./Geometry/Vector2D";
import { IPoint2D } from "./Geometry/IPoint2D";
import { Point2D } from "./Geometry/Point2D";

export class Viewport {
    private constructor(
        public readonly size: Vector) {
    }

    static create(width: number, height: number) {
        const size = Vector.create(width, height);
        return new Viewport(size);
    }

    getCenter(): IPoint2D {
        return this.size.shrink(2).toPoint();
    }
}