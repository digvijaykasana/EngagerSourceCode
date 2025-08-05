$(document).ready(function (e) {
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
    var SalesInvoiceSummaryNo = "";
    $('#result').load("/WorkOrder/List?CurrentPage=" + orderPage + "&Column=" + orderColumn + "&OrderBy=" + orderOrderBy + "&DataType=" + orderDataType + "&Customers=" + customers + "&Vessels=" + vessels + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Status=" + status + "&DriverId=" + driver + "&SalesInvoiceSummaryNo=" + SalesInvoiceSummaryNo + "&CurrentId=" + currentId);
    
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
                $('#frmPDF').attr('action', '/InvoicePDF/Download');
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
                $('#frmPDF').attr('action', '/InvoicePDF/Download');
                this.form.submit();
            } else {
                alert('Please check an invoice!');
            }
        } else if (actionThing == 2) {
            taxInclude = true;
            $('#TaxInclude').val(taxInclude);
            var isChecked = false;
            $('.invoices').each(function (index, item) {
                if (isChecked != true) {
                    isChecked = $(this).prop('checked');
                }
            });
            if (isChecked == true) {
                $('#frmPDF').attr('action', '/CreditNotePDF/Download');
                this.form.submit();
            } else {
                alert('Please check a work order!');
            }
        } else if (actionThing == 3) {
            taxInclude = false;
            $('#TaxInclude').val(taxInclude);
            var isChecked = false;
            $('.invoices').each(function (index, item) {
                if (isChecked != true) {
                    isChecked = $(this).prop('checked');
                }
            });
            if (isChecked == true) {
                $('#frmPDF').attr('action', '/CreditNotePDF/Download');
                this.form.submit();
            } else {
                alert('Please check a work order!');
            }
        } else if (actionThing == 4) {
            $('.invoices').each(function (index, item) {
                if (isChecked != true) {
                    isChecked = $(this).prop('checked');
                }
            });
            if (isChecked == true) {
                $('#frmPDF').attr('action', '/InvoiceExcel/Download');
                this.form.submit();
            } else {
                alert('Please check an invoice!');
            }
        }
        else if (actionThing == 5) {
            $('.invoices').each(function (index, item) {
                if (isChecked != true) {
                    isChecked = $(this).prop('checked');
                }
            });
            if (isChecked == true) {
                $('#frmPDF').attr('action', '/CreditNoteExcel/Download');
                this.form.submit();
            } else {
                alert('Please check an invoice!');
            }
        }
        else if (actionThing == 100) {
            $('#divLoading').modal({ backdrop: 'static', keyboard: false });
            var invoiceIds = new Array();
            $('.invoices').each(function (index, item) {
                var isChecked = $(this).prop('checked');
                if (isChecked) {
                    var id = $(this).val();
                    invoiceIds.push(id);
                }
            });
            $.post('/WorkOrder/MoveToBill',
                {
                    invoiceIds: invoiceIds
                }, function success(data, status) {
                    if (data == 'success') {
                        for (var i = 0; i <= invoiceIds.length; i++) {
                            var id = invoiceIds[i];
                            $('#status_' + id).html('Billed');
                        }
                        $('#divLoading').modal('toggle');

                    

                    }else{
                        $('#divLoading').modal('toggle');
                        alert("Moving to Billed failed. Please contact the Admin for further assistance.");
                    }
;
                });
        }
    });

    $('.acknowledge').click(function (e) {
        e.preventDefault();
        var notificationId = $(this).attr('notificationId');
        $.post('/WorkOrder/Acknowledge',
            {
                notificationId: notificationId
            }, function success(data, status) {
                if (data == 'success') {
                    $('#notiButton_' + notificationId).attr('style', 'display:none;');
                    $('#notiLink_' + notificationId).attr('style', 'color:cornflowerblue;');
                }
            })
    });

    $('#btnSearch').click(function (e) {
        e.preventDefault();

        var customers = $('#Customers').val();
        var vessels = $('#Vessels').val();
        var fromDate = $('#txtFromDate').val();
        var toDate = $('#txtToDate').val();
        var status = $('#Status').val();
        var SalesInvoiceSummaryNo = "";
        var drivers = $('#DriverId').val();
        $('#result').load("/WorkOrder/List?Customers=" + customers + "&Vessels=" + vessels + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Status=" + status + "&SalesInvoiceSummaryNo=" + SalesInvoiceSummaryNo + "&DriverId=" + drivers);
        
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

                //var customerId = $('#Customers').val();

                //if (customerId !=  "") {
                    $.each(data, function (index, item) {
                        $('#Vessels').append($('<option>').html(item.Vessel).val(item.VesselId));
                    });
                //}
                //else {
                //    $.each(data, function (index, item) {
                //        $('#Vessels').append($('<option>').html(item.Name).val(item.Id));
                //    });
                //}                

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
});

