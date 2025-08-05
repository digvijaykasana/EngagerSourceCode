$(document).ready(function (e) {
    $('#result').load('/CreditNoteSummary/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var referenceNo = $('#ReferenceNo').val();
        var customerId = $('#CustomerId').val();
        var fromDate = $('#txtFromDate').val();
        var toDate = $('#txtToDate').val();
        var status = $('#Status').val();

        $('#result').load('/CreditNoteSummary/List?ReferenceNo=' + referenceNo + "&CustomerId=" + customerId + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Status=" + status);
    });

    $('#divFromDate').datetimepicker({
        format: 'DD/MM/YYYY'
    });

    $('#divToDate').datetimepicker({
        format: 'DD/MM/YYYY'
    });

});