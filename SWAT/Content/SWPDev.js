$(document).ready(function () {
    $.showHideBestManagement = function () {
        var devsite1 = parseInt($('#devSite1').val());
        var devsite2 = parseInt($('#devSite2').val());
        var devsite3 = parseInt($('#devSite3').val());
        var devsite4 = parseInt($('#devSite4').val());
        var total = ($.isNumeric(devsite1) ? devsite1 : 0) + ($.isNumeric(devsite2) ? devsite2 : 0) + ($.isNumeric(devsite3) ? devsite3 : 0) + ($.isNumeric(devsite4) ? devsite4 : 0);
        if (total > 0) {
            $('#bestManInd').prop('disabled', false);
        }
        else {
            $('#bestManInd').val("");
            $('#bestManInd').prop('disabled', true);
        }
    }

    $('#devSite1').change($.showHideBestManagement);
    $('#devSite2').change($.showHideBestManagement);
    $('#devSite3').change($.showHideBestManagement);
    $('#devSite4').change($.showHideBestManagement);
    $.showHideBestManagement();
});