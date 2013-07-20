(function ($, pubsub, util, data, renderEngine) {
    var isSelected = false,
        generateAddress = function (albumId) {
            var currentMetadata = data.currentMetadata();
            return util.uriTemplate(currentMetadata.resources.mvcmusicstore_framework_inventoryresource, { 'albumId': albumId });
        },
        setup = function (args) {
            args.newData.data.inventory = { name: 'Inventory', data: '', isPermanent: true };
            args.newData.metadata.plugins.inventory = {};
        },
        build = function (args) { 
            var targets = $('[data-albumId]');
            if (targets.length > 0) {
                targets.mouseenter(function () {
                    if (isSelected) {
                        request(args.panel, $(this).attr('data-albumId'));
                    }
                })
            } 
        },
        request = function (panel, albumId) {
            $.ajax({
                url: generateAddress(albumId),
                type: 'GET',
                contentType: 'application/json',
                success: function (result) {
                    layout(panel, result);
                }
            }); 
        },
        layout = function (panel, result) {
            renderEngine.insert(panel, result, { "keysHeadings": true });
        },
        activate = function () {
            isSelected = true; 
        },
        deactivate = function () {
            isSelected = false;
        };


    pubsub.subscribe('action.panel.hiding.inventory', deactivate);
    pubsub.subscribe('action.panel.showing.inventory', activate);
    pubsub.subscribe('action.data.initial.changed', setup);
    pubsub.subscribe('action.panel.rendered.inventory', build);
})(jQueryGlimpse, glimpse.pubsub, glimpse.util, glimpse.data, glimpse.render.engine);