$(document).ready(function () {
    $.showHideToilet = function () {
        var toiletAll = $.trim($('#toiletsAll option:selected').text());
        if (toiletAll == "yes") {
            $('input[type=text]').val("");
            $('input[type=text]').prop('disabled', true);
        }
        else {
            $('input[type=text]').prop('disabled', false);
        }
    }

    $('#toiletsAll').change($.showHideToilet);
    $.showHideToilet();
});