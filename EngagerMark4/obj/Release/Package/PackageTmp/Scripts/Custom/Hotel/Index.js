$(document).ready(function (e) {
    $('#result').load('/Hotel/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/Hotel/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

