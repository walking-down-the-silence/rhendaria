import { IGetGameViewResponse } from "../Responses/IGetGameViewResponse";

export async function getGameInfo(nickname: string) : IGetGameViewResponse {
    const url = `api/player/${nickname}`;
    return fetch(url, { method: "GET" })
        .then(result => result.json())
        .catch(error => console.log(error));
}