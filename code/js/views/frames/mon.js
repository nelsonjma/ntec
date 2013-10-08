var defaultColor;
var defaultFontWeight;

$(document).ready(function () {
    defaultColor = $('#lbTitle').css('color');
    defaultFontWeight = $('#lbTitle').css('font-weight');
});

function UpdatingDataInformation() {

    $('#lbTitle').css('color', 'darkred');
    $('#lbTitle').css('font-weight', '900');
}

function ConfirmDataInformation() {
    $('#lbTitle').css('color', defaultColor);
    $('#lbTitle').css('font-weight', defaultFontWeight);
}