function longest_common_str(data, separator) {
    var sorted = _.clone(data).sort();
    var parts = sorted[0].split(separator);
    var str = longest = '';
    for (var i = 0; i < parts.length; i++) {
        str += parts[i] + separator;
        if (_.every(sorted, function (s) { return s.indexOf(str) === 0; })) { // IE11 doesn't support startsWith :(
            longest = str;
        } else {
            break;
        }
    }
    return longest;
}

function parseHashBangArgs(aURL) {
    aURL = aURL || window.location.href;
    if (aURL.indexOf('#') < 0) { return null; }

    var hashes = aURL.split('#')[1].split('&');
    var vars = {};
    for (var i = 0; i < hashes.length; i++) {
        var hash = hashes[i].split('=');
        if (hash[0] === '') { continue; }
        vars[hash[0]] = hash.length > 1 ? decodeURI(hash[1]) : null;
    }
    return _.size(vars) > 0 ? vars : null;
}

function hierarchy(paths, seperator = '/') {
    return paths.reduce(function (hier, path) {
        var x = hier;
        path.split(seperator).forEach(function (item) {
            if (!x[item]) {
                x[item] = { };
            }
            x = x[item];
        });
        x.path = path;
        return hier;
    }, { });
}