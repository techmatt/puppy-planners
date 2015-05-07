var URL = "http://localhost:8080/puppies/";
var sessionID;
var sessionList = [];
var gameState = null;
var selectedPuppy = null;


function init() {

  networkUpdate = setInterval(function () {
    {networkGetEverything()}
  },1000);
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

function buildingListUpdate () {
  listUpdate ('buildingList',gameState.database.buildings,
    function (name, object) {return object.displayName;}
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

  // Create the menus for owner and task selection
  if (selectedPuppy) {
    var assignedPlayer = gameState.puppies[selectedPuppy].assignedPlayer;
    var task = gameState.puppies[selectedPuppy].task;
    drawList("playerRolesList", assignedPlayer, gameState.database.playerRoles, function(name,object){return object.displayName;});
    drawListValue("tasksList", task, gameState.database.playerRoles[assignedPlayer].tasks, function(name,object){return object;},function(name,object){return object;});
  } else {
    drawList("playerRolesList", null, {}, function(name,object){return "";});
    drawListValue("tasksList", null, {}, function(object){return "";},function(name,object){return "";});
  }
}

function playerRolesListChange () {
  var select = document.getElementById("playerRolesList");
  networkAssignPuppyToRole(selectedPuppy,select.value);
}
function tasksListChange () {
  var selectRoles = document.getElementById("playerRolesList");
  var selectTasks = document.getElementById("tasksList");
  networkAssignPuppyToTask(selectedPuppy,selectRoles.value,selectTasks.value);
}
