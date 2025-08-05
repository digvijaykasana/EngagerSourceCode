$(document).ready(function (e) {
    $('#result').load('/Admin/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/Admin/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

