$(document).ready(function () {
    $.getCountryList = function () {
        if ($('#regionID > option:selected').val() == "")
        {
            $('#countryID').html("");
        }
        else
        {
            $.getJSON('/Location/Countries/List/' + $('#regionID > option:selected').attr('value'), function (data) {
                var items = "<option></option>";
                $.each(data, function (i, country) {
                    if (country.Value == $('#countryID > option:selected').attr('value')) {
                        items += "<option selected='selected' value='" + country.Value + "'>" + country.Text + "</option>";
                    }
                    else {
                        items += "<option value='" + country.Value + "'>" + country.Text + "</option>";
                    }
                });
                $('#countryID').html(items);
            });
        }
    }

    $.getSubnationList = function () {
        if ($('#countryID > option:selected').val() == "")
        {
            $('#subnationalID').html("");
        }
        else
        {
            $.getJSON('/Location/Subnations/List/' + $('#countryID > option:selected').attr('value'), function (data) {
                var items = "<option></option>";
                $.each(data, function (i, subnation) {
                    if (subnation.Value == $('#subnationalID > option:selected').attr('value')) {
                        items += "<option selected='selected' value='" + subnation.Value + "'>" + subnation.Text + "</option>";
                    }
                    else {
                        items += "<option value='" + subnation.Value + "'>" + subnation.Text + "</option>";
                    }
                });
                $('#subnationalID').html(items);
            });
        }
    }
        

    $('#regionID').change(function () {
        $.getCountryList();
        $('#subnationalID').html("");
    });

        $('#countryID').change($.getSubnationList);
        $.getCountryList();
        $.getSubnationList();
});