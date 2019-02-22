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
        console.log(playerToScreenOffset);
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
(function () {
    var zone = Zone.create(Vector.create(12, 8), Vector.create(24, 16));
    var viewport = Viewport.create(12, 8);
    var sprites = [
        Sprite.create("player1", Vector.create(16, 11)),
        Sprite.create("player2", Vector.create(18, 9)),
        Sprite.create("player3", Vector.create(11, 11)),
        Sprite.create("player4", Vector.create(18, 18))
    ];
    var player = Player.create(sprites[0]);
    var game = Game.create(zone, viewport, player, sprites)
        .updatePosition("player1", Vector.create(15, 11))
        .updatePosition("player3", Vector.create(12, 11))
        .updatePosition("player3", Vector.create(12, 12));
})();
//# sourceMappingURL=domain.js.map