var map;
var currentTime = 0;
var NS="http://www.w3.org/2000/svg";
renderTimeout = null;
mapInitialized = false;
mapSVGTerrainSquares = [];
mapSVGGridLines = [];
mapSVGPuppies={};
mapPuppyHeight = 15;
mapPuppyWidth = 18;
mapPuppyTextStart = 11;
mapSquareBuildingNameOffset = 4;

//object constructors
function mapProgressBar(x,y,height,width){
  this.x=x;
  this.y=y;
  this.height=height;
  this.width=width;

  //centered progress bar
  this.container=document.createElementNS(NS,"svg");
  this.container.setAttributeNS(null,"id","mapProgressContainer");
  this.container.setAttributeNS(null,"height",height);
  this.container.setAttributeNS(null,"width",width);
  this.container.setAttributeNS(null,"x",x-width/2);
  this.container.setAttributeNS(null,"y",y-height/2);

  this.background=document.createElementNS(NS,"rect");
  this.background.setAttributeNS(null,"id","mapProgressBackground");
  this.background.setAttributeNS(null,"height",height);
  this.background.setAttributeNS(null,"width",width);
  this.container.appendChild(this.background);

  this.bar=document.createElementNS(NS,"rect");
  this.bar.setAttributeNS(null,"id","mapProgressBar");
  this.bar.setAttributeNS(null,"height",height);
  this.bar.setAttributeNS(null,"width",0);
  this.container.appendChild(this.bar);

  this.border=document.createElementNS(NS,"rect");
  this.border.setAttributeNS(null,"id","mapProgressBorder");
  //height and width need to be set explicitly as an attribute
  this.border.setAttributeNS(null,"height",height);
  this.border.setAttributeNS(null,"width",width);
  this.container.appendChild(this.border);

  this.set = function (fraction) {
    this.bar.setAttributeNS(null,"width",fraction*width);
  }
  this.hide = function() {
    this.container.setAttributeNS(null,"visibility","hidden");
  }
  this.show = function() {
    this.container.setAttributeNS(null,"visibility","visible");
  }
}

function mapPuppy(initials) {
  this.selected=false;
  this.initials=initials;

  //text object
  this.container=document.createElementNS(NS,"svg");
  this.container.setAttributeNS(null,"id","mapPuppyContainer");
  this.container.setAttributeNS(null,"height",mapPuppyHeight);
  this.container.setAttributeNS(null,"width",mapPuppyWidth);
  this.container.setAttributeNS(null,"onclick","selectPuppy(\""+initials+"\")");
  map.appendChild(this.container);

  this.background=document.createElementNS(NS,"rect");
  this.background.setAttributeNS(null,"id","mapPuppyBackground");
  this.background.setAttributeNS(null,"height",mapPuppyHeight);
  this.background.setAttributeNS(null,"width",mapPuppyWidth);
  this.container.appendChild(this.background);

  this.text = document.createElementNS(NS,"text");
  this.text.textContent=initials;
  this.text.setAttributeNS(null,"id","mapPuppyText");
  this.text.setAttributeNS(null,"x",mapPuppyWidth/2);
  this.text.setAttributeNS(null,"y",mapPuppyTextStart);
  this.container.appendChild(this.text);

  this.moveTo=function(x,y) {
    this.container.setAttributeNS(null,"x",x*squarePixels-mapPuppyWidth/2);
    this.container.setAttributeNS(null,"y",y*squarePixels-mapPuppyHeight/2);
  }
  this.remove=function() {
    mapSVGPuppies[initials].container.remove();
  }
  this.select=function() {
    this.background.setAttributeNS(null,"id","mapPuppyBackgroundSelected");
    this.selected=true;
  }
  this.unselect=function() {
    this.background.setAttributeNS(null,"id","mapPuppyBackground");
    this.selected=false;
  }
}

function mapSquare() {
  this.container=document.createElementNS(NS,"svg");
  this.container.setAttributeNS(null,"id","mapSquareContainer");
  this.container.setAttributeNS(null,"height",squarePixels);
  this.container.setAttributeNS(null,"width",squarePixels);
  map.appendChild(this.container);

  this.background=document.createElementNS(NS,"rect");
  this.background.setAttributeNS(null,"id","mapSquareBackground");
  this.background.setAttributeNS(null,"height",squarePixels);
  this.background.setAttributeNS(null,"width",squarePixels);
  this.container.appendChild(this.background);

  this.buildingName = document.createElementNS(NS,"text");
  this.buildingName.textContent="purple";
  this.buildingName.setAttributeNS(null,"id","mapPuppyText");
  this.buildingName.setAttributeNS(null,"x",squarePixels/2);
  this.buildingName.setAttributeNS(null,"y",squarePixels-mapSquareBuildingNameOffset);
  this.container.appendChild(this.buildingName);

  this.progressBar = new mapProgressBar(squarePixels/2,5,6,squarePixels/1.2);
  this.progressBar.hide();
  this.container.appendChild(this.progressBar.container);

  this.update=function (square) {
    //update square color
    if (square.explored) {
      this.background.setAttributeNS(null,"fill",terrainColors[square.type]);

      if (square.building) {
        //explored and contains a building
        if (square.building.construted) {
          this.progressBar.hide();
        } else {
          this.progressBar.show();
          this.progressBar.set(square.building.constructionProgress/square.building.info.constructionTime);
        }
      } else {
        //explored and no building
        this.progressBar.hide();
      }

      this.progressBar.hide();
    } else {
      //unexplored
      this.background.setAttributeNS(null,"fill",unexploredColor);
      if (square.explorationProgress>0) {
        this.progressBar.show();
        this.progressBar.set(square.explorationProgress/square.scoutCost);
      }

    }

    //move to the correct position
    this.container.setAttributeNS(null,"x",square.coord.x*squarePixels);
    this.container.setAttributeNS(null,"y",square.coord.y*squarePixels);
    this.container.setAttributeNS(null,"onclick","squareClick("+square.coord.x+","+square.coord.y+")");

    if (square.building) {
      this.buildingName.textContent=square.building.name;
    } else {
      this.buildingName.textContent="---";
    }
  }
}

//initialization
function initMap() {
  if (mapInitialized) {return;} else {mapInitialized=true;}
  mapInitialized=true;
  map = document.getElementById("svgMap");
  if (!gameState) {console.error("can not draw map: no game loaded");return;}

  initializeSquares();
  initializeGrid();
  drawMap();
}

function clearMap() {
  if(!mapInitialized) {return;} //don't reset if we havent drawn the map yet

  if (renderTimeout) {clearInterval(renderTimeout);} // stop rendering

  // reset our remembered SVG elements
  mapInitialized = false;
  mapSVGTerrainSquares = [];
  mapSVGGridLines = [];
  mapSVGPuppies={};

  // clear everything from the svg canvas
  while (map.lastChild) {
    map.removeChild(map.lastChild);
  }
}

function initializeSquares() {
  for (var i in gameState.map.mapAsList) {
    mapSVGTerrainSquares.push(new mapSquare());
  }
}

function initializeGrid() {
  for (var x=0; x<mapSquares+1; x++) {
    var line = document.createElementNS(NS,"line");
    map.appendChild(line);
    //<line x1="0" y1="0" x2="200" y2="200" style="stroke:rgb(255,0,0);stroke-width:2" />
    line.setAttribute("x1",squarePixels*x);
    line.setAttribute("y1",0);
    line.setAttribute("x2",squarePixels*x);
    line.setAttribute("y2",mapPixels);
    line.setAttribute("id","mapGridLine");
    map.appendChild(line);
    mapSVGGridLines.push(line);
  }
  for (var x=0; x<mapSquares+1; x++) {
    var line = document.createElementNS(NS,"line");
    //<line x1="0" y1="0" x2="200" y2="200" style="stroke:rgb(255,0,0);stroke-width:2" />
    line.setAttribute("y1",squarePixels*x);
    line.setAttribute("x1",0);
    line.setAttribute("y2",squarePixels*x);
    line.setAttribute("x2",mapPixels);
    line.setAttribute("id","mapGridLine");
    map.appendChild(line);
    mapSVGGridLines.push(line);
  }
}

function drawMap(){
  if (!gameState) {console.error("can not draw map: no game loaded");return;}
  if (!mapInitialized) {console.error("can not draw map: map not initialized");return;}

  currentTime = ((new Date()).getTime()-gameState.time)/1000.;

  updateSquares();
  updatePuppies();

  if (renderTimeout) {clearInterval(renderTimeout);}
  renderTimeout=setInterval(drawMap,100);
}

var mapPixels=520; //ideally a multiple of mapSquares
var mapSquares=13;
var squarePixels = mapPixels/mapSquares;
var unexploredColor = "#666666";
var terrainColors = {"dirt":"#c9b189", "grass":"#b7d1aa"};
var initialsStrokeColor="#000000";
var puppyColor = "black";
var puppySelectedColor="#FF0000";

function updateSquares() {
  for (var i in gameState.map.mapAsList) {
    square = gameState.map.mapAsList[i];
    mapSVGTerrainSquares[i].update(square);
  }
}

function updatePuppies() {
  // Any puppies to delete?
  var puppies = gameState.puppies;
  var puppynames = Object.getOwnPropertyNames(puppies);

  // delete removed puppies.  Should I set them to hidden instead?
  for (var initials in mapSVGPuppies) {
    if (!(initials in puppies)) {
      // delete the puppy
      mapSVGPuppies[initials].remove();
      mapSVGPuppies[initials].delete();
    }
  }

  //Any puppies to add?
  for (var i in puppynames) {
    initials=puppynames[i];
    if (!(initials in mapSVGPuppies)) {
      mapSVGPuppies[initials]=new mapPuppy(initials);
    }
  }

  for (var i in puppynames) {
    initials = puppynames[i];
    var puppy = puppies[initials];

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
    mapSVGPuppies[initials].moveTo(x0+0.5,y0+0.5);

  }

}
