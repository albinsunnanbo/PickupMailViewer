/// <reference path="../Scripts/_references.js" />

(function () {
    "use strict";

    Swag.registerHelpers(Handlebars);

    jQuery.fn.flash = function (color, duration, noTimes, durationOut) {
        noTimes = noTimes || 1;
        durationOut = durationOut | duration;
        var current = this.css('background-color');
        for (var i = 0; i < noTimes; i++) {
            this.animate({ 'background-color': 'rgb(' + color + ')' }, duration / 2);
            this.animate({ 'background-color': current }, durationOut / 2);
        }
    }

    $(function () {
        $("body").on("click", ".clickable-row", function () {
            var mailId = $(this).data("mail-id");
            if (typeof (mailId) !== "undefined") {
                $.get(baseUrl + "Home/GetMailDetails", { mailId: mailId, subPath: subpath },
                   function (message) {
                       var linkedMessage = Autolinker.link(message);
                       var dialogContent = $(linkedMessage);
                       dialogContent.dialog({ width: 800, height: 600 });
                   }
               );
            }
        });

        $("#search-form").submit(function (e) {
            e.preventDefault();
            var searchText = $("#search-text").val();

            if (searchText.length >= 3) {
                $(".message-row").hide();
                $.get(baseUrl + "Home/Search", { searchText: searchText, subPath: subpath },
                   function (messageIds) {
                       $.each(messageIds, function (idx, val) {
                           $(".message-row[data-mail-id='" + val + "']").show();
                       });
                   }
               );
            }
            else if (searchText.length === 0) {
                $(".message-row").show();
            }
        });
    });

    function renderMessageRow(message) {
        var newRow = $(mailviewer.handlebars.templates.mailrow(message));
        return newRow;
    }

    var loadInitialList = function () {
        var messages = JSON.parse($("#initial-messages").html());
        $.each(messages, function (idx, message) {
            var newRow = renderMessageRow(message);
            $('#message-table tbody').append(newRow); // add as last row
        });
    };

    var subpath;
    $(function () {
        subpath = $("#subpath").val();

        // Reference the auto-generated proxy for the hub.
        var chat = $.connection.signalRHub;
        // Create a function that the hub can call back to display messages.
        chat.client.newMessage = function (messages, onTop, msgSubPath) {
            if (msgSubPath != subpath) {
                // Not for the current path
                return;
            }

            // if just a single string, wrap in an array
            if (!$.isArray(messages)) {
                messages = [messages];
            }

            if (onTop) {
                messages.reverse(); // since we are adding the last element at the top
            }

            // Add the messages to the page.
            $.each(messages, function (idx, message) {

                var newRow = renderMessageRow(message);
                if (onTop) {
                    $('#message-table tbody').prepend(newRow); // add as first row after header row

                    // flash row color
                    newRow.flash('255,255,0', 1000, 3);
                    newRow.flash('255,255,128', 1000, 1, 60000);
                }
                else {
                    $('#message-table tbody').append(newRow); // add at bottom
                }
            });
        };
        // Start the connection.
        $.connection.hub.start().done(function () {
            // Connect event
            var lastId = "";
            var messages = JSON.parse($("#initial-messages").html());
            $.each(messages, function (idx, message) {
                lastId = message.MessageId;
            });
            chat.server.getRest(lastId, subpath);
        });

        continousReconnect();
        setupNotifications();

        loadInitialList();
    });

    // This optional function html-encodes messages for display in the page.
    var htmlEncode = function (value) {
        var encodedValue = $('<div />').text(value).html();
        return encodedValue;
    }

    // Setup infinite reconnect
    var continousReconnect = function () {
        $.connection.hub.disconnected(function () {
            setTimeout(function () {
                $.connection.hub.start();
            }, 5000); // Restart connection after 5 seconds.
        });
    };

    var setupNotifications = function () {
        $.connection.hub.connectionSlow(function () {
            $("#notification-area").text("Connection is slow");
        });

        $.connection.hub.reconnecting(function () {
            $("#notification-area").text("Reconnecting...");
        });

        $.connection.hub.reconnected(function () {
            var notificationText = "Connected!";
            $("#notification-area").text(notificationText);
            // Dirty hack to clear, just check that we still have the same text
            setTimeout(function () {
                if ($("#notification-area").text() === notificationText); {
                    $("#notification-area").text(''); // Clear
                }
            }, 5000);
        });
    };
})();