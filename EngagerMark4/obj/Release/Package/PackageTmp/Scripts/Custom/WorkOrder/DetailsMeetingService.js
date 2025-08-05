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

    var meetingServicePassengerInCharge = $('#' + tableIndex + 'MeetingServicePassengerInCharge').val();
    var isLastMinuteCharge = $('#' + tableIndex + 'IsLastMinuteCharge').val();
    var lastMinuteCharge = $('#' + tableIndex + 'LastMinuteCharge').val();
    var overnightCharge = $('#' + tableIndex + 'OvernightCharge').val();
    var isMajorAmendment = $('#' + tableIndex + 'IsMajorAmendment').val();
    var majorAmendmentCharge = $('#' + tableIndex + 'MajorAmendmentCharge').val();
    var perPaxChargeLabel = $('#' + tableIndex + 'PerPaxChargeLabel').val();
    var perPaxCharge = $('#' + tableIndex + 'PerPaxCharge').val();
    var maxPaxRange = $('#' + tableIndex + 'MaxPaxRange').val();
    var meetingServiceDetailId = $('#' + tableIndex + 'MeetingServiceDetailId').val();
    var additionalPersonCharge = $('#' + tableIndex + 'AdditionalPersonCharge').val();
    var totalPaxCharge = $('#' + tableIndex + 'TotalPaxCharge').val();

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

    $('#MeetingServicePassengers').val(meetingServicePassengerInCharge);

    if (isLastMinuteCharge == 'True') {
        $('#chkLastMinuteCharge').prop('checked', true);
    }
    $('#txtLastMinuteCharge').val(lastMinuteCharge);

    $('#txtOvernightCharge').val(overnightCharge);

    if (isMajorAmendment == 'True') {
        $('#chkMajorAmendmentCharge').prop('checked', true);
    }
    $('#txtMajorAmendmentCharge').val(majorAmendmentCharge);

    $('#txtMeetingServiceDetailId').val(meetingServiceDetailId);
    $('#txtMaxPaxRange').val(maxPaxRange);

    $('#lblPerPaxChargeLabel').text(perPaxChargeLabel);
    $('#txtPerPaxCharge').val(perPaxCharge);
    $('#txtAdditionalPersonCharge').val(additionalPersonCharge);

    $('#txtTotalPaxCharge').val(totalPaxCharge);

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


    var meetingServicePassengerInCharge = $('#MeetingServicePassengers option:selected').text();

    var isLastMinuteCharge = 'false';
    if ($('#chkLastMinuteCharge').prop("checked") == true) {
        isLastMinuteCharge = 'true';
    }

    var lastMinuteCharge = $('#txtLastMinuteCharge').val();
    var overnightCharge = $('#txtOvernightCharge').val();

    var isMajorAmendmentCharge = 'false';
    if ($('#chkMajorAmendmentCharge').prop("checked") == true) {
        isMajorAmendmentCharge = 'true';
    }
    
    var majorAmendmentCharge = $('#txtMajorAmendmentCharge').val();
    var meetingServiceDetailId = $('#txtMeetingServiceDetailId').val();
    var perPaxChargeLabel = $('#lblPerPaxChargeLabel').text();
    var perPaxCharge = $('#txtPerPaxCharge').val();
    var maxPaxRange = $('#txtMaxPaxRange').val();
    var additionalPersonCharge = $('#txtAdditionalPersonCharge').val();
    var totalPaxCharge = $('#txtTotalPaxCharge').val();


    if (!meetingServiceId) {
        alert("Meeting Service required!");
        $('#MeetingServices').focus();
        return false;
    }


    if ($.isNumeric(noOfPax) == false) {
        alert("No of Pax is a numeric field!");
        return false;
    }

    if ($.isNumeric(lastMinuteCharge) == false) {
        alert("Last Minute Charge is a numeric field!");
        $('#txtLastMinuteCharge').focus();
        return false;
    }

    if ($.isNumeric(lastMinuteCharge) == false) {
        alert("Last Minute Charge is a numeric field!");
        $('#txtLastMinuteCharge').focus();
        return false;
    }

    if ($.isNumeric(overnightCharge) == false) {
        alert("Overnight Charge is a numeric field!");
        $('#txtOvernightCharge').focus();
        return false;
    }

    if ($.isNumeric(majorAmendmentCharge) == false) {
        alert("Major Amendment Charge is a numeric field!");
        $('#txtMajorAmendmentCharge').focus();
        return false;
    }

    if ($.isNumeric(perPaxCharge) == false) {
        alert("Per Pax Charge is a numeric field!");
        $('#txtPerPaxCharge').focus();
        return false;
    }


    if ($.isNumeric(additionalPersonCharge) == false) {
        alert("Additional Person Charge is a numeric field!");
        $('#txtAdditionalPersonCharge').focus();
        return false;
    }    
    
    $('#' + tableIndex + 'AirportMeetingServiceId').val(meetingServiceId);
    $('#' + tableIndex + 'VesselId').val(vesselId);
    $('#' + tableIndex + 'MeetingService_Name').val(meetingService);
    $('#' + tableIndex + 'Vessel').val(vessel);
    $('#' + tableIndex + 'FlightNo').val(flightNo);
    $('#' + tableIndex + 'NoOfPax').val(noOfPax);
    $('#' + tableIndex + 'Charges').val(charges);

    $('#' + tableIndex + 'MeetingServicePassengerInCharge').val(meetingServicePassengerInCharge);
    $('#' + tableIndex + 'IsLastMinuteCharge').val(isLastMinuteCharge);
    $('#' + tableIndex + 'LastMinuteCharge').val(lastMinuteCharge);
    $('#' + tableIndex + 'OvernightCharge').val(overnightCharge);
    $('#' + tableIndex + 'IsMajorAmendment').val(isMajorAmendmentCharge);
    $('#' + tableIndex + 'MajorAmendmentCharge').val(majorAmendmentCharge);
    $('#' + tableIndex + 'PerPaxChargeLabel').val(perPaxChargeLabel);
    $('#' + tableIndex + 'PerPaxCharge').val(perPaxCharge);
    $('#' + tableIndex + 'MaxPaxRange').val(maxPaxRange);
    $('#' + tableIndex + 'MeetingServiceDetailId').val(meetingServiceDetailId);
    $('#' + tableIndex + 'AdditionalPersonCharge').val(additionalPersonCharge);
    $('#' + tableIndex + 'TotalPaxCharge').val(totalPaxCharge);

    $('#workOrderMeetingServiceModal').modal('toggle');
})

function clearMeetingService() {
    $('#tableMeetingServiceIndex').val(null);
    $('#MeetingServices').val(null);
    $('#Vessels').val(null);
    $('#txtMeetingServiceNoOfPax').val(null);
    $('#txtMeetingServiceCharges').val(null);
    $('#txtMeetingServiceFlightNo').val(null);   
    
    $('#MeetingServicePassengers').val(null);
    $('#chkLastMinuteCharge').prop('checked', false);
    $('#txtLastMinuteCharge').val(null);
    $('#txtOvernightCharge').val(null);
    $('#chkMajorAmendmentCharge').prop('checked', false);
    $('#txtMajorAmendmentCharge').val(null);
    $('#txtMeetingServiceDetailId').val(null);
    $('#txtMaxPaxRange').val(null);
    $('#lblPerPaxChargeLabel').text('-');
    $('#txtPerPaxCharge').val(null);
    $('#txtAdditionalPersonCharge').val(null);
    $('#txtTotalPaxCharge').val(null);
}

function saveMeetingService() {
    var meetingServiceId = $('#MeetingServices').val();
    var meetingService = $('#MeetingServices option:selected').text();
    var vesselId = $('#Vessels').val();
    var vessel = $('#Vessels option:selected').text();
    var flightNo = $('#txtMeetingServiceFlightNo').val();
    var noOfPax = $('#txtMeetingServiceNoOfPax').val();
    var charges = $('#txtMeetingServiceCharges').val();

    var meetingServicePassengerInCharge = $('#MeetingServicePassengers option:selected').text();

    var isLastMinuteCharge = 'false';
    if ($('#chkLastMinuteCharge').prop("checked") == true) {
        isLastMinuteCharge = 'true';
    }

    var lastMinuteCharge = $('#txtLastMinuteCharge').val();
    var overnightCharge = $('#txtOvernightCharge').val();

    var isMajorAmendmentCharge = 'false';
    if ($('#chkMajorAmendmentCharge').prop("checked") == true) {
        isMajorAmendmentCharge = 'true';
    }

    var majorAmendmentCharge = $('#txtMajorAmendmentCharge').val();
    var meetingServiceDetailId = $('#txtMeetingServiceDetailId').val();
    var perPaxChargeLabel = $('#lblPerPaxChargeLabel').text();
    var perPaxCharge = $('#txtPerPaxCharge').val();
    var maxPaxRange = $('#txtMaxPaxRange').val();
    var additionalPersonCharge = $('#txtAdditionalPersonCharge').val();
    var totalPaxCharge = $('#txtTotalPaxCharge').val();


    if (!meetingServiceId) {
        alert("Meeting Service required!");
        $('#MeetingServices').focus();
        return false;
    }

    if (!noOfPax) {
        noOfPax = 0;
    }

    if (!charges) {
        charges = 0;
    }

    if ($.isNumeric(noOfPax) == false) {
        alert("No of Pax is a numeric field!");
        return false;
    }

    if ($.isNumeric(lastMinuteCharge) == false) {
        alert("Last Minute Charge is a numeric field!");
        $('#txtLastMinuteCharge').focus();
        return false;
    }

    if ($.isNumeric(lastMinuteCharge) == false) {
        alert("Last Minute Charge is a numeric field!");
        $('#txtLastMinuteCharge').focus();
        return false;
    }

    if ($.isNumeric(overnightCharge) == false) {
        alert("Overnight Charge is a numeric field!");
        $('#txtOvernightCharge').focus();
        return false;
    }

    if ($.isNumeric(majorAmendmentCharge) == false) {
        alert("Major Amendment Charge is a numeric field!");
        $('#txtMajorAmendmentCharge').focus();
        return false;
    }

    if ($.isNumeric(perPaxCharge) == false) {
        alert("Per Pax Charge is a numeric field!");
        $('#txtPerPaxCharge').focus();
        return false;
    }



    if ($.isNumeric(additionalPersonCharge) == false) {
        alert("Additional Person Charge is a numeric field!");
        $('#txtAdditionalPersonCharge').focus();
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

    $('#MeetingServiceList_' + lastIndex + '__MeetingServicePassengerInCharge').val(meetingServicePassengerInCharge);
    $('#MeetingServiceList_' + lastIndex + '__IsLastMinuteCharge').val(isLastMinuteCharge);
    $('#MeetingServiceList_' + lastIndex + '__LastMinuteCharge').val(lastMinuteCharge);
    $('#MeetingServiceList_' + lastIndex + '__OvernightCharge').val(overnightCharge);
    $('#MeetingServiceList_' + lastIndex + '__IsMajorAmendment').val(isMajorAmendmentCharge);
    $('#MeetingServiceList_' + lastIndex + '__MajorAmendmentCharge').val(majorAmendmentCharge);
    $('#MeetingServiceList_' + lastIndex + '__PerPaxChargeLabel').val(perPaxChargeLabel);
    $('#MeetingServiceList_' + lastIndex + '__PerPaxCharge').val(perPaxCharge);
    $('#MeetingServiceList_' + lastIndex + '__MaxPaxRange').val(maxPaxRange);
    $('#MeetingServiceList_' + lastIndex + '__MeetingServiceDetailId').val(meetingServiceDetailId);
    $('#MeetingServiceList_' + lastIndex + '__AdditionalPersonCharge').val(additionalPersonCharge);
    $('#MeetingServiceList_' + lastIndex + '__TotalPaxCharge').val(totalPaxCharge);

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



$('#MeetingServices').change(function (e) {
    var meetingServiceId = $('#MeetingServices').val();
    var url = "/WorkOrder/GetMeetingServiceByMeetingServiceId";

    $.post(url,
        {
            meetingServiceId: meetingServiceId
        },
        function (Data, status) {

            if (Data != null) {
                $('#txtLastMinuteCharge').val(Data.LastMinuteCharge);
                $('#txtOvernightCharge').val(Data.OvernightCharge);
                $('#txtMajorAmendmentCharge').val(Data.MajorAmendmentCharge);
                $('#txtMeetingServiceDetailId').val(Data.Id);
                $('#txtAdditionalPersonCharge').val(Data.AdditionalPersonCharge);
            }

            CalculateOverallMeetingServiceCharges();
        });
});

$('#chkLastMinuteCharge').change(function () {
    CalculateOverallMeetingServiceCharges();
});

$('#txtLastMinuteCharge').on('keyup', function () {
    CalculateOverallMeetingServiceCharges();
});

$('#txtOvernightCharge').on('keyup', function () {
    CalculateOverallMeetingServiceCharges();
});

$('#chkMajorAmendmentCharge').change(function () {
    CalculateOverallMeetingServiceCharges();
});

$('#txtMajorAmendmentCharge').on('keyup', function () {
    CalculateOverallMeetingServiceCharges();
});

$('#txtMeetingServiceNoOfPax').on('keyup', function () {

    var noOfPax = $('#txtMeetingServiceNoOfPax').val();

    if (noOfPax === null || noOfPax === 0) {
        return false;
    }

    var meetingServiceId = $('#MeetingServices').val();

    if (meetingServiceId === null || meetingServiceId === 0) {
        alert("Please select a meeting service first.");
        return false;
    }

    var url = "/WorkOrder/GetPricePerPaxInfo";

    $.post(url,
        {
            meetingServiceId: meetingServiceId,
            noOfPax: noOfPax
        },
        function (Data, status) {

            if (Data != null) {
                $('#lblPerPaxChargeLabel').text(Data.NoOfPax);
                $('#txtPerPaxCharge').val(Data.Charges);
                $('#txtMeetingServiceDetailId').val(Data.Id);
                $('#txtMaxPaxRange').val(Data.MaxPax);
            }

            CalculatePaxcharge();

        });
});

$('#txtMajorAmendmentCharge').on('keyup', function () {
    CalculateOverallMeetingServiceCharges();
});

$('#txtAdditionalPersonCharge').on('keyup', function () {
    CalculatePaxcharge();
});

function CalculatePaxcharge() {
    var perPaxCharge = 0;
    var maxPaxRange = 0;
    var noOfPax = 0;
    var additionalPax = 0;
    var additionalPersonCharge = 0;

    noOfPax = $('#txtMeetingServiceNoOfPax').val();
    perPaxCharge = $('#txtPerPaxCharge').val();
    maxPaxRange = $('#txtMaxPaxRange').val();

    if (parseFloat(noOfPax) > parseFloat(maxPaxRange)) {
        additionalPax = noOfPax - maxPaxRange;
    }
    additionalPersonCharge = $('#txtAdditionalPersonCharge').val();

    var totalPaxCharge = (parseFloat(perPaxCharge)) + (parseFloat(additionalPersonCharge) * parseFloat(additionalPax));

    $('#txtTotalPaxCharge').val(totalPaxCharge);

    CalculateOverallMeetingServiceCharges();   

}

function CalculateOverallMeetingServiceCharges() {
    var lastMinuteCharge = 0;
    var overNightCharge = 0;
    var majorAmendmentCharge = 0;
    var paxCharge = 0;
    var totalCharge = 0;

    if ($('#chkLastMinuteCharge').prop("checked") == true) {
        lastMinuteCharge = $('#txtLastMinuteCharge').val();

        if (!lastMinuteCharge) lastMinuteCharge = 0;
    }

    if ($('#chkMajorAmendmentCharge').prop("checked") == true) {
        majorAmendmentCharge = $('#txtMajorAmendmentCharge').val();
    }

    if ($('#IsOverNightJob').val() == 'True') {
        overNightCharge = $('#txtOvernightCharge').val();
    }

    paxCharge = $('#txtTotalPaxCharge').val();

    if (!lastMinuteCharge) lastMinuteCharge = 0;
    if (!overNightCharge) overNightCharge = 0;
    if (!majorAmendmentCharge) majorAmendmentCharge = 0;
    if (!paxCharge) paxCharge = 0;

    totalCharge = parseFloat(lastMinuteCharge) + parseFloat(overNightCharge) + parseFloat(majorAmendmentCharge) + parseFloat(paxCharge);

    $('#txtMeetingServiceCharges').val(totalCharge);
}





