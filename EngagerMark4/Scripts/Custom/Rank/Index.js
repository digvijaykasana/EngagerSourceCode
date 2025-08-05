$(document).ready(function (e) {
    $('#result').load('/Rank/List');
});

$('#btnSearch').click(function (e) {
    e.preventDefault();
    var searchValue = $('#SearchValue').val();
    $('#result').load('/Rank/List?SearchValue=' + encodeURIComponent(searchValue));
});