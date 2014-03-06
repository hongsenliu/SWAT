$(document).ready(function () {
    
    $.hideShowQ4to5 = function () {
        var od = parseFloat($('#ODPercent').val());
        if ($.isNumeric(od) && od > 0) {
            $('#ODdemoGender1').prop('disabled', false);
            $('#ODdemoGender2').prop('disabled', false);
            $('#ODdemoGender3').prop('disabled', false);
            $('#ODdemoGender4').prop('disabled', false);
            $('#ODdemoGenderNONE').prop('disabled', false);
            // Question 5
            $('#ODfacilitator1').prop('disabled', false);
            $('#ODfacilitator2').prop('disabled', false);
            $('#ODfacilitator3').prop('disabled', false);
            $('#ODfacilitator4').prop('disabled', false);
            $('#ODfacilitator5').prop('disabled', false);
            $('#ODfacilitator6').prop('disabled', false);
            $('#ODfacilitator7').prop('disabled', false);
            $('#ODfacilitator8').prop('disabled', false);
            $('#ODfacilitator9').prop('disabled', false);
            $('#ODfacilitator10').prop('disabled', false);
            $('#ODfacilitator11').prop('disabled', false);            
        }
        else {
            $('#ODdemoGender1').prop('checked', false);
            $('#ODdemoGender1').prop('disabled', true);
            $('#ODdemoGender2').prop('checked', false);
            $('#ODdemoGender2').prop('disabled', true);
            $('#ODdemoGender3').prop('checked', false);
            $('#ODdemoGender3').prop('disabled', true);
            $('#ODdemoGender4').prop('checked', false);
            $('#ODdemoGender4').prop('disabled', true);
            $('#ODdemoGenderNONE').prop('checked', false);
            $('#ODdemoGenderNONE').prop('disabled', true);
            // Question 5
            $('#ODfacilitator1').prop('checked', false);
            $('#ODfacilitator1').prop('disabled', true);
            $('#ODfacilitator2').prop('checked', false);
            $('#ODfacilitator2').prop('disabled', true);
            $('#ODfacilitator3').prop('checked', false);
            $('#ODfacilitator3').prop('disabled', true);
            $('#ODfacilitator4').prop('checked', false);
            $('#ODfacilitator4').prop('disabled', true);
            $('#ODfacilitator5').prop('checked', false);
            $('#ODfacilitator5').prop('disabled', true);
            $('#ODfacilitator6').prop('checked', false);
            $('#ODfacilitator6').prop('disabled', true);
            $('#ODfacilitator7').prop('checked', false);
            $('#ODfacilitator7').prop('disabled', true);
            $('#ODfacilitator8').prop('checked', false);
            $('#ODfacilitator8').prop('disabled', true);
            $('#ODfacilitator9').prop('checked', false);
            $('#ODfacilitator9').prop('disabled', true);
            $('#ODfacilitator10').prop('checked', false);
            $('#ODfacilitator10').prop('disabled', true);
            $('#ODfacilitator11').prop('checked', false);
            $('#ODfacilitator11').prop('disabled', true);
        }
    }

    $.setOD = function () {
        var shared = parseFloat($('#sharedFacPercent').val());
        var od = parseFloat($('#ODPercent').val());
        if ($.isNumeric(shared)) {
            od = 100 - shared;
            $('#ODPercent').val(od);
        }
        $.hideShowQ4to5();
    }

    $.setShared = function () {
        var shared = parseFloat($('#sharedFacPercent').val());
        var od = parseFloat($('#ODPercent').val());
        if ($.isNumeric(od)) {
            shared = 100 - od;
            $('#sharedFacPercent').val(shared);
        }
        $.hideShowQ4to5();
    }
    $.hideShowQ3to5 = function () {
        var hh = parseInt($('#hhNoToilet').val());
        if ($.isNumeric(hh) && hh > 0) {
            $('input[type=text]').prop('disabled', false);
        }
        else {
            $('input[type=text]').val("");
            $('input[type=text]').prop('disabled', true);
        }
        $.hideShowQ4to5();
    }

    $.hideShowQ2to5 = function () {
        if ($('#ODdemographicNONE').is(':checked') || !($('#ODdemographic1').is(':checked') || $('#ODdemographic2').is(':checked') || $('#ODdemographic3').is(':checked') || $('#ODdemographic4').is(':checked'))) {
            $('input[type=number]').val("");
            $('input[type=number]').prop('disabled', true);
        }
        else {
            $('input[type=number]').prop('disabled', false);
        }
        $.hideShowQ3to5();
    }

    $.trigerQ4Ans1to4 = function () {
        if ($('#ODdemoGender1').is(':checked') || $('#ODdemoGender2').is(':checked') || $('#ODdemoGender3').is(':checked') || $('#ODdemoGender4').is(':checked')) {
            $('#ODdemoGenderNONE').prop('checked', false);
        }
    }

    $.trigerQ4Ans5 = function () {
        if ($('#ODdemoGenderNONE').is(':checked')) {
            $('#ODdemoGender1').prop('checked', false);
            $('#ODdemoGender2').prop('checked', false);
            $('#ODdemoGender3').prop('checked', false);
            $('#ODdemoGender4').prop('checked', false);
        }
    }

    $.trigerQ1Ans1to4 = function () {
        if ($('#ODdemographic1').is(':checked') || $('#ODdemographic2').is(':checked') || $('#ODdemographic3').is(':checked') || $('#ODdemographic4').is(':checked')) {
            $('#ODdemographicNONE').prop('checked', false);
        }
        $.hideShowQ2to5();
    }

    $.trigerQ1Ans5 = function () {
        if ($('#ODdemographicNONE').is(':checked')) {
            $('#ODdemographic1').prop('checked', false);
            $('#ODdemographic2').prop('checked', false);
            $('#ODdemographic3').prop('checked', false);
            $('#ODdemographic4').prop('checked', false);
        }
        //else {
        //    $.showQ2to5();
        //}
        $.hideShowQ2to5();
    }

    $('#ODdemographic1').change($.trigerQ1Ans1to4);
    $('#ODdemographic2').change($.trigerQ1Ans1to4);
    $('#ODdemographic3').change($.trigerQ1Ans1to4);
    $('#ODdemographic4').change($.trigerQ1Ans1to4);
    $('#ODdemographicNONE').change($.trigerQ1Ans5);
    $('#hhNoToilet').change($.hideShowQ3to5);
    $('#sharedFacPercent').change($.setOD);
    $('#ODPercent').change($.setShared);
    $('#ODdemoGender1').change($.trigerQ4Ans1to4);
    $('#ODdemoGender2').change($.trigerQ4Ans1to4);
    $('#ODdemoGender3').change($.trigerQ4Ans1to4);
    $('#ODdemoGender4').change($.trigerQ4Ans1to4);
    $('#ODdemoGenderNONE').change($.trigerQ4Ans5);
    $.trigerQ1Ans1to4();
    $.trigerQ1Ans5();
    $.trigerQ4Ans1to4();
    $.trigerQ4Ans5();
});