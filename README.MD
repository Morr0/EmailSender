# Email Sender
## Brief
This application sends batch emails behalf of your Gmail account. You need to get credentials in the form of **client id** and 
**client secret** from [https://console.cloud.google.com/apis](https://console.cloud.google.com/apis) and put them in `appsettings.json`
as it is done in `appsettings.Development.json`.

## To use
Just run the server using:
```bash
dotnet run
```
Then go to the `Email` tab, it will prompt you to authorize with Gmail. Once you are authorized, the UI is simple to use.

## Remarks
- I assume you are familiar with the [Gmail Api](https://developers.google.com/gmail/api/). In order to send to any
  email you need to verify your Google application in the developer console.
- No Javascript is used, so if you don't add any destinations (Name + Email) and click send, nothing will be sent.