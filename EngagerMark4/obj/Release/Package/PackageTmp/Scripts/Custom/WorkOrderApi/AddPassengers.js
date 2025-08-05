$(document).ready(function (e) {
    $('#txtName').focus();
});

$('.clsAddPassenger').click(function (e) {
    e.preventDefault();
    var passengerName = $('#txtName').val();
    var rankId = $('#Ranks').val();
    var rank = $('#Ranks option:selected').text();
    var rows = $('tbody#PassengerContainer').find("tr");
    var lastIndex = rows.last().index();
    if (!passengerName) {
        alert("Passenger Name required!");
        $('#WorkOrderPassengerList_' + lastIndex + "__RankId").parents('tr').last().remove();
        $('#txtName').focus();
        return;
    }
    $('#WorkOrderPassengerList_' + lastIndex + "__Name").val(passengerName);
    $('#WorkOrderPassengerList_' + lastIndex + "__Rank").val(rank);
    $('#WorkOrderPassengerList_' + lastIndex + '__RankId').val(rankId);
    $("#txtName").val('');
    $('#txtName').focus();
});

$('#btnSave').click(function (e) {
    e.preventDefault();
    $(this).prop('disabled', 'true');
    this.form.submit();
});