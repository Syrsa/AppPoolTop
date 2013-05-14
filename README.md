AppPoolTop
==========

An Windows app to view CPU usage per AppPool for IIS, doens't always give a fair image of the cpu used since its only monitoring the usage the w3wp.exe resource, if any other resource is started it gets missed.

The app doesn't handle it well when an AppPool dies either (simply crashes)

But for simple usage it does what I wanted it to.
