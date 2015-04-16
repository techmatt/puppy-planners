function networkCall(string, action) {
  require(["dojo/request"], function(request) {
    var requestText=URL+"p&"+string;
    request.get(requestText).then(
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

function networkCallJSON(string) {
  require(["dojo/request"], function(request) {
    var requestText=URL+"p&"+string;
    request.get(requestText, {handleAs: "json"}).then(
      function(text) {
        console.log("Returned text: ", text);
        return text;
      },
      function(error) {
        console.log("An error occurred: " + error);
        return null;
      }
    );
  });
}

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

    listSessions();
  });
}

function assignPuppyTask (intials,x,y,task) {
  var request = "p&assignPuppyTask&session=" + sessionID + "&puppy=" + intials + "&x=" + x + "&y="+y+"&task="+task;
  networkCall(request, function (text) {});
}


function joinSession() {
  require(["dojo/request"], function(request) {
    var textName = document.getElementById('textName').value;
    var textRole = document.getElementById('textRole').value;
    var textPlayer = document.getElementById('textPlayer').value;


    if (!sessionID) {return;}
    console.log(sessionID);

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

    listSessions();
  });
}

function listSessions() {
  require(["dojo/request"], function(request) {
    var requestText = URL+"p&sessionList";
    request.get(requestText, {handleAs: "json"}).then(
      function(text) {
        sessionList = text;
        console.log("The current sessions are: ", text);
        sessionListForm();
      },
      function(error) {
        console.log("An error occurred: " + error);
      }
    );
  });
}


function getEverything() {
  if (!sessionID) {return;}
  require(["dojo/request"], function(request) {
    var textName = document.getElementById('textName').value;
    var requestText = URL + "p&getAllState&session=" + sessionID;
    console.log(requestText);
    request.get(requestText, {handleAs: "json"}).then(
      function(text) {
        gameState=text;
        puppyListUpdate();
        console.log("Returned text: ", text);
      },
      function(error) {
        console.log("An error occurred: " + error);
      }
    );
  });
}
