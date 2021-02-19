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


## Introduction

### Event Storming

A flexible workshop format for collaborative exploration of complex business domains, invented by Alberto Brandolini. It is an extremely lightweight methodology for quickly improving, envisioning, exploring and designing business flows and processes within your organization.

The workshop consists of a group of people with different expertise using colored sticky notes to collaboratively layout relevant business processes. It is mandatory for an EventStorming workshop to both have the right people present and to have enough surface area to place the sticky notes on. Required people typically include those who know the questions to ask (typically developers) and those who know the answers (domain experts, product owners).

The aim of this workshop is to have its participants learn from each other, reveal and refute misconceptions and, e.g. in this GitHub project, do the groundwork for the development of an event based software solution reflecting a correlating business domain.

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
A conceptual boundary within which a particular domain model is defined and applicable. This typically represents a subsystem or a field of work. It is mainly a linguistic delimitation, with each bounded context having its own Ubiquitous Language.
E.g.: Customer Management where a user is called „customer“ and 

