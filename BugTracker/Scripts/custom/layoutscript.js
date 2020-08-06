$(document).ready(function () {
    var content = $('#openProject');
    var render = $('#renderbody');
    var fetchAndinsert;

    fetchAndinsert = function (href) {
        //console.log(href)
        $.ajax({
            url: href,
            async: true,
            method: 'GET',
            cache: false,
            success: function (data) {
                render.html(data);
            }
        });
    }

    content.on('click', function (e) {
        e.preventDefault();
        var href = "/Projects/OpenProject";
        history.pushState(null, null, href);
        fetchAndinsert(href);

    });

    $(window).on('popstate', function () {
        fetchAndinsert(location.pathname);
    });
});