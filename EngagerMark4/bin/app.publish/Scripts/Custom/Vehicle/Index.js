$(document).ready(function (e) {
    $('#result').load('/Vehicle/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/Vehicle/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

