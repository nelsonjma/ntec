function getDocHeight() {
    return Math.max(
        $(document).height(), $(window).height(), document.documentElement.clientHeight /* For opera: */);
}


function getDocWidth() {
    return Math.max(
        $(document).width(), $(window).width(), document.documentElement.clientWidth /* For opera: */);
}

/********************************** Url Ctrls **********************************/
function OpenInNewTab(url) {
    var win = window.open(url, '_blank');
    win.focus();
}

function GetURLParameter(name) {
    return decodeURI(
        (RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]
    );
}

// remove special char in html of in ajax transfer
function RemoveSpecialChars(data) {
    
    // &lt; - <
    while (data.indexOf('&lt;') >= 0) { data = data.replace('&lt;', '<', 'gm'); }

    // &gt; - >
    while (data.indexOf('&gt;') >= 0) { data = data.replace('&gt;', '>', 'gm'); }

    return data;

}


function IsPageHidden() {
    return document.hidden || document.msHidden || document.webkitHidden || document.mozHidden;
}