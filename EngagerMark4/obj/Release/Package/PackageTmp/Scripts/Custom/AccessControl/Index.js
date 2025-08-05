$('#btnSave').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    this.form.submit();
});

$('#UserRoleId').change(function (e) {
    var roleId = $(this).val();
    window.location.href = "/AccessControl?UserRoleId=" + roleId;
});

$('.permissionDetails').click(function (e) {
    e.preventDefault();
    var permissionId = $(this).attr('permissionid');
    var roleId = $(this).attr('roleId');
    $('#RoleId').val(roleId);
    $('#PermissionId').val(permissionId);
    $.get('/RolePermissionDetailsApi',
        {
            roleId: roleId,
            permissionId: permissionId
        }, function (data, status) {
            $.each(data, function (index, item) {
                if (item.Code == 'Create') {
                    if (item.Value == 'true') {
                        $('#chkCreate').prop('checked', true);
                    } else {
                        $('#chkCreate').prop('checked', false);
                    }
                }
                if (item.Code == 'Edit') {
                    if (item.Value == 'true') {
                        $('#chkUpdate').prop('checked', true);
                    } else {
                        $('#chkUpdate').prop('checked', false);
                    }
                }
                if (item.Code == 'Delete') {
                    if (item.Value == 'true') {
                        $('#chkDelete').prop('checked', true);
                    } else {
                        $('#chkDelete').prop('checked', false);
                    }
                }
            });
            
            $('#divAccessDetails').modal('show');
        });
    
});

$('#btnSaveAccessDetails').click(function (e) {
    e.preventDefault();
    var create = $('#chkCreate').prop('checked');
    var edit = $('#chkUpdate').prop('checked');
    var l_delete = $('#chkDelete').prop('checked');
    var roleId = $('#RoleId').val();
    var permissionId = $('#PermissionId').val();
    $.post('/RolePermissionDetailsApi/Save',
        {
            roleId: roleId,
            permissionId: permissionId,
            create: create,
            edit: edit,
            delete: l_delete
        }, function success (data, status) {
            alert(data);

            if (data == 'success') {
                $('#divAccessDetails').modal('toggle');
            }
        });

});