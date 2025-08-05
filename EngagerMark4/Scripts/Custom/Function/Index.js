$(document).ready(function (e) {
    $('#result').load('/Function/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/Function/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

