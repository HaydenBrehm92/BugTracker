$(document).ready(function () {
    //var render = $('#renderbody');
    //var pager = $('#pager');
    //var deletebtn = $('.delete');
    //var deleteform = $('.delete');

    //var fetchAndDelete, fetchAndinsert;

    //fetchAndDelete = function (href) {
    //    $.ajax({
    //        url: href,
    //        method: 'POST',
    //        async: true,
    //        cache: false,
    //        success: function (data) {
    //            render.html(data);
    //        }

    //    });
    //};

    //fetchAndinsert = function (href) {
    //    //console.log(href)
    //    $.ajax({
    //        url: href,
    //        async: true,
    //        method: 'GET',
    //        cache: false,
    //        success: function (data) {
    //            render.html(data);
    //        }
    //    });
    //}

    //pager.find('a').on('click', function (e) {
    //    e.preventDefault();
    //    var href = $(this).attr('href');
    //    console.log(href);
    //    history.pushState(null, null, href);
    //    fetchAndinsert(href);

    //});

    //deleteform.submit(function (e) {
    //    e.preventDefault();
    //    var c = confirm('Are you sure you want to delete this project?');
    //    if (c == true) {
    //        var href = $(this).attr('action');
    //        var type = $(this).attr('method');
    //        console.log(href);
    //        //history.pushState(null, null, href);
    //        $.ajax({
    //            type: type,
    //            url: href,
    //            method: 'POST',
    //            data: deleteform.serialize,
    //            async: true,
    //            cache: false,
    //            success: function (data) {
    //                //render.html(data);
    //            }

    //        });

    //    }
    //    else {
    //        return false;
    //    }

    //});

    //deletebtn.on('click', function (e) {
    //    e.preventDefault();
    //    var c = confirm('Are you sure you want to delete this project?');
    //    if ( c == true) {
    //        var href = $(this).attr('href')
    //        console.log(href);
    //        //history.pushState(null, null, href);
    //        fetchAndDelete(href);

    //    }
    //    else {
    //        return false;
    //    }
        

    //});

    //$(window).on('popstate', function () {
    //    fetchAndinsert(location.pathname);
    //});

    //var btn = $('.deletelink');

    //btn.on('click',function (e) {
    //    e.preventDefault();
    //    var c = confirm('Are you sure you want to delete this project?');
    //    if ( c == true) {
    //        return true;
    //    }
    //    else {
    //        return false;
    //    }
    //});
});