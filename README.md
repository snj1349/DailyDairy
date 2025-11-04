# 🧾 Daily Dairy Management System

## 🧠 Overview
**DailyDairyMVCApp** is a complete **ASP.NET Core MVC** web application designed to help users manage their daily dairy shop records efficiently.  
It uses **Entity Framework Core** for database operations, follows the **Repository Pattern**, and supports secure **user authentication** using session-based login.

---

## 🏗️ Project Architecture

This project follows a **3-Layer Architecture**:

### 1️⃣ Database Layer (SQL Server)
- **Database Name:** `DailyDairyDB`
- Created using the SQL script: `DailyDairyDB.sql`
- **Main Tables:**
  - `Users`
  - `DairyProducts`
  - `Sales`
  - `Customers`
  - `Suppliers`
  - `Orders`
  - `Payments`

### 2️⃣ Data Access Layer (EF Core)
- **Project Name:** `DailyDairyDataAccessLayer`
- Built using **Entity Framework Core (EF Core 8.0)**
- Implements the **Repository Pattern** for clean and maintainable code.

**Common Repository Functions:**
```csharp
GetAll()
GetById(int id)
Add(entity)
Update(entity)
Delete(int id)
SearchByName(string name)
GetByDateRange(DateTime start, DateTime end)
