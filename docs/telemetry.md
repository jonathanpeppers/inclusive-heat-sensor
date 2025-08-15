# Telemetry

The backend web service records a log message like:

```json
Rating: {"offensive":0,"anger":0,"url":"https://github.com/dotnet/maui/issues/31186"}
```

This data is purged on a standard timeframe by Azure Log Analytics. Callers can decide if they want to pass `url` or not.

## Queries

These are just some useful queries we've written:

```kusto
traces
| where timestamp > ago(90d)
| where message startswith "Rating:"
| extend anger = todouble(parse_json(tostring(customDimensions.rating)).anger)
| extend offensive = todouble(parse_json(tostring(customDimensions.rating)).offensive)
| extend url = tostring(parse_json(tostring(customDimensions.rating)).url)
| where anger != 0 or offensive != 0
| order by anger, offensive desc
```

[Run Query][descending]

```kusto
traces
| where timestamp > ago(90d)
| where message startswith "Rating:"
| extend anger = todouble(parse_json(tostring(customDimensions.rating)).anger)
| extend offensive = todouble(parse_json(tostring(customDimensions.rating)).offensive)
| extend url = tostring(parse_json(tostring(customDimensions.rating)).url)
| where anger != 0 or offensive != 0
| summarize avgAnger=avg(anger), avgOffensive=avg(offensive) by bin(timestamp, 1d)
| render timechart
```

[Run Query][over-time]

[descending]: https://portal.azure.com#@72f988bf-86f1-41af-91ab-2d7cd011db47/blade/Microsoft_OperationsManagementSuite_Workspace/Logs.ReactView/resourceId/%2Fsubscriptions%2Fd21a525e-7c86-486d-a79e-a4f3622f639a%2FresourceGroups%2Ficr-heat-sensor-2409-rg%2Fproviders%2Fmicrosoft.insights%2Fcomponents%2Ficr-heat-sensor-ai/source/LogsBlade.AnalyticsShareLinkToQuery/q/H4sIAAAAAAAAA52QTQ6CQAxG956isoLEELaa4MoTeAEzMOXHwJS0RTTx8A5ggK1u53vf67TKJkfZvWGokBG0blHUtB2cwZQUHhMbLaGPxJQIHmCVodYKgqvR2pWnwEP4VHQWjCuRIQUlS33WYNgZFrzdhVyoJMqeD%252FNelNqLn%252BakJicxT54oiqd6tOqoKEbmgf8rF8VG23MzCb%252Fl34S%252BvF5l3nefQgLEm%252B%252BOLx4itj7OXjN32AAWJf8AeVva6X8BAAA%253D/limit/1000
[over-time]: https://portal.azure.com#@72f988bf-86f1-41af-91ab-2d7cd011db47/blade/Microsoft_OperationsManagementSuite_Workspace/Logs.ReactView/resourceId/%2Fsubscriptions%2Fd21a525e-7c86-486d-a79e-a4f3622f639a%2FresourceGroups%2Ficr-heat-sensor-2409-rg%2Fproviders%2Fmicrosoft.insights%2Fcomponents%2Ficr-heat-sensor-ai/source/LogsBlade.AnalyticsShareLinkToQuery/q/H4sIAAAAAAAAA52Rz0oDQQzG7z5F7GkWSqlHhRWE3gVfQLK76XSkMylJtv7Bhzc7td1e9TYk3%252FfLl4kJ9qQ33%252FC%252BIyGwlEkN8wEeASOH%252B%252FXQXJreUowEPRfDVBQWL2ipxIeFS%252BjDqAyAJZJAC8YDj92ewgFF6fVNuQRjNXF96Ec1zhufVTRx0ZVUTtOsqr2ZcbzdTpoj%252FR95QVxhR9lX4K%252F5b0A3z39y2ve2hTWwXMWdKi7SMWeU9OXCY3yatK0%252FwmnN5VR8PltqYw4L3Sd0yQOdL7KEu3oL8QV85FTvdyj2A0nqnczCAQAA/limit/1000
