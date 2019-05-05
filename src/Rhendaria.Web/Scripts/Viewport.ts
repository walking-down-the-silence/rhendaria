import { Vector } from "./Vector";

export class Viewport {
    private constructor(
        public readonly size: Vector,
        public readonly backSprite: PIXI.extras.TilingSprite = null) {
        if (backSprite == null) {
            const backTexture = PIXI.Texture.fromImage('images/Background.png');
            this.backSprite = new PIXI.extras.TilingSprite(backTexture, size.x, size.y);
        } else {
            backSprite.width = size.x;
            backSprite.height = size.y;
        }
    }

    shift(direction: Vector) {
        this.backSprite.tilePosition.x += direction.x;
        this.backSprite.tilePosition.y += direction.y;
    }

    static create(width: number, height: number, backSprite: PIXI.extras.TilingSprite = null) {
        const size = Vector.create(width, height);
        return new Viewport(size, backSprite);
    }
}