window.onload = function () {
    showOrHidePrep();
};

function showOrHidePrep() {
    var riskFire = document.getElementById('riskFire').value;
    var riskFlood = document.getElementById('riskFlood').value;
    var riskDrought = document.getElementById('riskDrought').value;
    document.getElementById('prepFire').disabled = true;
    document.getElementById('prepFlood').disabled = true;
    document.getElementById('prepDrought').disabled = true;

    if (riskFire.trim() != "" && riskFire.trim() < "274") {
        document.getElementById('prepFire').disabled = false;

    }
    else {
        document.getElementById('prepFire').value = "";
        document.getElementById('prepFire').disabled = true;
    }

    if (riskFlood.trim() != "" && riskFlood.trim() < "274") {
        document.getElementById('prepFlood').disabled = false;

    }
    else {
        document.getElementById('prepFlood').value = "";
        document.getElementById('prepFlood').disabled = true;
    }

    if (riskDrought.trim() != "" && riskDrought.trim() < "274") {
        document.getElementById('prepDrought').disabled = false;

    }
    else {
        document.getElementById('prepDrought').value = "";
        document.getElementById('prepDrought').disabled = true;
    }
}