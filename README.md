PickupMailViewer
================

A web front end for viewing all files created by smtp with deliveryMethod="SpecifiedPickupDirectory"
[See screenshots of Pickup Mail Viewer in action](Doc/screenshot.md) 

##Installation##
Build and publish to your IIS server.
By default the PickupMailViewer will be installed in http://yourserver/MailViewer/
You may edit the publish settings if you want to install in another path.

##Configuration##
PickupMailViewer is configured in web.config
Default config is

    <configuration>
      <applicationSettings>
        <PickupMailViewer.Properties.Settings>
          <setting name="MailDir" serializeAs="String">
            <value>c:\temp</value>
          </setting>
        </PickupMailViewer.Properties.Settings>
      </applicationSettings>
    </configuration>

The only settings you can change is `MailDir`. Default configuration is `c:\temp` as that matches the setup in my current project. Change it to the same path your application is saving your mails to.

##Sending application configuration##
To configure your own application to save all outgoing mails to a folder, edit app.config or web.config for your application to

    <configuration>
      <system.net>
        <mailSettings>
          <smtp deliveryMethod="SpecifiedPickupDirectory" from="my-sender@mydomain.com">
            <specifiedPickupDirectory pickupDirectoryLocation="c:\temp"/>
            <!-- This settings isn't used, but without it an exception occurs on disposing
            of the SmtpClient.-->
            <network host="localhost"/>
          </smtp>
        </mailSettings>
      </system.net>
    </configuration>

You may of course use a path different from `c:\temp`

##Is it safe?##
###tl;dr;###
No way!
###The longer story###
By default there is no login, no encryption. Nothing that prevents anyone to read your sensitive mails.
However the PickupMailViewer at least makes an effort to only display information from eml files and only from the MailDir path. No parent folders, no subfolders. That's about the protection you get.

##Continous integration##
Latest build exists on [AppVeyor](https://ci.appveyor.com/project/albinsunnanbo/pickupmailviewer).  
Click on Artifacts for the latest deploy package.

##Contribution##
Feel free to leave a pull request on GitHub.