// list of frames (frame is defined below)
var listFrames = new Array();

// load new frame
function AddFrame(frame) {
    listFrames.push(frame);
}

// invoked by children frames to refresh another frame, created specifically to be used in filters
function RefreshSlaves(masterFilterId) {
    for (var i = 0; i < listFrames.length; i++) {

        if (listFrames[i].GetIsSlave() == 1)
            if (listFrames[i].GetMasterTitle() == masterFilterId) {
                $("#" + listFrames[i].GetHash()).attr("src", $("#" + listFrames[i].GetHash()).attr("src"));
            }
    }
}

// frame class definition
function Frame() {

    var _hashId = "";
    var _isSlave = 0;
    var _masterTitle = '';

    this.GetHash = function () {
        return _hashId;
    };

    this.SetHashId = function (hashId) {
        _hashId = hashId;
    };

    this.GetIsSlave = function () {
        return _isSlave;
    };

    this.SetIsSlave = function (isSlave) {
        _isSlave = isSlave;
    };

    this.GetMasterTitle = function () {
        return _masterTitle;
    };

    this.SetMasterTitle = function (masterTitle) {
        _masterTitle = masterTitle;
    };
}