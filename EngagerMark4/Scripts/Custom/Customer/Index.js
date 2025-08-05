//$(document).ready(function (e) {
//    var customer = $('#txtCustomer').val();
//    $('#result').load('/Customer/List?name=' + encodeURIComponent(customer));

//    $('#btnSearch').click(function (e) {
//        e.preventDefault();
//        var customer = $('#txtCustomer').val();
//        window.location.href = "/Customer?name=" + customer;
//    });

//    $('#btnSearch').click(function (e) {
//        e.preventDefault();
//        var searchValue = $('#txtCustomer').val();
//        window.location.href = "/Customer?name=" + encodeURIComponent(searchValue);
//    });
//});

$(document).ready(function (e) {
    $('#result').load('/Customer/List');

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var searchValue = $('#txtCustomer').val();
        $('#result').load('/Customer/List?Name=' + encodeURIComponent(searchValue));
    });
});

