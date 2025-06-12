$(function () {
    $('.js-delete-confirm').on('click', function () {
        return confirm('確定要刪除？');
    });
});

/*取得當前querystring*/
function getUrlVars() {
    var i = window.location.href.indexOf('?');
    if (i == -1) {
        return {};
    }
    var vars = [], hash;
    var hashes = window.location.href.slice(i + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars[hash[0]] = hash[1];
    }
    return vars;
}