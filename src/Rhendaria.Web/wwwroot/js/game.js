let app = new PIXI.Application({ width: 500, height: 500 });

(function () {
    let gameField = document.getElementById("game-field");
    gameField.appendChild(app.view);
})();