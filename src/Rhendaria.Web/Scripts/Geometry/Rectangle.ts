import { Vector2D as Vector } from "./Vector2D";

export class Rectangle {
    private constructor(
        public readonly topLeft: Vector,
        public readonly bottomRight: Vector) {
    }

    static create(topLeft: Vector, bottomRight: Vector) {
        return new Rectangle(topLeft, bottomRight);
    }
}