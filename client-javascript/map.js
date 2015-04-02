
function drawMap(){
  if (!gameState) {console.error("can not draw map: no game loaded");return;}

  //clear the canvas before redrawing
  var map = document.getElementById("canvasMap").getContext("2d");
  map.clearRect(0, 0, mapPixels, mapPixels);

  drawTerrain();
  drawGrid();
  drawKittens();

}

var mapPixels=520; //ideally a multiple of mapSquares
var mapSquares=13;
var squarePixels = mapPixels/mapSquares;
var unexploredColor = "#000000";
var terrainColors = {"dirt":"#996915", "grass":"#99CF84"};
var gridColor = "#888888";
var gridWidth = 2;
var initialsStrokeWidth=.5;
var initialsStrokeColor="#000000";

function drawTerrain(){
  var map = document.getElementById("canvasMap").getContext("2d");

  var mapAsList = gameState.map.mapAsList;
  for (var i in gameState.map.mapAsList) {
    var square = mapAsList[i];
    var x = square.coord.x;
    var y = square.coord.y;
    if (square.explored) {
      map.fillStyle = terrainColors[square.type];
    } else {
      map.fillStyle = unexploredColor;
    }

    map.fillRect(x*squarePixels,y*squarePixels,(x+1)*squarePixels,(y+1)*squarePixels);
  }
}

function drawGrid() {
  var map = document.getElementById("canvasMap").getContext("2d");

  map.strokeStyle = gridColor;
  map.lineWidth = gridWidth;

  for (var x=0; x<mapSquares+1; x++) {
    map.beginPath();
    map.moveTo(0, squarePixels*x);
    map.lineTo(mapPixels, squarePixels*x);
    map.stroke();
  }
  for (var x=0; x<mapSquares+1; x++) {
    map.beginPath();
    map.moveTo(squarePixels*x,0);
    map.lineTo(squarePixels*x,mapPixels);
    map.stroke();
  }
}

function drawKittens() {
  var map = document.getElementById("canvasMap").getContext("2d");
  var puppies = gameState.puppies;


  //map.font = '60pt Calibri'
  map.lineWidth=initialsStrokeWidth;
  map.strokeStyle=initialsStrokeColor;
  map.fillStyle="#FFFFFF";
  map.textAlign = "center";
  map.textBaseline = "middle";
  map.font = "10pt Arial";
  for (var initials in puppies) {
    // currently places the puppies at a random location.  Will fix later.
    var x = Math.floor(mapSquares*Math.random());
    var y = Math.floor(mapSquares*Math.random());

    //we could use map.fillText before this to color the puppies
    map.fillText(initials,(x+.5)*squarePixels,(y+.5)*squarePixels);
    map.strokeText(initials,(x+.5)*squarePixels,(y+.5)*squarePixels);
  }

}
