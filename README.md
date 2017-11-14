# DarkXaHTeP.Extensions.Configuration.Consul

Consul configuration provider implementation for Microsoft.Extensions.Configuration. Allows reading Asp.Net.Core app configuration from Consul Key-Value store

#### NuGet

[![NuGet Version](https://img.shields.io/nuget/v/DarkXaHTeP.Extensions.Configuration.Consul.svg)](https://www.nuget.org/packages/DarkXaHTeP.Extensions.Configuration.Consul/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/DarkXaHTeP.Extensions.Configuration.Consul.svg)](https://www.nuget.org/packages/DarkXaHTeP.Extensions.Configuration.Consul/)

#### Build

[![Travis build](https://img.shields.io/travis/DarkXaHTeP/DarkXaHTeP.Extensions.Configuration.Consul/master.svg)](https://travis-ci.org/DarkXaHTeP/DarkXaHTeP.Extensions.Configuration.Consul)

## Usage

Install package into your project from [NuGet](https://www.nuget.org/packages/DarkXaHTeP.Extensions.Configuration.Consul/) and use `AddConsul` extension method on `IConfigurationBuilder` as shown in examples below:

#### Creating `ConfigurationBuilder` manually

```c#
IConfigurationBuilder builder = new ConfigurationBuilder()
    .AddConsul("ExampleConsulKey");
    
IConfiguration configuration = builder.Build()
```

#### Building Asp.Net Core 2.0 Host

```c#
IWebHost webHost = new WebHostBuilder()
    .UseKestrel()
    .UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config
            .AddJsonFile("appsettings.json", true, true)
            .AddConsul("ExampleConsulKey")
            .AddEnvironmentVariables()
            .AddCommandLine(args);
    })
    .UseStartup<Startup>()
    .Build();
```

#### What happens internally

In both cases Consul configuration provider will make a GET HTTP request to Consul using URL `http://localhost:8500/v1/kv/ExampleConsulKey?recurse=true` to list all sub-keys of `ExampleConsulKey` and add them to configuration dictionary

## Release notes

For release notes please see [CHANGELOG.md](https://github.com/DarkXaHTeP/DarkXaHTeP.Extensions.Configuration.Consul/blob/master/CHANGELOG.md)

## Advanced Usage

#### Providing custom Consul address

In case your Consul agent doesn't run on `http://localhost:8500` it is possible to provide custom Consul address:

1. By specifying additional arguments to "AddConsul" method e.g. `.AddConsul("ExampleConsulKey", "example.com", 9999)`.
    In this case provider will make a request to `http://example.com:9999/v1/kv/ExampleConsulKey?recurse=true` instead of localhost

2. By setting two ENV variables - `CONSUL_HOST` and `CONSUL_PORT` to Consul host and port respectively
    e.g. setting `CONSUL_HOST=example.com` and `CONSUL_PORT=9999` and calling `.AddConsul("ExampleConsulKey")` will give same result as the first case.

Please note that both host and port should be specified to be used.

#### Parsing sub-keys

Let's assume that Consul provider is added like this `.AddConsul("ExampleConsulKey")`and Consul KV Store contains keys listed in the left table column. Corresponding configuration keys, that will be created by provider, are listed on the right:

| Consul KV Store Key                      | Configuration Key            |
|------------------------------------------|------------------------------|
| ExampleConsulKey/key1                    | key1                         |
| ExampleConsulKey/key2                    | key2                         |
| ExampleConsulKey/key3/subkey1            | key3:subkey1                 |
| ExampleConsulKey/key4/subkey1/subsubkey1 | key4:subkey1:subsubkey1      |
| OtherKey                                 |                              |

`OtherKey` will not be included because it is not under `ExampleConsulKey` key.

#### Parsing arrays

Consul provider allows defining arrays by adding item index in square brackets to the Consul KV Store key

| Consul KV StoreKey                | Configuration Key |
|-----------------------------------|-------------------|
| ExampleConsulKey/keys[0]          | keys:0            |
| ExampleConsulKey/keys[1]          | keys:1            |
| ExampleConsulKey/keys2[0]/subkey1 | keys2:0:subkey1   |
| ExampleConsulKey/keys2[0]/subkey2 | keys2:0:subkey2   |
| ExampleConsulKey/keys2[1]/subkey1 | keys2:1:subkey1   |
| ExampleConsulKey/keys2[1]/subkey2 | keys2:1:subkey2   |

#### Creating strongly typed configuration

Consul provider defines settings in a well-known format (colon as delimiter for sub-keys and array indexes) that allows binding to types
using [Configuration Binder](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder/)
and [Options](https://www.nuget.org/packages/Microsoft.Extensions.Options).

For examples of this functionality you may take a look at tests [here](https://github.com/DarkXaHTeP/DarkXaHTeP.Extensions.Configuration.Consul/blob/master/ConsulConfiguration.Test/ConsulConfigurationExtensionTest.cs).
