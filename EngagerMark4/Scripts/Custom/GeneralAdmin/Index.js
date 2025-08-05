$(document).ready(function (e) {
    $('#result').load('/GeneralAdmin/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/GeneralAdmin/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

