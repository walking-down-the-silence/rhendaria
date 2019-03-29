import { Sprite } from "./Sprite";
import { Viewport } from "./Viewport";
import { Vector } from "./Vector";
import { Player } from "./Player";

export class Game {
    private constructor(
        public viewport: Viewport,
        public player: Player,
        public sprites: Sprite[]) {
    }

    static create(viewport: Viewport, player: Player, sprites: Sprite[]) {
        return new Game(viewport, player, sprites);
    }

    static fromRaw(raw: any) {
        const viewport = Viewport.create(0, 0);
        const player = Player.create(raw.player.nickname);
        const sprites = raw.sprites ? raw.sprites.map(Sprite.fromRaw) : [];
        return new Game(viewport, player, sprites);
    }

    changeViewport(width: number, height: number) {
        this.viewport = Viewport.create(width, height);

        return this;
    }

    updatePosition(nickname: string, position: Vector) {
        // first update the moved player actual position
        this.sprites = this.sprites.map(sprite => sprite.nickname === nickname
            ? this.updateSpriteActual(sprite, position)
            : sprite);
        // then update all the other sprite relative positions.
        // Current player unchanged in the center.
        this.sprites = this.sprites.map(sprite => this.updateSpriteRelative(sprite));
        return this;
    }

    findPlayerSprite() {
        return this.sprites.filter(sprite => sprite.nickname === this.player.nickname)[0];
    }

    private updateSpriteActual(sprite: Sprite, actual: Vector) {
        // update actual for current player
        const player = sprite.setActualPosition(actual);

        return player;
    }

    private updateSpriteRelative(sprite: Sprite) {
        // find and set relative for other that current player
        const player = this.findPlayerSprite();
        const offset = player.actual.subtract(sprite.actual);
        // relative for non current player is actually relevant to current

        const relative = sprite.nickname === player.nickname
            ? this.viewport.size.shrink(2)
            : player.relative.subtract(offset);

        return sprite.setRelativePosition(relative);
    }
}