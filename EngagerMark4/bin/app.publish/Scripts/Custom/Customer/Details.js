$('#btnSave').click(function (e) {
    e.preventDefault();


    //Detect Duplicate Customer based on Email, Phone No, Account No
    let email = $('#Email').val();
    let officeNo = $('#OfficeNo').val();
    let acctNo = $('#AccNo').val();

    let currentId = $('#Id').val();

    if (currentId == 0) {

        let currentForm = this.form;

        let url = "/Customer/CheckForSimilarCustomers";

        $.post(url, { Email: email, OfficeNo: officeNo, AccountNo: acctNo },
            function (data, status) {
                if (data === 'NoSimilarCustomer') {
                    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
                    currentForm.submit();
                    //alert('Submitted');
                }
                else {
                    if (confirm('Duplicate Customer(s) with the same info detected. Proceed to save this customer? Existing : ' + data)) {
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
});

$('.clsAddLocation').click(function (e) {
    e.preventDefault();
    clearModalData();
    $('#btnAddLocation').attr('style', 'display:normal');
    $('#btnEditLocation').attr('style', 'display:none');
    $('#customerLocationModal').modal({ backdrop: 'static', keyboard: false });
});

$('.clsAddVessel').click(function (e) {
    e.preventDefault();
    var vesselId = $('#Vessels').val();
    var vessel = $('#Vessels option:selected').text();
    var rows = $('tbody#VesselContainer').find('tr');
    var lastIndex = rows.last().index();
    if (!vesselId) {
        alert('Please select a vessel!');
        $('#VesselList_' + lastIndex + "__VesselId").parents('tr').last().remove();
        return;
    }
    rows.each(function (index, e) {
        var isDeleted = $('#VesselList_' + index + "__Delete").val();
        if (isDeleted != 'True') {
            var inFunctionId = $('#VesselList_' + index + '__VesselId').val();
            if (inFunctionId == vesselId) {
                alert('Vessel is already in the list!');
                $('#VesselList_' + lastIndex + "__VesselId").parents('tr').last().remove();
                return;
            }
        }
    });
    $('#VesselList_' + lastIndex + "__VesselId").val(vesselId);
    $('#VesselList_' + lastIndex + "__Vessel").val(vessel);
});

$('#btnCancel').click(function (e) {
    var rows = $("tbody#LocationContainer").find("tr");
    var tableIndex = $('#tableIndex').val();
    if (tableIndex) {

    } else {
        var lastIndex = rows.last().index();
        $('#Locations_' + lastIndex + "__LocationId").parents('tr').last().remove();
    }
});

$('#btnClose').click(function (e) {
    var tableIndex = $('#tableIndex').val();
    if (tableIndex) {

    } else {
        var rows = $("tbody#LocationContainer").find("tr");
        var lastIndex = rows.last().index();
        $('#Locations_' + lastIndex + "__LocationId").parents('tr').last().remove();
    }
})

function clearModalData() {
    $('#chkMain').prop('checked', false);
    $('#tableIndex').val(null);
    $('#txtContactPerson').val('');
    $('#txtContactNo').val('');
    $('#txtFax').val('');
    $('#txtEmail').val('');
    $('#txtCode').val('');
    $('#txtName').val('');
    $('#txtPostalCode').val('');
    $('#txtAddress').val('');
    $('#txtCity').val('');
    $('#txtCountry').val('');
    $('#txtLatitude').val('');
    $('#txtLongitude').val('');
}

$('#btnAddLocation').click(function (e) {
    e.preventDefault();
    if (saveLocation()) {
        $('#customerLocationModal').modal('toggle');
    }
});

function saveLocation() {
    var main = $('#chkMain').prop('checked');
    var contactPerson = $('#txtContactPerson').val();
    var contactNo = $('#txtContactNo').val();
    var fax = $('#txtFax').val();
    var email = $('#txtEmail').val();
    var code = $('#txtCode').val();
    var name = $('#txtName').val();
    var postalCode = $('#txtPostalCode').val();
    var address = $('#txtAddress').val();
    var city = $('#txtCity').val();
    var country = $('#txtCountry').val();
    var latitude = $('#txtLatitude').val();
    var longitude = $('#txtLongitude').val();

    if (!contactPerson) {
        alert('Contact Person required!');
        $('#txtContactPerson').focus();
        return false;
    }

    if (!code) {
        alert('Code required!');
        $('#txtCode').focus();
        return false;
    }

    if (!name) {
        alert('Name required!');
        $('#txtName').focus();
        return false;
    }

    if (!postalCode) {
        alert('Postal Code required!');
        $('#txtPostalCode').focus();
        return false;
    }

    var rows = $('tbody#LocationContainer').find('tr');
    var lastIndex = rows.last().index();

    $('#Locations_' + lastIndex + "__Main").prop('checked', main);
    $('#Locations_' + lastIndex + "__Location_Code").val(code);
    $('#Locations_' + lastIndex + "__Location_Name").val(name);
    $('#Locations_' + lastIndex + "__ContactPerson").val(contactPerson);
    $('#Locations_' + lastIndex + "__ContactNo").val(contactNo);
    $('#Locations_' + lastIndex + "__Fax").val(fax);
    $('#Locations_' + lastIndex + "__Email").val(email);
    $('#Locations_' + lastIndex + "__Location_Latitude").val(latitude);
    $('#Locations_' + lastIndex + "__Location_Longitude").val(longitude);
    $('#Locations_' + lastIndex + "__Location_PostalCode").val(postalCode);
    $('#Locations_' + lastIndex + "__Location_Address").val(address);
    $('#Locations_' + lastIndex + "__Location_City").val(city);
    $('#Locations_' + lastIndex + "__Location_Country").val(country);
    return true;
}

$('body').on('click', 'a.btnEdit', function () {
    var tableIndex = $(this).parents('tr').attr('tableIndex');
    var main = $('#' + tableIndex + "Main").prop('checked');
    var code = $('#' + tableIndex + "Location_Code").val();
    var name = $('#' + tableIndex + "Location_Name").val();
    var contactPerson = $('#' + tableIndex + "ContactPerson").val();
    var contactNo = $('#' + tableIndex + "ContactNo").val();
    var fax = $('#' + tableIndex + "Fax").val();
    var email = $('#' + tableIndex + "Email").val();
    var latitude = $('#' + tableIndex + "Location_Latitude").val();
    var longitude = $('#' + tableIndex + "Location_Longitude").val();
    var postalCode = $('#' + tableIndex + "Location_PostalCode").val();
    var address = $('#' + tableIndex + "Location_Address").val();
    var city = $('#' + tableIndex + "Location_City").val();
    var country = $('#' + tableIndex + "Location_Country").val();

    $('#tableIndex').val(tableIndex);
    $('#chkMain').prop('checked', main);
    $('#txtContactPerson').val(contactPerson);
    $('#txtContactNo').val(contactNo);
    $('#txtFax').val(fax);
    $('#txtEmail').val(email);
    $('#txtCode').val(code);
    $('#txtName').val(name);
    $('#txtPostalCode').val(postalCode);
    $('#txtAddress').val(address);
    $('#txtCity').val(city);
    $('#txtCountry').val(country);
    $('#txtLatitude').val(latitude);
    $('#txtLongitude').val(longitude);
    $('#btnAddLocation').attr('style', 'display:none');
    $('#btnEditLocation').attr('style', 'display:normal');
    $('#customerLocationModal').modal('show');
});

$('#btnEditLocation').click(function (e) {
    e.preventDefault();
    var tableIndex = $('#tableIndex').val();
    var main = $('#chkMain').prop('checked');
    var contactPerson = $('#txtContactPerson').val();
    var contactNo = $('#txtContactNo').val();
    var fax = $('#txtFax').val();
    var email = $('#txtEmail').val();
    var code = $('#txtCode').val();
    var name = $('#txtName').val();
    var postalCode = $('#txtPostalCode').val();
    var address = $('#txtAddress').val();
    var city = $('#txtCity').val();
    var country = $('#txtCountry').val();
    var latitude = $('#txtLatitude').val();
    var longitude = $('#txtLongitude').val();

    if (!contactPerson) {
        alert('Contact Person required!');
        $('#txtContactPerson').focus();
        return false;
    }

    if (!code) {
        alert('Code required!');
        $('#txtCode').focus();
        return false;
    }

    if (!name) {
        alert('Name required!');
        $('#txtName').focus();
        return false;
    }

    if (!postalCode) {
        alert('Postal Code required!');
        $('#txtPostalCode').focus();
        return false;
    }

    $('#' + tableIndex + "Main").prop('checked', main);
    $('#' + tableIndex + "Location_Code").val(code);
    $('#' + tableIndex + "Location_Name").val(name);
    $('#' + tableIndex + "ContactPerson").val(contactPerson);
    $('#' + tableIndex + "ContactNo").val(contactNo);
    $('#' + tableIndex + "Fax").val(fax);
    $('#' + tableIndex + "Email").val(email);
    $('#' + tableIndex + "Location_Latitude").val(latitude);
    $('#' + tableIndex + "Location_Longitude").val(longitude);
    $('#' + tableIndex + "Location_PostalCode").val(postalCode);
    $('#' + tableIndex + "Location_Address").val(address);
    $('#' + tableIndex + "Location_City").val(city);
    $('#' + tableIndex + "Location_Country").val(country);

    $('#customerLocationModal').modal('toggle');
});


$('#txtPostalCode').blur(function (e) {
    var postalCode = $(this).val();
    if (postalCode) {
        $.get('/LocationApi/GetCoordinate',
            {
                aPostalCode: postalCode
            }, function success(data, status) {
                $('#txtLatitude').val(data.Latitude);
                $('#txtLongitude').val(data.Longitude);
            })
    }
});
