import { IPoint2D } from "../Geometry/IPoint2D";

export interface ICellModel {
    nickname: string;
    color: number;
    score: number;
    position: IPoint2D;
}