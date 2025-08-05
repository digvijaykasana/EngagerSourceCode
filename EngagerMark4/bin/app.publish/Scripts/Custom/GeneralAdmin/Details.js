$('#btnSave').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    this.form.submit();
});

$('.clsAddRole').click(function (e) {
    e.preventDefault();
    var functionId = $('#RoleId').val();
    var functionName = $('#RoleId option:selected').text();
    var rows = $("tbody#RoleContainer").find("tr");
    var lastIndex = rows.last().index();

    if (!functionId) {
        alert('Please select a role!');
        $('#UserRoleList_' + lastIndex + "__RoleId").parents('tr').last().remove();
        return;
    }
    rows.each(function (index, e) {
        var isDeleted = $('#UserRoleList_' + index + "__Delete").val();
        if (isDeleted != 'True') {
            var inFunctionId = $('#UserRoleList_' + index + '__RoleId').val();
            if (inFunctionId == functionId) {
                alert('Role is already in the list!');
                $('#UserRoleList_' + lastIndex + "__RoleId").parents('tr').last().remove();
                return;
            }
        }
    });
    $('#UserRoleList_' + lastIndex + '__Role_Name').val(functionName);
    $('#UserRoleList_' + lastIndex + '__Role_Code').val(functionName);
    $('#UserRoleList_' + lastIndex + '__RoleId').val(functionId);
});

$('.clsAddVehicle').click(function (e) {
    e.preventDefault();
    var functionId = $('#VehicleId').val();
    var functionName = $('#VehicleId option:selected').text();
    var rows = $("tbody#VehicleContainer").find("tr");
    var lastIndex = rows.last().index();

    if (!functionId) {
        alert('Please select a vehicle!');
        $('#VehicleList_' + lastIndex + "__VehicleId").parents('tr').last().remove();
        return;
    }
    rows.each(function (index, e) {
        var isDeleted = $('#VehicleList_' + index + "__Delete").val();
        if (isDeleted != 'True') {
            var inFunctionId = $('#VehicleList_' + index + '__VehicleId').val();
            if (inFunctionId == functionId) {
                alert('Vehicle is already in the list!');
                $('#VehicleList_' + lastIndex + "__VehicleId").parents('tr').last().remove();
                return;
            }
        }
    });
    $('#VehicleList_' + lastIndex + '__VehicleNo').val(functionName);
    $('#VehicleList_' + lastIndex + '__VehicleId').val(functionId);
});

$('.clsAddCustomer').click(function (e) {
    e.preventDefault();
    var functionId = $('#CustomerId').val();
    var functionName = $('#CustomerId option:selected').text();
    var rows = $("tbody#CustomerContainer").find("tr");
    var lastIndex = rows.last().index();

    if (!functionId) {
        alert('Please select a customer!');
        $('#CustomerList_' + lastIndex + "__CustomerId").parents('tr').last().remove();
        return;
    }
    rows.each(function (index, e) {
        var isDeleted = $('#CustomerList_' + index + "__Delete").val();
        if (isDeleted != 'True') {
            var inFunctionId = $('#CustomerList_' + index + '__CustomerId').val();
            if (inFunctionId == functionId) {
                alert('Customer is already in the list!');
                $('#CustomerList_' + lastIndex + "__CustomerId").parents('tr').last().remove();
                return;
            }
        }
    });
    $('#CustomerList_' + lastIndex + '__CustomerName').val(functionName);
    $('#CustomerList_' + lastIndex + '__CustomerId').val(functionId);
});

$('#btnReset').click(function (e) {
    e.preventDefault();

    var confirmResult = confirm("Would you like to reset this driver's Log In Status?");
    if (confirmResult) {
        $('#divLoading').modal({ backdrop: 'static', keyboard: false });

        var _userName = $('#UserName').val();

        $.post('/UserApi/ResetFCMId',
            {
                userName: _userName
            }, function success(data, status) {
                if (data == 'success') {
                    alert("FCM Id resetted successfully!");
                    $('#FCMIdStr').val("");
                } else {
                    alert("User was not found. Please reload the page and try again.");
                }
            });
        $('#divLoading').modal('toggle');

    }
});