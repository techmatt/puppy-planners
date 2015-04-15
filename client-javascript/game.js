var URL = "http://localhost:8080/puppies/";
var sessionID;
var sessionList = [];
var gameState = null;


function init() {
  listSessions();
}
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

    listSessions();
  });
}

function sessionListChange () {
  var selected = document.getElementById('sessionSelect');
  sessionID = sessionList[selected.value].sessionID;
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

function sessionListForm () {
  var select = document.getElementById('sessionSelect')
  while (select.length>0) {
    select.remove(0);
  }

  for (var i in sessionList) {
    var option = document.createElement("option");
    option.text = sessionList[i].sessionName;
    option.value = i;
    select.add(option);
  }

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

function puppyListUpdate() {
  var select = document.getElementById('puppyList')
  var selected = select.value;

  while (select.length>0) {
    select.remove(0);
  }

  var puppies = gameState.puppies;
  var puppyNames = Object.getOwnPropertyNames(puppies);

  for (var i in puppyNames) {
    initials=puppyNames[i];
    var option = document.createElement("option");
    option.text = initials+" ("+puppies[initials].name+")";
    option.value = initials;
    select.add(option);
  }
  
}
