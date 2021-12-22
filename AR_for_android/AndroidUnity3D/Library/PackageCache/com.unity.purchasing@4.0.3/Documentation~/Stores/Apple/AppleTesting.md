# Testing

To test on Apple stores you must be using an App Store Connect test account, which can be created in App Store Connect.

Sign out of the App Store on the iOS device or laptop, launch your application and you will be prompted to log in when you attempt either a purchase or to restore transactions.

If you receive an initialization failure with a reason of `NoProductsAvailable`, follow this checklist:

* App Store Connect product identifiers must exactly match the product identifiers supplied to Unity IAP
* In-App purchases must be enabled for your application in App Store Connect
* Products must be cleared for sale in App Store Connect
* It may take many hours for newly created App Store Connect products to be available for purchase
* You must agree to the latest App Store Connect developer agreements and have active bank details

# Mac App Store

When building a desktop Mac build you must select `Mac App Store validation` within Unity’s build settings.

Once you have built your App, you must update its info.plist file with your bundle identifier and version strings. Right click on the `.app` file and click `show package contents`, locate the `info.plist` file and update the `CFBundleIdentifier` string to your application's bundle identifier.

You must then sign, package and install your application. You will need to run the following commands from an OSX terminal:

````
codesign -f --deep -s "3rd Party Mac Developer Application: " your.app/Contents/Plugins/unitypurchasing.bundle
codesign -f --deep -s "3rd Party Mac Developer Application: " your.app
productbuild --component your.app /Applications --sign "3rd Party Mac Developer Installer: " your.pkg
````

To sign the bundle, you may first need to remove the Contents.meta file if it exists: `your.app/Contents/Plugins/unitypurchasing.bundle/Contents.meta`

In order to install the package correctly you must delete the unpackaged .app file before running the newly created package.

You must then launch your App from the Applications folder. The first time you do so, you will be prompted to enter your iTunes account details, for which you should enter your App Store Connect test user account login. You will then be able to make test purchases against the sandbox environment.


