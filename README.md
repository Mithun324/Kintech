# Kintech

# Kintech E-Commerce Platform

A modern full-stack ASP.NET Core MVC e-commerce web application built with a clean architecture, premium UI/UX, secure authentication, session-based cart management, and admin order management features.

---

# Features

## Customer Features
- Browse products by categories and subcategories
- Premium storefront UI
- Product image support
- Add to cart functionality
- Increase/decrease cart quantity
- Remove items from cart
- Checkout system
- Delivery date & time slot selection
- Order placement system
- Order success confirmation

---

## Admin Features
- Secure admin login
- Product management
- Create/Edit/Delete products
- Category management
- Order management dashboard
- Update order status:
  - Pending
  - Processing
  - Shipped
  - Delivered
  - Cancelled
- Revenue tracking
- Recent orders overview

---

# Technologies Used

## Backend
- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- LINQ
- Dependency Injection
- Session Management

## Frontend
- Razor Views
- HTML5
- CSS3
- Responsive Design
- Premium Custom UI

## Security
- ASP.NET Identity Authentication
- Role-based Authorization
- Anti-Forgery Token Protection
- Secure Session Handling

---

# Project Architecture

The project follows a layered architecture:

Controllers
│
├── Services
│   ├── Interfaces
│   └── Implementations
│
├── Models
│
├── ViewModels
│
├── Data
│
└── Views

---

# Main Functional Modules

## Product Module
Handles:
- Product listing
- Product filtering
- Product creation
- Product editing
- Product stock management

---

## Cart Module
Handles:
- Session-based cart storage
- Quantity updates
- Cart total calculation
- Cart persistence

---

## Checkout Module
Handles:
- Customer information
- Delivery scheduling
- Order creation
- Cart clearing after order

---

## Order Module
Handles:
- Order history
- Order items
- Revenue calculation
- Admin order status updates

---

# Database Models

Main entities used in the project:

- Product
- Category
- SubCategory
- CartItem
- Order
- OrderItem
- DeliverySlot
- ApplicationUser

---

# UI Design

The application uses:
- DM Sans typography
- DM Serif Display headings
- Modern light-mode premium UI
- Responsive product cards
- Interactive cart controls
- Premium admin dashboard styling

---

# Installation Guide

## 1. Clone Repository

```bash
git clone https://github.com/yourusername/kintech.git
```

---

## 2. Open Project

Open the solution in:

Visual Studio 2022

---

## 3. Configure Database

Update your connection string inside:

appsettings.json

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=KintechDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

## 4. Run Migrations

Open Package Manager Console:

```bash
Add-Migration InitialCreate
Update-Database
```

---

## 5. Run Application

Press:

CTRL + F5

or click:

Start Without Debugging

---

# Admin Access

Default admin credentials can be configured inside the seed data or authentication setup.

Example:

Email: admin@kintech.com
Password: Admin123!

---

# Future Improvements

- Payment Gateway Integration
- User Registration & Login
- Product Reviews & Ratings
- Wishlist System
- Email Notifications
- Real-time Order Tracking
- Sales Analytics Dashboard
- Inventory Alerts
- Mobile App Version

---

# Author

### Michah Mithun Saha

Full Stack ASP.NET Core MVC Developer

Technologies:
- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- JavaScript
- React.js
- Node.js

---

# License

This project is created for educational and portfolio purposes.
