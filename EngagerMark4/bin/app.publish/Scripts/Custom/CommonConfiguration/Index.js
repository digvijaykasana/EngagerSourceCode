$(document).ready(function (e) {
    $('#result').load('/CommonConfiguration/List');
});

$('#btnSearch').click(function (e) {
    e.preventDefault();
    var configurationGroupId = $('#ConfigurationGroupId').val();
    $('#result').load('/CommonConfiguration/List?ConfigurationGroupId=' + configurationGroupId);
});