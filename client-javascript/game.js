var URL = "http://localhost:8080/puppies/";
var sessionID;
var sessionList = [];
var gameState = null;
var selectedPuppy = null;


function init() {
  listSessions();

  networkUpdate = setInterval(function () {
    if (mapInitialized) {getEverything()}
  },1000);
}
dojo.ready(init);


function sessionListChange () {
  var selected = document.getElementById('sessionSelect');
  sessionID = sessionList[selected.value].sessionID;
  // should reset everything here.
  gameState = null; clearMap();

  getEverything();
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

function puppyListUpdate() {
  var select = document.getElementById('puppyList')
  var selected = select.value;

  while (select.length>0) {
    select.remove(0);
  }

  var puppies = gameState.puppies;
  var puppyNames = Object.getOwnPropertyNames(puppies);

  var selectedIndex=-1;
  for (var i in puppyNames) {
    initials=puppyNames[i];
    var option = document.createElement("option");
    option.text = initials+" ("+puppies[initials].name+")";
    option.value = initials;
    select.add(option);
    if (selected==initials) {
      selectedIndex=i;
    };
  }
  select.selectedIndex=selectedIndex;
  updatePuppyDescription();
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
