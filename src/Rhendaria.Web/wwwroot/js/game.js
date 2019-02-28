/// <reference path="../node_modules/@types/pixi.js/index.d.ts" />
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
    Zone.fromRaw = function (raw) {
        if (raw) {
            var topLeft = Vector.create(raw.box.topLeft.x, raw.box.topLeft.y);
            var bottomRight = Vector.create(raw.box.bottomRight.x, raw.box.bottomRight.y);
            var rectangle = Rectangle.create(topLeft, bottomRight);
            return new Zone(rectangle);
        }
        return null;
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
    return Viewport;
}());
var Sprite = /** @class */ (function () {
    function Sprite(nickname, color, actual, relative, view) {
        if (relative === void 0) { relative = null; }
        if (view === void 0) { view = null; }
        this.nickname = nickname;
        this.color = color;
        this.actual = actual;
        this.relative = relative;
        this.view = view;
        if (view) {
            this.view = view;
            this.view.x = actual.x;
            this.view.y = actual.y;
        }
        else {
            this.view = new PIXI.Graphics();
            this.view.beginFill(color, 1);
            this.view.drawCircle(actual.x, actual.y, 50);
            this.view.endFill();
        }
    }
    Sprite.create = function (nickname, color, position) {
        return new Sprite(nickname, color, position);
    };
    Sprite.fromRaw = function (raw) {
        if (raw) {
            var position = Vector.create(raw.position.x, raw.position.y);
            return new Sprite(raw.nickname, raw.color, position);
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
    function Player(sprite) {
        this.sprite = sprite;
    }
    Player.create = function (sprite) {
        return new Player(sprite);
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
    Game.fromRaw = function (raw) {
        var viewport = Viewport.create(0, 0);
        var zone = Zone.fromRaw(raw.zone);
        var player = Player.create(Sprite.fromRaw(raw.player));
        var sprites = raw.sprites ? raw.sprites.map(function (sprite) { return Sprite.fromRaw(sprite); }) : [];
        return new Game(zone, viewport, player, sprites);
    };
    Game.prototype.changeViewport = function (width, height) {
        var viewport = Viewport.create(width, height);
        this.viewport = viewport;
        return this;
    };
    Game.prototype.updatePosition = function (nickname, position) {
        var _this = this;
        var sprites = this.sprites.map(function (sprite) {
            return sprite.nickname !== nickname
                ? sprite
                : _this.updateSpritePosition(sprite, position);
        });
        this.sprites = sprites;
        return this;
    };
    Game.prototype.updateSpritePosition = function (sprite, actual) {
        var offset = this.player.sprite.actual.subtract(actual);
        var relative = this.viewport.size.divide(2).subtract(offset);
        return sprite.setActualPosition(actual).setRelativePosition(relative);
    };
    return Game;
}());
//# sourceMappingURL=game.js.map