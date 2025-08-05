$('.sorter').click(function (e) {
    e.preventDefault();
    var column = $(this).attr('column');
    var orderBy = $(this).attr('orderBy');
    $('#result').load('/InvoiceSummary/List?column=' + column + '&orderBy=' + orderBy);
});

$('.pagination a').click(function (e) {
    e.preventDefault();
    $('#result').load($(this).attr('href'));
});