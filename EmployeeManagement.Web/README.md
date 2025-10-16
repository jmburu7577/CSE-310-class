# 📚 Employee Management System - Complete Documentation

> **A modern, full-featured web application for managing employee records with enterprise-grade authentication and role-based access control.**

---

## 🎯 Overview

The Employee Management System is a comprehensive web application built with **ASP.NET Core 8.0** and **React 18**. It provides secure employee record management with JWT authentication, role-based access control (5 levels), user management, and a beautiful responsive interface.

### ✨ Key Features

#### Core Features (Original)
✅ **Secure Authentication** - JWT token-based authentication with encrypted passwords  
✅ **Role-Based Access Control** - 5 management levels (Viewer, Employee, Manager, Senior, Admin)  
✅ **User Management** - Complete user lifecycle (create, modify roles, delete)  
✅ **Employee CRUD** - Full create, read, update, delete operations  
✅ **Real-time Dashboard** - Live statistics with employee analytics  
✅ **Responsive Design** - Beautiful UI that adapts to all devices  
✅ **RESTful API** - Well-documented API with Swagger integration  
✅ **Data Persistence** - Automatic JSON file-based storage  

#### New Features (Phases 1-4) ✨
✅ **Advanced Search & Filter** - Search by name/dept/ID, filter by department, multi-field sorting  
✅ **Export Data** - Export to CSV, Excel (XLSX), and PDF with professional formatting  
✅ **Import Data** - Bulk import employees from CSV with validation  
✅ **Payslip Management** - Generate, download, and manage employee payslips  
✅ **Payslip PDF** - Professional PDF payslips with automatic calculations  
✅ **Leave Management** - Complete leave request and approval workflow  
✅ **Leave Balance Tracking** - Automatic tracking of vacation, sick, and personal days  
✅ **Manager Approval System** - Streamlined approval/rejection workflow  
✅ **Business Days Calculation** - Smart calculation excluding weekends  
✅ **Comprehensive Statistics** - Analytics for employees, payslips, and leaves  

---

## 🚀 Quick Start

### Prerequisites
- **.NET 8.0 SDK** or later
- **Modern Web Browser** (Chrome, Firefox, Edge, Safari)

### Run in 3 Steps

1. **Open terminal** in the project directory:
   ```powershell
   cd EmployeeManagement.Web
   ```

2. **Start the application**:
   ```powershell
   dotnet run
   ```

3. **Open browser** and navigate to:
   ```
   http://localhost:5000
   ```

### 🔐 Default Admin Accounts

Login with any of these pre-configured administrator accounts:

| Username | Password | Full Name |
|----------|----------|------------|
| **admin** | admin123 | System Administrator |
| **jemo** | jemo123 | Jemo Administrator |
| **testuser** | test123 | Test User Administrator |

**All three accounts have full administrator privileges.**

## 📊 System Statistics (Updated)

| Metric | Value |
|--------|-------|
| **Total API Endpoints** | 39 |
| **Controllers** | 4 |
| **Services** | 5 |
| **C# Models** | 17+ |

## 📁 Project Structure

```
EmployeeManagement.Web/
├── Controllers/
│   ├── AuthController.cs          # Authentication & user management
│   └── EmployeesController.cs     # Employee CRUD operations
├── Models/
│   ├── Employee.cs                # Employee and Address models
│   ├── User.cs                    # User model with roles
│   └── Role.cs                    # Role definitions & permissions
├── Services/
│   ├── IAuthService.cs            # Auth interface
│   ├── AuthService.cs             # Auth implementation
│   ├── IEmployeeService.cs        # Employee interface
│   └── EmployeeService.cs         # Employee implementation
├── wwwroot/
│   ├── index.html                 # Main HTML page
│   └── app.js                     # React application (26KB)
├── Data (auto-generated)/
│   ├── users.json                 # User database
│   └── employees.json             # Employee database
├── Program.cs                     # Application startup
└── appsettings.json              # Configuration
```

## 🎨 User Interface Features

### Authentication Pages
- **Login Page** - Secure credential-based authentication
- **Signup Page** - Self-service user registration
- **Session Management** - Persistent login with LocalStorage
- **Auto-redirect** - Automatic login/logout flows

### Dashboard
- **Total Employees** - Real-time count
- **Average Salary** - Formatted as currency
- **Average Age** - Dynamically calculated
- **Department Breakdown** - Count by department

### Employee Views
- **Grid View** - Beautiful cards with avatars and department badges
- **Table View** - Professional data table with all information
- **Responsive Design** - Adapts to desktop, tablet, mobile

### Role-Based UI
- **Dynamic Elements** - Buttons show/hide based on permissions
- **Viewer** - No action buttons
- **Employee** - Add button only
- **Manager** - Add + Edit buttons
- **Senior** - Add + Edit + Delete buttons
- **Admin** - All buttons + Manage Users

### User Management (Admin Only)
- **User Table** - View all users with roles
- **Create Users** - Add new accounts
- **Change Roles** - Assign management levels
- **Delete Users** - Remove accounts (with protection)

## 🔌 API Endpoints

### Authentication Endpoints (`/api/auth`) - 8 endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register new user | No |
| POST | `/api/auth/login` | Login and get token | No |
| GET | `/api/auth/me` | Get current user info | Yes |
| GET | `/api/auth/validate` | Validate token | Yes |
| GET | `/api/auth/users` | Get all users (Admin) | Yes |
| POST | `/api/auth/create-user` | Create user (Admin) | Yes |
| POST | `/api/auth/change-role` | Change role (Admin) | Yes |
| DELETE | `/api/auth/delete-user/{id}` | Delete user (Admin) | Yes |

### Employee Endpoints (`/api/employees`) - 12 endpoints

| Method | Endpoint | Description | Permission Required |
|--------|----------|-------------|--------------------|
| GET | `/api/employees` | Get all employees | Employees.View |
| GET | `/api/employees/{id}` | Get employee by ID | Employees.View |
| POST | `/api/employees` | Create employee | Employees.Add |
| PUT | `/api/employees/{id}` | Update employee | Employees.Edit |
| DELETE | `/api/employees/{id}` | Delete employee | Employees.Delete |
| GET | `/api/employees/stats` | Get statistics | Statistics.View |
| GET | `/api/employees/search` | Search/filter/sort | Employees.View |
| GET | `/api/employees/departments` | Get departments | Employees.View |
| GET | `/api/employees/export/csv` | Export to CSV | Employees.View |
| GET | `/api/employees/export/excel` | Export to Excel | Employees.View |
| GET | `/api/employees/export/pdf` | Export to PDF | Employees.View |
| POST | `/api/employees/import/csv` | Import from CSV | Employees.Add |

### Payslip Endpoints (`/api/payslips`) - 8 endpoints ✨

| Method | Endpoint | Description | Permission Required |
|--------|----------|-------------|--------------------|
| GET | `/api/payslips` | Get all payslips | Manager+ |
| GET | `/api/payslips/{id}` | Get payslip by ID | Manager+ |
| GET | `/api/payslips/employee/{id}` | Get employee payslips | Manager+ |
| POST | `/api/payslips/generate/{id}` | Generate payslip | Manager+ |
| POST | `/api/payslips/generate-bulk` | Bulk generate | Senior Manager+ |
| GET | `/api/payslips/{id}/download` | Download PDF | Manager+ |
| DELETE | `/api/payslips/{id}` | Delete payslip | Admin |
| GET | `/api/payslips/stats` | Get statistics | Manager+ |

### Leave Endpoints (`/api/leaves`) - 11 endpoints ✨

| Method | Endpoint | Description | Permission Required |
|--------|----------|-------------|--------------------|
| GET | `/api/leaves` | Get all leaves | Manager+ |
| GET | `/api/leaves/{id}` | Get leave by ID | Manager+ |
| GET | `/api/leaves/employee/{id}` | Get employee leaves | Manager+ |
| GET | `/api/leaves/pending` | Get pending leaves | Manager+ |
| POST | `/api/leaves/employee/{id}` | Create leave request | Employee |
| POST | `/api/leaves/{id}/approve` | Approve leave | Manager+ |
| POST | `/api/leaves/{id}/reject` | Reject leave | Manager+ |
| POST | `/api/leaves/{id}/cancel` | Cancel leave | Employee |
| DELETE | `/api/leaves/{id}` | Delete leave | Admin |
| GET | `/api/leaves/balance/{id}` | Get leave balance | Manager+ |
| GET | `/api/leaves/stats` | Get statistics | Manager+ |

### Example API Usage

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}

Response:
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "admin",
    "fullName": "System Administrator",
    "role": 5,
    "permissions": ["Employees.View", "Employees.Add", ...]
  }
}
```

#### Get All Employees (with authentication)
```http
GET /api/employees
Authorization: Bearer <your-jwt-token>
```

#### Create Employee
```http
POST /api/employees
Authorization: Bearer <your-jwt-token>
Content-Type: application/json

{
  "id": 1001,
  "firstName": "John",
  "lastName": "Doe",
  "age": 30,
  "department": "Engineering",
  "salary": 75000,
  "address": {
    "street": "123 Main St",
    "city": "New York",
    "state": "NY",
    "zip": "10001"
  }
}
```

## 💾 Data Storage

### File-Based Persistence

**users.json** - Stores all user accounts
- User credentials (encrypted passwords)
- Role assignments
- Login timestamps
- Auto-created with admin accounts on first run

**employees.json** - Stores all employee records
- Employee information
- Address details
- Auto-saved on every change
- Thread-safe with SemaphoreSlim

### Features
✅ Automatic saving on create/update/delete  
✅ Thread-safe file operations  
✅ Async/await for non-blocking I/O  
✅ Error handling with graceful fallbacks  
✅ In-memory caching for performance

## 🛡️ Role-Based Access Control (RBAC)

### 5 Management Levels

| Level | Role | Add | Edit | Delete | Manage Users |
|-------|------|-----|------|--------|-------------|
| **1** | Viewer | ❌ | ❌ | ❌ | ❌ |
| **2** | Employee | ✅ | ❌ | ❌ | ❌ |
| **3** | Manager | ✅ | ✅ | ❌ | ❌ |
| **4** | Senior Manager | ✅ | ✅ | ✅ | ❌ |
| **5** | Administrator | ✅ | ✅ | ✅ | ✅ |

### Permission System

- **Employees.View** - View employee list and details
- **Employees.Add** - Create new employee records
- **Employees.Edit** - Update existing employees
- **Employees.Delete** - Remove employee records
- **Statistics.View** - Access dashboard statistics
- **Users.Manage** - View and manage users
- **Roles.Assign** - Change user roles

### Role Assignment

Only **Administrators** can change user roles:
1. Login as Admin
2. Click **"Manage Users"**
3. Select new role from dropdown
4. Click **"Change Role"**
5. Changes take effect immediately

### First User Privilege
- First user to register automatically becomes **Administrator**
- All subsequent users start as **Viewer**
- Admins can promote users to higher roles

---

## 🛠️ Development

### Building the Project
```powershell
dotnet build
```

### Running in Development
```powershell
dotnet run
```

### Testing with Swagger
Access interactive API documentation:
```
http://localhost:5000/swagger
```

**Using Protected Endpoints:**
1. Call `/api/auth/login` to get token
2. Copy the token from response
3. Click **"Authorize"** button in Swagger
4. Enter: `Bearer <your-token>`
5. Click **"Authorize"**
6. Now you can test all endpoints!

## 🎯 Technology Stack

### Backend
- **ASP.NET Core 8.0** - Web framework
- **C# 12** - Programming language
- **JWT Bearer** - Authentication
- **Swagger/OpenAPI** - API documentation
- **System.IdentityModel.Tokens.Jwt** - Token generation

### Frontend (CDN-based, no build required)
- **React 18** - UI framework
- **TailwindCSS 3.x** - Utility-first CSS
- **Font Awesome 6.4** - Icons
- **Babel Standalone** - JSX transpilation
- **Inter Font** - Typography

### Architecture Patterns
- **RESTful API** - Resource-based endpoints
- **Dependency Injection** - Service container
- **Repository Pattern** - Data access abstraction
- **Service Layer** - Business logic separation
- **Async/Await** - Asynchronous operations

## 🔒 Security Features

### Authentication Security
- **SHA256 Password Hashing** - Passwords encrypted before storage
- **JWT Tokens** - Secure, stateless authentication
- **7-Day Token Expiration** - Automatic session timeout
- **HMAC SHA256 Signing** - Token integrity verification
- **No Plain Text Passwords** - Never stored or logged

### Authorization Security
- **Backend Permission Checks** - All endpoints validate permissions
- **Frontend UI Hiding** - Elements hidden without permission
- **403 Forbidden** - Insufficient permissions
- **401 Unauthorized** - Missing/invalid token
- **Self-Protection** - Cannot delete or modify own account

### Best Practices
✅ Change default admin passwords  
✅ Use HTTPS in production  
✅ Configure strong JWT secret key  
✅ Implement rate limiting  
✅ Add input sanitization  
✅ Restrict CORS origins  

---

## ✨ Key Features

### Responsive Design
- **Desktop**: 3-column grid layout
- **Tablet**: 2-column layout
- **Mobile**: Single column
- **Smooth Transitions**: CSS animations

### Real-time Updates
- Add → Appears instantly
- Edit → Updates immediately
- Delete → Removed with confirmation
- Statistics → Recalculated automatically

### Data Validation
- Required fields marked with asterisks
- Client & server-side validation
- Unique ID enforcement
- Type checking (numbers, emails)

### User Experience
- Smooth hover animations
- Confirmation dialogs
- Loading states
- Friendly error messages
- Empty state prompts

## 📚 Documentation Files

The project includes comprehensive documentation:

- **README.md** (this file) - Complete system documentation
- **APPLICATION_OVERVIEW.md** - Application features and UI guide
- **AUTHENTICATION_GUIDE.md** - Authentication system details
- **ROLES_AND_PERMISSIONS_GUIDE.md** - RBAC comprehensive guide
- **ADMIN_ACCOUNTS.md** - Default admin account information
- **MIGRATION_GUIDE.md** - Console to web migration guide
- **START_HERE.md** - Quick start guide
- **TROUBLESHOOTING.md** - Common issues and solutions

## 📝 What's Implemented

### ✅ Authentication & Authorization
- [x] JWT token authentication
- [x] User registration and login
- [x] Password hashing (SHA256)
- [x] Role-based access control (5 levels)
- [x] Permission-based UI
- [x] Session management

### ✅ User Management
- [x] View all users
- [x] Create new users
- [x] Change user roles
- [x] Delete users
- [x] Three default admin accounts
- [x] First user as admin

### ✅ Employee Management
- [x] Create employees
- [x] Read/view employees
- [x] Update employees
- [x] Delete employees
- [x] Real-time statistics
- [x] Grid and table views

### ✅ UI/UX
- [x] Responsive design
- [x] Beautiful gradient styling
- [x] Smooth animations
- [x] Modal forms
- [x] Confirmation dialogs
- [x] Empty states

### 🔮 Future Enhancements

Potential improvements:
- [ ] Search and filter functionality
- [ ] Sorting and pagination
- [ ] Export to CSV/Excel
- [ ] Employee photos
- [ ] Email notifications
- [ ] Password recovery
- [ ] Two-factor authentication
- [ ] Audit logs
- [ ] Database integration (SQL)

## 🐛 Troubleshooting

### Cannot Login
**Issue**: "Invalid username or password"  
**Solution**: 
- Use default accounts: admin/admin123, jemo/jemo123, or testuser/test123
- Check username spelling (case-insensitive)
- Ensure password is correct

### Port Already in Use
**Issue**: Port 5000 is occupied  
**Solution**: 
- Close other applications using port 5000
- Or modify `Properties/launchSettings.json`:
  ```json
  "applicationUrl": "http://localhost:5001"
  ```

### Missing Permissions
**Issue**: Cannot see Add/Edit/Delete buttons  
**Solution**: 
- Check your role (shown in header)
- Contact admin to upgrade your role
- Login as admin to test all features

### Data Not Persisting
**Issue**: Changes disappear after restart  
**Solution**: 
- Check write permissions in project directory
- Verify `users.json` and `employees.json` exist
- Check console for file I/O errors

### API 401 Unauthorized
**Issue**: API returns 401 error  
**Solution**: 
- Login to get a valid token
- Check token hasn't expired (7 days)
- Include Authorization header: `Bearer <token>`

### API 403 Forbidden
**Issue**: API returns 403 error  
**Solution**: 
- Your role lacks required permission
- Contact admin for role upgrade
- Check ROLES_AND_PERMISSIONS_GUIDE.md for details

## 🎓 Learning Outcomes

This project demonstrates:

### Backend Skills
- ASP.NET Core Web API development
- RESTful API design
- JWT authentication implementation
- Role-based authorization
- Dependency injection
- Async/await patterns
- Error handling
- File I/O operations

### Frontend Skills
- React component development
- State management (useState, useEffect)
- API integration (Fetch API)
- Responsive design
- CSS frameworks (TailwindCSS)
- Form handling and validation
- Client-side routing

### Architecture Skills
- MVC/MVVM patterns
- Service layer architecture
- Repository pattern
- Separation of concerns
- Security best practices

---

## 📄 License

This project is for educational purposes as part of CSE-310 coursework.

## 👨‍💻 Credits

Created by: **BYU Student**  
Course: **CSE-310 - Software Development**  
Date: **October 2025**

---

## 🎉 Summary

You now have a **production-ready employee management system** with:

✅ **3 default admin accounts** ready to use  
✅ **5-level role hierarchy** for granular access control  
✅ **Complete CRUD operations** for employees  
✅ **User management system** for admin users  
✅ **Secure JWT authentication** with encrypted passwords  
✅ **Beautiful responsive UI** that works on all devices  
✅ **RESTful API** with Swagger documentation  
✅ **Comprehensive documentation** for all features  

### Quick Links
- **Application**: http://localhost:5000
- **API Docs**: http://localhost:5000/swagger
- **Default Login**: admin / admin123

**Enjoy managing your employees with enterprise-grade security!** 🚀
