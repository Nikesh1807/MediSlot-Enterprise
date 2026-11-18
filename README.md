# MediSlot Enterprise 🏥
A full-stack enterprise-grade healthcare appointment and slot booking platform built with **.NET 10** and **React**, designed to handle real-time concurrency, role-based workflows, and seamless patient-doctor interactions.

---

## 🚀 Tech Stack

### **Backend**
* **Framework:** .NET 10, ASP.NET Core Web API
* **Database & ORM:** PostgreSQL, Entity Framework Core (EF Core)
* **Architecture & Security:** RESTful APIs, Role-Based Access Control (RBAC), Database Transactions & Concurrency Handling

### **Frontend**
* **Library:** React.js (Vite)
* **Styling:** Tailwind CSS
* **Icons & UI:** Lucide React / Modern Responsive Components

---

## ✨ Key Features

* **Role-Based Workflows:** Distinct and secure dashboards tailored separately for **Patients** and **Doctors**.
* **Concurrency & Slot Locking:** Prevents double-booking conflicts by implementing temporary time-slot locking with strict expiration timers using database transactions.
* **Real-Time Symptom Feeds:** Patients can attach symptoms and notes during booking, which instantly populate on the assigned doctor's live dashboard.
* **Appointment Management:** Complete lifecycle handling including booking, live status tracking, and cancellation with automated slot release.
* **Robust Data Seeding:** Structured relational database configuration with pre-configured test users and medical specialists.

---

## 🛠️ Getting Started Locally

### Prerequisites
* .NET 10 SDK
* Node.js & npm
* PostgreSQL Server

### 1. Backend Setup
```bash
git clone [https://github.com/Nikesh1807/MediSlot-Enterprise.git](https://github.com/Nikesh1807/MediSlot-Enterprise.git)
cd MediSlot-Enterprise/HealthcareApi
dotnet restore
dotnet ef database update
dotnet run

Frontend Setup 
cd ../healthcare-ui
npm install
npm run dev

👤 Test Credentials
Patient Login: patient@medislot.com / 123456
Doctor Login: doctor@medislot.com / 123456
