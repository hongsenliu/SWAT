$(document).ready(function () {
    $.showHideFundAppSuccess = function () {
        var fundApp = $.trim($('#fundApp option:selected').text());
        if (fundApp == "Yes") {
            $('#fundAppSuccess').prop('disabled', false);
        }
        else
        {
            $('#fundAppSuccess').val("");
            $('#fundAppSuccess').prop('disabled', true);
        }
    }

    $('#fundApp').change($.showHideFundAppSuccess);
    $.showHideFundAppSuccess();
});