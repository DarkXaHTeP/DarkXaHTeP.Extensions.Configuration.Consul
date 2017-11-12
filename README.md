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
    .AddConsul("ConsulKey");
    
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
            .AddConsul("ConsulKey")
            .AddEnvironmentVariables()
            .AddCommandLine(args);
    })
    .UseStartup<Startup>()
    .Build();
```
### What happens internally
In both cases Consul configuration provider will make a GET HTTP Request to Consul ...

## Release notes
For release notes please see [CHANGELOG.md](https://github.com/DarkXaHTeP/DarkXaHTeP.Extensions.Configuration.Consul/blob/master/CHANGELOG.md)

## Advanced Usage

#### Providing custom Consul address

#### Parsing sub-keys

#### Parsing arrays