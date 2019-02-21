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
    function Sprite(position) {
        this.position = position;
    }
    Sprite.create = function (position) {
        return new Sprite(position);
    };
    return Sprite;
}());
var User = /** @class */ (function () {
    function User(nickname, sprite) {
        this.nickname = nickname;
        this.sprite = sprite;
    }
    User.create = function (nickname, position) {
        return new User(nickname, Sprite.create(position));
    };
    User.prototype.translate = function (zone, viewport) {
        var playerToScreenOffset = viewport.getOffsetRelativeTo(zone, this.sprite.position);
        console.log(playerToScreenOffset);
        return function (player) {
            var position = player.sprite.position
                .subtract(zone.box.topLeft)
                .subtract(playerToScreenOffset);
            return User.create(player.nickname, position);
        };
    };
    return User;
}());
//export {
//    Vector,
//    Rectangle,
//    Zone,
//    Viewport,
//    Sprite,
//    User
//}
(function () {
    var zone = Zone.create(Vector.create(12, 8), Vector.create(24, 16));
    var viewport = Viewport.create(12, 8);
    var players = [
        User.create("justmegaara", Vector.create(16, 11)),
        User.create("leaveme2010", Vector.create(18, 9)),
        User.create("sickranchez", Vector.create(11, 11)),
        User.create("alienware51", Vector.create(18, 18))
    ];
    console.log(players);
    var main = players[0];
    var translateRelativeTo = main.translate(zone, viewport);
    var tranlated = players.map(function (player) { return translateRelativeTo(player); });
    console.log(tranlated);
})();
//# sourceMappingURL=domain.js.map