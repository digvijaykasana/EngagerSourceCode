$('#btnSave').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    this.form.submit();
});

$('#PostalCode').blur(function (e) {
    var postalCode = $(this).val();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    $.get('/Hotel/GetCoordinate',
        {
            aPostalCode: postalCode
        }, function success(data, status) {
            $('#divLoading').modal('toggle');
            $('#Latitude').val(data.Latitude);
            $('#Longitude').val(data.Longitude);
        });
})