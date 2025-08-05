

/*
Developer : Myint Kyaw
Summary   : FXS Custom Script
*/
/// <reference path="../jquery-2.1.3.min.js" />
/// <reference path="../jquery-2.1.3.js" />

addNestedForm = function (container, counter, ticks, content) {
    var nextIndex = $(counter).length;
    var pattern = new RegExp(ticks, "gi");
    content = content.replace(pattern, nextIndex);
    $(container).append(content);
};

removeNestedForm = function (element, container, deleteElement) {
    $container = $(element).parents(container);
    $container.find(deleteElement).val('True');
    $container.hide();
};



function CommonSearchFormLoad(entityType) {

    //var searchurl = '@Url.Action("SearchFormLoad", "SearchCriterias")';
    var searchurl = '/SearchCriterias/SearchFormLoad';
    searchurl = searchurl + "?EntityType=" + entityType;
    $('#commonSearch').load(searchurl);
};

$(document).on('keyup', '.textValidate', function () 
{
        var allowedChar = $(this).attr('allowedCharacters');

        if (allowedChar == undefined) return;

        if (allowedChar.toUpperCase() == "NUMERIC") {
            if ($(this).val() == "") {
                return;
            }
            this.value = this.value.replace(/[^0-9\.-]/g, '');
            return;
        }

        if (allowedChar.toUpperCase() == "DOMAIN") {
            if ($(this).val() == "") {
                return;
            }
            this.value = this.value.replace(/[^a-zA-Z0-9\.\-\_]/g, '');
            return;
        }

        if (allowedChar.toUpperCase() == "ALPHANUMERIC") {
            if ($(this).val() == "") {
                return;
            }
            this.value = this.value.replace(/[^a-z0-9 ]/gi, '');
            return;
        }

        if (allowedChar.toUpperCase() == "INTEGER") {
            if ($(this).val() == "") {
                return;
            }
            this.value = this.value.replace(/[^0-9]/g, '');
            return;
        }
        if (allowedChar.toUpperCase() == "INTEGERPOSITIVE") {
            if ($(this).val() == "") {
                $(this).val(0);
                return;
            }
            this.value = this.value.replace(/[^0-9]/g, '');
            if (this.value == "") {
                this.value = 0;
            }
            this.value = parseInt(this.value);//to remove the front 0 value.  e.g - 011 to 11.
            return;
        }
        if (allowedChar.toUpperCase() == "ALPHA") {
            if ($(this).val() == "") {
                return;
            }
            this.value = this.value.replace(/[^a-zA-Z\.]/g, '');
            return;
        }
        if (allowedChar.toUpperCase() == "PHONE") {
            if ($(this).val() == "") {
                return;
            }
            this.value = this.value.replace(/[^0-9\+\-\(\)]/g, '');
            return;
        }

        if (allowedChar.toUpperCase() == "DECIMAL") {
            if ($(this).val() == "") {
                //$(this).val(0);
                return;
            }
            var deciVal = this.value = this.value.replace(/[^0-9\.]/g, '');
            var deciIdx = 0;
            if (deciVal.split(".").length > 2) {
                deciIdx = deciVal.lastIndexOf(".");
                var a = deciVal.split("");
                a[deciIdx] = '';
                deciVal = a.join('');
            }
            this.value = deciVal;
            return;
        }


        if (allowedChar.toUpperCase() == "DECIMALNEGATIVE") {

            if ($(this).val() == "") return;
            var deciVal = this.value = this.value.replace(/[^0-9\.-]/g, '');
            var deciIdx = 0;
            if (deciVal.split(".").length > 2) {
                deciIdx = deciVal.lastIndexOf(".");
                var a = deciVal.split("");
                a[deciIdx] = '';
                deciVal = a.join('');
            }


            deciIdx = 0;
            if (deciVal.split("-").length > 2) {
                deciIdx = deciVal.lastIndexOf("-");
                var a = deciVal.split("");
                a[deciIdx] = '';
                deciVal = a.join('');
            }

            this.value = deciVal;
            return;
        }


        if (allowedChar.toUpperCase() == "DECIMALMINUS") {
            //[-+]?([0-9]*\.[0-9]+|[0-9]+)
            // /^-?\d{2}(\.\d+)?$/
            if ($(this).val() == "") return;
            var deciVal = this.value = this.value.replace(/[^0-9\.-]/g, '');
            var deciIdx = 0;
            if (deciVal.split(".").length > 2) {
                deciIdx = deciVal.lastIndexOf(".");
                var a = deciVal.split("");
                a[deciIdx] = '';
                deciVal = a.join('');
            }
            this.value = deciVal;
            return;
        }

        if (typeof (allowedChar) == 'string') {
            this.value = this.value.replace(allowedChar, '');
            return;
        }
        return;
});





