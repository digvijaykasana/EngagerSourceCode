$('#btnSave').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    this.form.submit();
});

$('#divStartTime').datetimepicker({
    format: 'LT'
});

$('#divEndTime').datetimepicker({
    format: 'LT'
});

$('.clsAddLocation').click(function (e) {
    e.preventDefault();
    clearModalData();
    $('#btnAddLocation').attr('style', 'display:normal;');
    $('#btnEditLocation').attr('style', 'display:none;');
    $('#priceLocationModal').modal('show');
});

$('body').on('click', 'a.btnEditLocation', function () {
    var tableIndex = $(this).parents('tr').attr('tableIndex');
    var locationId = $('#' + tableIndex + 'LocationId').val();
    var locationType = $('#' + tableIndex + 'Type').val();
    var locationDescription = $('#' + tableIndex + 'Description').val();
    $('#LocationType').val(locationType);
    if (locationId)
        $('#Locations').val(locationId);
    else
        $('#Locations').val(null);
    $('#txtLocationDescription').val(locationDescription);
    $('#tableIndex').val(tableIndex);
    $('#btnAddLocation').attr('style', 'display:none;');
    $('#btnEditLocation').attr('style', 'display:normal;');
    $('#priceLocationModal').modal('show');
});

$('#btnLocationClose').click(function (e) {
    var tableIndex = $('#tableIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#LocationContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#PriceLocationList_' + lastIndex + "__LocationId").parents('tr').last().remove();
    }
})

$('#btnAddLocation').click(function (e) {
    e.preventDefault();
    if (saveLocation()) {
        $('#priceLocationModal').modal('toggle');
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
    if (!locationId && !locationDescription) {
        alert("Location Description required!");
        $('#txtLocationDescription').focus();
        return false;
    }
    $('#' + tableIndex + 'LocationId').val(locationId);
    $('#' + tableIndex + 'Location_Id').val(locationId);
    $('#' + tableIndex + 'Type').val(locationType);
    $('#' + tableIndex + 'TypeStr').val(locationTypeStr);
    $('#' + tableIndex + 'Location_Display').val(locationStr);
    $('#' + tableIndex + 'Description').val(locationDescription);
    $('#' + tableIndex + 'Location_PostalCode').val(locationStr);
    $('#priceLocationModal').modal('toggle');
})

function clearModalData() {
    $('#tableIndex').val(null);
    $('#LocationType').val(10);
    $('#Locations').val(null);
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

    var rows = $('tbody#LocationContainer').find("tr");
    var lastIndex = rows.last().index();
    $('#PriceLocationList_' + lastIndex + '__LocationId').val(locationId);
    $('#PriceLocationList_' + lastIndex + '__Location_Id').val(locationId);
    $('#PriceLocationList_' + lastIndex + '__Type').val(locationType);
    $('#PriceLocationList_' + lastIndex + '__TypeStr').val(locationTypeStr);
    $('#PriceLocationList_' + lastIndex + '__Location_Display').val(locationStr);
    $('#PriceLocationList_' + lastIndex + '__Description').val(locationDescription);
    $('#PriceLocationList_' + lastIndex + '__Location_Code').val('TBA');
    $('#PriceLocationList_' + lastIndex + '__Location_Name').val('TBA');
    $('#PriceLocationList_' + lastIndex + '__Location_PostalCode').val(locationStr);
    return true;
}



$('#btnCancel').click(function (e) {
    var tableIndex = $('#tableIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#LocationContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#PriceLocationList_' + lastIndex + "__LocationId").parents('tr').last().remove();
    }
})


