$('#btnSave').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });

    var code = $('#Code').val();
    var name = $('#Name').val();
    var cfgGrpId = $('#ConfigurationGroupId').val();
    var url = "/CommonConfiguration/CheckForSimilarConfigs";

    var currentForm = this.form;

    $.post(url, { Code: code, Name: name, ConfigurationGrpId: cfgGrpId},
        function (data, status) {
            if (data === 'NoSimilarConfig') {
                $('#divLoading').modal({ backdrop: 'static', keyboard: false });
                currentForm.submit();
            }
            else {
                $('#divLoading').modal('hide');
                alert('Configurations with similar code and name already exists in this current configuration group. Please check before saving again!');
            }
        }
    );
});