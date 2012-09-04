glimpse.util = (function($) {
    return {
        sortTabs: function (data) {
            var sorted = {},
                i, temp = [];
            
            for (i in data)
                temp.push({ id: i, name: data[i].name });
            temp.sort(function(a, b) {
                return a.name < b.name ? -1 : a.name > b.name ? 1 : 0;
            });
            for (i = 0; i < temp.length; i++)
                sorted[temp[i].id] = data[temp[i].id];
            return sorted;
        }
    };
})(jQueryGlimpse);