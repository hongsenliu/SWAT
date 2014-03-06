$(document).ready(function () {
    $.showHideFollow2 = function () {
        var fee = $.trim($('#centralfeesCharged option:selected').text());
        if (fee == "Yes") {
            $('#centralFeesLimitAccess').prop('disabled', false);
            $('#centralFeesEnsureClean').prop('disabled', false);
        }
        else {
            $('#centralFeesLimitAccess').val("");
            $('#centralFeesLimitAccess').prop('disabled', true);
            $('#centralFeesEnsureClean').val("");
            $('#centralFeesEnsureClean').prop('disabled', true);
        }
    }

    $('#centralfeesCharged').change($.showHideFollow2);
    $.showHideFollow2();
});