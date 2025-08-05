$('#CreditNoteDateStr').datetimepicker({
    format: 'DD/MM/YYYY'
});

$('#FromDateStr').datetimepicker({
    format: 'DD/MM/YYYY'
});

$('#ToDateStr').datetimepicker({
    format: 'DD/MM/YYYY'
});

$('#CustomerId').change(function (e) {
    RefreshData();
});


$('#FromDateStr').blur(function (e) {
    RefreshData();
});

$('#ToDateStr').blur(function (e) {
    RefreshData();
});

function RefreshData() {
    var id = $('#Id').val();
    var customerId = $('#CustomerId').val();
    var fromDate = $('#FromDateStr').val();
    var toDate = $('#ToDateStr').val();
    window.location.href = "/CreditNoteSummary/Details/" + id + "?customerId=" + customerId + "&fromDate=" + fromDate + "&toDate=" + toDate;
}