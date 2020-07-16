$(document).ready(function () {
    $('.btnclass').bind('click', function () {
        var btnval = $(this).attr('value');
        var btnsplit = $(this).attr('id').split("-");
        var id = btnsplit[2];
        
        var status = $('#status'+id);
        var category = $('#category'+id);
        var description = $('#description'+id);
        var expectedres = $('#expected'+id);
        var realres = $('#reality'+id);
        var optional = $('#optional'+id);

        if (btnval == "disabled") {
            status.prop('disabled', false);
            category.prop('disabled', false);
            description.prop('readonly', false);
            expectedres.prop('readonly', false);
            realres.prop('readonly', false);
            optional.prop('readonly', false);
            $(this).prop('value', 'enabled');
        }
        else {
            status.prop('disabled', true);
            category.prop('disabled', true);
            description.prop('readonly', true);
            expectedres.prop('readonly', true);
            realres.prop('readonly', true);
            optional.prop('readonly', true);
            $(this).prop('value', 'disabled');
        }
        
    });
});