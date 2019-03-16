/// <reference path="../node_modules/@types/pixi.js/index.d.ts" />
var Vector = /** @class */ (function () {
    function Vector(x, y) {
        this.x = x;
        this.y = y;
    }
    Vector.create = function (x, y) {
        return new Vector(x, y);
    };
    Vector.fromRaw = function (raw) {
        return new Vector(raw.x, raw.y);
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
    Vector.prototype.scale = function (scale) {
        var x = this.x * scale;
        var y = this.y * scale;
        return new Vector(x, y);
    };
    Vector.prototype.shrink = function (scale) {
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
var Viewport = /** @class */ (function () {
    function Viewport(size) {
        this.size = size;
    }
    Viewport.create = function (width, height) {
        var size = Vector.create(width, height);
        return new Viewport(size);
    };
    return Viewport;
}());
var Sprite = /** @class */ (function () {
    function Sprite(nickname, color, 
    // Actual, real position.
    actual, 
    // Position relative to main player. For main player is (width/2, height/2).
    relative, view) {
        if (view === void 0) { view = null; }
        this.nickname = nickname;
        this.color = color;
        this.actual = actual;
        this.relative = relative;
        this.view = view;
        if (view) {
            this.view = view;
            this.view.x = relative.x;
            this.view.y = relative.y;
        }
        else {
            this.view = new PIXI.Graphics();
            this.view.beginFill(color, 1);
            this.view.drawCircle(relative.x, relative.y, 50);
            this.view.endFill();
        }
    }
    Sprite.create = function (nickname, color, position) {
        var relative = Vector.create(0, 0);
        return new Sprite(nickname, color, position, relative);
    };
    Sprite.fromRaw = function (raw) {
        if (raw) {
            var position = Vector.create(raw.position.x, raw.position.y);
            var relative = Vector.create(0, 0);
            return new Sprite(raw.nickname, raw.color, position, relative);
        }
        return null;
    };
    Sprite.prototype.setActualPosition = function (actual) {
        return new Sprite(this.nickname, this.color, actual, this.relative, this.view);
    };
    Sprite.prototype.setRelativePosition = function (relative) {
        return new Sprite(this.nickname, this.color, this.actual, relative, this.view);
    };
    return Sprite;
}());
var Player = /** @class */ (function () {
    function Player(nickname) {
        this.nickname = nickname;
    }
    Player.create = function (nickname) {
        return new Player(nickname);
    };
    return Player;
}());
var Game = /** @class */ (function () {
    function Game(viewport, player, sprites) {
        this.viewport = viewport;
        this.player = player;
        this.sprites = sprites;
    }
    Game.create = function (viewport, player, sprites) {
        return new Game(viewport, player, sprites);
    };
    Game.fromRaw = function (raw) {
        var viewport = Viewport.create(0, 0);
        var player = Player.create(raw.player.nickname);
        var sprites = raw.sprites ? raw.sprites.map(Sprite.fromRaw) : [];
        return new Game(viewport, player, sprites);
    };
    Game.prototype.changeViewport = function (width, height) {
        this.viewport = Viewport.create(width, height);
        return this;
    };
    Game.prototype.updatePosition = function (nickname, position) {
        var _this = this;
        // first update the moved player actual position
        this.sprites = this.sprites.map(function (sprite) { return sprite.nickname === nickname
            ? _this.updateSpriteActual(sprite, position)
            : sprite; });
        // then update all the other sprite relative positions.
        // Current player unchanged in the center.
        this.sprites = this.sprites.map(function (sprite) { return _this.updateSpriteRelative(sprite); });
        return this;
    };
    Game.prototype.findPlayerSprite = function () {
        var _this = this;
        return this.sprites.find(function (sprite) { return sprite.nickname === _this.player.nickname; });
    };
    Game.prototype.updateSpriteActual = function (sprite, actual) {
        // update actual for current player
        var player = sprite.setActualPosition(actual);
        return player;
    };
    Game.prototype.updateSpriteRelative = function (sprite) {
        // find and set relative for other that current player
        var player = this.findPlayerSprite();
        var offset = player.actual.subtract(sprite.actual);
        // relative for non current player is actually relevant to current
        var relative = sprite.nickname === player.nickname
            ? this.viewport.size.shrink(2)
            : player.relative.subtract(offset);
        return sprite.setRelativePosition(relative);
    };
    return Game;
}());
//# sourceMappingURL=game.js.map