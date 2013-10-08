$(document).ready(function () {

    ColorBoxInit();

    // show/hide (view/config) image objects
    $('li').mouseover(function () {
        $(this).find('div').css("display", 'block');
    });

    $('li').mouseleave(function () {
        $(this).find('div').css("display", 'none');
    });

});

function ColorBoxInit() {
    $('.group1').colorbox({ rel: 'group1' });
    $('.group2').colorbox({ rel: 'group2' });
    $('.group3').colorbox({ rel: 'group3' });
    $('.group4').colorbox({ rel: 'group4' });
    $('.group5').colorbox({ rel: 'group5' });
    $('.group6').colorbox({ rel: 'group6' });
    $('.group7').colorbox({ rel: 'group7' });
    $('.group8').colorbox({ rel: 'group8' });
    $('.group9').colorbox({ rel: 'group9' });
    $('.group10').colorbox({ rel: 'group10' });
    $('.group11').colorbox({ rel: 'group11' });
    $('.group12').colorbox({ rel: 'group12' });
}