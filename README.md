# Kiosky
A kiosk/restricted browser for Windows based on Chromium Embedded Framework

This is in very early alpha!

## Features
 * Provides a full screen mechanism for viewing web pages
 * Can cycle between several pages on an interval
 * Allows restricting the resources a page loads such as blocking Ads or preventing browsing outside a certain domain
 * Blanks out additional screens
 
## How to use
### Launching
 1. Extract the Release.zip
 1. Run Kiosky.exe
 1. Configure by pressing the pause button in the right side of your keyboard
 1. Setup the URL(s) you want to load, and the interval you want it to cycle through them on
 1. Setup an admin password so the config can't be changed
 1. Save
 1. Add to startup to have it launch automatically
 
### Debugging
 * Logs are written to %LOCALAPPDATA%\KioskBrowser\logs\browser.log
