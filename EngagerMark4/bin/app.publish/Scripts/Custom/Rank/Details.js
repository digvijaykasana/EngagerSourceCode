$('#btnSave').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    this.form.submit();
});

$('.clsAddVessel').click(function (e) {
    e.preventDefault();
    var vesselId = $('#Customers').val();
    var vessel = $('#Customers option:selected').text();
    var rows = $('tbody#VesselContainer').find('tr');
    var lastIndex = rows.last().index();
    if (!vesselId) {
        alert('Please select a customer!');
        $('#CustomerList_' + lastIndex + "__CustomerId").parents('tr').last().remove();
        return;
    }
    rows.each(function (index, e) {
        var isDeleted = $('#CustomerList_' + index + "__Delete").val();
        if (isDeleted != 'True') {
            var inFunctionId = $('#CustomerList_' + index + '__CustomerId').val();
            if (inFunctionId == vesselId) {
                alert('Customer is already in the list!');
                $('#CustomerList_' + lastIndex + "__CustomerId").parents('tr').last().remove();
                return;
            }
        }
    });
    $('#CustomerList_' + lastIndex + "__CustomerId").val(vesselId);
    $('#CustomerList_' + lastIndex + "__Customer").val(vessel);
});
