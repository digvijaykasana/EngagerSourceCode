$(document).ready(function (e) {

    var agentCustomerId = $('#txtAgentCustomerId').val();
    $('#Customers').val(agentCustomerId); 

    LoadCustomerReferences(agentCustomerId);

    var customer = agentCustomerId;
    var vessels = $('#Vessels').val();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var status = $('#Status').val();
    $('#result').load('/CustomerOrder/List?Customers=' + customer + "&Vessels=" + vessels + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Status=" + status );

    });

    

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        //var customers = $('#Customers').val();
        var customer = $('#txtcurrentCustomer').val();
        var vessels = $('#Vessels').val();
        var fromDate = $('#txtFromDate').val();
        var toDate = $('#txtToDate').val();
        var status = $('#Status').val();
        $('#result').load("/CustomerOrder/List?Customers=" + customer + "&Vessels=" + vessels + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Status=" + status);
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

function LoadCustomerReferences(agentCustomerId)
{
    if (agentCustomerId) {
        $.get('/VesselApi/GetByCustomer',
            {
                id: agentCustomerId
            }, function success(data, status) {
                $('#Vessels').empty();
                $('#Vessels').append($('<option>').html("All"));
                $.each(data, function (index, item) {
                    $('#Vessels').append($('<option>').html(item.Vessel).val(item.VesselId));
                });
                
            });
    }
}

