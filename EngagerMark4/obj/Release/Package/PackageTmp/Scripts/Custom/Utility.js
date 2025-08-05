function decimalPlaces(value) {
    if (Math.floor(value) !== value)
        return value.toString().split(".")[1].length || 0;
    return 0;
}

function convertDecimal(value) {
    var decimalPlace = decimalPlaces(value);
    if (parseInt(decimalPlace) <= 2)
        return parseFloat(value).toFixed(2);
    else {
        return parseFloat(value).toFixed(3);
    }
}

$(".formatDecimal").blur(function (e) {
    var value = $(this).val();
    if (value) {
        $(this).val(parseFloat(value).toFixed(2));
        return;
    }
    $(this).val(parseFloat("0").toFixed(2));
});

function htmlEncode(value) {
    //create a in-memory div, set it's inner text(which jQuery automatically encodes)
    //then grab the encoded contents back out.  The div never exists on the page.
    return $('<div/>').text(value).html();
}

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}

$(function () {
    $(".datefield").datepicker({
        dateFormat: 'dd/mm/yy',
        //appendText: '(dd/mm/yy)'
    });
})

$(function () {
    $('input').on('keypress', function (event) {
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            if (key != ' ' && key != '.' && key != '@' && key != '$' && key != '%' && key!=':' &&key!='-' && key!='_' && key!='\\' && key!='/' ) {
                event.preventDefault();
                return false;
            }
        }
    });
})

$('.decimalNumber').blur(function (e) {
    $(this).val(convertDecimal($(this).val()));
});

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function isDecimalNumber(txt) {
    if (event.keyCode > 47 && event.keyCode < 58 || event.keyCode == 46) {
        //alert(txt.value);
        //var txtbx = document.getElementById(txt);
        //var amount = document.getElementById(txt).value;
        var amount = txt.value;
        var present = 0;
        var count = 0;

        if (amount.indexOf(".", present) || amount.indexOf(".", present + 1));
        {
            // alert('0');
        }

        /*if(amount.length==2)
        {
          if(event.keyCode != 46)
          return false;
        }*/

        if (present == -1 && amount.length == 0 && event.keyCode == 46) {
            event.keyCode = 0;
            //alert("Wrong position of decimal point not  allowed !!");
            return false;
        }
        
        if (count >= 1 && event.keyCode == 46) {

            event.keyCode = 0;
            //alert("Only one decimal point is allowed !!");
            return false;
        }
        if (count == 1) {
            var lastdigits = amount.substring(amount.indexOf(".") + 1, amount.length);
            if (lastdigits.length >= 2) {
                //alert("Two decimal places only allowed");
                event.keyCode = 0;
                return false;
            }
        }
        return true;
    }
    else {
        event.keyCode = 0;
        //alert("Only Numbers with dot allowed !!");
        return false;
    }
}

function validateEmail(sEmail) {
    var filter = /^[\w\-\.\+]+\@[a-zA-Z0-9\.\-]+\.[a-zA-z0-9]{2,4}$/;
    if (filter.test(sEmail)) {
        return true;
    }
    else {
        return false;
    }
}

function iframeLoaded() {
    var iFrameID = document.getElementById('fileUploadIFrame');
    if (iFrameID) {
        iFrameID.height = "";
        iFrameID.height = iFrameID.contentWindow.document.body.scrollHeight + "px";
    }
}