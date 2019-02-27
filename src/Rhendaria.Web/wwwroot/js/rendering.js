/// <reference path="../node_modules/@types/pixi.js/index.d.ts" />
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
 * game view setup and initialization
 **/
function initializeGame(nickname) {
    return __awaiter(this, void 0, void 0, function () {
        var url, response, zone, viewport, player, sprites, game;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    url = "http://localhost:54016/api/player/" + nickname;
                    return [4 /*yield*/, fetch(url, { method: "GET" })
                            .then(function (result) { return result.json(); })
                            .catch(function (error) { return console.log(error); })];
                case 1:
                    response = _a.sent();
                    zone = Zone.fromRaw(response.zone);
                    viewport = Viewport.create(12, 8);
                    player = Player.create(Sprite.fromRaw(response.player));
                    sprites = response.sprites ? response.sprites.map(function (sprite) { return Sprite.fromRaw(sprite); }) : [];
                    game = Game.create(zone, viewport, player, sprites);
                    console.log(game);
                    return [2 /*return*/, game];
            }
        });
    });
}
var app = (function () {
    return __awaiter(this, void 0, void 0, function () {
        var container, gameOptions, app, gameChannel, game;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    container = document.getElementById("game-field");
                    gameOptions = {
                        fullWidth: container.offsetWidth,
                        fullHeight: container.offsetHeight
                    };
                    app = new PIXI.Application({
                        antialias: true,
                        autoResize: true,
                        resolution: devicePixelRatio
                    });
                    app.stage.interactive = true;
                    app.renderer.resize(gameOptions.fullWidth, gameOptions.fullHeight);
                    app.view.addEventListener("mousemove", function (e) {
                        // TODO: get relative coordinates
                        var mouse = {
                            position: Vector.create(e.clientX - container.offsetLeft, e.clientY - container.offsetTop)
                        };
                    });
                    container.appendChild(app.view);
                    gameChannel = new GameChannel();
                    return [4 /*yield*/, gameChannel.setupCommunicationChannel()];
                case 1:
                    _a.sent();
                    return [4 /*yield*/, initializeGame("justmegaara")];
                case 2:
                    game = _a.sent();
                    game.sprites
                        .concat(game.player.sprite)
                        .forEach(function (sprite) { return app.stage.addChild(sprite.view); });
                    gameChannel.setGame(game);
                    // event subscriptions
                    app.ticker.add(function () { });
                    window.addEventListener("resize", function () { return app.renderer.resize(window.innerWidth, window.innerHeight); });
                    return [2 /*return*/, app];
            }
        });
    });
})();
//# sourceMappingURL=rendering.js.map