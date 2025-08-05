$(document).ready(function (e) {
    $('#result').load('/User/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/User/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

