$(document).ready(function () {
    $.showHideDomDanger = function () {
        var dangerID = $('#domcollectDanger option:selected').val();
        if (dangerID < 1223 && $.isNumeric(dangerID)) {
            $('#domdangerType1').prop('disabled', false);
            $('#domdangerType2').prop('disabled', false);
            $('#domdangerType3').prop('disabled', false);
        }
        else {
            $('#domdangerType1').prop('checked', false);
            $('#domdangerType2').prop('checked', false);
            $('#domdangerType3').prop('checked', false);
            $('#domdangerType1').prop('disabled', true);
            $('#domdangerType2').prop('disabled', true);
            $('#domdangerType3').prop('disabled', true);
        }
    }

    $.showHideInDanger = function () {
        var dangerID = $('#incollectDanger option:selected').val();
        if (dangerID < 1223 && $.isNumeric(dangerID)) {
            $('#indangerType1').prop('disabled', false);
            $('#indangerType2').prop('disabled', false);
            $('#indangerType3').prop('disabled', false);
        }
        else {
            $('#indangerType1').prop('checked', false);
            $('#indangerType2').prop('checked', false);
            $('#indangerType3').prop('checked', false);
            $('#indangerType1').prop('disabled', true);
            $('#indangerType2').prop('disabled', true);
            $('#indangerType3').prop('disabled', true);
        }
    }

    $.showHidewpaInter = function () {
        var rankID = $('#wpaInterruptionFreq option:selected').val();
        if (rankID != 1545 && $.isNumeric(rankID)) {
            $('#wpaInterruption1').prop('disabled', false);
            $('#wpaInterruption2').prop('disabled', false);
            $('#wpaInterruption3').prop('disabled', false);
            $('#wpaInterruption4').prop('disabled', false);
            $('#wpaInterruption5').prop('disabled', false);
            $('#wpaInterruption6').prop('disabled', false);
        }
        else {
            $('#wpaInterruption1').prop('checked', false);
            $('#wpaInterruption2').prop('checked', false);
            $('#wpaInterruption3').prop('checked', false);
            $('#wpaInterruption4').prop('checked', false);
            $('#wpaInterruption5').prop('checked', false);
            $('#wpaInterruption6').prop('checked', false);
            $('#wpaInterruption1').prop('disabled', true);
            $('#wpaInterruption2').prop('disabled', true);
            $('#wpaInterruption3').prop('disabled', true);
            $('#wpaInterruption4').prop('disabled', true);
            $('#wpaInterruption5').prop('disabled', true);
            $('#wpaInterruption6').prop('disabled', true);
        }
    }

    $('#domcollectDanger').change($.showHideDomDanger);
    $('#incollectDanger').change($.showHideInDanger);
    $('#wpaInterruptionFreq').change($.showHidewpaInter);
    $.showHideDomDanger();
    $.showHideInDanger();
    $.showHidewpaInter();
});