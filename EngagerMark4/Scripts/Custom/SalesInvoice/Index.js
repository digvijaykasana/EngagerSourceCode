$(document).ready(function (e) {
    var customers = $('#Customers').val();
    var vessels = $('#Vessels').val();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var status = $('#Status').val();
    var SalesInvoiceSummaryNo = "";
    
    $('#result').load('/SalesInvoice/List?Customers=' + customers + "&Vessels=" + vessels + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Status=" + status + "&SalesInvoiceSummaryNo=" + SalesInvoiceSummaryNo);

    $('#btnDownload').click(function (e) {
        e.preventDefault();
        var taxInclude = true;
        var actionThing = $('#ActionThing').val();
        if (actionThing == 0) {
            taxInclude = true;
            $('#TaxInclude').val(taxInclude);
            var isChecked = false;
            $('.invoices').each(function (index, item) {
                if (isChecked != true) {
                    isChecked = $(this).prop('checked');
                }
            });
            if (isChecked == true) {
                this.form.submit();
            } else {
                alert('Please check an invoice!');
            }
        }
        else if (actionThing == 1) {
            taxInclude = false;
            $('#TaxInclude').val(taxInclude);
            var isChecked = false;
            $('.invoices').each(function (index, item) {
                if (isChecked != true) {
                    isChecked = $(this).prop('checked');
                }
            });
            if (isChecked == true) {
                this.form.submit();
            } else {
                alert('Please check an invoice!');
            }
        }
        
    });

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var customers = $('#Customers').val();
        var vessels = $('#Vessels').val();
        var fromDate = $('#txtFromDate').val();
        var toDate = $('#txtToDate').val();
        var status = $('#Status').val();
        var SalesInvoiceSummaryNo = $('#txtInvoiceSummaryNumber').val();
        $('#result').load("/SalesInvoice/List?Customers=" + customers + "&Vessels=" + vessels + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Status=" + status + "&SalesInvoiceSummaryNo=" + SalesInvoiceSummaryNo);
        //window.location.href = "/SalesInvoice?Customers=" + customers + "&Vessels=" + vessels + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Status=" + status + "&SalesInvoiceSummaryNo=" + SalesInvoiceSummaryNo;
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

    //$('#Customers').change(function (e) {
    //    var customerId = $(this).val();
    //    if (customerId) {
    //        $.get('/VesselApi/GetByCustomer',
    //            {
    //                id: customerId
    //            }, function success(data, status) {
    //                $('#Vessels').empty();
    //                $('#Vessels').append($('<option>All</option>'));
    //                $.each(data, function (index, item) {
    //                    $('#Vessels').append($('<option>').html(item.Vessel).val(item.VesselId));
    //                });

    //            });
    //    }
    //});

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
});

