//Page Load

$(document).ready(function () {

    var customerId = $('#CustomerId').val();

    if (customerId) {

        $.get('/VesselApi/GetByCustomer',
            {
                id: customerId
            }, function success(data, status) {

                var currentId = $('#VesselId').val();

                $('#VesselId').empty();

                $.each(data, function (index, item) {

                    $('#VesselId').append($('<option>').html(item.Vessel).val(item.VesselId));
                });

                if (currentId != "") {

                    $('#VesselId').val(currentId);
                }

                var currentMeetingServiceVesselId = $('#Vessels').val();
                $('#Vessels').empty();

                $.each(data, function (index, item) {

                    $('#Vessels').append($('<option>').html(item.Vessel).val(item.VesselId));
                });

                if (currentMeetingServiceVesselId != "") {

                    $('#Vessels').val(currentMeetingServiceVesselId);
                }

            });
    }
});


$('#divPickUpDate').datetimepicker({
    format: 'DD/MM/YYYY'
});

$('#divPickUpTime').datetimepicker({
    format: 'HH:mm'
});

$('#divStandByDate').datetimepicker({
    format: 'DD/MM/YYYY'
});

$('#divStandByTime').datetimepicker({
    format: 'HH:mm'
});

//Load References

$('#CustomerId').change(function (e) {

    $('#divLoading').modal('toggle');

    var customerId = $(this).val();
    if (customerId) {
        $.get('/VesselApi/GetByCustomer',
            {
                id: customerId
            }, function success(data, status) {
                $('#VesselId').empty();
                $.each(data, function (index, item) {
                    $('#VesselId').append($('<option>').html(item.Vessel).val(item.VesselId));
                });
                $('#Vessels').empty();
                $.each(data, function (index, item) {
                    $('#Vessels').append($('<option>').html(item.Vessel).val(item.VesselId));
                });
            });
        $.get('/AgentApi/GetByCustomerId',
            {
                customerId: customerId
            }, function success(data, status) {
                $('#AgentId').empty();
                $.each(data, function (index, item) {
                    $('#AgentId').append($('<option>').html(item.Name).val(item.Id));
                });

                $('#divLoading').modal('hide');


            });
    }
});

$('#Customers').change(function (e) {

    $('#divLoading').modal('toggle');

    var customerId = $(this).val();

    if (customerId == "") customerId = 0;


    $.get('/VesselApi/GetByCustomer',
        {
            id: customerId
        }, function success(data, status) {
            $('#Vessels').empty();
            $('#Vessels').append($('<option>All</option>'));

            //var customerId = $('#Customers').val();

            //if (customerId !=  "") {
            $.each(data, function (index, item) {
                $('#Vessels').append($('<option>').html(item.Vessel).val(item.VesselId));
            });
            //}
            //else {
            //    $.each(data, function (index, item) {
            //        $('#Vessels').append($('<option>').html(item.Name).val(item.Id));
            //    });
            //}                

            $('#divLoading').modal('hide');

        });
});

//Button Clicks

$('#btnSave').click(function (e) {
    e.preventDefault();

    //$('#btnSave').attr("disabled", "disabled");

    var pickupDate = $('#PickUpDateBinding').val().split('/');
    var pickupTime = $('#PickUpTimeBinding').val().split(':');

    if (pickupDate.length == 1 || pickupTime.length == 1) {
        alert('Pickup Date and Time are required fields. Please ensure to fill in the fields.');
        return;
    }
    else {
        var pickupDateTime = new Date(pickupDate[2], pickupDate[1] - 1, pickupDate[0], pickupTime[0], pickupTime[1]);

        var currentEntityRefNo = $('#ReferenceNo').val();

        if (currentEntityRefNo == 'TBD') {

            var rows = $('tbody#LocationContainer').find("tr");

            var locations = new Array();

            $('tbody#LocationContainer').find("tr").each(function (index) {
                var location = {};
                location.locationId = $('#WorkOrderLocationList_' + index + '__LocationId').val();
                location.hotelId = $('#WorkOrderLocationList_' + index + '__HotelId').val();
                location.locationType = $('#WorkOrderLocationList_' + index + '__Type').val();
                locations.push(location);
            });

            if (!locations.length) {
                alert('Locations for pick-up and drop-off points must be specified.');
                return;
            }


            var vessel = $('#VesselId').val();
            var customer = $('#CustomerId').val();
            var pickUpDate = $('#PickUpDateBinding').val();
            var pickUpTime = $('#PickUpTimeBinding').val();

            var currentForm = this.form;

            var url = "/WorkOrder/CheckForSimilarOrders";

            $.post(url, { PickUpDateBinding: pickUpDate, PickUpTimeBinding: pickUpTime,  CustomerId: customer, VesselId: vessel, Locations: locations },
                function (data, status) {
                    if (data === 'NoSimilarOrder') {
                        $('#divLoading').modal({ backdrop: 'static', keyboard: false });
                        currentForm.submit();
                        //alert('Submitted');
                    }
                    else {
                        if (confirm('Work orders with the same information already exist in the system. Are you sure you would like to submit this workorder? Existing Work Orders : ' + data))
                        {
                            $('#divLoading').modal({ backdrop: 'static', keyboard: false });
                            currentForm.submit();
                            //alert('OK');
                        }
                    }
                }
            );
        }
        else {
            $('#divLoading').modal({ backdrop: 'static', keyboard: false });
            this.form.submit();
        }
    }
});

$('#btnSaveCopySign').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    $('#YesNo1').val(true);
    this.form.submit();


    //$('#divLoading').modal({ backdrop: 'static', keyboard: false });
    //$.ajax({
    //    url: '@Url.Action("", "")',
    //    type: 'POST',
    //    dataType: 'json',
    //    cache: false,
    //    data: {
    //        'serviceJobId': id,
    //        'isCopySign': isCopySign
    //    },
    //    success: function (results) {
    //        alert(results);
    //        $('#divLoading').toggle();
    //    },
    //    error: function () {
    //        alert('Error occured');
    //        $('#divLoading').toggle();
    //    }
    //});
});

//Work Order Location

$('.clsAddLocation').click(function (e) {
    e.preventDefault();
    clearModalData();
    $('#btnAddLocation').attr('style', 'display:normal;');
    $('#btnEditLocation').attr('style', 'display:none;');
    //$('#workOrderLocationModal').modal('show');
    $('#workOrderLocationModal').modal({ backdrop: 'static', keyboard: false }) 
});

$('body').on('click', 'a.btnEditLocation', function () {
    var tableIndex = $(this).parents('tr').attr('tableIndex');
    var locationId = $('#' + tableIndex + 'LocationId').val();
    var hotelId = $('#' + tableIndex + 'HotelId').val();
    var locationType = $('#' + tableIndex + 'Type').val();
    var locationDescription = $('#' + tableIndex + 'Description').val();
    $('#LocationType').val(locationType);
    if (locationId)
        $('#Locations').val(locationId);
    else
        $('#Locations').val(null);
    if (hotelId)
        $('#Hotels').val(hotelId);
    else
        $('#Hotels').val(null);
    $('#txtLocationDescription').val(locationDescription);
    $('#tableIndex').val(tableIndex);
    $('#btnAddLocation').attr('style', 'display:none;');
    $('#btnEditLocation').attr('style', 'display:normal;');
    //$('#workOrderLocationModal').modal('show');
    $('#workOrderLocationModal').modal({ backdrop: 'static', keyboard: false }) 
});

$('#btnLocationClose').click(function (e) {
    var tableIndex = $('#tableIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#LocationContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#WorkOrderLocationList_' + lastIndex + "__LocationId").parents('tr').last().remove();
    }
});

$('#btnAddLocation').click(function (e) {
    e.preventDefault();
    if (saveLocation()) {
        $('#workOrderLocationModal').modal('toggle');
    }
})

$('#btnEditLocation').click(function (e) {
    e.preventDefault();
    var tableIndex = $('#tableIndex').val();
    var locationType = $('#LocationType').val();
    var locationTypeStr = $('#LocationType option:selected').text();
    var locationId = $('#Locations').val();
    var locationStr = $('#Locations option:selected').text();
    var locationDescription = $('#txtLocationDescription').val();
    var hotelId = $('#Hotels').val();
    var hotel = $('#Hotels option:selected').text();

    if (!locationId && !locationDescription) {
        alert("Location Description required!");
        $('#txtLocationDescription').focus();
        return false;
    }


    if (hotelId) {
        locationStr += hotel;
    }
    $('#' + tableIndex + 'LocationId').val(locationId);
    $('#' + tableIndex + 'Location_Id').val(locationId);
    $('#' + tableIndex + 'HotelId').val(hotelId);
    $('#' + tableIndex + 'Hotel').val(hotel);
    $('#' + tableIndex + 'Type').val(locationType);
    $('#' + tableIndex + 'TypeStr').val(locationTypeStr);
    $('#' + tableIndex + 'Location_Display').val(locationStr);
    $('#' + tableIndex + 'Description').val(locationDescription);
    $('#' + tableIndex + 'Location_PostalCode').val(locationStr);
    $('#workOrderLocationModal').modal('toggle');
    //$('#workOrderLocationModal').modal({ backdrop: 'static', keyboard: false }) 
})

function clearModalData() {
    $('#tableIndex').val(null);
    $('#LocationType').val(10);
    $('#Locations').val(null);
    $('#Hotels').val(null);
    $('#txtLocationDescription').val(null);
}

function saveLocation() {
    var locationType = $('#LocationType').val();
    var locationTypeStr = $('#LocationType option:selected').text();
    var locationId = $('#Locations').val();
    var locationStr = $('#Locations option:selected').text();
    var locationDescription = $('#txtLocationDescription').val();
    if (!locationId && !locationDescription) {
        alert("Location Description required!");
        $('#txtLocationDescription').focus();
        return false;
    }
    var hotelId = $('#Hotels').val();
    var hotel = $('#Hotels option:selected').text();
    if (hotelId)
    {
        locationStr += hotel;
    }

    var rows = $('tbody#LocationContainer').find("tr");
    var lastIndex = rows.last().index();
    $('#WorkOrderLocationList_' + lastIndex + '__LocationId').val(locationId);
    $('#WorkOrderLocationList_' + lastIndex + '__HotelId').val(hotelId);
    $('#WorkOrderLocationList_' + lastIndex + '__Hotel').val(hotel);
    $('#WorkOrderLocationList_' + lastIndex + '__Location_Id').val(locationId);
    $('#WorkOrderLocationList_' + lastIndex + '__Type').val(locationType);
    $('#WorkOrderLocationList_' + lastIndex + '__TypeStr').val(locationTypeStr);
    $('#WorkOrderLocationList_' + lastIndex + '__Location_Display').val(locationStr);
    $('#WorkOrderLocationList_' + lastIndex + '__Description').val(locationDescription);
    $('#WorkOrderLocationList_' + lastIndex + '__Location_Code').val('TBA');
    $('#WorkOrderLocationList_' + lastIndex + '__Location_Name').val('TBA');
    $('#WorkOrderLocationList_' + lastIndex + '__Location_PostalCode').val(locationStr);
    return true;
}

$('#btnCancel').click(function (e) {
    var tableIndex = $('#tableIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#LocationContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#WorkOrderLocationList_' + lastIndex + "__LocationId").parents('tr').last().remove();
    }
});


