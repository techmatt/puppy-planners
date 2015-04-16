var map;
//var mapInitialized = false;
var currentTime = 0;
var NS="http://www.w3.org/2000/svg";
renderTimeout = null;
mapInitialized = false;
mapSVGTerrainSquares = [];
mapSVGGridLines = [];
mapSVGPuppies={};

function initMap() {
  if (mapInitialized) {return;} else {mapInitialized=true;}
  mapInitialized=true;
  map = document.getElementById("svgMap");
  if (!gameState) {console.error("can not draw map: no game loaded");return;}
  //addFakePuppieData();
  //initializeMapObjects();

  //if (renderTimeout) {clearInterval(renderTimeout);}
  //renderTimeout=setInterval(drawMap,1000);
  drawMap();
}

function initializeMapObjects() {
  // the terrain
  mapSVGTerrainSquares = [];
  var mapAsList = gameState.map.mapAsList;
  for (var i in gameState.map.mapAsList) {
    var square = mapAsList[i];
    var x = square.coord.x;
    var y = square.coord.y;

    if (square.explored) {
      fill = terrainColors[square.type];
    } else {
      fill = unexploredColor;
    }

    var square= document.createElementNS(NS,"rect");
    square.width.baseVal.value=squarePixels;
    square.height.baseVal.value=squarePixels;
    square.style.fill=fill;
    square.x.baseVal.value=x*squarePixels;
    square.y.baseVal.value=y*squarePixels;
    map.appendChild(square);
    mapSVGTerrainSquares.push(square);
  }

  // the grid

  map.strokeStyle = gridColor;
  map.lineWidth = gridWidth;

  for (var x=0; x<mapSquares+1; x++) {
    var line = document.createElementNS(NS,"line");
    map.appendChild(line);
    //<line x1="0" y1="0" x2="200" y2="200" style="stroke:rgb(255,0,0);stroke-width:2" />
    line.setAttribute("x1",squarePixels*x);
    line.setAttribute("y1",0);
    line.setAttribute("x2",squarePixels*x);
    line.setAttribute("y1",mapPixels);
    line.style.stroke = gridColor;
    line.style["stroke-width"] = gridWidth;
    map.appendChild(line);
    mapSVGGridLines.push(line);
  }
  for (var x=0; x<mapSquares+1; x++) {
    var line = document.createElementNS(NS,"line");
    //<line x1="0" y1="0" x2="200" y2="200" style="stroke:rgb(255,0,0);stroke-width:2" />
    line.setAttribute("y1",squarePixels*x);
    line.setAttribute("x1",0);
    line.setAttribute("y2",squarePixels*x);
    line.setAttribute("x1",mapPixels);
    line.style.stroke = gridColor;
    line.style["stroke-width"] = gridWidth;
    map.appendChild(line);
    mapSVGGridLines.push(line);
  }

}

function drawNewPuppy(initials) {
  var text = document.createElementNS(NS,"text");
  text.setAttribute("text-anchor","middle");
  text.setAttribute("lineWidth",initialsStrokeWidth);
  text.textContent=initials;
  text.setAttribute("onmouseover","mouseOver(\""+initials+"\")");
  text.setAttribute("onmouseout","mouseOut()");
  //text.setAttribute("draggable","true");
  //text.setAttribute("ondrag","onDrag(\""+initials+"\")");
  console.log(text);
  //text.font="48px serif";
  text.setAttributeNS(null,"fill","#black");
  mapSVGPuppies[initials]=text;
  text.setAttributeNS(null,"fill","#blue");
  map.appendChild(text);

}

function drawMap(){
  if (!gameState) {console.error("can not draw map: no game loaded");return;}
  currentTime = ((new Date()).getTime()-gameState.time)/1000.;

  drawPuppies();

  if (renderTimeout) {clearInterval(renderTimeout);}
  renderTimeout=setInterval(drawMap,100);
}

var mapPixels=520; //ideally a multiple of mapSquares
var mapSquares=13;
var squarePixels = mapPixels/mapSquares;
var unexploredColor = "#666666";
var terrainColors = {"dirt":"#c9b189", "grass":"#b7d1aa"};
var gridColor = "#888888";
var gridWidth = 2;
var initialsStrokeWidth=1;
var initialsStrokeColor="#000000";
var puppyColor = "black";
var puppySelectedColor="#FF0000";

function drawTerrain(){
  var mapAsList = gameState.map.mapAsList;
  for (var i in gameState.map.mapAsList) {
    var square = mapAsList[i];
    if (square.explored) {
      fill = terrainColors[square.type];
    } else {
      fill = unexploredColor;
    }

    var square= mapSVGTerrainSquares[i];
    square.style.fill=fill;
  }
}

function drawGrid() {
  for (var i in mapSVGGridLines) {
    mapSVGGridLines[i].x1.baseVal=mapSVGGridLines[i].x1.baseVal;
  }
}

function drawPuppies() {
  // Any puppies to delete?
  var puppies = gameState.puppies;
  var puppynames = Object.getOwnPropertyNames(puppies);

  // delete removed puppies.  Should I set them to hidden instead?
  for (var initials in mapSVGPuppies) {
    if (!(initials in puppies)) {
      // delete the puppy
      mapSVGPuppies[initials].remove();
    }
  }

  //Any puppies to add?
  for (var i in puppynames) {
    initials=puppynames[i];
    if (!(initials in mapSVGPuppies)) {
      drawNewPuppy(initials);
    }
  }

  for (var i in puppynames) {
    initials = puppynames[i];
    var puppy = puppies[initials];
    var text = mapSVGPuppies[initials];

    var x0 = puppy.currentLocation.x;
    var y0 = puppy.currentLocation.y;
    var x1 = puppy.destination.x;
    var y1 = puppy.destination.y;

    // var distance = Math.sqrt(Math.pow(x0-x1,2)+Math.pow(y0-y1,2));
    // if (distance>puppy.movementRate) {
    //   var fracTravelled = Math.min(puppy.movementRate*currentTime/distance,1);
    //
    //   x0 += (x1-x0)*fracTravelled;
    //   y0 += (y1-y0)*fracTravelled;
    // }
    text.setAttributeNS(null,"x", (x0+.5)*squarePixels);
    text.setAttributeNS(null,"y", (y0+.5)*squarePixels);
  }

}

// function mouseOut() {
//   dojo.byId("puppyMouseOver").innerHTML = "";
// }
// function mouseOver(initials) {
//   var puppy = gameState.puppies[initials];
//   dojo.byId("puppyMouseOver").innerHTML = JSON.stringify(puppy);
// }
