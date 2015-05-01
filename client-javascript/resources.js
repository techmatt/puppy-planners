resourceEntries = {};
resourceTableInitialized = false;
var resourceTable;

function resourceEntry (name) {
  var database = gameState.database.resources[name];
  this.name = name;
  this.tooltip = database.flavorText;
  this.element = document.createElement("tr");
  this.element.id = "resourceRow";
  resourceTable.appendChild(this.element);

  this.update = function () {
    var data = gameState.data.resources[name];

    this.value = data.value;
    this.storage = data.storage;
    this.productionRate = data.productionPerSecond;

    this.element.innerHTML = this.displayFunction();

    if (this.value<.001) { // hide the row if the resource count is too low
      this.element.setAttribute("style","display: none;");
    } else {
      this.element.setAttribute("style","");
    }
  }

  this.displayFunction = function () {
    var out = "";
    out+="<td style=\"width: 40px; color: rgb(223, 1, 215);\">"+this.name+":</td>";
    out+="<td style=\"width: 60px; color: rgb(0, 0, 0);\">"+this.value.toFixed(2)+"</td>";
    out+="<td style=\"width: 60px; color: rgb(120, 120,120);\">/"+this.storage.toFixed(2)+"</td>";
    out+="<td style=\"width: 60px; color: rgb(0, 0, 0);\">(+"+this.productionRate.toFixed(2)+")</td>";
    return out
  }

  this.update();
}


function initializeResourceTable () {
  resourceTable = document.getElementById("resourceTable");

  var resourceNames = Object.keys(gameState.data.resources);
  resourceNames = resourceNames.sort();
  console.log(resourceNames);

  for (i in resourceNames) {
    var name = resourceNames[i];
    resourceEntries[name]=new resourceEntry(name);
  }

  resourceTableInitialized=true;

}

function updateResourceTable() {
  if (!resourceTableInitialized) {initializeResourceTable();}

  var resourceNames = Object.getOwnPropertyNames(gameState.data.resources);
  for (i in resourceNames) {
    var name = resourceNames[i];
    resourceEntries[name].update();
  }
}
