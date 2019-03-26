import D = require("./IPoint2D");
import IPoint2D = D.IPoint2D;

export class Point2D implements IPoint2D{
    public constructor(
        public readonly x: number,
        public readonly y: number) {
    }

    // Allows to convert anonymous interface realization to class.
    fromPoint(point: IPoint2D) {
        return new Point2D(point.x, point.y);
    }

    // Translates point as if center at given point.
    translate(center: Point2D) {
        return new Point2D(this.x - center.x, this.y - center.y);
    }
}