export class Player {
    private constructor(
        public readonly nickname: string) {
    }

    static create(nickname: string) {
        return new Player(nickname);
    }
}