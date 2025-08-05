$(document).ready(function (e) {
    $('#result').load('/Vessel/List');
});

$('#btnSearch').click(function (e) {
    e.preventDefault();
    var searchValue = $('#SearchValue').val();
    $('#result').load('/Vessel/List?SearchValue=' + encodeURIComponent(searchValue));
});     