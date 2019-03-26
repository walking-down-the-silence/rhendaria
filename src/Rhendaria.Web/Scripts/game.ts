import { Sprite } from "./Sprite";
import { Viewport } from "./Viewport";
import { Vector2D as Vector } from "./Geometry/Vector2D";
import { ICellModel as Player, ICellModel } from "./Model/ICellModel";
import { IGetGameViewResponse } from "./Infrastructure/Responses/IGetGameViewResponse";
import { IDrawable } from "./Drawing/IDrawable";
import { PlayerCell } from "./Drawing/PlayerCell";
import { IPoint2D } from "./Geometry/IPoint2D";
import { Point2D } from "./Geometry/Point2D";

export class Game {
    private cells : PlayerCell[];
    private playerCell: PlayerCell;

    private constructor(
        public viewport: Viewport,
        public player: ICellModel,
        private meta: IGetGameViewResponse) {
        
    }

    // Idea of this function is to get cell point relative to player.
    // Player's relative coordinates equal to viewport center.
    coordTransformer(absolutePosition: IPoint2D): IPoint2D {
        const relativeCenter = this.viewport.getCenter();
        const absoluteCenter = this.meta.player.position;

        // Vector from absolute center to passed position.
        const absoluteVectorFromPLayer = Vector.fromLine(absoluteCenter, absolutePosition);

        // Absolute vector can be applied to relative center as relative distances are equal to absolute distances.
        const relativePoint = absoluteVectorFromPLayer.toPoint(relativeCenter.x, relativeCenter.y);

        return relativePoint;
    }
    
    static fromGameMeta(meta: IGetGameViewResponse) {
        const viewport = Viewport.create(0, 0);
        const player  = meta.player;
        
        return new Game(viewport, player, meta);
    }

    changeViewport(width: number, height: number) {
        this.viewport = Viewport.create(width, height);

        return this;
    }

    prepareFrame() {
        const isCellsExist = !!this.cells && !!this.cells.length;
        if (!isCellsExist) {
            this.cells = this.meta.cells.map(c => new PlayerCell(c, this.coordTransformer));
        } else {
            this.cells = 
        }
    }

    renderFrame() {
        this.cells = (!!this.cells && !!this.cells.length) ?  this.cells.ma
    }
}