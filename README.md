# Lizard

Lizard is a system monitoring service designed for integration with applications running alongside RabbitMQ.

Client applications instantiate `Lizard.Monitor.LizardMessageClient` and send log messages, exception details or 
http logs via RabbitMQ.  Those messages are picked up by the `Lizard` service, listening on the message queue, and
stored in an SQL database where they can be assessed.

## Usage

Install the client package into services which are to report to Lizard using the NuGet package manager:
```
Install-Package Lizard.Monitor
```

Here is an example of usage:
```c#
//Initialise with the RabbitMQ connection settings
LizardMessageClient.Initialise(rabbithost, rabbituser, rabbitpass);

//For an ASP.Net app, you may want to send exceptions via an exception handler
app.UseExceptionHandler(ops =>
{
  ops.Run(async (context) => {
    IExceptionHandlerFeature? feature = context.Features.Get<IExceptionHandlerFeature>();
    if (feature != null)
      await Task.Run(() => LizardMessageClient.Instance.SendException(feature.Error));
  });
});

//You can send updates as required:
LizardMessageClient.Instance.SendLog(new Lizard.Models.LogEntryAddOptions()
{
  Occurred = DateTime.UtcNow,
  Message = "Komodo Dragon!"
});
```

## Basic API Documentation

Details of errors can be retrieved via the HTTP web API.

This is still under construction and is subject to change.

### Get latest errors:

`/log-entry?limit=50&offset=0`

Returns

```json
[
  {
    "occurrenceID" : 123,
    "logEntryID" : 123,
    "message" : "string",
    "occurred" : "2022-01-01 00:00",
    "written" : "2022-01-01 00:00",
    "source" : {
      "name" : "ProgramA",
      "version" : "1.0.0"
    }
  }
]
```

### Get sources

`/source`

Returns

```json
{
  "sourceID": 123,
  "name" : "string",
  "version": "1.0.0"
}
```

### Get log entries for source

`/source/{sourceID:int}/log-entry`

Returns

```json
[
  {
    "occurrenceID" : 123,
    "logEntryID" : 213,
    "message" : "string",
    "occurred" : "2022-01-01 00:00",
    "written" : "2022-01-01 00:00"
  }
]
```

### Get Latest Issues

`/issue?limit=50&offset=0`

Returns

```json
[
  {
    "message" : "string",
    "logEntryID" : 123,
    "lastOccurredID" : 123,
    "lastWrittenID" : 123,
    "numberOfOccurrences" : 234,
    "source" : {
      "sourceID" : 123,
      "name" : "ProgramA",
      "version" : "1.0.0"
    }
  }
]
```

### Get Issue Occurrences

`/issue/{logEntryID:int}/occurrences```

Returns

```json
[
  { "occurred" : "2022-01-01 00:00" },
  { "occurred" : "2022-01-01 02:35" },
  { "occurred" : "2022-01-01 02:42" }
]
```
