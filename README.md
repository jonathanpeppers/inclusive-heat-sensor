# inclusive-heat-sensor

An idea for a bot that will detect "heated conversations" scoring based on an anger or offensive rating.

## Testing on Azure

```powershell
Invoke-WebRequest -Method POST -Uri https://inclusiveheatsensorfunctions.azurewebsites.net/api/heatsensor -ContentType "application/json" -Body "{ 'comment': 'THIS PROJECT IS TERRIBLE'}"
```

## Testing Locally

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
