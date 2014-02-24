Serilog.Sinks.ExceptionLess
===========================

Serilog sink to publish to ExceptionLess

```
PM> Install-Package Serilog.Sinks.ExceptionLess
```

Version 1.1.2
-------------
Adjusted the formatting of properties included in the log message

Version 1.1
-----------
Added the ability to add additional ExceptionLess configuration when configuring Serilog

```csharp
Log.Logger = new LoggerConfiguration()
    .Enrich.With(new HttpRequestIdEnricher())
    .WriteTo.Glimpse()
    .WriteTo.ExceptionLess(builder => builder.AddRequestInfo())
    .CreateLogger();
```