$('.clsAddMeetingService').click(function (e) {
    e.preventDefault();
    clearMeetingService();
    $('#btnAddMeetingService').attr('style', 'display:normal;');
    $('#btnEditMeetingService').attr('style', 'display:none;');
    $('#workOrderMeetingServiceModal').modal({ backdrop: 'static', keyboard: false });
});

$('body').on('click', 'a.btnEditMeetingService', function () {
    var tableIndex = $(this).parents('tr').attr('tableIndex');
    var meetingServiceId = $('#' + tableIndex + 'AirportMeetingServiceId').val();
    var vesselId = $('#' + tableIndex + 'VesselId').val();
    var flightNo = $('#' + tableIndex + 'FlightNo').val();
    var noOfPax = $('#' + tableIndex + 'NoOfPax').val();
    var charges = $('#' + tableIndex + 'Charges').val();
    
    if (meetingServiceId)
        $('#MeetingServices').val(meetingServiceId);
    else
        $('#MeetingServices').val(null);
    if (vesselId) {
        $('#Vessels').val(vesselId);
    }
    else {
        $('#Vessels').val(null);
    }
    $('#tableMeetingServiceIndex').val(tableIndex);
    $('#txtMeetingServiceFlightNo').val(flightNo);
    $('#txtMeetingServiceNoOfPax').val(noOfPax);
    $('#txtMeetingServiceCharges').val(charges);
    $('#btnAddMeetingService').attr('style', 'display:none;');
    $('#btnEditMeetingService').attr('style', 'display:normal;');
    $('#workOrderMeetingServiceModal').modal('show');
});

$('#btnMeetingServiceClose').click(function (e) {
    var tableIndex = $('#tableMeetingServiceIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#MeetingServiceContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#MeetingServiceList_' + lastIndex + "__AirportMeetingServiceId").parents('tr').last().remove();
    }
})

$('#btnAddMeetingService').click(function (e) {
    e.preventDefault();
    if (saveMeetingService()) {
        $('#workOrderMeetingServiceModal').modal('toggle');
    }
})

$('#btnEditMeetingService').click(function (e) {
    e.preventDefault();
    var tableIndex = $('#tableMeetingServiceIndex').val();
    var meetingServiceId = $('#MeetingServices').val();
    var meetingService = $('#MeetingServices option:selected').text();
    var vesselId = $('#Vessels').val();
    var vessel = $('#Vessels option:selected').text();
    var flightNo = $('#txtMeetingServiceFlightNo').val();
    var noOfPax = $('#txtMeetingServiceNoOfPax').val();
    var charges = $('#txtMeetingServiceCharges').val();

    if (!meetingServiceId) {
        alert("Meeting Service required!");
        $('#MeetingServices').focus();
        return false;
    }
    $('#' + tableIndex + 'AirportMeetingServiceId').val(meetingServiceId);
    $('#' + tableIndex + 'VesselId').val(vesselId);
    $('#' + tableIndex + 'MeetingService_Name').val(meetingService);
    $('#' + tableIndex + 'Vessel').val(vessel);
    $('#' + tableIndex + 'FlightNo').val(flightNo);
    $('#' + tableIndex + 'NoOfPax').val(noOfPax);
    $('#' + tableIndex + 'Charges').val(charges);
    $('#workOrderMeetingServiceModal').modal('toggle');
})

function clearMeetingService() {
    $('#tableMeetingServiceIndex').val(null);
    $('#MeetingServices').val(null);
    $('#Vessels').val(null);
    $('#txtMeetingServiceNoOfPax').val(null);
    $('#txtMeetingServiceCharges').val(null);
}

function saveMeetingService() {
    var meetingServiceId = $('#MeetingServices').val();
    var meetingService = $('#MeetingServices option:selected').text();
    var vesselId = $('#Vessels').val();
    var vessel = $('#Vessels option:selected').text();
    var flightNo = $('#txtMeetingServiceFlightNo').val();
    var noOfPax = $('#txtMeetingServiceNoOfPax').val();
    var charges = $('#txtMeetingServiceCharges').val();

    if (!meetingServiceId) {
        alert("Meeting Service required!");
        $('#MeetingServices').focus();
        return false;
    }


    var rows = $('tbody#MeetingServiceContainer').find("tr");
    var lastIndex = rows.last().index();
    $('#MeetingServiceList_' + lastIndex + '__AirportMeetingServiceId').val(meetingServiceId);
    $('#MeetingServiceList_' + lastIndex + '__VesselId').val(vesselId);
    $('#MeetingServiceList_' + lastIndex + '__MeetingService_Name').val(meetingService);
    $('#MeetingServiceList_' + lastIndex + '__Vessel').val(vessel);
    $('#MeetingServiceList_' + lastIndex + '__FlightNo').val(flightNo);
    $('#MeetingServiceList_' + lastIndex + '__NoOfPax').val(noOfPax);
    $('#MeetingServiceList_' + lastIndex + '__Charges').val(charges);
    return true;
}



$('#btnCancelMeetingService').click(function (e) {
    var tableIndex = $('#tableMeetingServiceIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#MeetingServiceContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#MeetingServiceList_' + lastIndex + "__AirportMeetingServiceId").parents('tr').last().remove();
    }
})


