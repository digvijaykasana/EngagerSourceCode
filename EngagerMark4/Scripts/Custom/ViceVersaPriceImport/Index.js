
$(document).ready(function (e) {

    $('#btnImportReturnTrips').click(function (e) {
        e.preventDefault();

        $('#divLoading').modal('toggle');

        var customerId = $('#CustomerId').val();

        

        if (customerId == null || customerId == "") {
            alert("A Company needs to be selected!");
        }
        else {

            $.post('/ViceVersaPriceImport/ImportReturnTripChargesForCustomer',
                {
                    customerId: customerId
                }, function success(data, status) {

                    $('#divLoading').modal('hide');
                    
                    alert("{ " + data + " } price items inserted.");

                });

        }
    });

});

