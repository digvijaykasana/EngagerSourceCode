$(document).ready(function (e) {

    var customers = $('#Customers').val();
    var vessels = $('#Vessels').val();

    $('#result').load('/CreditNote/List?Customers=' + customers + "&Vessels=" + vessels );

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
                alert('Please check a credit note!');
            }
        } else if (actionThing == 1) {
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
                alert('Please check a credit note!');
            }
        }
    });

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var customers = $('#Customers').val();
        var vessels = $('#Vessels').val();
        //window.location.href = "/CreditNote?Customers=" + customers + "&Vessels=" + vessels;
        $('#result').load('/CreditNote/List?Customers=' + customers + "&Vessels=" + vessels);
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
});

