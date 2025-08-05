$(document).ready(function (e) {
    $('#result').load('/Driver/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        var vehicle = $('#Vehicle').val();
        $('#result').load('/Driver/List?SearchValue=' + encodeURIComponent(searchValue) + '&Vehicle=' + encodeURIComponent(vehicle));
    });
});

