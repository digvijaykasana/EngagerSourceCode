$('#btnSave').click(function (e) {
    e.preventDefault();
    $('#divLoading').modal({ backdrop: 'static', keyboard: false });
    this.form.submit();
});

$('#divInvoiceDate').datetimepicker({
    format: 'DD/MM/YYYY'
});

$('body').on('click', 'a.deleteRecord', function () {
    calculateOverallTotal();
});

$('body').on('click', 'a.btnEditDetails', function () {
    var tableIndex = $(this).parents('tr').attr('tableIndex');
    var priceId = $('#' + tableIndex + 'PriceId').val();
    var priceCode = $('#' + tableIndex + 'Code').val();
    var gstEssentials = $('#' + tableIndex + 'GSTEssentials').prop('checked');
    var description = $('#' + tableIndex + 'Description').val();
    var qty = $('#' + tableIndex + 'Qty').val();
    var price = $('#' + tableIndex + 'Price').val();
    var totalAmt = $('#' + tableIndex + 'TotalAmt').val();
    var discountPercent = $('#' + tableIndex + 'DiscountPercent').val();
    var discountAmt = $('#' + tableIndex + 'DiscountAmount').val();
    var totalNetAmt = $('#' + tableIndex + 'TotalNetAmount').val();
    var sortOrder = $('#' + tableIndex + 'SortOrder').val();

    $('#tableIndex').val(tableIndex);
    $('#Prices').val(priceId);
    $('#txtSalesDetailsCode').val(priceCode);
    $('#chkGSTEssential').prop('checked', gstEssentials);
    $('#txtDescription').val(description);
    $('#txtQty').val(qty);
    $('#txtPrice').val(price);
    $('#txtTotalAmt').val(totalAmt);
    $('#txtDiscountPercent').val(discountPercent);
    $('#txtDiscountAmt').val(discountAmt);
    $('#txtNetAmt').val(totalNetAmt);
    $('#txtSortOrder').val(sortOrder);

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
    $('#Prices').val("0");
    $('#txtDescription').val(null);
    $('#txtQty').val(1);
    $('#txtPrice').val(0);
    $('#txtTotalAmt').val(0);
    $('#txtDiscountPercent').val(0);
    $('#txtDiscountAmt').val(0);
    $('#txtNetAmt').val(0);
    $('#txtSortOrder').val(0);
    $('#chkGSTEssential').prop('checked', false);
}

$('#btnCancel').click(function (e) {
    e.preventDefault();
    var tableIndex = $('#tableIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#DetailsContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#Details_' + lastIndex + "__PriceId").parents('tr').remove();
    }
})

$('#btnClose').click(function (e) {
    e.preventDefault();
    var tableIndex = $('#tableIndex').val();
    if (tableIndex) {

    } else {
        var rows = $('tbody#DetailsContainer').find("tr");
        var lastIndex = rows.last().index();
        $('#Details_' + lastIndex + "__PriceId").parents('tr').remove();
    }
})

$('#Prices').change(function (e) {
    calculateLineTotalAfterSelected();   
})

$('#txtQty').blur(function () {
    calculateLineTotal();
});

$('#txtPrice').blur(function () {
    calculateLineTotal();
})

$('#GSTId').change(function (e) {
    $('#GSTPercent').val($('#GSTId option:selected').attr('gstPercent'));
    calculateOverallTotal();
});

$('#txtDiscountPercent').blur(function (e) {
    var disPercent = parseFloat($(this).val());
    var totalAmt = parseFloat($("#txtTotalAmt").val());
    var disAmt = totalAmt * (disPercent / 100);
    var netAmt = totalAmt - disAmt;
    $('#txtDiscountAmt').val(disAmt);
    $('#txtNetAmt').val(netAmt);
});

$('#txtDiscountAmt').blur(function (e) {
    var disAmt = parseFloat($(this).val());
    var totalAmt = parseFloat($("#txtTotalAmt").val());
    var disPer = (disAmt / totalAmt) * 100;
    var netAmt = totalAmt - disAmt;
    $('#txtDiscountPercent').val(disPer);
    $('#txtNetAmt').val(netAmt);
});

function calculateLineTotal() {
    var assignedPrice = parseFloat($('#txtPrice').val());
    var qty = parseFloat($('#txtQty').val());
    var totalAmt = assignedPrice * qty;
    var netAmt = totalAmt;
    var disPercent = parseFloat($('#txtDiscountPercent').val());
    var disAmt = netAmt * (disPercent / 100);
    $('#txtDiscountAmt').val(disAmt);
    netAmt = netAmt - disAmt;
    $('#txtPrice').val(assignedPrice);
    $('#txtTotalAmt').val(totalAmt);
    $('#txtNetAmt').val(netAmt);
}


function calculateLineTotalAfterSelected() {
    
    var priceName = $('#Prices option:selected').text();

    if (priceName) $('#txtSalesDetailsCode').val(priceName);

    var priceId = $('#Prices').val();

    if (priceId != "0") {
        var assignedPrice = parseFloat($('#Prices option:selected').attr('assignedPrice'));
        var qty = parseFloat($('#txtQty').val());
        var totalAmt = assignedPrice * qty;
        var netAmt = totalAmt;
        var disType = $('#DisType').val();
        if (disType == 0) {
            var disPercent = parseFloat($('#Prices option:selected').attr('discountPercent'));
            var disAmt = netAmt * (disPercent / 100);
            $('#txtDiscountPercent').val(disPercent);
            $('#txtDiscountAmt').val(disAmt);
            netAmt = netAmt - disAmt;
        } else {
            var disAmt = parseFloat($('#Prices option:selected').attr('discountAmt'));
            $('#txtDiscountAmt').val(disAmt);
            var disPercent = (disAmt / netAmt) * 100;
            $('#txtDiscountPercent').val(disPercent);
            netAmt = netAmt - disAmt;
        }
        $('#txtPrice').val(assignedPrice);
        $('#txtTotalAmt').val(totalAmt);
        $('#txtNetAmt').val(netAmt);
    }
}

$('#btnAdd').click(function (e) {
    e.preventDefault();
    if (saveDetails()) {
        calculateOverallTotal();
        $('#detailsModal').modal('toggle');
    }
});

$('#btnEdit').click(function (e) {
    e.preventDefault();

    var tableIndex = $('#tableIndex').val();
    var priceId = $('#Prices').val();
    //var name = $('#Prices option:selected').text();
    var name = $('#txtSalesDetailsCode').val();
    var taxable = $('#Prices option:selected').attr('taxable');
    var description = $('#txtDescription').val();
    var qty = $('#txtQty').val();
    var price = $('#txtPrice').val();
    var totalAmt = $('#txtTotalAmt').val();
    var discountPercent = $('#txtDiscountPercent').val();
    var discountAmt = $('#txtDiscountAmt').val();
    var netAmt = $('#txtNetAmt').val();
    var gstEssentials = $('#chkGSTEssential').prop('checked');
    var sortOrder = $('#txtSortOrder').val();

    if (priceId == "0") {
        alert("Please select a Price!");
        $('#Prices').focus();
        return false;
    }

    $('#' + tableIndex + 'PriceId').val(priceId);
    $('#' + tableIndex + 'Type').val(0);
    $('#' + tableIndex + 'GSTEssentials').prop('checked', gstEssentials);
    if (gstEssentials) {
        $('#' + tableIndex + 'GSTEssentials').val('True');
    } else {
        $('#' + tableIndex + 'GSTEssentials').val('False');
    }
    $('#' + tableIndex + 'Code').val(name);
    $('#' + tableIndex + 'Description').val(description);
    $('#' + tableIndex + 'Qty').val(qty);
    $('#' + tableIndex + 'Price').val(price);
    $('#' + tableIndex + 'TotalAmt').val(totalAmt);
    $('#' + tableIndex + 'DiscountPercent').val(discountPercent);
    $('#' + tableIndex + 'DiscountAmount').val(discountAmt);
    $('#' + tableIndex + 'TotalNetAmount').val(netAmt);
    $('#' + tableIndex + 'Taxable').val(taxable);
    $('#' + tableIndex + 'SortOrder').val(sortOrder);

    calculateOverallTotal();
    $('#detailsModal').modal('toggle');
});

$('.chkGST').change(function () {
    var check = $(this).prop('checked');
    if (check) {
        $(this).val('True');
    } else {
        $(this).val('False');
    }
});

function saveDetails() {
    var priceId = $('#Prices').val();
    //var name = $('#Prices option:selected').text();
    var name = $('#txtSalesDetailsCode').val();
    var taxable = $('#Prices option:selected').attr('taxable');
    var description = $('#txtDescription').val();
    var qty = $('#txtQty').val();
    var price = $('#txtPrice').val();
    var totalAmt = $('#txtTotalAmt').val();
    var discountPercent = $('#txtDiscountPercent').val();
    var discountAmt = $('#txtDiscountAmt').val();
    var netAmt = $('#txtNetAmt').val();
    var gstEssentials = $('#chkGSTEssential').prop('checked');
    var sortOrder = $('#txtSortOrder').val();

    if (priceId == "0") {
        alert("Please select a Price!");
        $('#Prices').focus();
        return false;
    }

    var rows = $('tbody#DetailsContainer').find('tr');
    var lastIndex = rows.last().index();
    $('#Details_' + lastIndex + '__PriceId').val(priceId);
    $('#Details_' + lastIndex + '__Type').val(0);
    $('#Details_' + lastIndex + '__GSTEssentials').prop('checked', gstEssentials);
    if (gstEssentials) {
        $('#Details_' + lastIndex + '__GSTEssentials').val('True');
    } else {
        $('#Details_' + lastIndex + '__GSTEssentials').val('False');
    }
    $('#Details_' + lastIndex + '__Code').val(name);
    $('#Details_' + lastIndex + '__Description').val(description);
    $('#Details_' + lastIndex + '__Qty').val(qty);
    $('#Details_' + lastIndex + '__Price').val(price);
    $('#Details_' + lastIndex + '__TotalAmt').val(totalAmt);
    $('#Details_' + lastIndex + '__DiscountPercent').val(discountPercent);
    $('#Details_' + lastIndex + '__DiscountAmount').val(discountAmt);
    $('#Details_' + lastIndex + '__TotalNetAmount').val(netAmt);
    $('#Details_' + lastIndex + '__Taxable').val(taxable);
    $('#Details_' + lastIndex + '__SortOrder').val(sortOrder);
    return true;
}

function calculateOverallTotal() {
    var totalAmt = 0;
    var discountAmt = 0;
    var netAmount = 0;
    var nonTaxable = 0;
    debugger;
    $('.totalAmt').each(function () {
        var tableIndex = $(this).parents('tr').attr('tableIndex');
        var isDelete = $('#' + tableIndex + 'Delete').val();
        if (isDelete == 'False') {
            var taxable = $('#' + tableIndex + 'Taxable').val();
            if (taxable == 'True')
                totalAmt += parseFloat($(this).val());
            else
            {
                nonTaxable += parseFloat($(this).val());
            }
        }
    });
    $('.discountAmt').each(function () {
        var tableIndex = $(this).parents('tr').attr('tableIndex');
        var isDelete = $('#' + tableIndex + 'Delete').val();
        if (isDelete == 'False')
            discountAmt += parseFloat($(this).val());
    });
    $('.netAmount').each(function () {
        var tableIndex = $(this).parents('tr').attr('tableIndex');
        var isDelete = $('#' + tableIndex + 'Delete').val();
        if (isDelete == 'False')
            netAmount += parseFloat($(this).val());
    });

    var gstPercent = $('#GSTId option:selected').attr('gstPercent');

    var gstAmt = (totalAmt * gstPercent) / 100;

    var totalNetAmt = totalAmt + gstAmt + nonTaxable;

    $('#TotalAmt').val(totalAmt.toFixed(2));
    $('#GSTAmount').val(gstAmt.toFixed(2));
    $('#DiscountAmount').val(discountAmt.toFixed(2));
    $('#TotalNonTaxable').val(nonTaxable.toFixed(2));
    $('#TotalNetAmount').val(totalNetAmt.toFixed(2));
}