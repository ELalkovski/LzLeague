function displayMsg(successMsg, tagId) {

    if (successMsg.length > 0) {
        console.log(successMsg);
        $('#' + tagId).text(successMsg);
        $('#' + tagId).show();

        setTimeout(function () {
            $('#' + tagId).fadeOut();
        }, 4000);
    }
}