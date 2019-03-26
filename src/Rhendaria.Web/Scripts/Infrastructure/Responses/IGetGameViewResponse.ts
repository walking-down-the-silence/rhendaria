import { ICellModel } from "../../Model/ICellModel";

export interface IGetGameViewResponse {
    player: ICellModel;
    cells: ICellModel[];
}