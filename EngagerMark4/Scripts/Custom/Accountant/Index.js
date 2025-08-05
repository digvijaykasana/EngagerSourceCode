$(document).ready(function (e) {
    $('#result').load('/Accountant/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/Accountant/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

