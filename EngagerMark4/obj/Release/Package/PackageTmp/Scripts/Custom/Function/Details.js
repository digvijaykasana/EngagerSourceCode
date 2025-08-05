$('#btnSave').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    this.form.submit();
});

$('#IconsList').change(function (e) {
    var selectedIcon = $(this).val();

    $('#IconName').val(selectedIcon);
});

$(document).ready(function () {

    var selectedIcon = $('#IconName').val();

    if (selectedIcon != "") {

        $('#IconsList').val(selectedIcon);
    }
    else {
        $('#IconName').val($('#IconsList').val());
    }
});