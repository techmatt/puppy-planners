var map;
//var mapInitialized = false;
var currentTime = 0;


function initMap() {
//  if (mapInitialized) {return;} else {mapInitialized=true;}
  map = document.getElementById("canvasMap").getContext("2d");
  addFakeKittenData();

  window.requestAnimationFrame(drawMap);
}

function addFakeKittenData() {
  gameState.time = (new Date()).getTime();

  var puppies = gameState.puppies;
  for (var initials in puppies) {
    var puppy = puppies[initials];
    puppy.currentLocation = {
      x: Math.floor(mapSquares*Math.random()),
      y: Math.floor(mapSquares*Math.random())
    }
    puppy.destination = {
      x: Math.floor(mapSquares*Math.random()),
      y: Math.floor(mapSquares*Math.random())
    }
    puppy.movementRate = .2; // squares per second
  }

}

function drawMap(){
  if (!gameState) {console.error("can not draw map: no game loaded");return;}
  currentTime = ((new Date()).getTime()-gameState.time)/1000.;

  //clear the canvas before redrawing
  map.clearRect(0, 0, mapPixels, mapPixels);

  drawTerrain();
  drawGrid();
  drawKittens();

  window.requestAnimationFrame(drawMap);
}

var mapPixels=520; //ideally a multiple of mapSquares
var mapSquares=13;
var squarePixels = mapPixels/mapSquares;
var unexploredColor = "#000000";
var terrainColors = {"dirt":"#996915", "grass":"#99CF84"};
var gridColor = "#888888";
var gridWidth = 2;
var initialsStrokeWidth=1;
var initialsStrokeColor="#000000";

function drawTerrain(){
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
  var puppies = gameState.puppies;


  //map.font = '60pt Calibri'
  map.lineWidth=initialsStrokeWidth;
  map.strokeStyle=initialsStrokeColor;
  map.fillStyle="#FFFFFF";
  map.textAlign = "center";
  map.textBaseline = "middle";
  map.font = "10pt Arial";
  for (var initials in puppies) {
    var puppy = puppies[initials];
    var x0 = puppy.currentLocation.x;
    var y0 = puppy.currentLocation.y;
    var x1 = puppy.destination.x;
    var y1 = puppy.destination.y;

    var distance = Math.sqrt(Math.pow(x0-x1,2)+Math.pow(y0-y1,2));
    var fracTravelled = Math.min(puppy.movementRate*currentTime/distance,1);

    var x = x0 + (x1-x0)*fracTravelled;
    var y = y0 + (y1-y0)*fracTravelled;

    //we could use map.fillText before this to color the puppies
    map.fillText(initials,(x+.5)*squarePixels,(y+.5)*squarePixels);
    map.strokeText(initials,(x+.5)*squarePixels,(y+.5)*squarePixels);
  }

}
