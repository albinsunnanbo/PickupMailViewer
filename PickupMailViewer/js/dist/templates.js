this["mailviewer"] = this["mailviewer"] || {};
this["mailviewer"]["handlebars"] = this["mailviewer"]["handlebars"] || {};
this["mailviewer"]["handlebars"]["templates"] = this["mailviewer"]["handlebars"]["templates"] || {};
this["mailviewer"]["handlebars"]["templates"]["mailrow"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
    var helper, alias1=helpers.helperMissing, alias2="function", alias3=this.escapeExpression;

  return "<tr class=\"message-row\" data-mail-id=\""
    + alias3(((helper = (helper = helpers.MailId || (depth0 != null ? depth0.MailId : depth0)) != null ? helper : alias1),(typeof helper === alias2 ? helper.call(depth0,{"name":"MailId","hash":{},"data":data}) : helper)))
    + "\">\r\n    <td>\r\n        <span class=\"glyphicon glyphicon-envelope\"></span>\r\n        <span class=\"glyphicon glyphicon-phone\"></span>\r\n    </td>\r\n    <td>\r\n        "
    + alias3(((helper = (helper = helpers.SentOnFormatted || (depth0 != null ? depth0.SentOnFormatted : depth0)) != null ? helper : alias1),(typeof helper === alias2 ? helper.call(depth0,{"name":"SentOnFormatted","hash":{},"data":data}) : helper)))
    + "\r\n    </td>\r\n    <td>\r\n        "
    + alias3(((helper = (helper = helpers.ToAddress || (depth0 != null ? depth0.ToAddress : depth0)) != null ? helper : alias1),(typeof helper === alias2 ? helper.call(depth0,{"name":"ToAddress","hash":{},"data":data}) : helper)))
    + "\r\n    </td>\r\n    <td>\r\n        "
    + alias3(((helper = (helper = helpers.Subject || (depth0 != null ? depth0.Subject : depth0)) != null ? helper : alias1),(typeof helper === alias2 ? helper.call(depth0,{"name":"Subject","hash":{},"data":data}) : helper)))
    + "\r\n    </td>\r\n</tr>";
},"useData":true});