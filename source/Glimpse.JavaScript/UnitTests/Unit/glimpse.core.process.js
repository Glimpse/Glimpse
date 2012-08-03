/*
module("glimpse.core.process");

test("Basic requirements", function() {
    expect(14);
    
    //Trigger things off
    pubsub.publish('state.build.shell');
    var sRegistry = pubsub.sRegistry();  
    var pRegistry = pubsub.pRegistry();  

    //Raised and subscribed to the correct events 
    ok(sRegistry['state.build.shell'].length == 1); 
    ok(pRegistry['data.elements.processed'].length == 1); 
    
    //Find methods work
    ok(elements.findPanel('Test').length > 0);
    ok(elements.findTab('Test').length > 0); 
    ok(elements.findPanel('NotThere').length == 0);
    ok(elements.findTab('NotThere').length == 0);

    //Found required elements 
    ok(elements.scope.length > 0);
    ok(elements.holder.length > 0);
    ok(elements.opener.length > 0);
    ok(elements.spacer.length > 0);
    ok(elements.tabHolder.length > 0);
    ok(elements.panelHolder.length > 0);
    ok(elements.title.length > 0);
    ok(elements.options.length > 0);
});
*/