//分頁
$(function () {
    var urlVars = getUrlVars();

    $('table.dataTable thead th').each(function (i, e) {
        var sorting = $(e).attr('data-sorting');
        if (sorting) {
            $(e).addClass('sorting');
        }
    });

    $('th.sorting').each(function (i, e) {
        var sorting = $(this).attr('data-sorting');
        var isDescending = (urlVars['IsDescending'] === 'true') || false;
        if (sorting === urlVars['Sorting']) {
            $(this).removeClass('sorting');
            var addCss = isDescending ? 'sorting_desc' : 'sorting_asc';
            $(this).addClass(addCss);
        }
    });

    $('.paginate_button:not(.disabled) a').on('click', function () {//
        const pager = $(this).attr('data-pager');
        urlVars['CurrentPage'] = pager;
        goToPage(urlVars);
    });

    $('.paginate_button:not(.disabled)').on('click', function () {
        const pager = $(this).attr('data-pager');
        urlVars['CurrentPage'] = pager;
        goToPage(urlVars);
    });

    $('.pagination__page:not(.disabled)').on('click', function () {
        const pager = $(this).attr('data-pager');
        urlVars['CurrentPage'] = pager;
        goToPage(urlVars);
    });

    $('#js-pageSize').on('change', function () {
        const pageSize = $(this).val();
        urlVars['CurrentPage'] = 1;
        urlVars['PageSize'] = pageSize;
        goToPage(urlVars);
    });

    $('.sorting').on('click', function () {
        var sorting = $(this).attr('data-sorting');
        urlVars['CurrentPage'] = 1;
        urlVars['Sorting'] = sorting;
        urlVars['IsDescending'] = true;
        goToPage(urlVars);
    });

    $('.sorting_desc').on('click', function () {
        var sorting = $(this).attr('data-sorting');
        urlVars['CurrentPage'] = 1;
        urlVars['Sorting'] = sorting;
        urlVars['IsDescending'] = false;
        goToPage(urlVars);
    });

    $('.sorting_asc').on('click', function () {
        var sorting = $(this).attr('data-sorting');
        urlVars['CurrentPage'] = 1;
        urlVars['Sorting'] = sorting;
        urlVars['IsDescending'] = true;
        goToPage(urlVars);
    });
})

function goToPage(params) {

    var queryString = Object.keys(params).map(function (key) {
        return key + '=' + params[key]
    }).join('&');

    const origin = window.location.origin;
    const pathname = window.location.pathname;
    const url = origin + pathname + '?' + queryString;
    window.location.href = url;
}

/*取得當前querystring*/
function getUrlVars() {
    var i = window.location.href.indexOf('?');
    if (i === -1) {
        return {};
    }
    var vars = [], hash;
    var hashes = window.location.href.slice(i + 1).split('&');
    for (var j = 0; j < hashes.length; j++) {
        hash = hashes[j].split('=');
        vars[hash[0]] = hash[1];
    }
    return vars;
}