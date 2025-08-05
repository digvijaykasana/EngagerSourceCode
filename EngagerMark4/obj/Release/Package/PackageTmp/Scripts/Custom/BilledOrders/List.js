$(document).ready(function () {
    $('#tblBilledOrder').DataTable({
        "columnDefs": [
            { "targets": [0, 1], "orderable": false }
        ],
        order: [[2, 'asc']],
        "pageLength": 100
    });
});


$('.sorter').click(function (e) {
    e.preventDefault();
    var column = $(this).attr('column');
    var orderBy = $(this).attr('orderBy');
    var dataType = $(this).attr('dataType');
    var parameters = $('#headerParameters').val();
    var url = '/BilledOrders/List';
    url += '?column=' + column + '&orderBy=' + orderBy + "&dataType=" + dataType + parameters;
    $('#result').load(url);
});

$('.btn-danger').click(function (e) {
    e.preventDefault();
    var entityId = $(this).attr('entityId');
    $('#DeleteFormId').val(entityId);
    $('#divRecordDelete').modal('show');
});

$('.pagination a').click(function (e) {
    e.preventDefault();
    $('#result').load($(this).attr('href'));
});

$('.lnkDownloadInvoice').click(function (e) {
    e.preventDefault();

    var invoiceNo = $(this).attr('invoiceNo');

    window.open("/BilledOrders/DownloadInvoice?invoiceSummaryNo=" + invoiceNo, "popupWindowINV", "width=800, height=600, scrollbars=yes");
});

$('.lnkDownloadCN').click(function (e) {
    e.preventDefault();

    var invoiceNo = $(this).attr('invoiceNo');

    window.open("/BilledOrders/DownloadCreditNote?invoiceSummaryNo=" + invoiceNo, "popupWindowCN", "width=800, height=600, scrollbars=yes");
});