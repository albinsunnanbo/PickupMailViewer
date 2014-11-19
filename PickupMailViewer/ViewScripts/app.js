/// <reference path="C:\Users\Albin\Documents\GitHub\PickupMailViewer\PickupMailViewer\Scripts/_references.js" />

$(function () {
    "use strict";

    $("body").on("click", ".mail-row", function () {
        document.location.href = "Home/DownloadMail?mailId=" + $(this).data("mail-id");
    });
});