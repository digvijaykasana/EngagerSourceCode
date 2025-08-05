$(document).ready(function (e) {
    $('#result').load('/Agent/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/Agent/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

