$(document).ready(function (e) {


    var SalesInvoiceSummaryNo = $('#SalesInvoiceSummaryNo').val();

    if (SalesInvoiceSummaryNo == null || SalesInvoiceSummaryNo == "") {
        $('#divDownloadReport').hide();
    }
    else {
        $('#divDownloadReport').show();
    }

    var customers = $('#Customers').val();
    var vessels = $('#Vessels').val();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var status = $('#Status').val();
    var driver = $('#DriverId').val();
    var orderPage = $('#orderPage').val();
    var orderColumn = $('#orderColumn').val();
    var orderOrderBy = $('#orderOrderBy').val();
    var orderDataType = $('#orderDataType').val();
    var currentId = $('#CurrentId').val();
    $('#result').load("/GenerateInvoice/List?CurrentPage=" + orderPage + "&Column=" + orderColumn + "&OrderBy=" + orderOrderBy + "&DataType=" + orderDataType + "&Customers=" + customers + "&Vessels=" + vessels + "&Status=" + status + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&DriverId=" + driver + "&SalesInvoiceSummaryNo=" + SalesInvoiceSummaryNo + "&CurrentId=" + currentId);   

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var customers = $('#Customers').val();
        var vessels = $('#Vessels').val();
        var fromDate = $('#txtFromDate').val();
        var toDate = $('#txtToDate').val();
        var status = $('#Status').val();
        var driver = $('#DriverId').val();
        //var SalesInvoiceSummaryNo = $('#SalesInvoiceSummaryNo').val();
        var SalesInvoiceSummaryNo = "";
        $('#result').load("/GenerateInvoice/List?Customers=" + customers + "&Vessels=" + vessels + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Status=" + status + "&DriverId=" + driver + "&SalesInvoiceSummaryNo=" + SalesInvoiceSummaryNo);

        $('html, body').animate({
            scrollTop: $("#collapse2").offset().top
        }, 1000);
    });


    $('#btnGenerate').click(function (e) {
        e.preventDefault();

        $('#divLoading').modal({ backdrop: 'static', keyboard: false });

        var isChecked = false;

        var customers = $('#Customers').val();
        var vessels = $('#Vessels').val();

        $('#CustomerId').val(customers);
        $('#VesselId').val(vessels);
        
        $('#frmGenerate').attr('action', '/GenerateInvoice/GenerateInvoice');
        $('#frmGenerate').submit();
    });

    $('#btnDownloadInvoice').click(function (e) {
        e.preventDefault();

        var isChecked = false;

        var customers = $('#Customers').val();
        var vessels = $('#Vessels').val();

        $('#CustomerId').val(customers);
        $('#VesselId').val(vessels);

        $('#frmGenerate').attr('action', '/GenerateInvoice/DownloadInvoice');
        $('#frmGenerate').submit();
    });

    $('#btnGenerateCreditNote').click(function (e) {
        e.preventDefault();
        var taxInclude = $('#IncludeTax').prop('checked');
        $('#TaxInclude').val(taxInclude);
        var excelFormat = $('#IsExcel').prop('checked');
        $('#ExcelFormat').val(excelFormat);        
        var isChecked = false;
        $('.workOrders').each(function (index, item) {
            if (isChecked != true) {
                isChecked = $(this).prop('checked');
            }
        });
        if (isChecked == true) {
            $('#frmGenerate').attr('action', '/CreditNotePDF/Download');
            $('#frmGenerate').submit();
        } else {
            alert('Please check a work order!');
        }
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

