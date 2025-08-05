$('#btnSave').click(function (e) {

    var meetingServiceName = $('#Name').val();

    if (meetingServiceName == null || meetingServiceName == "") {
        alert("Name is a required field!");
        $('#Name').focus();
        return false;
    }

    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    this.form.submit();
});

$('.clsAddmsDetails').click(function (e) {
    e.preventDefault();
    clearModalData();
    $('#btnAdd').attr('style', 'display:normal');
    $('#btnEditmsDetails').attr('style', 'display:none');
    $('#msDetailsModal').modal({ backdrop: 'static', keyboard: false });
});


$('#btnCancel').click(function (e) {
    $('#btnAddmsDetails').attr('style', 'display:normal');

    var rows = $("tbody#msDetailsContainer").find("tr");
    var tableIndex = $('#tableIndex').val();
    if (tableIndex) {

    } else {
        var lastIndex = rows.last().index();
        $('#MeetingServiceDetails_' + lastIndex + "__Id").parents('tr').last().remove();
    }
});

$('#btnClose').click(function (e) {

    $('#btnAddmsDetails').attr('style', 'display:normal');

    var rows = $("tbody#msDetailsContainer").find("tr");
    var tableIndex = $('#tableIndex').val();
    if (tableIndex) {

    } else {
        var lastIndex = rows.last().index();
        $('#MeetingServiceDetails_' + lastIndex + "__Id").parents('tr').last().remove();
    }
})

function clearModalData() {
    $('#txtNoOfPax').val('');
    $('#txtMinPax').val('');
    $('#txtMaxPax').val('');
    $('#txtCharges').val('');
    $('#txtSerial').val('');
}

$('#btnAddmsDetails').click(function (e) {
    e.preventDefault();
    if (savemsDetails()) {
        $('#msDetailsModal').modal('toggle');
    }
});

function  savemsDetails() {

    var noOfPax = $('#txtNoOfPax').val();
    var minPax = $('#txtMinPax').val();
    var maxPax = $('#txtMaxPax').val();
    var charges = $('#txtCharges').val();
    var serial = $('#txtSerial').val();

    if (!noOfPax) {
        alert('Field "Description." is a required field.');
        $('#txtNoOfPax').focus();
        return false;
    }

    if (!minPax) {
        alert('Field "Min Pax" is a required field.');
        $('#txtMinPax').focus();
        return false;
    }

    if (!maxPax) {
        alert('Field "Max Pax is a required field.');
        $('#txtMaxPax').focus();
        return false;
    }

    if (!charges) {
        alert('Field "Charges" is a required field.');
        $('#txtCharges').focus();
        return false;
    }

    if (!serial) {
        alert('Field "Serial" is a required field.');
        $('#txtSerial').focus();
        return false;
    }

    var rows = $('tbody#msDetailsContainer').find('tr');
    var lastIndex = rows.last().index();

    $('#MeetingServiceDetails_' + lastIndex + "__NoOfPax").val(noOfPax);
    $('#MeetingServiceDetails_' + lastIndex + "__MinPax").val(charges);
    $('#MeetingServiceDetails_' + lastIndex + "__MaxPax").val(charges);
    $('#MeetingServiceDetails_' + lastIndex + "__Charges").val(charges);
    $('#MeetingServiceDetails_' + lastIndex + "__Serial").val(serial);
    
    return true;
}

$('body').on('click', 'a.btnEdit', function () {
    var tableIndex = $(this).parents('tr').attr('tableIndex');

    var noOfPax = $('#' + tableIndex + "NoOfPax").val();
    var minPax = $('#' + tableIndex + "MinPax").val();
    var maxPax = $('#' + tableIndex + "MaxPax").val();
    var charges = $('#' + tableIndex + "Charges").val();
    var serial = $('#' + tableIndex + "Serial").val();

    $('#tableIndex').val(tableIndex);

    $('#txtNoOfPax').val(noOfPax);
    $('#txtMinPax').val(minPax);
    $('#txtMaxPax').val(maxPax);
    $('#txtCharges').val(charges);
    $('#txtSerial').val(serial);
    
    $('#btnAddmsDetails').attr('style', 'display:none');
    $('#btnEditmsDetails').attr('style', 'display:normal');
    $('#msDetailsModal').modal('show');
});

$('#btnEditmsDetails').click(function (e) {
    e.preventDefault();
  

    var noOfPax = $('#txtNoOfPax').val();
    var minPax = $('#txtMinPax').val();
    var maxPax = $('#txtMaxPax').val();
    var charges = $('#txtCharges').val();
    var serial = $('#txtSerial').val();
    

    if (!noOfPax) {
        alert('Field "Description" is a required field.');
        $('#txtNoOfPax').focus();
        return false;
    }

    if (!minPax) {
        alert('Field "Min Pax" is a required field.');
        $('#txtMinPax').focus();
        return false;
    }

    if (!maxPax) {
        alert('Field "Max Pax is a required field.');
        $('#txtMaxPax').focus();
        return false;
    }

    if (!charges) {
        alert('Field "Charges" is a required field.');
        $('#txtCharges').focus();
        return false;
    }

    if (!serial) {
        alert('Field "Serial" is a required field.');
        $('#txtSerial').focus();
        return false;
    }

    $('#btnAddmsDetails').attr('style', 'display:normal');
    $('#msDetailsModal').modal('toggle');

    var tableIndex = $('#tableIndex').val();
    //var tableIndex = $(this).parents('tr').attr('tableIndex');
    $('#' + tableIndex + "NoOfPax").val(noOfPax);
    $('#' + tableIndex + "Charges").val(charges);
    $('#' + tableIndex + "Serial").val(serial);
    $('#' + tableIndex + "MinPax").val(minPax);
    $('#' + tableIndex + "MaxPax").val(maxPax);
});



