$(document).ready(function (e) {
    var customerId = $('#CustomerId').val();
    var glcodeId = $('#GLCodeId').val();
    var itemCode = $('#txtCode').val();
    var pickupPoint = $('#txtPickupPoint').val();
    var dropPoint = $('#txtDropPoint').val();
    var orderPage = $('#orderPage').val();
    var orderColumn = $('#orderColumn').val();
    var orderOrderBy = $('#orderOrderBy').val();
    var orderDataType = $('#orderDataType').val();
    var currentId = $('#CurrentId').val();

    $('#result').load("/Price/List?CurrentPage=" + orderPage + "&Column=" + orderColumn + "&OrderBy=" + orderOrderBy + "&DataType=" + orderDataType + "&CustomerId=" + customerId + "&GLCodeId=" + glcodeId + "&ItemCode=" + encodeURIComponent(itemCode) + "&PickupPoint=" + encodeURIComponent(pickupPoint) + "&DropPoint=" + encodeURIComponent(dropPoint) + "&CurrentId=" + currentId);

    $('#btnSearch').click(function (e) {
        e.preventDefault();
        var customerId = $('#CustomerId').val();
        var glcodeId = $('#GLCodeId').val();
        var itemCode = $('#txtCode').val();
        var pickupPoint = $('#txtPickupPoint').val();
        var dropPoint = $('#txtDropPoint').val();
        $('#result').load("/Price/List?CustomerId=" + customerId + "&GLCodeId=" + glcodeId + "&ItemCode=" + encodeURIComponent(itemCode) + "&PickupPoint=" + encodeURIComponent(pickupPoint) + "&DropPoint=" + encodeURIComponent(dropPoint));
    });
});

