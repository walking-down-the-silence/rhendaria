export class Vector {
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