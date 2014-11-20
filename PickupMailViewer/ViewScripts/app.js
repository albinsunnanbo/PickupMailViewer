/// <reference path="C:\Users\Albin\Documents\GitHub\PickupMailViewer\PickupMailViewer\Scripts/_references.js" />

(function () {
    "use strict";

    $(function () {
        $("body").on("click", ".mail-row", function () {
            document.location.href = "Home/DownloadMail?mailId=" + $(this).data("mail-id");
        });
    });

    $(function () {
        // Reference the auto-generated proxy for the hub.
        var chat = $.connection.signalRHub;
        // Create a function that the hub can call back to display messages.
        chat.client.newMail = function (messages) {
            // if just a single string, wrap in an array
            if (!$.isArray(messages)) {
                messages = [messages];
            }

            messages.reverse(); // since we are adding the last element at the top

            // Add the messages to the page.
            $.each(messages, function (idx, message) {
                $('#mail-table tr:first()').after(
                '<tr class="mail-row" data-mail-id="' + message.MailId + '">' +
                '<td>' + message.SentOn + '</td>' +
                '<td>' + message.ToAddress + '</td>' +
                '<td>' + message.Subject + '</td>' +
                '</tr>');
            });
        };
        // Start the connection.
        $.connection.hub.start().done(function () {
            // Connect event
        });

        continousReconnect();
        setupNotifications();
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