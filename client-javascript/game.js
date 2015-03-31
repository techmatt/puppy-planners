var URL = "http://localhost:8080/puppies/";
var sessionID;
var sessionList = [];
var gameState = null;


function init() {}
dojo.ready(init);


function createSession() {
  require(["dojo/request"], function(request) {
    var textName = document.getElementById('textName').value;
    var requestText = URL + "p&newSession&sessionName=" + textName;

    request.get(requestText).then(
      function(text) {
        console.log("Returned text: ", text);
        sessionid = text[0].sessionID;
      },
      function(error) {
        console.log("An error occurred: " + error);
      }
    );
  });
}

function joinSession() {
  require(["dojo/request"], function(request) {
    var textName = document.getElementById('textName').value;
    var textRole = document.getElementById('textRole').value;
    var textPlayer = document.getElementById('textPlayer').value;

    tempSessionID = null;
    for (var i in sessionList) {
      if (sessionList[i].sessionName == textName) {
        tempSessionID = sessionList[i].sessionID;
        break;
      }
    }
    if (!tempSessionID) {
      console.error("Failed to find session " + textName + ". Try reloading sessions.");
      return;
    }
    sessionID = tempSessionID;

    var requestText = URL + "p&joinSession&session=" + sessionID + "&playerName=" + textPlayer + "&role=" + textRole;

    request.get(requestText).then(
      function(text) {
        console.log("Returned text: ", text);
      },
      function(error) {
        console.log("An error occurred: " + error);
      }
    );
  });
}

function sessionListToString () {
  var out = "";
  for (var i in sessionList) {
    out+=sessionList[i].sessionName+"<br />";
    var players = sessionList[i].players;
    for (var j in players){
      out+="&nbsp&nbsp&nbsp&nbsp"+players[j].role+": "+players[j].name+"<br />\n";
    }
  }
  return out;
}

function listSessions() {
  require(["dojo/request"], function(request) {
    var requestText = URL+"p&sessionList";
    request.get(requestText, {handleAs: "json"}).then(
      function(text) {
        sessionList = text;
        console.log("The current sessions are: ", text);
        dojo.byId("sessionListDisplay").innerHTML = sessionListToString();
      },
      function(error) {
        console.log("An error occurred: " + error);
      }
    );
  });
}


function getEverything() {
  require(["dojo/request"], function(request) {
    var textName = document.getElementById('textName').value;

    tempSessionID = null;
    for (var i in sessionList) {
      if (sessionList[i].sessionName == textName) {
        tempSessionID = sessionList[i].sessionID;
        break;
      }
    }
    if (!tempSessionID) {
      console.error("Failed to find session " + textName + ". Try reloading sessions.");
      return;
    }
    sessionID = tempSessionID;

    var requestText = URL + "p&getAllState&session=" + sessionID;
    console.log(requestText);
    request.get(requestText, {handleAs: "json"}).then(
      function(text) {
        gameState=text;
        console.log("Returned text: ", text);
      },
      function(error) {
        console.log("An error occurred: " + error);
      }
    );
  });
}

function drawMap(){
  drawTerrain();

}



var mapPixels=520; //ideally a multiple of mapSquares
var mapSquares=13;
unexploredColor = "#000000";
terrainColors = {"dirt":"#996915", "grass":"#99CF84"};
function drawTerrain(){
  if (!gameState) {console.error("can not draw map: no game loaded");return;}
  var map = document.getElementById("canvasMap").getContext("2d");

  var mapAsList = gameState.map.mapAsList;
  var squarePixels = mapPixels/mapSquares;
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
