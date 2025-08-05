$(document).ready(function (e) {
    $('#result').load('/Letterhead/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/Letterhead/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

