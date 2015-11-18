this["mailviewer"] = this["mailviewer"] || {};
this["mailviewer"]["handlebars"] = this["mailviewer"]["handlebars"] || {};
this["mailviewer"]["handlebars"]["templates"] = this["mailviewer"]["handlebars"]["templates"] || {};
this["mailviewer"]["handlebars"]["templates"]["mailrow"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
    return " clickable-row";
},"3":function(depth0,helpers,partials,data) {
    var helper;

  return " data-mail-id=\""
    + this.escapeExpression(((helper = (helper = helpers.MessageId || (depth0 != null ? depth0.MessageId : depth0)) != null ? helper : helpers.helperMissing),(typeof helper === "function" ? helper.call(depth0,{"name":"MessageId","hash":{},"data":data}) : helper)))
    + "\" ";
},"5":function(depth0,helpers,partials,data) {
    return "        <span class=\"glyphicon glyphicon-envelope\"></span>\r\n";
},"7":function(depth0,helpers,partials,data) {
    return "        <span class=\"glyphicon glyphicon-phone\"></span>\r\n";
},"9":function(depth0,helpers,partials,data) {
    return "        <span class=\"glyphicon glyphicon-paperclip\" />\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
    var stack1, helper, alias1=helpers.helperMissing, alias2="function", alias3=this.escapeExpression;

  return "<tr class=\"message-row"
    + ((stack1 = (helpers.is || (depth0 && depth0.is) || alias1).call(depth0,(depth0 != null ? depth0.MessageType : depth0),"Mail",{"name":"is","hash":{},"fn":this.program(1, data, 0),"inverse":this.noop,"data":data})) != null ? stack1 : "")
    + "\" "
    + ((stack1 = (helpers.is || (depth0 && depth0.is) || alias1).call(depth0,(depth0 != null ? depth0.MessageType : depth0),"Mail",{"name":"is","hash":{},"fn":this.program(3, data, 0),"inverse":this.noop,"data":data})) != null ? stack1 : "")
    + ">\r\n    <td>\r\n"
    + ((stack1 = (helpers.is || (depth0 && depth0.is) || alias1).call(depth0,(depth0 != null ? depth0.MessageType : depth0),"Mail",{"name":"is","hash":{},"fn":this.program(5, data, 0),"inverse":this.noop,"data":data})) != null ? stack1 : "")
    + ((stack1 = (helpers.is || (depth0 && depth0.is) || alias1).call(depth0,(depth0 != null ? depth0.MessageType : depth0),"Sms",{"name":"is","hash":{},"fn":this.program(7, data, 0),"inverse":this.noop,"data":data})) != null ? stack1 : "")
    + ((stack1 = helpers['if'].call(depth0,(depth0 != null ? depth0.AttachmentNames : depth0),{"name":"if","hash":{},"fn":this.program(9, data, 0),"inverse":this.noop,"data":data})) != null ? stack1 : "")
    + "    </td>\r\n    <td>\r\n        "
    + alias3(((helper = (helper = helpers.SentOnFormatted || (depth0 != null ? depth0.SentOnFormatted : depth0)) != null ? helper : alias1),(typeof helper === alias2 ? helper.call(depth0,{"name":"SentOnFormatted","hash":{},"data":data}) : helper)))
    + "\r\n    </td>\r\n    <td>\r\n        "
    + alias3(((helper = (helper = helpers.ToAddress || (depth0 != null ? depth0.ToAddress : depth0)) != null ? helper : alias1),(typeof helper === alias2 ? helper.call(depth0,{"name":"ToAddress","hash":{},"data":data}) : helper)))
    + "\r\n    </td>\r\n    <td>\r\n        "
    + alias3(((helper = (helper = helpers.Subject || (depth0 != null ? depth0.Subject : depth0)) != null ? helper : alias1),(typeof helper === alias2 ? helper.call(depth0,{"name":"Subject","hash":{},"data":data}) : helper)))
    + "\r\n    </td>\r\n</tr>";
},"useData":true});