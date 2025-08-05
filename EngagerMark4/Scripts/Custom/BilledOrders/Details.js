$('#btnSave').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    this.form.submit();
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

$('#CustomerId').change(function (e) {
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
                
            });
        $.get('/AgentApi/GetByCustomerId',
            {
                customerId: customerId
            }, function success(data, status) {
                $('#AgentId').empty();
                $.each(data, function (index, item) {
                    $('#AgentId').append($('<option>').html(item.Name).val(item.Id));
                });

            });
    }
});

$('.clsAddLocation').click(function (e) {
    e.preventDefault();
    clearModalData();
    $('#btnAddLocation').attr('style', 'display:normal;');
    $('#btnEditLocation').attr('style', 'display:none;');
    $('#workOrderLocationModal').modal('show');
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
    $('#workOrderLocationModal').modal('show');
});

$('#btnLocationClose').click(function (e) {
    var tableIndex = $('#tableIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#LocationContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#WorkOrderLocationList_' + lastIndex + "__LocationId").parents('tr').last().remove();
    }
})

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
})


