# Sharer

Share photo and text from any device to computer

## Warning

This app does NOT provide any security. The message sent is not encrypted, which might be safely passed in a trusted LAN or through a private hot spot.

## Build

This might cost a long time

    dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true

## How to use

For windows users

- run `Launch.bat`
- make sure your device is in the same LAN as your computer
- let your device scan the QR code of the secure channel
- send photo or text
- check the photo in the popped-up file explorer or find the text in the console

## TODO

- [ ] Password + ban IP after excessive tries
- [ ] Add "Open download folder" on the QR page
