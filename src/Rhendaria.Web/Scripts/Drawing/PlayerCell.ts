import { IDrawable } from "./IDrawable";
import { ICellModel as IPlayerInfo } from "../Model/ICellModel";
import { IPoint2D } from "../Geometry/IPoint2D";

export class PlayerCell implements IDrawable {
    constructor(
        private readonly playerInfo: IPlayerInfo,
        private readonly relativeCoordTransformer: (p: IPoint2D) => IPoint2D,
        public view?: PIXI.Graphics) {
    }

    draw() {
        const relativeCoords = this.relativeCoordTransformer(this.playerInfo.position);

        if (this.view == null) {
            this.view = new PIXI.Graphics();
            this.view.beginFill(this.playerInfo.color, 1);
            this.view.drawCircle(relativeCoords.x, relativeCoords.y, this.playerInfo.score);
            this.view.endFill();
        } else {
            this.view.x = relativeCoords.x;
            this.view.y = relativeCoords.y;
        }
    }
}