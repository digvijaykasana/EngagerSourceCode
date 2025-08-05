$(document).ready(function (e) {
    $('#result').load('/InvoicingClerk/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/InvoicingClerk/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

