$(document).ready(function () {
    // when you try to download if you click the wrong button it sends a message
    $('#lbDownload').click(function (e) {
        alert('right click and select save link');
        e.preventDefault();
    });

    CenterGridview();
});


// if gridview is smaller than iframe then it will try to center it, else grow mpanel to iframe inside document
function CenterGridview() {
    var windWidth = $(window).width();
    var tbContWidth = $('#mGridView').width();

    if (tbContWidth <= windWidth) {

        $('#mGridView').css('position', 'absolute');
        $('#mGridView').css('left', (windWidth / 2) - (tbContWidth / 2));

    } else {
        // set gridview width equal to document width when the table is bigger than the iframe
        $('#mPanel').css('width', getDocWidth());
    }
}