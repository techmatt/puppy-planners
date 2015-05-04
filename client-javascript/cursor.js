unselectedCursorMode = {name: "unselected", cursorStyle: 'default'};
cursorMode=unselectedCursorMode;
//document.body.style.cursor = 'default';

function updateCursor() {document.body.style.cursor = cursorMode.cursorStyle;}
function resetCursor() {cursorMode=unselectedCursorMode; updateCursor();}

document.onkeydown = function(evt) {
    evt = evt || window.event;
    console.log(evt.keyCode);
    if (evt.keyCode == 27) {
        //escape key
        resetCursor()
    }

    switch (evt.keyCode) {
      case 27:
        //escape key
        resetCursor();
        return;
      case 77:
      case 109:
        // m key
        movePuppy();
        return;
      case 66:
      case 98:
        // b key
        build();
        return;
    }
};

function getSquare(x,y) {
  var list = gameState.map.mapAsList;
  for (i in list) {
    if (list[i].coord.x==x&&list[i].coord.y==y) {return list[i]}
  }
  console.error("Failed to find square at coordinates",x,y)
  return null
}

function squareClick(x,y) {
  switch (cursorMode.name) {
    case "unselected": return;
    case "movePuppy":
      var initials = cursorMode.puppy.initials;
      //assignPuppyToSquare(initials,x,y)
      networkMovePuppy(initials,x,y);
      resetCursor();
      return;
    case "build":
      networkBuild(cursorMode.building.name,x,y);
      resetCursor()
  }
}

//Depricated
// function assignPuppyToSquare (initials, x,y) {
//   var square = getSquare(x,y);
//
//   if (square.building && !square.building.constructed) {
//     networkAssignPuppyTask(initials,x,y,"construction");
//     return;
//   }
//
//
//   networkAssignPuppyTask(cursorMode.puppy.initials,x,y,"scout");
// }

function movePuppy() {
  if (!selectedPuppy) {return;}
  if (cursorMode.name=="movePuppy") {
    resetCursor();
    return;
  }
  cursorMode = {name: "movePuppy", puppy: gameState.puppies[selectedPuppy], cursorStyle: 'crosshair'};
  updateCursor();
}

function build() {
  var select = document.getElementById('buildingList');
  var selected = select.value;
  if (!selected) {return;}

  if (cursorMode.name=="build") {
    resetCursor();
    return;
  }

  var select = document.getElementById('buildingList')
  var selected = select.value;
  if (!selected) {return;}

  cursorMode = {name: "build", building: gameState.database.buildings[selected], cursorStyle: 'crosshair'};
  updateCursor();
}
