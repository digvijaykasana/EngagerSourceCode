$('#btnSave').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });

    var code = $('#Code').val();
    var name = $('#Name').val();
    var url = "/Vessel/CheckForSimilarVessels";

    var currentForm = this.form;

    $.post(url, { Code: code, Name: name},
        function (data, status) {
            if (data === 'NoSimilarVessel') {
                $('#divLoading').modal({ backdrop: 'static', keyboard: false });
                currentForm.submit();
            }
            else {
                $('#divLoading').modal('hide');
                alert('Vessels with similar code and name already exists. Please check before saving again!');
            }
        }
    );
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
