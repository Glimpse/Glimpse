objects = function () { 
    var //Constructors
        ConnectionNotice = function (scope) {
            var that = (this === window) ? {} : this;
            that.scope = scope;
            that.text = scope.find('span');
            return that;
        };
    ConnectionNotice.prototype = {
        connected : false, 
        prePoll : function () {
            var that = this;
            if (!that.connected) { 
                that.text.text('Connecting...'); 
                that.scope.removeClass('gconnect').addClass('gdisconnect');
            }
        },
        complete : function (textStatus) {
            var that = this;
            if (textStatus != "Success") {
                that.connected = false;
                that.text.text('Disconnected...');
                that.scope.removeClass('gconnect').addClass('gdisconnect');
            }
            else {
                that.connected = true;
                that.text.text('Connected...');
                that.scope.removeClass('gdisconnect').addClass('gconnect');
            }
        }
    };
    
    return {
        ConnectionNotice : ConnectionNotice
    }
} ()