$(document).ready(function (e) {
    $('#result').load('/AgentManager/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#SearchValue').val();
        $('#result').load('/AgentManager/List?SearchValue=' + encodeURIComponent(searchValue));
    });
});

