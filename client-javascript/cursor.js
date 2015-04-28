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

function squareClick(x,y) {
  switch (cursorMode.name) {
    case "unselected": return;
    case "movePuppy":
      var initials = cursorMode.puppy.initials;
      networkAssignPuppyTask(cursorMode.puppy.initials,x,y,"scout");
      resetCursor()
      return;
    case "build":
      networkBuild(cursorMode.building.name,x,y);
      resetCursor()
  }
}

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
  if (!selectedPuppy) {return;}
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
