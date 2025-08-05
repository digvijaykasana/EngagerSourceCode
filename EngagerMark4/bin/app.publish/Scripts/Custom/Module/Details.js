$('#btnSave').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    this.form.submit();
});

$('.clsAddFunction').click(function (e) {
    e.preventDefault();
    var functionId = $('#FunctionId').val();
    var functionName = $('#FunctionId option:selected').text();
    var rows = $("tbody#FunctionContainer").find("tr");
    var lastIndex = rows.last().index();

    if (!functionId) {
        alert('Please select a function!');
        $('#FunctionList_' + lastIndex + "__PermissionId").parents('tr').last().remove();
        return;
    }
    rows.each(function (index, e) {
        var isDeleted = $('#FunctionList_' + index + "__Delete").val();
        if (isDeleted != 'True') {
            var inFunctionId = $('#FunctionList_' + index + '__PermissionId').val();
            if (inFunctionId == functionId) {
                alert('Function is already in the list!');
                $('#FunctionList_' + lastIndex + "__PermissionId").parents('tr').last().remove();
                return;
            }
        }
    });
    $('#FunctionList_' + lastIndex + '__Permission_Name').val(functionName);
    $('#FunctionList_' + lastIndex + '__Permission_Controller').val(functionName);
    $('#FunctionList_' + lastIndex + '__PermissionId').val(functionId);
});