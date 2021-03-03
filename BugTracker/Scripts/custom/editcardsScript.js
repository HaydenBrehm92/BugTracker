$(document).ready(function () {
    $('#search').keyup(function () {
        var input, search, bugCat, inProgressCat, TestCat, CompletedCat, i, textDesc, bugcard, progresscard,
            testcard, completecard, card, idCard;
        var statusval, catval, expectedval, realval, optionalval, prevID;

        input = document.getElementById('search')
        search = input.value.toUpperCase();

        bugCat = document.getElementById('bugcontainer')
        if (bugCat != null) { bugcard = bugCat.getElementsByClassName("form-control"); }
        
        inProgressCat = document.getElementById('inprogresscontainer')
        if (inProgressCat != null) { progresscard = inProgressCat.getElementsByClassName("form-control"); }
        
        TestCat = document.getElementById('testingcontainer')
        if (TestCat != null) { testcard = TestCat.getElementsByClassName("form-control"); }
        
        CompletedCat = document.getElementById('completedcontainer')
        if (CompletedCat != null) { completecard = CompletedCat.getElementsByClassName("form-control"); }
        
        

        //Bug Cards
        if (bugcard != null) {
            for (i = 0; i < bugcard.length; i++) {
                idCard = bugcard[i].getAttribute('id').split('-');
                if (prevID != "" && prevID != idCard[1]) {
                    statusval = "";
                    catval = "";
                    expectedval = "";
                    realval = "";
                    optionalval = "";
                }
                prevID = idCard[1];
                card = document.getElementById('card#' + idCard[1]);
                var temp = bugcard[i].getAttribute('id');

                if (temp === ('status-' + idCard[1]) || temp === ('category-' + idCard[1])) {
                    if (temp === ('status-' + idCard[1])) {
                        id = document.getElementById('status-' + idCard[1]);
                        statusval = id.options[id.selectedIndex].text.toUpperCase().indexOf(search);
                    }
                    else {
                        id = document.getElementById('category-' + idCard[1]);
                        catval = id.options[id.selectedIndex].text.toUpperCase().indexOf(search);
                    }
                }

                if (temp === ('expected-' + idCard[1])) {
                    expectedval = bugcard[i].textContent.toUpperCase().indexOf(search);
                }

                if (temp === ('reality-' + idCard[1])) {
                    realval = bugcard[i].textContent.toUpperCase().indexOf(search);
                }

                if (temp === ('optional-' + idCard[1])) {
                    optionalval = bugcard[i].textContent.toUpperCase().indexOf(search);
                }

                textDesc = document.getElementById('desc-' + idCard[1]).innerHTML;

                if (statusval > -1 || catval > -1 || expectedval > -1 || realval > -1 || optionalval > -1 ||
                    textDesc.toUpperCase().indexOf(search) > -1) {
                    card.style.display = '';
                }
                else {
                    card.style.display = "none";
                }
            }
        }
        

        //In Progress Cards
        if (progresscard != null) {
            for (i = 0; i < progresscard.length; i++) {
                idCard = progresscard[i].getAttribute('id').split('-');
                if (prevID != "" && prevID != idCard[1]) {
                    statusval, catval, expectedval, realval, optionalval = "";
                }
                prevID = idCard[1];
                card = document.getElementById('card#' + idCard[1]);
                var temp = progresscard[i].getAttribute('id');

                if (temp === ('status-' + idCard[1]) || temp === ('category-' + idCard[1])) {
                    if (temp === ('status-' + idCard[1])) {
                        id = document.getElementById('status-' + idCard[1]);
                        statusval = id.options[id.selectedIndex].text.toUpperCase().indexOf(search);
                    }
                    else {
                        id = document.getElementById('category-' + idCard[1]);
                        catval = id.options[id.selectedIndex].text.toUpperCase().indexOf(search);
                    }
                }

                if (temp === ('expected-' + idCard[1])) {
                    expectedval = progresscard[i].textContent.toUpperCase().indexOf(search);
                }

                if (temp === ('reality-' + idCard[1])) {
                    realval = progresscard[i].textContent.toUpperCase().indexOf(search);
                }

                if (temp === ('optional-' + idCard[1])) {
                    optionalval = progresscard[i].textContent.toUpperCase().indexOf(search);
                }

                textDesc = document.getElementById('desc-' + idCard[1]).innerHTML;

                if (statusval > -1 || catval > -1 || expectedval > -1 || realval > -1 || optionalval > -1 ||
                    textDesc.toUpperCase().indexOf(search) > -1) {
                    card.style.display = '';
                }
                else {
                    card.style.display = "none";
                }
            }
        }
        

        //Testing Cards
        if (testcard != null) {
            for (i = 0; i < testcard.length; i++) {
                idCard = testcard[i].getAttribute('id').split('-');
                if (prevID != "" && prevID != idCard[1]) {
                    statusval, catval, expectedval, realval, optionalval = "";
                }
                prevID = idCard[1];
                card = document.getElementById('card#' + idCard[1]);
                var temp = testcard[i].getAttribute('id');

                if (temp === ('status-' + idCard[1]) || temp === ('category-' + idCard[1])) {
                    if (temp === ('status-' + idCard[1])) {
                        id = document.getElementById('status-' + idCard[1]);
                        statusval = id.options[id.selectedIndex].text.toUpperCase().indexOf(search);
                    }
                    else {
                        id = document.getElementById('category-' + idCard[1]);
                        catval = id.options[id.selectedIndex].text.toUpperCase().indexOf(search);
                    }
                }

                if (temp === ('expected-' + idCard[1])) {
                    expectedval = testcard[i].textContent.toUpperCase().indexOf(search);
                }

                if (temp === ('reality-' + idCard[1])) {
                    realval = testcard[i].textContent.toUpperCase().indexOf(search);
                }

                if (temp === ('optional-' + idCard[1])) {
                    optionalval = testcard[i].textContent.toUpperCase().indexOf(search);
                }

                textDesc = document.getElementById('desc-' + idCard[1]).innerHTML;

                if (statusval > -1 || catval > -1 || expectedval > -1 || realval > -1 || optionalval > -1 ||
                    textDesc.toUpperCase().indexOf(search) > -1) {
                    card.style.display = '';
                }
                else {
                    card.style.display = "none";
                }
            }
        }
        

        //Completed
        if (completecard != null) {
            for (i = 0; i < completecard.length; i++) {
                idCard = completecard[i].getAttribute('id').split('-');
                if (prevID != "" && prevID != idCard[1]) {
                    statusval, catval, expectedval, realval, optionalval = "";
                }
                prevID = idCard[1];
                card = document.getElementById('card#' + idCard[1]);
                var temp = completecard[i].getAttribute('id');

                if (temp === ('status-' + idCard[1]) || temp === ('category-' + idCard[1])) {
                    if (temp === ('status-' + idCard[1])) {
                        id = document.getElementById('status-' + idCard[1]);
                        statusval = id.options[id.selectedIndex].text.toUpperCase().indexOf(search);
                    }
                    else {
                        id = document.getElementById('category-' + idCard[1]);
                        catval = id.options[id.selectedIndex].text.toUpperCase().indexOf(search);
                    }
                }

                if (temp === ('expected-' + idCard[1])) {
                    expectedval = completecard[i].textContent.toUpperCase().indexOf(search);
                }

                if (temp === ('reality-' + idCard[1])) {
                    realval = completecard[i].textContent.toUpperCase().indexOf(search);
                }

                if (temp === ('optional-' + idCard[1])) {
                    optionalval = completecard[i].textContent.toUpperCase().indexOf(search);
                }

                textDesc = document.getElementById('desc-' + idCard[1]).innerHTML;
                if (statusval > -1 || catval > -1 || expectedval > -1 || realval > -1 || optionalval > -1 ||
                    textDesc.toUpperCase().indexOf(search) > -1) {
                    card.style.display = '';
                }
                else {
                    card.style.display = "none";
                }
            }
        }
        
    });

    function undoChanges(status, category, id) {
        var Status = $('#Status' + id).val(), Category = $('#Category' + id).val(), ExpectedResult = $('#ExpectedResult' + id).val(), RealityResult = $('#RealityResult' + id).val(),
            OptionalInformation = $('#OptionalInformation' + id).val();

        switch (Status) {
            case ('Trivial'):
                $(status).val('0')
                break;
            case ('Low'):
                $(status).val('1')
                break;
            case ('Moderate'):
                $(status).val('2')
                break;
            case ('Severe'):
                $(status).val('3')
                break;
        };

        switch (Category) {
            case ('Unassigned'):
                $(category).val('0')
                break;
            case ('In Progress'):
                $(category).val('1')
                break;
            case ('Testing'):
                $(category).val('2')
                break;
            case ('Completed'):
                $(category).val('3')
                break;
        };

        document.getElementById("expected-" + id).value = ExpectedResult;
        document.getElementById("reality-" + id).value = RealityResult;
        document.getElementById("optional-" + id).value = OptionalInformation;
    }

    $('.btnclass').on('click', function () {
        var btnval = $(this).attr('value');
        var btnsplit = $(this).attr('id').split("-");
        var id = btnsplit[2];
        
        var status = $('#status-'+id);
        var category = $('#category-'+id);
        var expectedres = $('#expected-'+id);
        var realres = $('#reality-'+id);
        var optional = $('#optional-'+id);

        if (btnval == "disabled") {
            status.prop('disabled', false);
            category.prop('disabled', false);
            expectedres.prop('readonly', false);
            realres.prop('readonly', false);
            optional.prop('readonly', false);
            $(this).prop('value', 'enabled');
            $(this).html('<i class="fa fa-undo fa-lg" aria-hidden="false" data-toggle="tooltip" data-placement="bottom" title="Undo"></i>');
            $('#save-card-' + id).addClass('visible').removeClass('invisible');
        }
        else {
            undoChanges(status, category, id);
            status.prop('disabled', true);
            category.prop('disabled', true);
            expectedres.prop('readonly', true);
            realres.prop('readonly', true);
            optional.prop('readonly', true);
            $(this).prop('value', 'disabled');
            $(this).html('<i class="fa fa-pencil-alt fa-lg" aria-hidden="false" data-toggle="tooltip" data-placement="bottom" title="Edit"></i>');
            $('#save-card-' + id).addClass('invisible').removeClass('visible');
        }
    });
});