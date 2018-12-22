# README

## Abstract

This solution provides various health checks as nuget packages. The packages require `dotnetstandard2.0` or later.

#### Core

This package defines the health check results & service layer for various health checks to plug into and report findings.

Each health check has an option to provide useful description and vary the connect timeout (in seconds).
The connect timeout drives whether the health check operation passes or fails.
The health check will abort when connect timeout elapses prior to operation completing and report "Fail". It *might* provide a helpful error message, too.

#### Available Health Checks

* [HttpEndpoint](#http)
* [RedisConnection](#redis)
* [SqlConnection](#sql)
* [MySqlConnection](#maria)

<a name="http"></a>
#### HttpEndpoint

This health check module can be used to verify uptime of an arbitrary http `GET` endpoint.
It expects to receive `HTTP/200` `OK` within the connect timeout interval.
For efficiency, the health check stops short of other processing once headers are received.

>> Example Use

Add health check on postman echo delay endpoint. To install http endpoint check, do the following:

1. Open package manager console & install package:

`PM>` ``` Install-Package Cerberix.HealthCheck.HttpEndpoint -Version 1.0.0 ```

2. Add the following in your health check service injector:

```cs
new Cerberix.HealthCheck.HttpEndpoint.HttpGetEndpointHealthCheck(
  description: "http endpoint health check (postman-echo)",
  endpoint: "https://postman-echo.com/delay/2",
  connectTimeout: 5
),
```

Here's an example report formatted as `application/json`:

```json
[
  {
    name: "HttpGetEndpointHealthCheck",
    description: "http endpoint health check (postman-echo)",
    error: null,
    status: "Pass"
  },
]
```

**Note:** In the above example, if the endpoint were down, or too slow to respond, then the response would be similar to below:

```json
[
  {
    name: "HttpGetEndpointHealthCheck",
    description: "http endpoint health check (postman-echo)",
    error: "The operation timed out.",
    status: "Fail"
  }
]
```

<a name="redis"></a>
### Redis Connection

This health check module can be used to verify Redis connection up & responsive.
It expects a successful connection response within the connect timeout interval.
It also supports an option to retry connection some arbitrary number of times.
For efficiency, the health check opens connection, and stops short of getting or setting data.

>> Example Use

Add health check on local Redis connection. To install redis connection check, do the following:

1. Open package manager console & install package:

`PM>` ``` Install-Package Cerberix.HealthCheck.RedisConnection -Version 1.0.0 ```

2. Add the following in your health check service injector:

```cs
new Cerberix.HealthCheck.RedisConnection.RedisConnectionHealthCheck(
  description: "redis connection health check (localhost:6379)",
  connectionString: "localhost:6379",
  connectTimeout: 5,
  connectRetry: 3
),
```
Here's an example report formatted as `application/json`:

```json
[
  {
    name: "RedisConnectionHealthCheck",
    description: "redis connection health check (localhost:6379)",
    error: null,
    status: "Pass"
  }
]
```

**Note:** In the above example, if the connection were down or too slow to respond, then the response would be similar to below:
```json
[
  {
    name: "RedisConnectionHealthCheck",
    description: "redis connection health check (localhost:6379)",
    error: "The operation timed out.",
    status: "Fail"
  }
]
```

<a name="sql"></a>
### Microsoft SQL Server Connection

This health check module provides a mechanism to verify sql connection.
It expects connection to be up & responsive within the connect timeout given.
For efficiency, the health check verifies connection, and gets the current sql server datetime. It stops short of getting or setting data.

>> Example Use

Add health check on local sql connection. For instructions on installing SQL server locally [See footnote](#1) 
To install sql connection check, do the following:

1. Open package manager console & install package:

`PM>` ``` Install-Package Cerberix.HealthCheck.SqlConnection -Version 1.0.0 ```

2. Add the following in your health check service injector:

```cs
new Cerberix.HealthCheck.RedisConnection.RedisConnectionHealthCheck(
  description: "redis connection health check (localhost:6379)",
  connectionString: "localhost:6379",
  connectTimeout: 5,
  connectRetry: 3
),
```

Here's an example report formatted as `application/json`:

```json
[
  {
    name: "SqlConnectionHealthCheck",
    description: "dockerized sql server connection (localhost:1433)",
    error: null,
    status: "Pass"
  }
]
```

**Note:** In the above example, if the connection failed due to invalid credentials, then the response would be similar to below:

```json
[
  {
    name: "SqlConnectionHealthCheck",
    description: "dockerized sql server connection (localhost:1433)",
    error: "Login failed for user 'sa'.",
    status: "Fail"
  }
]
```

<a name="maria"></a>
### MySql (MariaDB) Server Connection

This health check module provides a mechanism to verify mysql connection. It's also compatible w/ mariadb.
It expects connection to be up & responsive within the connect timeout given.
For efficiency, the health check verifies connection, and gets the current mysql server datetime. It stops short of getting or setting data.

>> Example Use

Add health check on local mysql connection.

For instructions on installing MySQL server locally using docker [See footnote](#2).
For instructions on installing MariaDB server locally using docker [See footnote](#3).

To install sql connection check, do the following:

1. Open package manager console & install package:

`PM>` ``` Install-Package Cerberix.HealthCheck.SqlConnection -Version 1.0.0 ```

2. Add the following in your health check service injector:

```cs
new Cerberix.HealthCheck.RedisConnection.RedisConnectionHealthCheck(
  description: "redis connection health check (localhost:6379)",
  connectionString: "localhost:6379",
  connectTimeout: 5,
  connectRetry: 3
),
```

Here's an example report formatted as `application/json`:

```json
[
  {
    name: "MySqlConnectionHealthCheck",
    description: "dockerized mysql server connection (localhost:3306)",
    error: null,
    status: "Pass"
  }
]
```

**Note:** In the above example, if the connection failed due to host being offline, then the response would be similar to below:

```json
[
  {
    name: "MySqlConnectionHealthCheck",
    description: "dockerized mysql server connection (localhost:3306)",
    error: "Unable to connect to any of the specified MySQL hosts.",
    status: "Fail"
  }
]
```

### Footnotes 

<a name="1"></a>
[1] Setting up a SQL connection is outside the scope of this README. To setup using `docker`, follow this [link](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-2017).
<a name="2"></a>
[2] Setting up a MySQL connection is outside the scope of this README. To setup using `docker`, follow this [link](https://hub.docker.com/_/mysql/).
<a name="3"></a>
[3] Setting up a MariaDB connection is outside the scope of this README. To setup using `docker`, follow this [link](https://hub.docker.com/_/mariadb).
