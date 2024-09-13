# inclusive-heat-sensor

An idea for a bot that will detect "heated conversations" scoring based on an anger or offensive rating.

> [!NOTE]
> This project is WIP, and we'll post a release when it's ready for others to use. Thanks!

## Testing on Azure

```powershell
Invoke-WebRequest -Method POST -Uri https://inclusiveheatsensorfunctions.azurewebsites.net/api/heatsensor -ContentType "application/json" -Body "{ 'comment': 'THIS PROJECT IS TERRIBLE'}"
```

## Testing Locally

You will need a `Inclusive.HeatSensor.Functions\local.settings.json` file with the contents:

```json
{
  "IsEncrypted": false,
  "Values": {
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "OPENAI_API_KEY": "TODO"
  }
}
```

I ran this powershell command to test the bot:

```powershell
Invoke-WebRequest -Method POST -Uri http://localhost:7241/api/heatsensor -ContentType "application/json" -Body "{ 'comment': 'THIS PROJECT IS TERRIBLE'}"
```
```
StatusCode        : 200
StatusDescription : OK
Content           : {"offensive":8,"anger":7}
RawContent        : HTTP/1.1 200 OK
                    Transfer-Encoding: chunked
                    Content-Type: application/json; charset=utf-8
                    Date: Fri, 23 Aug 2024 16:03:16 GMT
                    Server: Kestrel

                    {"offensive":8,"anger":7}
Forms             : {}
Headers           : {[Transfer-Encoding, chunked], [Content-Type, application/json; charset=utf-8], [Date, Fri, 23 Aug 2024 16:03:16 GMT], [Server, Kestrel]}
Images            : {}
InputFields       : {}
Links             : {}
ParsedHtml        : mshtml.HTMLDocumentClass
RawContentLength  : 25
```

## Debugging in VSCode

1. Install the `Azure Functions extension` from the VSCode Extensions tab
2. Install the `Azure Tools Core` via homebew
  ```
brew tap azure/functions
brew install azure-functions-core-tools@4
  ```
  alternatively the `Azure Functions extension` will install them for you on the first run.
  
3. Open the workspace using `code .` 
4. Cmd+P and run `.NET: Open Solution`
5. Goto the Debugging Tab and it should auto pickup the `Attach to .NET Functions` debug option. Press F5 to run. 
