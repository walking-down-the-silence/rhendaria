(function () {
    let response = fetch("pavlo.hodysh", { method: "GET" });

    var zone = Zone.create(
        Vector.create(response.zone.x, response.zone.y),
        Vector.create(response.zone.width, response.zone.height));
    var viewport = Viewport.create(12, 8);
    var sprites = response.sprites.map(sprite => {
        return Sprite.create(
            sprite.nickname,
            Vector.create(sprite.positionX, sprite.positionY));
    });
    let player = Player.create(
        Sprite.create(
            response.player.nickname,
            response.player.positionX,
            response.player.positionY
        ));
    let game = Game.create(zone, viewport, player, sprites);

    console.log(game);
})();