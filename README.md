# foodtruacker - Implementation of DDD, CQRS and Event Sourcing

This event-driven project utilizes priciples, frameworks and achitectures — all centered around the idea of enhancing maintainability when dealing with systems reflecting complex business domains. The application's Web API is build upon Microsoft's ASP.NET Core framework and implements Domain-driven Design, as well as the CQRS and Event Sourcing patterns. A fictional business case lays the foundation of this project and is the result of an event storming workshop.

_Please note:_ The fictional business domain introduced to this project is heavily simplified and should only be seen as a provider of relatable use cases.

## Motivation

Since it is not always best to use CRUD-operations and POCO-objects in projects with rather complex business domains, I decided to create this project as a practical implemention of my research on — and interest in — the Domain-driven Design (DDD) approach of developing software.

Since the fictional business case intoduced to this project is heavily event-driven, I decided to also implement the CQRS and Event Sourcing patterns. Both caught my attention while doing the research for this project and go well together with DDD.


## Features

- ASP.NET Core 5.0 Web API application
- Clean Architecture implementation with applying SOLID principles
- Domain-driven Design (DDD)
- xUnit-based Test-driven Design (TDD)
- Hexagonal architecture a.k.a. Ports & Adapters (Domain, Application and Framework Layers)
- CQRS implementation on Commands, Queries and Projections
- MediatR implementation (Request- and Notification-Handling, Pipeline-Behaviour for Logging, Metrics and Authentication)
- Swagger Open API endpoint
- Dockerfile and Docker Compose (YAML) file for environmental setup
- EventStoreDB Repository and custom Client for storing of events
- MongoDB Repository for performance-optimized querying
- Authentication Service based on ASP.NET Core Idenity
- Email Service for account verification
- Shared Kernel class library implementation for DDD & Event Sourcing

## Overview
This project consists of one executable Web API application and several functional components, each provided via class libraries. The code is organized by namespaces. In ASP.NET Core applications created in Visual Studio the namespaces, by default, are automatically created from the projects' folder structure. See the below diagram for a grasp overview of the folder (and namespace) structure of this project:

    .
    ├── foodtruacker.AcceptanceTests
    │   ├── BoundedContexts                     # Tests - organized by bounded context (see Domain-driven Design)
    │   │   ├── UserAccountManagement           # All tests related to user account managment
    │   │   └── ...
    │   └── Framework                           # XUnit- and Moq-based test setup based on given-when-then priciple
    ├── foodtruacker.API
    │   ├── Controllers                         # API Endpoints - request handlers are invoking MediatR Commands and Queries
    │   ├── DTOs                                # DTOs to be used in API requests to manipulate data (commands)
    │   ├── Extensions                          # Bundled services to be registered in ConfigureServices() function in Startup.cs file 
    │   ├── Middleware                          # Middleware functions to be injected into Configure() pipeline in Startup.cs file
    │   ├── appsettings.json                    # Application configuration file used to store configuration settings such as database connections strings
    │   ├── Dockerfile                          # Instructions for building a Docker image from the foodtruacker application
    │   ├── Program.cs                          # Entry point of the application
    │   └── Startup.cs                          # Configuration class used for registering services and injecting middleware into the application pipeline
    ├── foodtruacker.Application
    │   ├── BoundedContexts                     # CQRS logic - organized by bounded context (see Domain-driven Design)
    │   │   ├── UserAccountManagement           # All classes and functions related to user account managment
    │   │   │   ├── Commands                    # MediatR based Commands and Command-handlers - communicating with Event Sourcing database
    │   │   │   ├── EventHandlers               # MediatR based Event-handlers for Notifications - used for triggering external services
    │   │   │   ├── Projections                 # MediatR based Event-handlers for Notifications - used for updating Query database
    │   │   │   ├── Queries                     # MediatR based Queries and Query-handlers - communicating with Query database
    │   │   │   └── QueryObjects                # Response objects used in API requests to read data (queries)
    │   │   └── ...
    │   ├── Pipelines                           # MediatR pipeline behaviours (middleware)
    │   └── Results                             # Wrapper classes and error definitions used for handling commands
    ├── foodtruacker.Authentication
    │   ├── Configuration                       # POCOs for roles, JWT, database communication
    │   ├── Entities                            # User and Role entities required by Identity ASP.NET Core
    │   ├── Exceptions                          # Exceptions related to authentication
    │   ├── Migrations                          # Entity Framework migration scripts for using Identity ASP.NET Core with MySQL-database
    │   ├── Repository                          # DbContext, Repository Interface and corresponding service for Identity ASP.NET Core 
    │   └── Services                            # Interfaces and corresponding services related to authentication (e.g. JWT)
    ├── foodtruacker.Domain
    │   ├── BoundedContexts                     # Domain objects - organized by bounded context (see Domain-driven Design)
    │   │   ├── UserAccountManagement           # All objects and exceptions related to user account managment
    │   │   │   ├── Aggregates                  # Aggregates with internal business logic (see Domain-driven Design)
    │   │   │   ├── Events                      # Domain-related events (see Event Sourcing)
    │   │   │   ├── Exceptions                  # Exceptions related to business logic
    │   │   │   └── ValueObjects                # Value Objects with internal business logic (see Domain-driven Design)
    │   │   └── ...
    │   └── Exceptions                          # General domain and business logic exceptions
    ├── foodtruacker.EmailService
    │   └── Services                            # Interface and corresponding service related to sending emails (dummy - writes output into logs)
    ├── foodtruacker.EventSourcingRepository
    │   ├── Client                              # Interface and corresponding service for reading and appending events from/to EventStoreDB
    │   ├── Configuration                       # POCO for database communication
    │   └── Repository                          # Repository interface and corresponding service for custom Event Sourcing Client (see above)
    ├── foodtruacker.QueryRepository
    │   ├── Configuration                       # POCO for database communication
    │   └── Repository                          # Repository interface and corresponding service for MongoDB
    └── foodtruacker.SharedKernel
        ├── IAggregateRoot.cs                   # Interface for base functionality of an aggregate - used for recreation of aggregates from EventStoreDB (via reflection)
        ├── AggregateRoot.cs                    # Abstract class providing base functionality of an aggregate - inherits IAggregateRoot, is derived by all aggregates
        ├── IDomainEvent.cs                     # Interface for base functionality of an domain-related event - used for recreation of events stored in EventStoreDB (via reflection)
        ├── DomainEvent.cs                      # Abstract class providing base functionality of an domain-related event - inherits IDomainEvent, is derived by all domain-related events
        ├── IValueObject.cs                     # Interface for base functionality of a value object (see Domain-driven Design)
        ├── ValueObject.cs                      # Abstract class providing functionality for comparing value objects - inherits IValueObjects, is derived by all value objects
        └── ...

## Introduction

### Event Storming

A flexible workshop format for collaborative exploration of complex business domains, invented by Alberto Brandolini. It is an extremely lightweight methodology for quickly improving, envisioning, exploring and designing business flows and processes within your organization.

The workshop consists of a group of people with different expertise using colored sticky notes to collaboratively layout relevant business processes. It is mandatory for an EventStorming workshop to both have the right people present and to have enough surface area to place the sticky notes on. Required people typically include those who know the questions to ask (typically developers) and those who know the answers (domain experts, product owners).

The aim of this workshop is to have its participants learn from each other, reveal and refute misconceptions and, e.g. in this GitHub project, do the groundwork for the development of an event based software solution reflecting a correlating business domain.

![Event Storming Illustration](https://github.com/hiiammalte/foodtruacker/blob/master/illustrations/Foodtruacker%20Event%20Storming.jpg?raw=true "Result of Event Storming Session")

### Domain-driven Design (DDD)

An approach to software development that centers the development on programming a domain model that has a rich understanding of the processes and rules of a correlating business domain. The term „Domain-driven Design“ was coined by Eric Evans in his book of the same title.
DDD aims to ease the creation of complex applications and focusses on three core principles:
- Focus on the core domain and business domain logic
- Base complex designs on models of the business domain
- Constantly collaborate with domain experts, in order to iteratively refine the application model

Eric Evans’ book defines a few common terms for Domain-Driven Design:

#### ***Domain Model***
A system of abstractions that describes the processes and policies of a business domain and is used to handle required tasks associated to that domain.

#### ***Ubiquitous Language***
Words and statements for certain elements of the business domain. In order to guard again misconception, all team members should adopt certain terms, typically those used by the domain experts.

#### ***Bounded Context***
A conceptual boundary within which a particular domain model is defined and applicable. This typically represents a subsystem or a field of work. It is mainly a linguistic delimitation, with each bounded context having its own Ubiquitous Language.\
E.g.: Customer Management where a user is called „customer“.

Eric Evans’ book further differentiates certain parts of the domain model. To name a few:

#### ***Entity***
An object which is defined by its identity rather than its attributes.\
E.g.: A person will always remain the same person, no matter the choice of jacket, hair color or language spoken at a certain moment.

#### ***Value Object***
An object which is defined solely by the value of its attributes. Value Objects are immutable and do not have a unique identity. Value Objects can be replaced by other Value Objects with the same attributes.\
E.g.: When focussing on a person, a broken pair of sunglasses can easily be replaced with a new, equally looking pair of sunglasses.

#### ***Aggregate***
A cluster of one or more Entities and optional Value Objects, unified to be a single transactional unit. One Entity will form the base of the Aggregate and is thereby declared *Aggregate root*. All its collaborating Entities’ and Value Objects’ properties may only be accessible through this single base Entity. An Aggregate must always be in a consistent state. In object-oriented programming, this is typically done by using private setters and protected getters.
E.g: In a car sales context, one car (Entity) is defined by its vehicle identification number. This car might have four wheels (Value Objects), which might need to be replaced after a certain time.

#### ***Domain Event***
An object that is created as a result of activity within the Domain Model. It is used to hold and forward information related to this activity. Domain Events are typically created for those activities the domain experts consider relevant.

### Hexagonal architecture (Ports & Adapters)

An architecture pattern used in software design, proposed by Alistair Cockburn in 2005. The pattern aims to achieve a high degree of maintainability and describes an application in three layers. Each layer communicate with the adjacent layer(s) using interfaces (ports) and implementations (adapters):

1. The *Domain Layer* is the inner-most layer. The Domain Model resides here. The Domain Layer defines all the business logic of the application. Everything on how the business logic works is defined in abstract terms, to then be implemented by the adjacent *Application Layer*. Domain-driven Design is a common choice for defining the business logic.
2. The *Application Layer* is the „glue“ between the business logic and the specific details of how the application communicates with the outside world. It adapts requests from the *Framework Layer* to the *Domain Layer* and — in return — translates the received results from the *Domain Layer* into whatever format is expected by the *Framework Layer*. Some additional responsibilities of the *Application Layer* are to e.g. orchestrate the use of the entities found in the *Domain Layer*, to check incoming data for consistency and to dispatch Domain Events raised in the *Domain Layer*.
3. The *Framework Layer* is the outer-most layer. It doesn’t know anything on how the application works but is full of concrete implementation details for all the external interfaces that the application deals with. On the one hand, the *Framework Layer* adapts requests from the outside (primary actors). For example, it might be responsible for accepting HTTP requests. On the other hand, it might also implement services driven by the application (secondary actors). These could be an external Database, third party cloud- or email-services or even other services within the system.

The key rule in this architecture pattern is that dependencies can only point inwards. Nothing in an inner circle can know anything at all about something in an outer circle. Any dependencies willing to pointing outwards, e.g calling a database from the *Application Layer*, need to be instantiated via inversion of Control (IoC) or Dependency Injection (DI).

![Architecture Illustration](https://github.com/hiiammalte/foodtruacker/blob/master/illustrations/Foodtruacker%20Architecture.jpg?raw=true "Hexagonal architecture")

### CQRS using MediatR (a pre-built messaging framework)

CQRS stands for Command/Query Responsibility Segregation and was first described by Greg Young in 2010. It is based upon the Command Query Separation (CQS) principle and allows for separation of read and write operations. CQS states:

1. Commands are utilized to modify the system, but should not return any data.
2. Queries are utilized to read a system's data, but should should not modify state.

The improvement from CQRS over CQS is that those commands and queries are treated as models rather than methods. These models can be dispatched as objects at one point, to then be handled by their required, respective handlers at another point in the system, each returning their response models for clear segregation of each action.

The mediator pattern allows to implement Command/Queries and Handlers loosely coupled, utilizing a mediator object. Objects no longer communicate directly with each other, but instead communicate through the mediator.

The MediatR framework is an open source implementation of the mediator pattern, created by Jimmy Bogard. It will be utilized in this project for communication between the Framework Layer and the Application Layer. It will also be used for projecting data from the Command database to the Query database.

![MediatR Implementation Illustration](https://github.com/hiiammalte/foodtruacker/blob/master/illustrations/Foodtruacker%20MediatR%20CQRS.jpg?raw=true "CQRS using MediatR")

### Event Sourcing

An architectural design pattern for storing every change in the state of an application, rather than  storing just the current state of the data in a domain. This pattern was introduced by Greg Young and has since seen numerous adoptions.

The pattern intends to capture every change to the state of an application as an event object. These event object are then stored, in the sequence of occurrence, in an append only manner. This not only allows for the recreation of the current state of an object over the sequence of events that have happened so far, but ultimately allows to go back in time and recreate the objects state for any given time.

A bank account can be a good example of the Event Sourcing principle. Every time money is withdrawn or deposited, instead of just updating the current balance, the amount of change is recorded. The current balance is then calculated by going over the sequence of events, with their corresponding information on how much money was withdrawn or deposited each time.

Event Sourcing plays well with Domain-driven Design, since it is a great fit for storing Domain Events, triggered by the Domain Model with every change request.

Event Sourcing also greatly benefits from CQRS. Instead of having to make a Query against the Event Sourcing database, which would have to go through all recorded events related to the object being requested in order to recreate the current state, this Query can be made against a dedicated Query Database. This Query Database gets updated by its own event handlers, listening to the same events being dispatched right after being appended to the Event Sourcing Database. These updating processes are called Projections.

This separation of databases also lays ground for a huge potential in scalability and performance optimization. Multiple instances of the Query database can be created and hold in sync simple by having their event handlers listening for the events being dispatched from the Event Sourcing Database Client right after a relevant change to the state of an application occurred. The choice of database type as well as the degree of data-denormalization, optimized per query, can greatly enhance performance.

This constant updating of the read model can either happen synchronously or asynchronously. The latter comes at the cost of eventual consistency, with the read model being out of sync with the write model for a tiny time gap (usually milliseconds).

### Shared Kernel

A common library for the Domain Layer, which contains common Domain-driven Design specific base classes, Domain Entities, Value Objects, etc. which are shared across Bounded Contexts.

## Getting started

To get this project up and running as is, feel free to follow these steps:

### Prerequisites

- Install the .NET 5 or above SDK
- Install Visual Studio 2019 v16.x or above
- Install Docker Desktop

### Setup

1. Clone this repository
2. Make sure Docker Desktop is running
3. At the root directory, restore required packages by running:

```
dotnet restore
```

4. Still at the root directory, start Docker Containers from YAML file by running:

```
docker compose up
```

5. Navigate into the `\foodtruacker.Framework\Authentication` directory and run the following command in the package manager console:

```
dotnet ef database update
```

6. Next, build the solution by running:

```
dotnet build
```

7. Once done, launch the application by running:

```
dotnet run
```

8. Launch https://localhost:5001/swagger/index.html in your browser to view the Swagger documentation of your API.

9. Use Swagger, Postman or any other application to send a POST-request to https://localhost:5001/api/Administration/Register to register your initial admin account. Send the following object:
```
{
    "email": "...",
    "password": "...",
    "firstname": "...",
    "surname": "...",
    "secretProductKey": "12345"
}
```

10. Look into the console application or whichever output is re-configured for the application's logs. After any successful registration of a user, there should be an email-verification link - provided by the EmailService - written into the logs. Copy and paste this url into your browser and hit enter to complete registration. Feel free to change or build upon this unproper implementation of an email service ;-)

11. You are all set. Login next.

12. Launch http://localhost:2113/ in your browser to view the EventStoreDB GUI. Open the "Stream Browser" tab to see all stored events.

## Technologies

This project utilizes the following Technologies / NuGet packages:
- .NET 5
- Docker
- MediatR
- EventStoreDB
- MongoDB
- ASP.NET Core Indentity
- EF Core
- xUnit
- Moq
- FluentAssertions
- Swashbuckle
- Serilog

## Resources / Recommended Reading

Alberto Brandolini:\
https://www.eventstorming.com

Vaughn Vernon:\
https://dddcommunity.org/wp-content/uploads/files/pdf_articles/Vernon_2011_1.pdf \
https://dddcommunity.org/wp-content/uploads/files/pdf_articles/Vernon_2011_2.pdf \
https://dddcommunity.org/wp-content/uploads/files/pdf_articles/Vernon_2011_3.pdf

Alistair Cockburn:\
https://web.archive.org/web/20180822100852/http://alistair.cockburn.us/Hexagonal+architecture

Robert C. Martin (Uncle Bob):\
https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html

Cesar de la Torre, Bill Wagner, Mike Rousos:\
https://docs.microsoft.com/en-us/dotnet/architecture/microservices/

Greg Young\
https://cqrs.files.wordpress.com/2010/11/cqrs_documents.pdf \
https://cqrs.wordpress.com/documents/building-event-storage/ \
https://msdn.microsoft.com/en-us/library/jj591559.aspx

Martin Fowler:\
https://www.martinfowler.com/bliki/CQRS.html

Jimmy Bogard:\
https://github.com/jbogard/MediatR \
https://www.youtube.com/watch?v=SUiWfhAhgQw

Domain-driven Design:\
https://dddcommunity.org \
https://thedomaindrivendesign.io \
https://dotnetcodr.com/2013/09/12/a-model-net-web-service-based-on-domain-driven-design-part-1-introduction/ \
https://dotnetcodr.com/2015/10/22/domain-driven-design-with-web-api-extensions-part-1-notifications/

Hexagonal Architecture:\
https://fideloper.com/hexagonal-architecture \
https://herbertograca.com/2017/09/14/ports-adapters-architecture/

## Credits

http://www.andreavallotti.tech/en/2018/01/event-sourcing-and-cqrs-in-c/ \
https://www.exceptionnotfound.net/real-world-cqrs-es-with-asp-net-and-redis-part-1-overview/ \
https://buildplease.com/pages/fpc-1/ \
https://dotnetcoretutorials.com/2019/04/30/the-mediator-pattern-in-net-core-part-1-whats-a-mediator/ \
https://itnext.io/why-and-how-i-implemented-cqrs-and-mediator-patterns-in-a-microservice-b07034592b6d
