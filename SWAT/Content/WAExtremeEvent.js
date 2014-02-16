window.onload = function () {
    showOrHideOtherEvent();
};

function showOrHideOtherEvent() {
    var selectedOther = document.getElementById('extremeOther').value;
    document.getElementById('extremeOtherComment').disabled = true;

    if (selectedOther.trim() != "" && selectedOther.trim() != "251") {
        document.getElementById('extremeOtherComment').disabled = false;
        
    }
    else {
        document.getElementById('extremeOtherComment').value = "";
        document.getElementById('extremeOtherComment').disabled = true;
        document.getElementById("extremeOtherComment").setAttribute("class", "text-box single-line");
        document.getElementById("extremeOtherCommentErrMsg").style.visibility = "hidden";
    }
}