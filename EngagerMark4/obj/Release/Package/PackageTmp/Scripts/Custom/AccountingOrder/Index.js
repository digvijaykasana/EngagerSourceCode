$(document).ready(function (e) {
    
    var customers = $('#Customers').val();
    var vessels = $('#Vessels').val();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var status = $('#Status').val();
    var driver = $('#DriverId').val();
    var SalesInvoiceSummaryNo = $('#SalesInvoiceSummaryNo').val();
    var orderPage = $('#orderPage').val();
    var orderColumn = $('#orderColumn').val();
    var orderOrderBy = $('#orderOrderBy').val();
    var orderDataType = $('#orderDataType').val();
    var currentId = $('#CurrentId').val();
    $('#result').load("/AccountingOrder/List?CurrentPage=" + orderPage + "&Column=" + orderColumn + "&OrderBy=" + orderOrderBy + "&DataType=" + orderDataType + "&Customers=" + customers + "&Vessels=" + vessels + "&Status=" + status + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&DriverId=" + driver + "&SalesInvoiceSummaryNo=" + SalesInvoiceSummaryNo + "&CurrentId=" + currentId);


   

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var customers = $('#Customers').val();
        var vessels = $('#Vessels').val();
        var fromDate = $('#txtFromDate').val();
        var toDate = $('#txtToDate').val();
        var status = $('#Status').val();
        var driver = $('#DriverId').val();
        var SalesInvoiceSummaryNo = $('#SalesInvoiceSummaryNo').val();
        $('#result').load("/AccountingOrder/List?Customers=" + customers + "&Vessels=" + vessels + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Status=" + status + "&DriverId=" + driver + "&SalesInvoiceSummaryNo=" + SalesInvoiceSummaryNo);
    });


    $('#btnGenerate').click(function (e) {
        e.preventDefault();

        var isChecked = false;

        $('#frmGenerate').attr('action', '/AccountingOrder/GenerateInvoice');
        $('#frmGenerate').submit();
    });

    $('#btnGenerateCreditNote').click(function (e) {
        e.preventDefault();
        var taxInclude = $('#IncludeTax').prop('checked');
        $('#TaxInclude').val(taxInclude);
        var isChecked = false;
        $('.workOrders').each(function (index, item) {
            if (isChecked != true) {
                isChecked = $(this).prop('checked');
            }
        });
        if (isChecked == true) {
            $('#frmGenerate').attr('action', '/CreditNotePDF/Download');
            this.form.submit();
        } else {
            alert('Please check a work order!');
        }
    });

    $('#btnMoveToBilled').click(function (e) {
        e.preventDefault();
        $('#divLoading').modal({ backdrop: 'static', keyboard: false });
        var workOrderIds = new Array();
        $('.workOrders').each(function (index, item) {
            var isChecked = $(this).prop('checked');
            if (isChecked) {

                var tableIndex = $(this).parents('tr').attr('tableIndex');

                var invoice = $(this).prop('')

                var id = $(this).val();
                workOrderIds.push(id);
            }
        });
        $.post('/AccountingOrder/MoveToBill',
            {
                workOrderIds: workOrderIds
            }, function success(data, status) {
                if (data == 'success') {
                    for (var i = 0; i <= workOrderIds.length; i++) {
                        var id = workOrderIds[i];
                        $('#status_' + id).html('Billed');
                        $('#statusTD_' + id).css("background-color", "#000000");
                    }
                    $('#divLoading').modal('toggle');
                    location.reload();

                } else {

                    if (data == 'noInvoiceException') {
                        $('#divLoading').modal('toggle');
                        alert("Moving to Billed failed. Invoices need to be generated for one or more work orders.");
                    }
                    else {

                        $('#divLoading').modal('toggle');
                        alert("Moving to Billed failed. Please contact the Admin for further assistance.");
                    }                   
                }
                
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

