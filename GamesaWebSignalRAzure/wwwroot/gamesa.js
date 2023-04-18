const canvas = document.getElementById("canvas");
const canvasContext = canvas.getContext("2d");

document.addEventListener("DOMContentLoaded", onLoad);

let points = [];
let steps = 0;
let moving = false;
let pointIndex;

async function onLoad() {
    canvas.addEventListener("mousedown", pointBeginMove);
    canvas.addEventListener("mouseup", pointEndMove);
    canvas.addEventListener("mousemove", pointMoving);
    canvas.addEventListener("touchstart", pointBeginMove);
    canvas.addEventListener("touchend", pointEndMove);
    canvas.addEventListener("touchmove", pointMoving);

    await start(GameId, (p, s) => {
        points = p;
        steps = s;
        drawGame();
        updateGameStatus();
    });
}

function updateGameStatus() {
    document.title = steps;
}

function drawGame() {
    clearCanvas();
    let i = 0;
    for (i = 0; i < points.length - 1; i++) {
        drawLine("red", points[i], points[i + 1]);
        drawCircle("blue", points[i].x - (Delta4Point / 2), points[i].y - (Delta4Point / 2), Delta4Point);
    }
    drawLine("red", points[i], points[0]);
    drawCircle("blue", points[i].x - (Delta4Point / 2), points[i].y - (Delta4Point / 2), Delta4Point);
}

async function pointBeginMove(e) {
    const [x, y] = getCoordinates(e);
    for (let i = 0; i < points.length; i++) {
        if (Math.abs(points[i].x - x) < Delta4Point && Math.abs(points[i].y - y) < Delta4Point) {
            pointIndex = i;
            moving = true;
            await startMove(GameId, pointIndex, x, y);
            break;
        }
    }
}

async function pointEndMove(e) {
    moving = false;
}

async function pointMoving(e) {
    if (moving) {
        const [x, y] = getCoordinates(e);
        await doMove(GameId, pointIndex, x, y);
    }
}

function getCoordinates(e) {
    e.preventDefault();
    if (e instanceof TouchEvent) {
        const bcr = e.target.getBoundingClientRect();
        const x = e.targetTouches[0].clientX - bcr.x;
        const y = e.targetTouches[0].clientY - bcr.y;
        return [x, y];
    }
    else {
        const x = e.offsetX;
        const y = e.offsetY;
        return [x, y];
    }
}

function clearCanvas() {
    canvasContext.beginPath();
    canvasContext.rect(0, 0, canvas.width, canvas.height);
    canvasContext.fillStyle = "white";
    canvasContext.fill();
}

function drawLine(color, pt1, pt2) {
    canvasContext.beginPath();
    canvasContext.moveTo(pt1.x, pt1.y);
    canvasContext.lineTo(pt2.x, pt2.y);
    canvasContext.strokeStyle = color;
    canvasContext.stroke();
}

function drawCircle(color, x, y, diameter) {
    canvasContext.beginPath();
    canvasContext.arc(x + (diameter / 2), y + (diameter / 2), diameter / 2, 0, 2 * Math.PI);
    canvasContext.strokeStyle = color;
    canvasContext.stroke();
}

class GamePoint {
    constructor(x, y) {
        this.x = x;
        this.y = y;
    }
}