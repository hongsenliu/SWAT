$(document).ready(function () {
    $.showHidelivestockEffluent = function () {
        if ($('#livestock1').prop('checked')) {
            $('#livestockEffluent').prop('disabled', false);
        }
        else {
            $('#livestockEffluent').val("");
            $('#livestockEffluent').prop('disabled', true);
        }
    }

    $('#livestock1').change($.showHidelivestockEffluent);
    $.showHidelivestockEffluent();
});