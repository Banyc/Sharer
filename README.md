# Sharer

Share photo and text from any device to computer

## Demo

### Server-side

![](img/demo.showQr.png)

### Client-side

![](img/demo.upload.png)

## Warning

- About security
    - This app does NOT provide any security. Under HTTP protocol, the message sent is not encrypted, which might be safely passed in a trusted LAN or through a private hot spot.
    - If using SSL protocol, the certificate is self-signed. Before sending files, please make sure the public key fields of certificates in both client and server are identical.
- About performance
    - The file transfer is only performed by form posting.

## Notices

The presumed `pwd` is `./src/Sharer`.

## Build

This might cost a long time

    dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true

## How to use

For windows users:

1. run `Launch.bat`
1. make sure your device is in the same LAN as your computer
1. let your device scan the QR code of the secure channel
1. send photo or text
1. check the photo in the popped-up file explorer or find the text in the console

For Linux + MacOS users:

- run `dotnet run`
- the folder `upload.user` in the root of the project is where the files locate.

## TODO

- ~~[ ] Password + ban IP after excessive tries~~
- [ ] Add "Open download folder" on the QR page
- [ ] Huge file transfer
- [ ] Transfer through RESTful API
    - on project "SharerBlazorServer"
