# 🧈 Daily Dairy Management System (ASP.NET Core MVC)

## 📘 Overview
The **Daily Dairy Management System** is a web application built using **ASP.NET Core MVC** that helps manage daily milk and dairy product transactions for a local dairy shop.  
It allows admins and users to efficiently record sales, purchases, customer data, and manage inventory with an integrated SQL database and Entity Framework Core (DAL layer).

---

## 📦 Quick Start

<div align="center">

```bash
# Clone & Run
git clone https://github.com/your-username/DailyDairy.git
cd DailyDairy/DailyDairyMVCApp
dotnet restore
dotnet run

</div>
## 🏗️ Project Structure

DailyDairy/
│
├── DailyDairyMVCApp/              # ASP.NET Core MVC Frontend
│   ├── Controllers/               # MVC Controllers (Business Logic)
│   ├── Models/                    # View Models / DTOs
│   ├── Views/                     # Razor Views for UI
│   ├── wwwroot/                   # Static assets (CSS, JS, Images)
│   └── appsettings.json           # Configuration (DB Connection, etc.)
│
├── DailyDairy.DAL/                # Data Access Layer (EF Core)
│   ├── Entities/                  # Database Entities
│   ├── Repositories/              # Repositories for CRUD Operations
│   └── DailyDairyDbContext.cs     # EF Core DbContext
│
├── DailyDairyDB.sql               # SQL Script for Database Schema
│
└── README.md                      # Project Documentation

</div>

## 🧩 Features

- 🥛 Manage dairy products, customers, and transactions  
- 🧾 Record daily milk collection and sales  
- 📊 View reports and summaries  
- 🔐 User authentication (Admin/User roles)  
- ⚙️ Entity Framework Core (Code First + SQL Integration)  
- 🎨 MVC architecture for clear separation of layers  

---

## 🛠️ Technologies Used

| Layer | Technology |
|-------|-------------|
| **Frontend (UI)** | ASP.NET Core MVC, Razor Pages, Bootstrap |
| **Backend (API + Logic)** | ASP.NET Core 8.0 |
| **Database** | Microsoft SQL Server |
| **ORM** | Entity Framework Core |
| **Language** | C# |
| **Version Control** | Git & GitHub |
