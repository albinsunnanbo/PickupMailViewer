/// <reference path="../Scripts/_references.js" />

(function () {
    "use strict";


    jQuery.fn.flash = function (color, duration, noTimes) {
        noTimes = noTimes || 1;
        var current = this.css('background-color');
        for (var i = 0; i < noTimes; i++) {
            this.animate({ 'background-color': 'rgb(' + color + ')' }, duration / 2);
            this.animate({ 'background-color': current }, duration / 2);
        }
    }

    $(function () {
        $("body").on("click", ".mail-row", function () {
            var mailId = $(this).data("mail-id");
            $.getJSON("Home/GetMailDetails", { mailId: mailId },
                function (message) {
                    $('<div>' +
                        '<div>From: ' + message.FromAddress + '</div>' +
                        '<div>To: ' + message.ToAddress + '</div>' +
                        '<div>Sent date: ' + new Date(parseInt(message.SentOn.substr(6))).toLocaleString() + '</div>' +
                        '<div>Subject: ' + message.Subject + '</div>' +
                        '<hr/>' +
                        '<div class="mail-body">' + message.Body + '</div>' +
                        '<hr/>' +
                        '<a href="Home/DownloadMail?mailId=' + mailId + '">Download mail</a>' +
                        '</div>').dialog({ width: 800, height: 600 });
                }
            );
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
                var newRow = $(
                    '<tr class="mail-row" data-mail-id="' + message.MailId + '">' +
                    '<td>' + new Date(message.SentOn).toLocaleString() + '</td>' +
                    '<td>' + message.ToAddress + '</td>' +
                    '<td>' + message.Subject + '</td>' +
                    '</tr>');
                $('#mail-table tr:first()').after(newRow);

                //flash
                newRow.flash('255,255,0', 1000, 3);

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