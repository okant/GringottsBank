# GringottsBank

## A simple online banking API app

This project contains three microservices prepared for Gringotts Bank's DotNet Backend Services:

- BankCustomer
- BankAccount
- Identity

### BankCustomer

With BankCustomer API, user can create an customer, update it, delete it, get it and get all customers with endpoints.

### BankAccount

With BankAccount API, user can create a new account for customer, update it, add money for customer account or withdraw money from customer account with endpoints.

### Identity

With Identity API, user can register to the system for authentication purpose. After registration, user can use jwt token to call BankCustomer and BankAccount APIs.

## Architectural Design

In this project, I have created a common class library which has generic repository class. In this way, every newly created rest api can use this repository with only few configuration settings. (Dependency injection purpose)

And also in this project I have implemented RabbitMQ and MassTransit. In this way, even one of the related services is going down, other microservice will continue its own operations.

In identity section, I have used IdentityServer4, AspnetCore.Identity and AspNetCore.Identity.MongoDbCore. In this way, without adding extra class fields (MongoIdentityUser), I can register an user and with that user token, I can safely call these endpoint in secure way.

For database layer, I have used MongoDB NoSql database. Because it provides low latency, high availability and high scalability.

For DTOs I have used record type rather than class type, because records are simply to declare, immutable as default.

### Keynotes

In Gringotts.Infra folder there is a docker-compose file. In this folder, the following code should be run to serve MongoDB and RabbitMQ local services:

```powershell
docker-compose up -d
```

I have a nuget package in this repository. Therefore, if there is an error to build solutions, `Gringotts.Common` and `Gringotts.BankCustomer.Contracts` may be added to `nuget.config` file manually:

```powershell
dotnet nuget add source {Path-to-Packages-folder}\ -n GringottsLibrary
```

and then in the service project folder you can run this command

```powershell
dotnet add package Gringotts.Common
```

And also `Gringotts.BankCustomer.Service` folder (which has .csproj file)

```powershell
dotnet add reference ..\Gringotts.BankCustomer.Contracts\Gringotts.BankCustomer.Contracts.csproj
```

In Postman for authentication purposes, in authorization tab, some configurations must be added:

- **Access Token**: Available Tokens
- **Header Prefix**: Bearer
- **Grant Type**: Authorization Code(With PKCE)
- **Callback URL**: urn:ietf:wg:oauth:2.0:oob -- It is url of Postman application
- **Auth URL**: https://localhost:5003/connect/authorize
- **AccessToken URL**: https://localhost:5003/connect/token
- **Client ID**: postman
- **Code Challenge Method**: SHA-256
- **Scope**: openid profile customer.fullaccess account.fullaccess IdentityServerApi
- **Client Authentication**: Send as Basic Auth header

Click `Get New Access Token` button and for once register an user, after that you can use created jwt token.
