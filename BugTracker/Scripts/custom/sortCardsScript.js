$(document).ready(function () {
    var categorychoice = document.getElementById('categoryval');
    var propertychoice = document.getElementById('propertyval');
    var orderchoice = document.getElementById('orderval');
    var categoryCheckbox = document.getElementById('sortCategory');
    var propertyCheckbox = document.getElementById('sortProperties');
    var orderCheckbox = document.getElementById('orderProperties');
    var cCheck = categoryCheckbox.getElementsByTagName('input');
    var pCheck = propertyCheckbox.getElementsByTagName('input');
    var oCheck = orderCheckbox.getElementsByTagName('input');
    var sortSubmit = document.getElementById('sortsubmit');

    sortSubmit.onclick = function () {
        
        var cVal, pVal, oVal;
        for (var i = 0; i < cCheck.length; i++){
            if (cCheck[i].checked == true) {
                cVal = cCheck[i].value;
            }
        }
        categorychoice.value = cVal;
        for (var i = 0; i < pCheck.length; i++) {
            if (pCheck[i].checked == true) {
                pVal = pCheck[i].value;
            }
        }
        propertychoice.value = pVal;
        for (var i = 0; i < oCheck.length; i++) {
            if (oCheck[i].checked == true) {
                oVal = oCheck[i].value;
            }
        }
        orderchoice.value = oVal;
        document.getElementById('sortForm').submit(); // doesn't work need formid or name or something

    };

    function uncheckOthers(input, str) {
        if (str === 'category') {
            for (var i = 0; i < cCheck.length; i++) {
                if (cCheck[i].value === input) {
                    continue;
                }
                else {
                    cCheck[i].checked = false;
                }
            }
        }

        if (str === 'property') {
            for (var i = 0; i < pCheck.length; i++) {
                if (pCheck[i].value === input) {
                    continue;
                }
                else {
                    pCheck[i].checked = false;
                }
            }
        }

        if (str === 'order') {
            for (var i = 0; i < oCheck.length; i++) {
                if (oCheck[i].value === input) {
                    continue;
                }
                else {
                    oCheck[i].checked = false;
                }
            }
        }
    }

    function uncheckAll(str) {
        if (str === 'category') {
            for (var i = 0; i < pCheck.length; i++) {
                pCheck[i].checked = false;
            }
            for (var i = 0; i < oCheck.length; i++) {
                oCheck[i].checked = false;
            }
            sortSubmit.disabled = true;
        }

        if (str === 'property') {
            for (var i = 0; i < oCheck.length; i++) {
                oCheck[i].checked = false;
            }
            sortSubmit.disabled = true;
        }

        if (str === 'order') {
            sortSubmit.disabled = true;
        }
    }

    function checkExist(str) {
        if (str === 'category') {
            var count = 0;
            for (var i = 0; i < cCheck.length; i++) {
                if (cCheck[i].checked == false) {
                    continue;
                }
                else {
                    count++;
                }
            }

            if (count == 0) {
                return false;
            }
            else
                return true;
        }

        if (str === 'property') {
            var count = 0;
            for (var i = 0; i < pCheck.length; i++) {
                if (pCheck[i].checked == false) {
                    continue;
                }
                else {
                    count++;
                }
            }

            if (count == 0) {
                return false;
            }
            else
                return true;
        }

        if (str === 'order') {
            var count = 0;
            for (var i = 0; i < oCheck.length; i++) {
                if (oCheck[i].checked == false) {
                    continue;
                }
                else {
                    count++;
                }
            }

            if (count == 0) {
                return false;
            }
            else
                return true;
        }
    }

    function disableNext(str) {
        if (str === 'category') {
            for (var i = 0; i < pCheck.length; i++) {
                if (pCheck[i].type === 'checkbox') {
                    pCheck[i].disabled = true;
                }
            }
            for (var i = 0; i < oCheck.length; i++) {
                if (oCheck[i].type === 'checkbox') {
                    oCheck[i].disabled = true;
                }
            }
            uncheckAll(str);
        }


        if (str === 'property') {
            for (var i = 0; i < oCheck.length; i++) {
                if (oCheck[i].type === 'checkbox') {
                    oCheck[i].disabled = true;
                }
            }
            uncheckAll(str);
        }

        if (str === 'order') {
            uncheckAll(str);
        }
    }

    function enableNext(str) {
        if (str === 'category') {
            for (var i = 0; i < pCheck.length; i++) {
                if (pCheck[i].type === 'checkbox') {
                    pCheck[i].disabled = false;
                    pCheck[i].onclick = function () {
                        var str = 'property';
                        var input = this.value;
                        var exists = checkExist(str);
                        if (exists) {
                            uncheckOthers(input, str);
                            enableNext(str);
                        }
                        else {
                            disableNext(str);
                        }

                    }
                }

            }
        }

        if (str === 'property') {
            for (var i = 0; i < oCheck.length; i++) {
                if (oCheck[i].type === 'checkbox') {
                    oCheck[i].disabled = false;
                    oCheck[i].onclick = function () {
                        var str = 'order';
                        var input = this.value;
                        var exists = checkExist(str);
                        if (exists) {
                            uncheckOthers(input, str);
                            enableNext(str);
                        }
                        else {
                            disableNext(str);
                        }

                    }
                }

            }
        }

        if (str === 'order') {
            sortSubmit.disabled = false;
        }
    }

    // main Sort
    for (var i = 0; i < cCheck.length; i++) {
        if (cCheck[i].type === 'checkbox') {
            cCheck[i].onclick = function () {
                var input = this.value;
                var str = 'category';
                var exists = checkExist(str)
                if (exists) {
                    uncheckOthers(input, str);
                    enableNext(str);
                }
                else {
                    disableNext(str);
                }
            }
        }
    }
});