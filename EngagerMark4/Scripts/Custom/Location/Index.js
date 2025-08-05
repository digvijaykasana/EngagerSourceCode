$(document).ready(function (e) {
    $('#result').load('/Location/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/Location/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

