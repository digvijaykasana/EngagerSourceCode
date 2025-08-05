$(document).ready(function (e) {
    $('#result').load('/InvoicingAdmin/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/InvoicingAdmin/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

