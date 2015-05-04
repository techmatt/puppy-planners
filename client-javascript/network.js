function networkCallJSON(string,action) {networkCallwithOptions(string,action,{handleAs: "json"})}
function networkCall(string,action) {networkCallwithOptions(string,action,{})}


function networkCallwithOptions(string,action,options) {
  console.log(string);
  require(["dojo/request"], function(request) {
    var requestText=URL+"p&"+string;
    request.get(requestText, options).then(
      function(text) {
        console.log("Returned text: ", text);
        action(text);
      },
      function(error) {
        console.log("An error occurred: " + error);
      }
    );
  });
}

function networkCreateSession() {
  var textName = document.getElementById('textName').value;
  var requestText = "newSession&sessionName=" + textName;
  networkCall(requestText,
      function(text) {
        console.log("Returned text: ", text);
        sessionid = text[0].sessionID;
      });
}

function networkAssignPuppyTask (intials,x,y,task) {
  var request = "assignPuppyTask&session=" + sessionID + "&puppy=" + intials + "&x=" + x + "&y="+y+"&task="+task;
  networkCall(request, function (text) {});
}
function networkAssignPuppyToRole (intials,role) {
  var request = "assignPuppyToRole&session=" + sessionID + "&puppy=" + intials +"&role="+role;
  networkCall(request, function (text) {});
}
function networkMovePuppy (intials,x,y) {
  var request = "movePuppy&session=" + sessionID + "&puppy=" + intials + "&x=" + x + "&y="+y;
  networkCall(request, function (text) {});
}
function networkAssignPuppyToTask (intials,role,task) {
  var request = "assignPuppyToTask&session=" + sessionID + "&puppy=" + intials +"&role="+role+"&task="+task;
  networkCall(request, function (text) {});
}
function networkBuild (building,x,y) {
  var request = "buildBuilding&session=" + sessionID + "&name=" + building + "&x=" + x + "&y="+y;
  networkCall(request, function (text) {});
}

function networkJoinSession() {
  var textName = document.getElementById('textName').value;
  var textRole = document.getElementById('textRole').value;
  var textPlayer = document.getElementById('textPlayer').value;
  if (!sessionID) {return;}

  var requestText = "joinSession&session=" + sessionID + "&playerName=" + textPlayer + "&role=" + textRole;
  networkCall(requestText,
    function(text) {
      console.log("Returned text: ", text);
    });
}

function networkCreateSession() {
  var textName = document.getElementById('textName').value;
  var requestText = "newSession&sessionName=" + textName;

  networkCall(requestText,
    function(text) {
      console.log("Returned text: ", text);
      sessionid = text[0].sessionID;
      networklistSessions();
    });
}

function networkListSessions() {
    var requestText = "sessionList";
    networkCallJSON(requestText,
      function(text) {
        var sessionListArray = text;
        sessionList={};
        for (i in sessionListArray) {
          sessionList[sessionListArray[i].sessionID]=sessionListArray[i];
        }
        console.log("The current sessions are: ", text);
        sessionListUpdate();
      });
}


function networkGetEverything() {
  if (!sessionID) {return;}
  var requestText = "getAllState&session=" + sessionID;
  networkCallJSON(requestText,
      function(text) {
        gameState=text;
        puppyListUpdate();
        buildingListUpdate();
        updateResourceTable();
        if(!mapInitialized) {initMap();}
        console.log("tick ",gameState.data.tickCount);
      });
}
