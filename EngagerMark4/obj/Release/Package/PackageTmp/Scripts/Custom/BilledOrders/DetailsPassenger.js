$('.clsAddPassenger').click(function (e) {
    e.preventDefault();
    clearPassenger();
    $('#btnAddPassenger').attr('style', 'display:normal;');
    $('#btnEditPassenger').attr('style', 'display:none;');
    $('#workOrderPassengerModal').modal({ backdrop: 'static', keyboard: false });
});

$('body').on('click', 'a.btnEditPassenger', function () {
    var tableIndex = $(this).parents('tr').attr('tableIndex');
    var rankId = $('#' + tableIndex + 'RankId').val();
    var vehicleId = $('#' + tableIndex + 'VehicleId').val();
    var passengerName = $('#' + tableIndex + 'Name').val();
    var inCharge = $('#' + tableIndex + 'InCharge').prop('checked');
    var noOfPax = $('#' + tableIndex + 'NoOfPax').val();

    if (rankId)
        $('#Ranks').val(rankId);
    else
        $('#Ranks').val(null);
    if (vehicleId) {
        $('#Vehicles').val(vehicleId);
    }
    else {
        $('#Vehicles').val(null);
    }
    $('#chkInCharge').prop('checked', inCharge);
    $('#txtPassengerName').val(passengerName);
    $('#txtNoOfPax').val(noOfPax);
    $('#tablePassengerIndex').val(tableIndex);
    $('#btnAddPassenger').attr('style', 'display:none;');
    $('#btnEditPassenger').attr('style', 'display:normal;');
    $('#workOrderPassengerModal').modal('show');
});

$('#btnPassengerClose').click(function (e) {
    var tableIndex = $('#tablePassengerIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#PassengerContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#WorkOrderPassengerList_' + lastIndex + "__RankId").parents('tr').last().remove();
    }
})

$('#btnAddPassenger').click(function (e) {
    e.preventDefault();
    if (savePassenger()) {
        $('#workOrderPassengerModal').modal('toggle');
    }
})

$('#btnEditPassenger').click(function (e) {
    e.preventDefault();
    var tableIndex = $('#tablePassengerIndex').val();
    var incharge = $('#chkInCharge').prop('checked');
    var passengerName = $('#txtPassengerName').val();
    var rankId = $('#Ranks').val();
    var rank = $('#Ranks option:selected').text();
    var vehicleId = $('#Vehicles').val();
    var vehicle = $('#Vehicles option:selected').text();
    var noOfPax = $('#txtNoOfPax').val();
    if (!passengerName) {
        alert("Passenger Name required!");
        $('#txtPassengerName').focus();
        return false;
    }
    $('#' + tableIndex + 'RankId').val(rankId);
    $('#' + tableIndex + 'VehicleId').val(vehicleId);
    $('#' + tableIndex + 'InCharge').prop('checked', incharge);
    $('#' + tableIndex + 'InCharge').val(incharge);
    $('#' + tableIndex + 'Name').val(passengerName);
    $('#' + tableIndex + 'Rank').val(rank);
    $('#' + tableIndex + 'NoOfPax').val(noOfPax);
    $('#' + tableIndex + 'Vehicle_VehicleNo').val(vehicle);
    $('#workOrderPassengerModal').modal('toggle');
})

function clearPassenger() {
    $('#tablePassengerIndex').val(null);
    $('#chkInCharge').prop('checked', false);
    $('#txtPassengerName').val(null);
    $('#Ranks').val(null);
    $('#Vehicles').val(null);
    $('#txtNoOfPax').val(0);
}

function savePassenger() {
    var incharge = $('#chkInCharge').prop('checked');
    var passengerName = $('#txtPassengerName').val();
    var rankId = $('#Ranks').val();
    var rank = $('#Ranks option:selected').text();
    var vehicleId = $('#Vehicles').val();
    var vehicle = $('#Vehicles option:selected').text();
    var noOfPax = $('#txtNoOfPax').val();

    if (!passengerName) {
        alert("Passenger Name required!");
        $('#txtPassengerName').focus();
        return false;
    }

    var rows = $('tbody#PassengerContainer').find("tr");
    var lastIndex = rows.last().index();
    $('#WorkOrderPassengerList_' + lastIndex + '__RankId').val(rankId);
    $('#WorkOrderPassengerList_' + lastIndex + '__VehicleId').val(vehicleId);
    $('#WorkOrderPassengerList_' + lastIndex + '__InCharge').prop('checked', incharge);
    $('#WorkOrderPassengerList_' + lastIndex + '__InCharge').val(incharge);
    $('#WorkOrderPassengerList_' + lastIndex + '__Name').val(passengerName);
    $('#WorkOrderPassengerList_' + lastIndex + '__Rank').val(rank);
    $('#WorkOrderPassengerList_' + lastIndex + '__NoOfPax').val(noOfPax);
    $('#WorkOrderPassengerList_' + lastIndex + '__Vehicle_VehicleNo').val(vehicle);
    return true;
}



$('#btnCancelPassenger').click(function (e) {
    var tableIndex = $('#tablePassengerIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#PassengerContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#WorkOrderPassengerList_' + lastIndex + "__RankId").parents('tr').last().remove();
    }
})


