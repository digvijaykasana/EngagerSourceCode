$('#btnSave').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    this.form.submit();
});

$('.clsAddChecklist').click(function (e) {
    e.preventDefault();
    var checklistId = $('#ChecklistId').val();
    var checkListName = $('#ChecklistId option:selected').text();
    var rows = $("tbody#ChecklistContainer").find("tr");
    var lastIndex = rows.last().index();

    if (!checklistId) {
        alert('Please select a checklist!');
        $('#Details_' + lastIndex + "__ChecklistId").parents('tr').last().remove();
        return;
    }
    rows.each(function (index, e) {
        var isDeleted = $('#Details_' + index + "__Delete").val();
        if (isDeleted != 'True') {
            var inFunctionId = $('#Details_' + index + '__ChecklistId').val();
            if (inFunctionId == checklistId) {
                alert('Checklist is already in the list!');
                $('#Details_' + lastIndex + "__ChecklistId").parents('tr').last().remove();
                return;
            }
        }
    });
    $('#Details_' + lastIndex + '__Checklist_Name').val(checkListName);
    $('#Details_' + lastIndex + '__Checklist_Code').val(checkListName);
    $('#Details_' + lastIndex + '__ChecklistId').val(checklistId);
});