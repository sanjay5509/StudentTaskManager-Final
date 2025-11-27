# 📚 StudyBuddy - Professional Task Manager

A complete, modern task management application built using ASP.NET Core MVC. This project showcases proficiency in full-stack development, real-time communication (SignalR), and robust data analytics.

---

## ✨ Key Project Features

This application goes beyond basic CRUD operations by including several advanced, real-time features:

1.  **Real-Time Reminders (SignalR):** Implements an **IHostedService** (a background service/The Watchman) that polls the database every 5 minutes and pushes instant, non-intrusive **Toast Notifications** to the user's screen via SignalR if a task is due soon.
2.  **Advanced Analytics & Reporting:** The "Reports" section calculates and visualizes key performance indicators (KPIs) and data trends:
    * **Task Completion Rate (KPI Card).**
    * **Category Breakdown (Donut Chart).**
    * **Monthly Productivity Trend (Line Chart).**
3.  **Robust Authentication:** Secure user management using **ASP.NET Core Identity**.
4.  **UX Filtering & Search:** Advanced filtering on the "My Tasks" page by Status (Pending, Completed) and Title/Category search.
5.  **Professional UI:** Modern, clean "Toko" inspired Soft UI/UX design.

---

## 🛠️ Technology Stack

* **Backend Framework:** ASP.NET Core MVC (C#)
* **Database:** SQL Server / EF Core (Code-First Migrations)
* **Real-Time:** ASP.NET Core SignalR
* **Frontend:** HTML5, CSS3, Bootstrap, JavaScript, Chart.js (Data Visualization)

---

## ⚙️ Architecture and Setup

### 1. Project Architecture

The application follows the **Model-View-Controller (MVC)** pattern.

* **Controllers:** Handle user input and business logic (e.g., `TaskController`, `ReportsController`).
* **Services:** `ReminderService.cs` (Runs background checks using `IHostedService`).
* **Data Flow:** **LINQ** queries fetch data from EF Core, which is then passed to the Razor Views via the **ViewModel** pattern.

### 2. Local Setup Guide

Follow these steps to get the application running locally:

**Prerequisites:** .NET 8 SDK, Visual Studio, Local SQL Server Instance.

**A. Database Setup (Migrations):**

You must update your database to include the necessary Identity and Task tables. Open the **Package Manager Console** in Visual Studio and run:

```bash
# 1. Database update
Update-Database
