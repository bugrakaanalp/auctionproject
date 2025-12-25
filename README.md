# 🏎️ Nexus Real-Time Luxury Auction Platform

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)
![SignalR](https://img.shields.io/badge/SignalR-RealTime-blue)
![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?logo=docker)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Database-336791?logo=postgresql)

**Nexus Auction** is a high-performance, real-time bidding system built with **Clean Architecture** principles. It allows users to participate in live auctions with instant price updates, spectator modes, and automated auction management using background workers.

## 🌟 Key Features

* **Real-Time Bidding:** Powered by **SignalR**, bid updates are pushed instantly to all connected clients without page refreshes.
* **Spectator Mode:** A responsive, "Vegas-style" UI that syncs live data for thousands of concurrent viewers.
* **Automated Background Workers:** A specialized `BackgroundService` monitors auction deadlines and automatically declares winners when time expires.
* **Interactive UI:** Gamified experience with confetti effects, live countdown timers, and dynamic status updates.
* **Clean Architecture:** Separation of concerns with Domain, Application, Infrastructure, and API layers.
* **Dockerized Database:** Zero-config database setup using Docker containers.

## 🏗️ Architecture

The solution follows the **Clean Architecture** pattern:

* **SilentAuction.Domain:** Enterprise logic and entities (Auction, Bid).
* **SilentAuction.Application:** Interfaces, DTOs, and business rules.
* **SilentAuction.Infrastructure:** Database context (EF Core), repositories, and external services.
* **SilentAuction.API:** RESTful endpoints and SignalR Hubs.

## 🚀 Getting Started

### Prerequisites
* .NET 8.0 SDK
* Docker Desktop (for PostgreSQL)

### Installation

1.  **Clone the repository**
    ```bash
    git clone https://github.com/bugrakaanalp/auctionproject.git
    ```

2.  **Start the Database (Docker)**
    ```bash
    docker run --name silent_auction_db -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=password -e POSTGRES_DB=SilentAuctionDb -p 5432:5432 -d postgres
    ```

3.  **Run the API**
    Open the solution in Visual Studio or run:
    ```bash
    dotnet run --project src/SilentAuction.API
    ```

4.  **Launch the Client**
    Open the `Client/index.html` file in any modern browser to start bidding!

---
*Built by [Buğra Kaan Alp]*
