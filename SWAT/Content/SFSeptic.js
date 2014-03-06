$(document).ready(function () {
    $.showHideFollow2 = function () {
        var fee = $.trim($('#septicfeesCharged option:selected').text());
        if (fee == "Yes") {
            $('#septicFeesLimitAccess').prop('disabled', false);
            $('#septicFeesEnsureClean').prop('disabled', false);
        }
        else {
            $('#septicFeesLimitAccess').val("");
            $('#septicFeesLimitAccess').prop('disabled', true);
            $('#septicFeesEnsureClean').val("");
            $('#septicFeesEnsureClean').prop('disabled', true);
        }
    }

    $('#septicfeesCharged').change($.showHideFollow2);
    $.showHideFollow2();
});