$('.clsAddServiceJob').click(function (e) {
    e.preventDefault();
    clearServiceJob();

    $('#divDriverName').hide();
    $('#divDriverList').show();

    $('#divVehicleName').hide();
    $('#divVehicleList').show();

    $('#serviceJobModal').modal({ backdrop: 'static', keyboard: false });
});

$('#Staffs').change(function (e) {
    var staffId = $(this).val();

    $('#driverIdNo').val(staffId);
    $('#txtDriverName').val($('#Staffs option:selected').text());

    $.get("/VehicleApi/GetByUserId",
        {
            userId: staffId
        }, function success(data, status) {
            $('#ServiceJobVehicles').empty();
            //$('#ServiceJobVehicles').append($('<option>').html("N/A"));
            $.each(data, function (index, item) {
                $('#ServiceJobVehicles').append($('<option>').html(item.VehicleNo).val(item.VehicleId));
            })
        })
})

$('body').on('click', 'a.btnEditServiceJob', function () {
    var tableIndex = $(this).parents('tr').attr('tableIndex');
    var staffId = $('#' + tableIndex + 'UserId').val();
    var vehicleId = $('#' + tableIndex + 'VehicleId').val();
    var startExecTime = $('#' + tableIndex + 'StartExecutionTime').val();
    var endExecTime = $('#' + tableIndex + 'EndExecutionTime').val();
    var startExecPlace = $('#' + tableIndex + 'StartExecutionPlace').val();
    var endExecPlace = $('#' + tableIndex + 'EndExecutionPlace').val();
    var customDetentionId = $('#' + tableIndex + 'CustomDetentionId').val();
    var disposals = $('#' + tableIndex + 'Disposals').val();
    var additionalStops = $('#' + tableIndex + 'AdditionalStops').val();
    var waitingTime = $('#' + tableIndex + 'WaitingTime').val();
    var chkListIds = $('#' + tableIndex + 'CheckListIds').val();

    var currentStatus = $('#' + tableIndex + 'ServiceJobStatusStr').val();
    var tripFees = $('#' + tableIndex + 'TripFees').val();
    var msFees = $('#' + tableIndex + 'MSFees').val();
    
    if (staffId) {

        $('#Staffs').val(staffId);
        $('#driverIdNo').val(staffId);
    }
    else {
        $('#Staffs').val(null);
        $('#driverIdNo').val(null);
    }
    if (vehicleId) 
        $('#ServiceJobVehicles').val(vehicleId);
    else 
        $('#ServiceJobVehicles').val(null);
    if (customDetentionId)
        $('#CustomDetentions').val(customDetentionId);
    else
        $('#CustomDetentions').val(null);
    
    if (currentStatus != 'Pending') {
        $('#divDriverName').show();
        $('#divDriverList').hide();
        $('#driverIdNo').val($('#' + tableIndex + 'UserId').val());
        $('#txtDriverName').val($('#' + tableIndex + 'User_Name').val());

        $('#divVehicleName').show();
        $('#divVehicleList').hide();
        $('#txtVehicleName').val($('#ServiceJobVehicles option:selected').text());

    }
    else {
        $('#divDriverName').hide();
        $('#divDriverList').show();

        $('#divVehicleName').hide();
        $('#divVehicleList').show();
    }

    $('#serviceJobTableIndex').val(tableIndex);
    $('#txtDisposal').val(disposals);
    $('#txtAdditionalStops').val(additionalStops);
    $('#txtWaitingTime').val(waitingTime);
    $('#txtStartExecTime').val(startExecTime);
    $('#txtEndExecTime').val(endExecTime);
    $('#txtStartExecPlace').val(startExecPlace);
    $('#txtEndExecPlace').val(endExecPlace);
    var checkListids = chkListIds.split(',');

    $('.chkAdditionInfo').each(function () {
        $(this).prop('checked', false);
        var additionalInfoId = $(this).val();
        $('#txtAdditionalInfo_' + additionalInfoId).val(0);

        let i;
        for (i = 0; i < checkListids.length; ++i) {

            let currentValArr = checkListids[i].split('|');

            let currentId = 0;
            let currentVal = 0;

            currentId = currentValArr[0];

            if (currentValArr.length > 1) currentVal = currentValArr[1];

            if (currentId == additionalInfoId) {
                $(this).prop('checked', true);
                $('#txtAdditionalInfo_' + additionalInfoId).val(currentVal);
                $('#txtAdditionalInfo_' + additionalInfoId).prop('readonly', false);
            }
        }

        //if (jQuery.inArray(additionalInfoId, checkListids) !== -1) {
        //    $(this).prop('checked', true);
        //}
    });
    $('#txtTripFees').val(tripFees);
    $('#txtMSFees').val(msFees);


    $('#btnAddServiceJob').attr('style', 'display:none;');
    $('#btnEditServiceJob').attr('style', 'display:normal;');
    $('#serviceJobModal').modal('show');
});

$('#btnServiceJobClose').click(function (e) {
    var tableIndex = $('#serviceJobTableIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#ServiceJobContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#ServiceJobList_' + lastIndex + "__UserId").parents('tr').last().remove();
    }
})

$('#btnAddServiceJob').click(function (e) {
    e.preventDefault();
    if (saveServiceJob()) {
        $('#serviceJobModal').modal('toggle');
    }
})

$('.chkAdditionInfo').change(function () {

    var curAdditionalInfoStr = $(this).val();
    var isCheck = $(this).prop('checked');

    if (isCheck) {
        $('#txtAdditionalInfo_' + curAdditionalInfoStr).prop('readonly', false);
    }
    else {
        $('#txtAdditionalInfo_' + curAdditionalInfoStr).prop('readonly', true);
    }
});

$('#btnEditServiceJob').click(function (e) {
    e.preventDefault();
    var tableIndex = $('#serviceJobTableIndex').val();
    //var staffId = $('#Staffs').val();
    var staffId = $('#driverIdNo').val();
    //var staff = $('#Staffs option:selected').text();
    var staff = $('#txtDriverName').val();
    var vehicleId = $('#ServiceJobVehicles').val();
    var vehicle = $('#ServiceJobVehicles option:selected').text();
    var customDetentionId = $('#CustomDetentions').val();
    var customDetention = $('#CustomDetentions option:selected').text();
    var disposal = $('#txtDisposal').val();
    var additionalStop = $('#txtAdditionalStops').val();
    var waitingTime = $('#txtWaitingTime').val();

    //PCR2021
    var isFeesValidationRequired = false;

    var currentStatus = $('#' + tableIndex + 'Status').val();

    if (currentStatus == "Submitted") {
        isFeesValidationRequired = true;
    }

    var isTripFeesRequired = false;
    var isMsFeesRequired = false;
    var tripFees = $('#txtTripFees').val();
    var msFees = $('#txtMSFees').val();

    if (!tripFees || tripFees == 0 || tripFees == "0" || tripFees == "0.00") {
        if (isFeesValidationRequired) {
            isTripFeesRequired = true;
        }
    }

    //if (!msFees || msFees == 0 || msFees == "0" || msFees == "0.00") {
    //    if (isFeesValidationRequired) {
    //        isMsFeesRequired = true;
    //    }
    //}

    var additionalInfoStr = "";

    var isAddInfoRequired = false;
    $('.chkAdditionInfo').each(function () {
        var isCheck = $(this).prop('checked');
        if (isCheck) {

            let curAdditionalInfoStr = "";
            let curAdditionalValue = "0.00";

            curAdditionalInfoStr += $(this).val();

            curAdditionalValue = $('#txtAdditionalInfo_' + curAdditionalInfoStr).val();

            if (!curAdditionalValue || curAdditionalValue == 0 || curAdditionalValue == "0") {
                if (isFeesValidationRequired) {
                    isAddInfoRequired = true;
                }
            }

            curAdditionalInfoStr += "|" + curAdditionalValue + ",";

            additionalInfoStr += curAdditionalInfoStr;
        }
    });

    var startExecTime = $('#txtStartExecTime').val();
    var endExecTime = $('#txtEndExecTime').val();
    var startExecPlace = $('#txtStartExecPlace').val();
    var endExecPlace = $('#txtEndExecPlace').val();
    if (!staffId) {
        alert("Staff required!");
        $('#Staffs').focus();
        return false;
    }

    if (!vehicleId) {
        alert('Vehicle required!');
        $('#ServiceJobVehicles').focus();
        return false;
    }

    $('#' + tableIndex + 'UserId').val(staffId);
    $('#' + tableIndex + 'VehicleId').val(vehicleId);
    $('#' + tableIndex + 'StartExecutionTime').val(startExecTime);
    $('#' + tableIndex + 'EndExecutionTime').val(endExecTime);
    $('#' + tableIndex + 'StartExecutionPlace').val(startExecPlace);
    $('#' + tableIndex + 'EndExecutionPlace').val(endExecPlace);
    $('#' + tableIndex + 'CustomDetentionId').val(customDetentionId);
    $('#' + tableIndex + 'CustomDetention').val(customDetention);
    $('#' + tableIndex + 'Disposals').val(disposal);
    $('#' + tableIndex + 'AdditionalStops').val(additionalStop);
    $('#' + tableIndex + 'WaitingTime').val(waitingTime);
    $('#' + tableIndex + 'CheckListIds').val(additionalInfoStr);
    $('#' + tableIndex + 'User_Name').val(staff);
    $('#' + tableIndex + 'Vehicle_VehicleNo').val(vehicle);

    if (!tripFees) {
        $('#' + tableIndex + 'TripFees').val("0");
    }
    else {
        $('#' + tableIndex + 'TripFees').val(tripFees);
    }

    if (!msFees) {
        $('#' + tableIndex + 'MSFees').val("0");
    }
    else {
        $('#' + tableIndex + 'MSFees').val(msFees);
    }




    if (!isAddInfoRequired && !isTripFeesRequired && !isMsFeesRequired) {

        $('#divDriverName').hide();
        $('#divDriverList').show();

        $('#divVehicleName').hide();
        $('#divVehicleList').show();

        $('#serviceJobModal').modal('toggle');
    }
    else {

        if (isAddInfoRequired) {
            alert("Must fill in the value of selected additional items before saving.");
        }
        else if (isTripFeesRequired) {
            alert("Driver Trip Fees required.");
        }
        //else if (isMsFeesRequired) {
        //    alert("Meeting Service Fees required.");
        //}

    }
})

function clearServiceJob() {
    $('#serviceJobTableIndex').val(null);
    $('#Staffs').val(null);
    $('#driverIdNo').val(null);
    $('#ServiceJobVehicles').val(null);
    $('#CustomDetentions').val(null);
    $('#txtDisposal').val(null);
    $('#txtAdditionalStops').val(null);
    $('#txtWaitingTime').val(null);
    $('.chkAdditionInfo').each(function () {
        $(this).prop('checked', false);
    });
    $('.txtAdditionInfos').each(function () {
        $(this).val(null);
    });
    $('#txtStartExecTime').val(null);
    $('#txtEndExecTime').val(null);
    $('#txtStartExecPlace').val(null);
    $('#txtEndExecPlace').val(null);
    $('#btnAddServiceJob').attr('style', 'display:normal');
    $('#btnEditServiceJob').attr('style', 'display:none;');
    $('#btnCancelServiceJob').attr('style', 'display:normal;');
}

function saveServiceJob() {
    var staffId = $('#Staffs').val();
    var staff = $('#Staffs option:selected').text();
    var vehicleId = $('#ServiceJobVehicles').val();
    var vehicle = $('#ServiceJobVehicles option:selected').text();
    var customDetentionId = $('#CustomDetentions').val();
    var customDetention = $('#CustomDetentions option:selected').text();
    var disposal = $('#txtDisposal').val();
    var additionalStop = $('#txtAdditionalStops').val();
    var waitingTime = $('#txtWaitingTime').val();
    var additionalInfoIdStr = "";
    $('.chkAdditionInfo').each(function () {
        var isCheck = $(this).prop('checked');
        if (isCheck) {
            additionalInfoIdStr += $(this).val() + ",";
        }
    });
    var startExecTime = $('#txtStartExecTime').val();
    var endExecTime = $('#txtEndExecTime').val();
    var startExecPlace = $('#txtStartExecPlace').val();
    var endExecPlace = $('#txtEndExecPlace').val();
    if (!staffId) {
        alert("Staff required!");
        $('#Staffs').focus();
        return false;
    }

    if (!vehicleId) {
        alert('Vehicle required!');
        $('#ServiceJobVehicles').focus();
        return false;
    }

    var tripFees = $('#txtTripFees').val();
    var msFees = $('#txtMSFees').val();

    var rows = $('tbody#ServiceJobContainer').find("tr");
    var lastIndex = rows.last().index();
    $('#ServiceJobList_' + lastIndex + '__UserId').val(staffId);
    $('#ServiceJobList_' + lastIndex + '__VehicleId').val(vehicleId);
    $('#ServiceJobList_' + lastIndex + '__StartExecutionTime').val(startExecTime);
    $('#ServiceJobList_' + lastIndex + '__EndExecutionTime').val(endExecTime);
    $('#ServiceJobList_' + lastIndex + '__StartExecutionPlace').val(startExecPlace);
    $('#ServiceJobList_' + lastIndex + '__EndExecutionPlace').val(endExecPlace);
    $('#ServiceJobList_' + lastIndex + '__CustomDetentionId').val(customDetentionId);
    $('#ServiceJobList_' + lastIndex + '__CustomDetention').val(customDetention);
    $('#ServiceJobList_' + lastIndex + '__Disposals').val(disposal);
    $('#ServiceJobList_' + lastIndex + '__AdditionalStops').val(additionalStop);
    $('#ServiceJobList_' + lastIndex + '__WaitingTime').val(waitingTime);
    $('#ServiceJobList_' + lastIndex + '__CheckListIds').val(additionalInfoIdStr);
    $('#ServiceJobList_' + lastIndex + '__User_Name').val(staff);
    $('#ServiceJobList_' + lastIndex + '__User_ApplicationUserId').val("N/A");
    $('#ServiceJobList_' + lastIndex + '__User_UserName').val("N/A");
    $('#ServiceJobList_' + lastIndex + '__User_FirstName').val("N/A");
    $('#ServiceJobList_' + lastIndex + '__User_LastName').val("N/A");
    $('#ServiceJobList_' + lastIndex + '__User_Email').val("N/A");
    $('#ServiceJobList_' + lastIndex + '__DriverRemark').val("");
    $('#ServiceJobList_' + lastIndex + '__ServiceJobReason').val("");
    $('#ServiceJobList_' + lastIndex + '__Vehicle_VehicleNo').val(vehicle);
    $('#ServiceJobList_' + lastIndex + '__TripFees').val("0.00");
    $('#ServiceJobList_' + lastIndex + '__MSFees').val("0.00");
    
    return true;
}



$('#btnCancelServiceJob').click(function (e) {
    var tableIndex = $('#serviceJobTableIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#ServiceJobContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#ServiceJobList_' + lastIndex + "__UserId").parents('tr').last().remove();
    }
})


