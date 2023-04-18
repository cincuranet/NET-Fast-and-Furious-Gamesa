const connection = new signalR.HubConnectionBuilder()
    .withUrl("/game-hub")
    .withAutomaticReconnect()
    .build();

async function start(id, onRefreshGame) {
    await connection.start();
    connection.on("RefreshGame", (points, steps) => {
        onRefreshGame(points, steps);
    });
    await connection.invoke("StartGame", id);
}

async function startMove(id, pointIndex, x, y) {
    await connection.invoke("StartMove", id, pointIndex, x, y);
}

async function doMove(id, pointIndex, x, y) {
    await connection.invoke("DoMove", id, pointIndex, x, y);
}