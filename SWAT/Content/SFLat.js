$(document).ready(function () {
    $.showHideFollow2 = function () {
        var fee = $.trim($('#latrinefeesCharged option:selected').text());
        if (fee == "Yes") {
            $('#latrineFeesLimitAccess').prop('disabled', false);
            $('#latrineFeesEnsureClean').prop('disabled', false);
        }
        else {
            $('#latrineFeesLimitAccess').val("");
            $('#latrineFeesLimitAccess').prop('disabled', true);
            $('#latrineFeesEnsureClean').val("");
            $('#latrineFeesEnsureClean').prop('disabled', true);
        }
    }

    $('#latrinefeesCharged').change($.showHideFollow2);
    $.showHideFollow2();
});