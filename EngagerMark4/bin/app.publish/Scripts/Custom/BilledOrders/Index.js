$(document).ready(function (e) {

    var customers = $('#Customers').val();
    var vessels = $('#Vessels').val();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var status = $('#Status').val();
    var SalesInvoiceSummaryNo = $('#SalesInvoiceSummaryNo').val();
    var isSearchByRange = 'false';
    if ($('#SearchByRange')[0].checked) {
        isSearchByRange = 'true';
    }
    var StartingSalesInvoiceSummaryNo = $('#SalesInvoiceSummaryStartingNo').val();
    var EndingSalesInvoiceSummaryNo = $('#SalesInvoiceSummaryEndingNo').val();
    var driver = $('#DriverId').val();
    var orderPage = $('#orderPage').val();
    var orderColumn = $('#orderColumn').val();
    var orderOrderBy = $('#orderOrderBy').val();
    var orderDataType = $('#orderDataType').val();
    var currentId = $('#CurrentId').val();
    $('#result').load("/BilledOrders/List?CurrentPage=" + orderPage + "&Column=" + orderColumn + "&OrderBy=" + orderOrderBy + "&DataType=" + orderDataType + "&Customers=" + customers + "&Vessels=" + vessels + "&Status=" + status + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&DriverId=" + driver + "&SalesInvoiceSummaryNo=" + SalesInvoiceSummaryNo + "&isSearchByRange=" + isSearchByRange + "&StartingSalesInvoiceSummaryNo=" + StartingSalesInvoiceSummaryNo + "&EndingSalesInvoiceSummaryNo=" + EndingSalesInvoiceSummaryNo + "&CurrentId=" + currentId);

    $("#ActionThing option[value='100']").remove();

    $('#btnDownload').click(function (e) {
        e.preventDefault();
        if ($('#SearchByRange')[0].checked) {
            if ((!($('#SalesInvoiceSummaryStartingNo').val().match($regexname))) || (!($('#SalesInvoiceSummaryEndingNo').val().match($regexname)))) {
                alert('Invalid Invoice Number Range Format.');
                return;
            }
        }
        var actionThing = $('#ActionThing').val();
        if (actionThing == 0) {
            //$('.invoices').each(function (index, item) {
            //    if (isChecked != true) {
            //        isChecked = $(this).prop('checked');
            //    }
            //});

            $('#frmPDF').attr('action', '/InvoiceExcel/Download');
            this.form.submit();
        }
        else if (actionThing == 1) {
            //$('.invoices').each(function (index, item) {
            //    if (isChecked != true) {
            //        isChecked = $(this).prop('checked');
            //    }
            //});
            $('#frmPDF').attr('action', '/CreditNoteExcel/Download');
            this.form.submit();
        }
    });

    $('#btnMoveToWithAccounts').click(function (e) {
        e.preventDefault();
        $('#divLoading').modal({ backdrop: 'static', keyboard: false });
        var invoiceIds = new Array();
        $('.invoices').each(function (index, item) {
            var isChecked = $(this).prop('checked');
            if (isChecked) {

                var id = $(this).val();
                invoiceIds.push(id);
            }
        });
        $.post('/BilledOrders/MoveToWithAccounts',
            {
                invoiceIds: invoiceIds
            }, function success(data, status) {
                if (data == 'success') {

                    $('#divLoading').modal('toggle');
                    location.reload();

                } else {
                    $('#divLoading').modal('toggle');
                    alert("Moving to With Accounts failed. Please contact the Admin for further assistance.");
                }
                ;
            });
    });

    $('#btnSearch').click(function (e) {
        e.preventDefault();

        if ($('#SearchByRange')[0].checked) {
            if ((!($('#SalesInvoiceSummaryStartingNo').val().match($regexname))) || (!($('#SalesInvoiceSummaryEndingNo').val().match($regexname)))) {
                alert('Invalid Invoice Number Range Format.');
                return;
            }
        }

        var customers = $('#Customers').val();
        var vessels = $('#Vessels').val();
        var fromDate = $('#txtFromDate').val();
        var toDate = $('#txtToDate').val();
        var status = $('#Status').val();
        var driver = $('#DriverId').val();
        var SalesInvoiceSummaryNo = $('#SalesInvoiceSummaryNo').val();
        var isSearchByRange = 'false';
        if ($('#SearchByRange')[0].checked) {
            isSearchByRange = 'true';
        }
        var StartingSalesInvoiceSummaryNo = $('#SalesInvoiceSummaryStartingNo').val();
        var EndingSalesInvoiceSummaryNo = $('#SalesInvoiceSummaryEndingNo').val();
        $('#result').load("/BilledOrders/List?Customers=" + customers + "&Vessels=" + vessels + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Status=" + status + "&DriverId=" + driver + "&SalesInvoiceSummaryNo=" + SalesInvoiceSummaryNo + "&isSearchByRange=" + isSearchByRange +  "&StartingSalesInvoiceSummaryNo=" + StartingSalesInvoiceSummaryNo + "&EndingSalesInvoiceSummaryNo=" + EndingSalesInvoiceSummaryNo);

    });

    $('#Customers').change(function (e) {

        $('#divLoading').modal('toggle');

        var customerId = $(this).val();

        if (customerId == "") customerId = 0;


        $.get('/VesselApi/GetByCustomer',
            {
                id: customerId
            }, function success(data, status) {
                $('#Vessels').empty();
                $('#Vessels').append($('<option>All</option>'));

                $.each(data, function (index, item) {
                    $('#Vessels').append($('<option>').html(item.Vessel).val(item.VesselId));
                });

                $('#divLoading').modal('hide');

            });
    });


    $('#divFromDate').datetimepicker({
        format: 'DD/MM/YYYY'
    });

    $('#divToDate').datetimepicker({
        format: 'DD/MM/YYYY'
    });

    $('#divInvoiceDate').datetimepicker({
        format: 'DD/MM/YYYY'
    });

    $('#SearchByRange').change(function () {
        if (this.checked) {
            $('#IsSearchByRange').val('true');
            $('#divSearchIndividualInvoice').hide();
            $('#divSearchInvoiceByRange').show();
        }
        else {
            $('#IsSearchByRange').val('false');
            $('#divSearchIndividualInvoice').show();
            $('#divSearchInvoiceByRange').hide();
        }
    });


    //Invoice Number Validations
    var $regexname = /^([0-9][0-9][0-9][0-9])\/([0-9][0-9])\/([0-9]*)$/;

    $('#SalesInvoiceSummaryStartingNo').on('keyup', function () {
        if ($('#SalesInvoiceSummaryStartingNo').val().match($regexname)) {
            $('#lblSalesInvoiceSummaryStartingNo').css({ "display": "none" });
            $('#lblSalesInvoiceSummaryStartingNo').hide();
        }
        else {
            $('#lblSalesInvoiceSummaryStartingNo').show();
        }
    });

    $('#SalesInvoiceSummaryEndingNo').on('keyup', function () {
        if ($('#SalesInvoiceSummaryEndingNo').val().match($regexname)) {
            $('#lblSalesInvoiceSummaryEndingNo').css({ "display": "none" });
            $('#lblSalesInvoiceSummaryEndingNo').hide();
        }
        else {
            $('#lblSalesInvoiceSummaryEndingNo').show();
        }
    });
});

