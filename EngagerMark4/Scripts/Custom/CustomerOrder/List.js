$('.sorter').click(function (e) {
    e.preventDefault();
    var column = $(this).attr('column');
    var orderBy = $(this).attr('orderBy');
    var dataType = $(this).attr('dataType');
    var parameters = $('#headerParameters').val();
    var url = "/CustomerOrder/List";
    url += '?column=' + column + '&orderBy=' + orderBy + "&dataType=" + dataType + parameters;
    $('#result').load(url);
});

$('.btn-danger').click(function (e) {
    e.preventDefault();
    var entityId = $(this).attr('entityId');
    $('#DeleteFormId').val(entityId);
    $('#divRecordDelete').modal('show');
});

$('.pagination a').click(function (e) {
    e.preventDefault();
    $('#result').load($(this).attr('href'));
});

$('#checkAll').click(function (e) {
    var checked = $(this).prop('checked');
    $('.invoices').each(function (index, element) {
        $(this).prop('checked', checked);
    });
});

$('.invoices').click(function (e) {
    var check = $(this).prop('checked');
    var id = $(this).val();
    $('#cn_' + id).prop('checked', check);
});