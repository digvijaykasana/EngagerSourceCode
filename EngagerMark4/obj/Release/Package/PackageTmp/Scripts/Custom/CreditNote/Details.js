$('#btnSave').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    this.form.submit();
});

$('body').on('click', 'a.deleteRecord', function () {
    calculateGrandTotal();
});

$('body').on('click', 'a.btnEditDetails', function () {

    var tableIndex = $(this).parents('tr').attr('tableIndex');   
    var description = $('#' + tableIndex + 'Description').val();
    var qty = $('#' + tableIndex + 'Qty').val();
    var price = $('#' + tableIndex + 'Price').val();
    var totalAmt = $('#' + tableIndex + 'TotalAmount').val();

    $('#tableIndex').val(tableIndex);
    $('#txtDescription').val(description);
    $('#txtQty').val(qty);
    $('#txtPrice').val(price);
    $('#txtTotalAmount').val(totalAmt);

    $('#btnAdd').attr('style', 'display:none;');
    $('#btnEdit').attr('style', 'display:normal;');
    $('#detailsModal').modal({ backdrop: 'static', keyboard: false });
});

$('.clsAddDetails').click(function (e) {
    e.preventDefault();
    clearModalData();
    $('#btnAdd').attr('style', 'display:normal;');
    $('#btnEdit').attr('style', 'display:none;');
    $('#detailsModal').modal({ backdrop: 'static', keyboard: false });
});

function clearModalData() {
    $('#txtDescription').val(null);
    $('#txtQty').val(1);
    $('#txtPrice').val(0);
    $('#txtTotalAmount').val(0);
}

$('#btnCancel').click(function (e) {
    e.preventDefault();
    var tableIndex = $('#tableIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#DetailsContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#Details_' + lastIndex + "__Id").parents('tr').remove();
    }
})

$('#btnClose').click(function (e) {
    e.preventDefault();
    var tableIndex = $('#tableIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#DetailsContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#Details_' + lastIndex + "__Id").parents('tr').remove();
    }
})

$('#txtQty').keyup(function () {
    calculateCNLineTotal();
});

$('#txtPrice').keyup(function () {
    calculateCNLineTotal();
})

function calculateCNLineTotal() {
    var assignedPrice = parseFloat($('#txtPrice').val());
    var qty = parseFloat($('#txtQty').val());
    var totalAmt = assignedPrice * qty;

    $('#txtTotalAmount').val(totalAmt);
}


$('#btnAdd').click(function (e) {
    e.preventDefault();
    if (saveCNDetails()) {
        calculateGrandTotal();
        $('#detailsModal').modal('toggle');
    }
});

$('#btnEdit').click(function (e) {
    e.preventDefault();

    var tableIndex = $('#tableIndex').val();
    var description = $('#txtDescription').val();
    var qty = $('#txtQty').val();
    var price = $('#txtPrice').val();
    var totalAmt = $('#txtTotalAmount').val();

    if (qty == "") {
        alert("Please enter a Quantity!");
        $('#txtQty').focus();
        return false;
    }

    if (price == "") {
        alert("Please enter a Price!");
        $('#txtPrice').focus();
        return false;
    }

    $('#' + tableIndex + 'Description').val(description);
    $('#' + tableIndex + 'Qty').val(qty);
    $('#' + tableIndex + 'Price').val(price);
    $('#' + tableIndex + 'TotalAmount').val(totalAmt);

    calculateGrandTotal();

    $('#detailsModal').modal('toggle');
});

$('#GSTId').change(function (e) {
    $('#GSTPercent').val($('#GSTId option:selected').attr('gstPercent'));
    calculateGrandTotal();
});

function saveCNDetails() {
    var description = $('#txtDescription').val();
    var qty = $('#txtQty').val();
    var price = $('#txtPrice').val();
    var totalAmt = $('#txtTotalAmount').val();

    if (qty == "") {
        alert("Please enter a Quantity!");
        $('#txtQty').focus();
        return false;
    }

    if (price == "") {
        alert("Please enter a Price!");
        $('#txtPrice').focus();
        return false;
    }

    var rows = $('tbody#DetailsContainer').find('tr');
    var lastIndex = rows.last().index();

    $('#Details_' + lastIndex + '__Description').val(description);
    $('#Details_' + lastIndex + '__Qty').val(qty);
    $('#Details_' + lastIndex + '__Price').val(price);
    $('#Details_' + lastIndex + '__TotalAmount').val(totalAmt);
    return true;
}



function calculateGrandTotal() {
    
    var subTotalAmt = 0;
    var grandTotalAmt = 0;

    $('.totalAmt').each(function() {
        var tableIndex = $(this).parents('tr').attr('tableIndex');
        var isDelete = $('#' + tableIndex + 'Delete').val();
        if (isDelete == 'False')
            subTotalAmt += parseFloat($(this).val());
    });

    var gstPercent = $('#GSTId option:selected').attr('gstPercent');
    
    var gstAmt = subTotalAmt * (gstPercent / 100);
    
    grandTotalAmt = subTotalAmt + gstAmt;

    $('#SubTotal').val(subTotalAmt.toFixed(2));
    $('#GSTAmount').val(gstAmt.toFixed(2));
    $('#GrandTotal').val(grandTotalAmt.toFixed(2));
}




