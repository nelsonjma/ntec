$(window).resize(function () { $('#objframe').attr('height', getDocHeight() - (24) + "px"); });
$(document).ready(function () { $('#objframe').attr('height', getDocHeight() - (24) + "px"); });

$(document).ready(function () {

    $('.navbar a').click(function (e) {
        if ($(this).html() != 'Home') {
            $('iframe').attr('src', $(this).attr('href')); // if its home refresh page...
            e.preventDefault();
        }
    });

});