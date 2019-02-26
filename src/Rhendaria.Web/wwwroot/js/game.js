"use strict";
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
Object.defineProperty(exports, "__esModule", { value: true });
var PIXI = require("pixi.js");
var signalR = require("@aspnet/signalr-client");
var Vector = /** @class */ (function () {
    function Vector(x, y) {
        this.x = x;
        this.y = y;
    }
    Vector.create = function (x, y) {
        return new Vector(x, y);
    };
    Vector.prototype.add = function (vector) {
        var x = this.x + vector.x;
        var y = this.y + vector.y;
        return new Vector(x, y);
    };
    Vector.prototype.subtract = function (vector) {
        var x = this.x - vector.x;
        var y = this.y - vector.y;
        return new Vector(x, y);
    };
    Vector.prototype.multiply = function (scale) {
        var x = this.x * scale;
        var y = this.y * scale;
        return new Vector(x, y);
    };
    Vector.prototype.divide = function (scale) {
        var x = this.x / scale;
        var y = this.y / scale;
        return new Vector(x, y);
    };
    return Vector;
}());
var Rectangle = /** @class */ (function () {
    function Rectangle(topLeft, bottomRight) {
        this.topLeft = topLeft;
        this.bottomRight = bottomRight;
    }
    Rectangle.create = function (topLeft, bottomRight) {
        return new Rectangle(topLeft, bottomRight);
    };
    return Rectangle;
}());
var Zone = /** @class */ (function () {
    function Zone(box) {
        this.box = box;
    }
    Zone.create = function (topLeft, bottomRight) {
        var box = Rectangle.create(topLeft, bottomRight);
        return new Zone(box);
    };
    return Zone;
}());
var Viewport = /** @class */ (function () {
    function Viewport(size) {
        this.size = size;
    }
    Viewport.create = function (width, height) {
        var size = Vector.create(width, height);
        return new Viewport(size);
    };
    Viewport.prototype.getOffsetRelativeTo = function (zone, position) {
        var viewportCenter = this.size.divide(2);
        return position
            .subtract(zone.box.topLeft)
            .subtract(viewportCenter);
    };
    return Viewport;
}());
var Sprite = /** @class */ (function () {
    function Sprite(nickname, color, position) {
        this.nickname = nickname;
        this.color = color;
        this.position = position;
    }
    Sprite.create = function (nickname, position) {
        return new Sprite(nickname, "", position);
    };
    Sprite.prototype.setPosition = function (position) {
        return new Sprite(this.nickname, this.color, position);
    };
    return Sprite;
}());
var Player = /** @class */ (function () {
    function Player(sprite) {
        this.sprite = sprite;
    }
    Player.create = function (sprite) {
        return new Player(sprite);
    };
    Player.prototype.translate = function (zone, viewport) {
        var playerToScreenOffset = viewport.getOffsetRelativeTo(zone, this.sprite.position);
        return function (sprite) {
            var position = sprite.position
                .subtract(zone.box.topLeft)
                .subtract(playerToScreenOffset);
            return Sprite.create(sprite.nickname, position);
        };
    };
    return Player;
}());
var Game = /** @class */ (function () {
    function Game(zone, viewport, player, sprites) {
        this.zone = zone;
        this.viewport = viewport;
        this.player = player;
        this.sprites = sprites;
    }
    Game.create = function (zone, viewport, player, sprites) {
        return new Game(zone, viewport, player, sprites);
    };
    Game.prototype.updatePosition = function (nickname, position) {
        var translateRelativeTo = this.player.translate(this.zone, this.viewport);
        var translated = this.sprites.map(function (sprite) {
            return sprite.nickname === nickname
                ? translateRelativeTo(sprite.setPosition(position))
                : translateRelativeTo(sprite);
        });
        return new Game(this.zone, this.viewport, this.player, translated);
    };
    return Game;
}());
///
/// game communication with backend via websockets
///
var gameCommunication = (function () {
    var connection = new signalR.HubConnection("/gameHub");
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
///
/// game view setup and initialization
///
var gameOptions = {
    fullWidth: 0,
    fullHeight: 0
};
var mouse = {
    position: null
};
var app = (function () {
    return __awaiter(this, void 0, void 0, function () {
        var container, app, centerX, centerY;
        return __generator(this, function (_a) {
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
                mouse = {
                    position: Vector.create(e.clientX - container.offsetLeft, e.clientY - container.offsetTop)
                };
            });
            container.appendChild(app.view);
            centerX = gameOptions.fullWidth / 2;
            centerY = gameOptions.fullHeight / 2;
            //let game = await loadGame();
            //game.sprites.forEach(sprite => app.stage.addChild(sprite));
            //app.stage.addChild(game.player.sprite);
            // event subscriptions
            app.ticker.add(function () { });
            window.addEventListener("resize", function () { return app.renderer.resize(window.innerWidth, window.innerHeight); });
            return [2 /*return*/, app];
        });
    });
})();
//# sourceMappingURL=game.js.map