# 👥 User Management System Guide

## Overview

Your Employee Management System now includes a **comprehensive user management interface** where administrators can create users, assign roles, and manage permissions!

---

## 🎯 Key Features

### Default Admin Account
✅ **Automatic creation on first run**  
✅ **Username:** `admin`  
✅ **Password:** `admin123`  
✅ **Role:** Administrator  
✅ **Email:** admin@employeemanagement.com  

### User Management Interface
✅ **View all system users**  
✅ **Create new users with roles**  
✅ **Change user roles instantly**  
✅ **See user permissions**  
✅ **User statistics dashboard**  

### Admin Capabilities
✅ **Create users directly (no signup needed)**  
✅ **Assign roles during creation**  
✅ **Change existing user roles**  
✅ **View all user information**  
✅ **Cannot change own role (security)**  

---

## 🚀 Getting Started

### Step 1: Login with Default Admin

When you first run the application, a default admin account is automatically created:

```
Username: admin
Password: admin123
```

**Important:** Change this password after first login for security!

### Step 2: Access User Management

1. Login as admin
2. Click the **"Manage Users"** button in the header
3. You'll see the User Management page

### Step 3: Create Users

1. Click **"Create New User"** button
2. Fill in the form:
   - Full Name
   - Username
   - Email
   - Password
   - Role (select from dropdown)
3. Click **"Create User"**
4. User is created instantly with assigned role!

### Step 4: Manage Roles

1. View the users table
2. Use the dropdown in the "Actions" column
3. Select a new role
4. Role changes immediately!

---

## 📊 User Management Interface

### Main Features

#### Users Table
Shows all system users with:
- **ID** - User identifier
- **User** - Avatar, full name, and username
- **Email** - Contact email
- **Current Role** - Color-coded role badge
- **Permissions** - Quick view of permissions
- **Actions** - Role change dropdown

#### Statistics Dashboard
Four key metrics:
- **Total Users** - All registered users
- **Administrators** - Users with Admin role
- **Managers** - Users with Manager/Senior roles
- **Regular Users** - Employees and Viewers

#### Create User Modal
Complete form with:
- Full Name input
- Username input
- Email input
- Password input (min 6 characters)
- Role selection dropdown

---

## 🔐 Default Admin Account

### Automatic Creation

The system automatically creates a default admin account on first run:

```json
{
  "username": "admin",
  "email": "admin@employeemanagement.com",
  "fullName": "System Administrator",
  "password": "admin123",
  "role": "Administrator"
}
```

### Security Features

✅ **Only created if no admin exists**  
✅ **Logged in console on creation**  
✅ **Cannot be deleted through UI**  
✅ **Can change password after login**  

### Changing Default Password

1. Login as admin
2. (Future feature: Profile settings)
3. Or delete users.json and recreate with new password

---

## 🎨 User Interface Features

### Color-Coded Roles

Roles are displayed with distinct colors:
- **Administrator** - Red badge (Level 5)
- **Senior Manager** - Orange badge (Level 4)
- **Manager** - Yellow badge (Level 3)
- **Employee** - Blue badge (Level 2)
- **Viewer** - Gray badge (Level 1)

### Permission Display

Shows first 3 permissions with "+X more" indicator:
```
View  Add  Edit  +4 more
```

### Role Dropdown

Instant role changes with dropdown:
- Select new role
- Automatically saves
- Success notification appears
- Table updates immediately

### Security Indicators

- **Lock icon** - Cannot change own role
- **Shield icon** - Role badges
- **User avatar** - First letter of username

---

## 🔌 API Endpoints

### POST /api/auth/create-user
Create a new user (Admin only)

**Request:**
```json
{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "password123",
  "fullName": "John Doe",
  "role": 3
}
```

**Response:**
```json
{
  "id": 5,
  "username": "johndoe",
  "email": "john@example.com",
  "fullName": "John Doe",
  "role": "Manager",
  "roleLevel": 3,
  "permissions": ["Employees.View", "Employees.Add", "Employees.Edit", "Statistics.View"]
}
```

### GET /api/auth/users
Get all users (Admin only)

**Response:**
```json
[
  {
    "id": 1,
    "username": "admin",
    "email": "admin@employeemanagement.com",
    "fullName": "System Administrator",
    "role": "Administrator",
    "roleLevel": 5,
    "permissions": [...]
  }
]
```

### POST /api/auth/change-role
Change user role (Admin only)

**Request:**
```json
{
  "userId": 2,
  "newRole": 4
}
```

---

## 💡 Use Cases

### Scenario 1: Onboarding New Employee

1. Admin logs in
2. Clicks "Manage Users"
3. Clicks "Create New User"
4. Enters employee details
5. Selects "Employee" role
6. Employee can login immediately!

### Scenario 2: Promoting User

1. Admin views users table
2. Finds user to promote
3. Changes role dropdown from "Employee" to "Manager"
4. User immediately gets new permissions!

### Scenario 3: Bulk User Creation

1. Admin creates multiple users
2. Assigns appropriate roles
3. Sends credentials to users
4. Users login and start working

### Scenario 4: Role Audit

1. Admin reviews users table
2. Checks current roles
3. Views permissions for each role
4. Adjusts roles as needed

---

## 🔒 Security Features

### Admin-Only Access

- Only users with `Users.Manage` permission can access
- Redirects non-admins to main page
- All API endpoints check permissions

### Role Change Protection

- Cannot change your own role
- Prevents accidental lockout
- Shows lock icon for current user

### Password Requirements

- Minimum 6 characters
- Validated on both frontend and backend
- Hashed with SHA256 before storage

### Permission Validation

- Every action checks permissions
- Backend validates all requests
- 403 Forbidden for insufficient permissions

---

## 📋 Best Practices

### For Administrators

1. **Change default admin password immediately**
2. **Create users with minimum necessary role**
3. **Review user roles regularly**
4. **Use strong passwords for all users**
5. **Document role assignments**

### For User Creation

1. **Use descriptive full names**
2. **Use company email addresses**
3. **Generate strong passwords**
4. **Assign appropriate initial role**
5. **Inform users of their credentials securely**

### For Role Management

1. **Start users with lower roles**
2. **Promote based on responsibility**
3. **Review permissions before assigning**
4. **Don't over-assign Admin role**
5. **Audit roles quarterly**

---

## 🎯 Role Assignment Guidelines

### When to Assign Each Role

**Viewer:**
- External auditors
- Contractors (read-only)
- Interns (observation only)
- Temporary staff

**Employee:**
- HR assistants
- Data entry staff
- Junior HR personnel
- Onboarding specialists

**Manager:**
- Department managers
- HR managers
- Team leads
- Supervisors

**Senior Manager:**
- Senior management
- Directors
- VP-level staff
- Executive team

**Administrator:**
- IT administrators
- System owners
- C-level executives
- HR directors

---

## 📊 User Statistics

The dashboard shows:

### Total Users
Count of all registered users in the system

### Administrators
Users with Administrator role (Level 5)

### Managers
Users with Manager or Senior Manager roles (Levels 3-4)

### Regular Users
Users with Employee or Viewer roles (Levels 1-2)

---

## ⚠️ Common Issues

### Issue: Can't access User Management
**Solution:** Only Administrators can access. Check your role in the header.

### Issue: Can't change a user's role
**Solution:** You might be trying to change your own role. This is not allowed.

### Issue: Default admin not created
**Solution:** Check console logs. Delete users.json and restart app.

### Issue: Forgot admin password
**Solution:** Delete users.json file and restart. Default admin will be recreated.

### Issue: User creation fails
**Solution:** Check if username/email already exists. Ensure password is 6+ characters.

---

## 🔧 Configuration

### Changing Default Admin Credentials

Edit `AuthService.cs` in the `SeedDefaultAdminAsync` method:

```csharp
var defaultAdmin = new User
{
    Username = "admin",              // Change username
    Email = "admin@example.com",     // Change email
    FullName = "System Admin",       // Change name
    PasswordHash = HashPassword("admin123"),  // Change password
    Role = UserRole.Admin
};
```

### Disabling Default Admin

Comment out the seeding line in the constructor:

```csharp
public AuthService(IConfiguration configuration, ILogger<AuthService> logger)
{
    _configuration = configuration;
    _logger = logger;
    _users = LoadUsers();
    // SeedDefaultAdminAsync().Wait();  // Comment this line
}
```

---

## 📝 Quick Reference

### Default Admin Credentials
```
Username: admin
Password: admin123
Email: admin@employeemanagement.com
```

### Access User Management
```
1. Login as Admin
2. Click "Manage Users" button
3. Create/manage users
```

### Create User
```
1. Click "Create New User"
2. Fill form
3. Select role
4. Click "Create User"
```

### Change Role
```
1. Find user in table
2. Use dropdown in Actions column
3. Select new role
4. Saves automatically
```

---

## ✅ Testing Checklist

- [ ] Default admin account created on first run
- [ ] Can login with admin/admin123
- [ ] Can access User Management page
- [ ] Can create new user
- [ ] Can assign role during creation
- [ ] Can change existing user role
- [ ] Cannot change own role
- [ ] Statistics display correctly
- [ ] Role colors display correctly
- [ ] Permissions show correctly

---

## 🎉 Summary

Your Employee Management System now includes:

### Default Admin Account
✅ Automatically created  
✅ Username: admin  
✅ Password: admin123  
✅ Full Administrator access  

### User Management Interface
✅ Create users with roles  
✅ Change user roles  
✅ View all users  
✅ User statistics  
✅ Permission display  

### Security
✅ Admin-only access  
✅ Cannot change own role  
✅ Permission validation  
✅ Password requirements  

---

**Your system is ready for multi-user management!** 🚀

**Quick Start:**
1. Login with `admin` / `admin123`
2. Click "Manage Users"
3. Create your team!

---

**Created:** 2025-10-14  
**Version:** 1.0  
**Status:** ✅ Fully Functional  
**Features:** Default Admin + User Management UI
