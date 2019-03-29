import { Vector } from "./Vector";

export class Viewport {
    private constructor(
        public readonly size: Vector) {
    }

    static create(width: number, height: number) {
        const size = Vector.create(width, height);
        return new Viewport(size);
    }
}