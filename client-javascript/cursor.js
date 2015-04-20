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
        // m key
        movePuppy();
        return;
    }
};

function squareClick(x,y) {
  switch (cursorMode.name) {
    case "unselected": return;
    case "movePuppy":
      var initials = cursorMode.puppy.initials;
      assignPuppyTask(cursorMode.puppy.initials,x,y,"scout");
      resetCursor()
      return;
  }
}

function movePuppy() {
  if (cursorMode.name=="movePuppy") {
    resetCursor();
    return;
  }
  cursorMode = {name: "movePuppy", puppy: gameState.puppies[selectedPuppy], cursorStyle: 'crosshair'};
  updateCursor();
}
