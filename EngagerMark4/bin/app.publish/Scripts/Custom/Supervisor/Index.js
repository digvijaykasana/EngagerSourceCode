$(document).ready(function (e) {
    $('#result').load('/Supervisor/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/Supervisor/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

