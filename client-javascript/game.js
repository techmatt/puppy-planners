var URL = "http://localhost:8080/puppies/";
var sessionID;
var sessionList = [];
var gameState = null;
var selectedPuppy = null;


function init() {
  networkListSessions();

  networkUpdate = setInterval(function () {
    if (mapInitialized) {networkGetEverything()}
  },1000);
}
dojo.ready(init);


function sessionListChange () {
  var selected = document.getElementById('sessionList');
  sessionID = sessionList[selected.value].sessionID;
  // should reset everything here.
  gameState = null; clearMap();

  networkGetEverything();
}

function listUpdate (elementID, objects, labelingFunction) {
  var select = document.getElementById(elementID);
  var selected = select.value;

  while (select.length>0) {
    select.remove(0);
  }

  var objectNames = Object.getOwnPropertyNames(objects);

  var selectedIndex = -1;
  for (var i in objectNames) {
    name = objectNames[i];
    var option = document.createElement("option");
    option.text = labelingFunction(name,objects[name]);
    option.value = name;
    select.add(option);
    if (selected==option.value) {
      selectedIndex=i;
    }
  }
  select.selectedIndex=selectedIndex;
}

function buildingListUpdate () {
  listUpdate ('buildingList',gameState.database.buildings,
    function (name, object) {return name;}
  );
}

function puppyListUpdate () {
  listUpdate ('puppyList',gameState.puppies,
    function (name, object) {return name + " ("+object.name+")";}
  );
  updatePuppyDescription();
}

function sessionListUpdate () {
  listUpdate ('sessionList',sessionList,
    function (name, object) {return object.sessionName;}
  );
}

function selectPuppy(initials) {
  var select = document.getElementById('puppyList');
  console.log("Attemping to select: "+initials);

  if (selectedPuppy==initials) {
    select.selectedIndex=-1;
    selectedPuppy=null;
    updatePuppyDescription();
    return;
  }

  select.selectedIndex=i;
  for (var i=0; i<select.length;i++) {
    if (select[i].value==initials) {
      select.selectedIndex=i;
    }
  }
  selectedPuppy=initials;
  updatePuppyDescription();
}

function puppyListChange() {
  var select = document.getElementById('puppyList')
  var selected = select.value;
  if (selected) {
    selectedPuppy=selected;
  } else {
    selectedPuppy=null;
  }

  updatePuppyDescription();
}

function updatePuppyDescription() {
  var text = document.getElementById('puppyDescription')

  if (!selectedPuppy) {
    text.innerHTML="No puppy selected.";
  } else {
    // add text
    var puppy = gameState.puppies[selectedPuppy];
    text.innerHTML = JSON.stringify(puppy);

    if (!mapInitialized) {return;}
  }

  // change the color of the corresponding puppy
  var puppies = gameState.puppies;
  var puppynames = Object.getOwnPropertyNames(puppies);
  if (!mapInitialized) {return;}
  for (var i in puppynames) {
    initials = puppynames[i];
    if(selectedPuppy==initials) {
      mapSVGPuppies[initials].select();
    } else {
      mapSVGPuppies[initials].unselect();
    }
  }
}
