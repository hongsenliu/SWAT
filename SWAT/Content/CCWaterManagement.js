$(document).ready(function () {
    $.showHideWaterAdmin = function () {
        var waterCom = $.trim($('#watCom option:selected').text());

        if (waterCom == "There is no Water Committee")
        {
            $('#watAdminOther').prop('disabled', false);
        }
        else
        {
            $('#watAdminOther').val("");
            $('#watAdminOther').prop('disabled', true);
        }

        $('#watAdminOther').attr('cols', 80);
        $('#watAdminOther').attr('rows', 6);
    }

    $('#watCom').change($.showHideWaterAdmin);
    $.showHideWaterAdmin();
});