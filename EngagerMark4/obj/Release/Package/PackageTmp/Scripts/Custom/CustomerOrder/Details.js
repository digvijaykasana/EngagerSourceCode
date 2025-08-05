//Page Load

$(document).ready(function (e) {

    //Set Agent Customer
    var agentCustomerId = $('#txtAgentCustomerId').val();
    $('#Customers').val(agentCustomerId);
    $('#CustomerId').val(agentCustomerId); 

    //var currentAgentId = $('#txtCurrentUserId').val();
    //$('#Agents').val(currentAgentId);
    //$('#AgentId').val(currentAgentId); 

    LoadCustomerReferences();
    
    //Set Previous Status
    var previousStatus = $('#Status').val();
    $('#PreviousStatus').val(previousStatus);

    if(previousStatus == "With_Accounts" || previousStatus== "Billed")
    {
        $('#txtDisplayStatus').val("Submitted");
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

$('#Customers').change(function (e) {
    LoadCustomerReferences();
});

function LoadCustomerReferences() {
    var customerId = $('#Customers').val();
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
        //$.get('/AgentApi/GetByCustomerId',
        //    {
        //        customerId: customerId
        //    }, function success(data, status) {
        //        $('#AgentId').empty();
        //        $.each(data, function (index, item) {
        //            $('#AgentId').append($('<option>').html(item.Name).val(item.Id));
        //        });

        //    });
    }
}

//Button Clicks

$('#btnSaveDraft').click(function (e) {
    e.preventDefault();
    $('#Status').val("Draft");

    var rows = $('tbody#LocationContainer').find("tr");

    var locations = new Array();

    $('tbody#LocationContainer').find("tr").each(function (index) {
        var location = {};
        location.locationId = $('#WorkOrderLocationList_' + index + '__LocationId').val();
        location.hotelId = $('#WorkOrderLocationList_' + index + '__HotelId').val();
        location.locationType = $('#WorkOrderLocationList_' + index + '__Type').val();
        locations.push(location);
    });

    //if (!locations.length) {
    //    alert('Locations for pick-up and drop-off points must be specified.');
    //    return;
    //}


    var vessel = $('#VesselId').val();
    var customer = $('#Customers').val();
    var pickUpDate = $('#PickUpDateBinding').val();
    var pickUpTime = $('#PickUpTimeBinding').val();


    var currentEntityRefNo = $('#RefereneceNo').val();
    var currentOrderId = 0;
    if (currentEntityRefNo != 'TBD') {
        currentOrderId = $('#Id').val();
    }


    var currentForm = this.form;

    var url = '/CustomerOrder/CheckForSimilarOrders';

    $.post(url, { PickUpDateBinding: pickUpDate, PickUpTimeBinding: pickUpTime, CustomerId: customer, VesselId: vessel, Locations: locations, CurrentId: currentOrderId  }, function (data) {
        if (data === 'NoSimilarOrder') {
            $('#divLoading').modal({ backdrop: 'static', keyboard: false });
            currentForm.submit();
        }
        else {
            if (data === $('#RefereneceNo').val()) {
                $('#divLoading').modal({ backdrop: 'static', keyboard: false });
                currentForm.submit();
            }
            else {
                if (confirm('Work orders with the same information already exist in the system. Are you sure you would like to submit this workorder? Existing Work Orders : ' + data)) {
                    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
                    currentForm.submit();
                }
            }
        }
    });
});

$('#btnSubmitOrder').click(function (e) {
    e.preventDefault();
    
    var pickupDateArr = $('#PickUpDateBinding').val().split('/');
    var pickupTimeArr = $('#PickUpTimeBinding').val().split(':');

    if (pickupDateArr.length == 1 || pickupTimeArr.length == 1)
    {
        alert('Pickup Date and Time are required fields. Please ensure to fill in the fields.');
        return;
    }
    else
    {
        var pickupDateTime = new Date(pickupDateArr[2], pickupDateArr[1] - 1, pickupDateArr[0], pickupTimeArr[0], pickupTimeArr[1]);

        var currentDateTime = new Date(Date.now());

        currentDateTime.setHours(currentDateTime.getHours() + 2);
    
        var result = pickupDateTime - currentDateTime;
        if(result < 0 || result === 0)
        {
            alert('Pickup Time cannot be less than two hours from current time. Contact MTS OPS to modify the details.');
            return;
        }

        var previousStatus = $('#Status').val();
               
        $('#Status').val("Ordered");

        var currentEntityRefNo = $('#ReferenceNo').val();

        var locations = new Array();

        $('tbody#LocationContainer').find("tr").each(function (index) {
            var location = {};
            location.locationId = $('#WorkOrderLocationList_' + index + '__LocationId').val();
            location.hotelId = $('#WorkOrderLocationList_' + index + '__HotelId').val();
            location.locationType = $('#WorkOrderLocationList_' + index + '__Type').val();
            locations.push(location);
        });

        if (!locations.length || locations.length < 2) {
            alert('Locations for pick-up and drop-off points must be specified.');
            return;
        }

        var passengers = new Array();

        $('tbody#PassengerContainer').find("tr").each(function (index) {
            var passenger = {};
            passenger.locationId = $('#WorkOrderPassengerList_' + index + '__InCharge').val();
            passengers.push(passenger);
        });

        if (!passengers.length || passengers.length < 1) {
            alert('At least one passenger must be specified.');
            return;
        }

        if (currentEntityRefNo == 'TBD' || previousStatus == 'Draft' || previousStatus == 'Ordered') {

            var rows = $('tbody#LocationContainer').find("tr");

            var vessel = $('#VesselId').val();
            var customer = $('#Customers').val();
            var pickUpDate = $('#PickUpDateBinding').val();
            var pickUpTime = $('#PickUpTimeBinding').val();

            var currentOrderId = 0;
            if (currentEntityRefNo != 'TBD')
            {
                currentOrderId = $('#Id').val();
            }


            var currentForm = this.form;

            var url = '/CustomerOrder/CheckForSimilarOrders';

            $.post(url, { PickUpDateBinding: pickUpDate, PickUpTimeBinding: pickUpTime, CustomerId: customer, VesselId: vessel, Locations: locations, CurrentId: currentOrderId }, function (data) {
                if (data === 'NoSimilarOrder') {
                    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
                    currentForm.submit();
                    //alert("Submitted : " + data);
                }
                else {
                    if (data === $('#RefereneceNo').val()) {
                        $('#divLoading').modal({ backdrop: 'static', keyboard: false });
                        currentForm.submit();
                    }
                    else {
                        if (confirm('Work orders with the same information already exist in the system. Are you sure you would like to submit this workorder? Existing Work Orders : ' + data)) {
                            $('#divLoading').modal({ backdrop: 'static', keyboard: false });
                            currentForm.submit();
                            //alert(data);
                        }
                        else {
                            $('#Status').val(previousStatus);
                        }

                    }
                }
            });
            //$.ajax({
            //    url: '/WorkOrder/CheckForSimilarOrders',
            //    dataType: "json",
            //    type: "POST",
            //    contentType: 'application/json; charset=utf-8',
            //    data: JSON.stringify({ PickUpDate: pickupDateTime.toISOString(), CustomerId: customer, VesselId: vessel, Locations: locations }),
            //    async: false,
            //    timeout: 30000,
            //    cache: false,
            //    success: function (data) {
            //        if (data == 'False') {
            //            $('#divLoading').modal({ backdrop: 'static', keyboard: false });
            //            currentForm.submit();
            //        }
            //        else {
            //            if (confirm('Work orders with the same information already exist in the system. Are you sure you would like to submit this workorder? Exisiting Work Orders : ' + data)) {
            //                $('#divLoading').modal({ backdrop: 'static', keyboard: false });
            //                currentForm.submit();
            //            }
            //        }
            //    },
            //    error: function (xhr) {
            //    }
            //});
        }
        else {
            $('#divLoading').modal({ backdrop: 'static', keyboard: false });
            this.form.submit();
        }        
    }
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
    $('#workOrderLocationModal').modal({ backdrop: 'static', keyboard: false }) 
    //$('#workOrderLocationModal').modal('show');
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


