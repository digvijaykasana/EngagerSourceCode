$(document).ready(function (e) {
    $('#result').load('/OpsAdmin/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/OpsAdmin/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

