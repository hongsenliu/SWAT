
function greyOutPrecipitation() {
    var precipVar = document.getElementById('precipVar').value;
    var precipVarAlt = document.getElementById('precipVarALT').value;

    if (precipVar.trim() != "") {
        document.getElementById('precipVarALT').value = " ";
        document.getElementById('precipVarALT').disabled = true;
    }
    else if (precipVarAlt.trim() != "") {
        document.getElementById('precipVar').value = "";
        document.getElementById('precipVar').disabled = true;
    }
    else {
        document.getElementById('precipVarALT').value = " ";
        document.getElementById('precipVarALT').disabled = false;
        document.getElementById('precipVar').value = "";
        document.getElementById('precipVar').disabled = false;
    }
}
