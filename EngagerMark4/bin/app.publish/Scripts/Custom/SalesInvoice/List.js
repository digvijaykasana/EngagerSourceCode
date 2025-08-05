$('.sorter').click(function (e) {
    e.preventDefault();
    var column = $(this).attr('column');
    var orderBy = $(this).attr('orderBy');
    var dataType = $(this).attr('dataType');
    var url = $(this).attr('href');
    url += '&column=' + column + '&orderBy=' + orderBy + "&dataType=" + dataType;
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