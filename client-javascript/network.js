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

function createSession() {
  var textName = document.getElementById('textName').value;
  var requestText = "newSession&sessionName=" + textName;
  networkCall(requestText,
      function(text) {
        console.log("Returned text: ", text);
        sessionid = text[0].sessionID;
      });
}

function assignPuppyTask (intials,x,y,task) {
  var request = "assignPuppyTask&session=" + sessionID + "&puppy=" + intials + "&x=" + x + "&y="+y+"&task="+task;
  networkCall(request, function (text) {});
}


function joinSession() {
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

function createSession() {
  var textName = document.getElementById('textName').value;
  var requestText = "newSession&sessionName=" + textName;

  networkCall(requestText,
    function(text) {
      console.log("Returned text: ", text);
      sessionid = text[0].sessionID;
      listSessions();
    });
}

function listSessions() {
    var requestText = "sessionList";
    networkCallJSON(requestText,
      function(text) {
        sessionList = text;
        console.log("The current sessions are: ", text);
        sessionListForm();
      });
}


function getEverything() {
  if (!sessionID) {return;}
  var textName = document.getElementById('textName').value;
  var requestText = "getAllState&session=" + sessionID;
  networkCallJSON(requestText,
      function(text) {
        gameState=text;
        puppyListUpdate();
      });
}
