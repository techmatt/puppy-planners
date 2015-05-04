var URL = "http://localhost:8080/puppies/";
var sessionID;
var sessionList = [];
var gameState = null;
var selectedPuppy = null;


function init() {
  networkListSessions();

  networkUpdate = setInterval(function () {
    networkListSessions();
  },10*1000);
}
dojo.ready(init);

function listUpdate (elementID, objects, labelingFunction) {
  var select = document.getElementById(elementID);
  var selected = select.value;

  drawList (elementID, selected, objects, labelingFunction)
}

function drawList (elementID, targetValue, objects, labelingFunction) {
  drawListValue(elementID, targetValue, objects, labelingFunction, function (name, object) {return name});
}

function drawListValue (elementID, targetValue, objects, labelingFunction,valueFunction) {

  var select = document.getElementById(elementID);

  // delete everything
  while (select.length>0) {
    select.remove(0);
  }

  if (!objects) {return;}

  var objectNames = Object.getOwnPropertyNames(objects);

  var selectedIndex = -1;

  var currentIndex=0;
  for (var name in objects) {
    if (!i) {continue;}
    var option = document.createElement("option");
    option.text = labelingFunction(name,objects[name]);
    option.value = valueFunction(name,objects[name]);
    select.add(option);
    if (targetValue==option.value) {
      selectedIndex=currentIndex;
    }
    currentIndex++;
  }
  select.selectedIndex=selectedIndex;
}

function sessionListUpdate () {
  listUpdate ('sessionList',sessionList,
    function (name, object) {return object.sessionName;}
  );
}

function sessionListChange() {
  var session = document.getElementById('sessionList');
  var sessionID = session.value;

  var links = document.getElementById('roleLinks');
  while (links.length>0) {
    select.remove(0);
  }

  var roles = ["master","builder","military","intrgue","culture"];
  var roleNames = ["all", "Builder", "Military","Intrgue","Culture"];
  for (i in roles) {
    var par = document.createElement("p")
    var link = document.createElement("a");
    link.href = URL+roles[i]+".html"+"?session="+sessionID;
    link.innerHTML = roleNames[i];

    par.appendChild(link);
    links.appendChild(par);
  }

}
