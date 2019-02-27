/// <reference types="@aspnet/signalr" />
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
/**
 * game communication with backend via websockets
 */
var gameCommunication = (function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
    // event subscriptions from backend server
    connection.on("UpdatePosition", function (nickname, message) {
        // TODO: update player position
        console.log(nickname, message);
    });
    connection.start().catch(function (err) { return console.error(err); });
    // action invocators for frontend client
    function movePlayer(nickname, position) {
        connection.invoke("MovePlayer", nickname, position).catch(function (err) { return console.error(err); });
    }
    return {
        movePlayer: movePlayer
    };
})();
function loadGame() {
    return __awaiter(this, void 0, void 0, function () {
        var response, zone, viewport, sprites, player, game;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, fetch("pavlo.hodysh", { method: "GET" })
                        .then(function (result) { return result.json(); })
                        .catch(function (error) { return console.log(error); })];
                case 1:
                    response = _a.sent();
                    zone = Zone.create(Vector.create(response.zone.topLeftX, response.zone.topLeftY), Vector.create(response.zone.bottomRightX, response.zone.bottomRightY));
                    viewport = Viewport.create(12, 8);
                    sprites = response.sprites.map(function (sprite) {
                        var position = Vector.create(sprite.positionX, sprite.positionY);
                        return Sprite.create(sprite.nickname, position);
                    });
                    player = Player.create(Sprite.create(response.player.nickname, Vector.create(response.player.positionX, response.player.positionY)));
                    game = Game.create(zone, viewport, player, sprites);
                    return [2 /*return*/, game];
            }
        });
    });
}
;
//# sourceMappingURL=communication.js.map