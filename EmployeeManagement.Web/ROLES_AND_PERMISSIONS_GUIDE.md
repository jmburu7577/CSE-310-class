# 🔐 Role-Based Access Control (RBAC) Guide

## Overview

Your Employee Management System now includes a **comprehensive role-based access control system** with 5 management levels and granular permissions!

---

## 🎯 Management Levels

### 1. **Viewer** (Level 1)
**Entry Level Access**
- ✅ View employees
- ✅ View statistics
- ❌ Cannot add employees
- ❌ Cannot edit employees
- ❌ Cannot delete employees
- ❌ Cannot manage users

**Use Case:** Read-only access for auditors, interns, or external stakeholders.

---

### 2. **Employee** (Level 2)
**Junior Management**
- ✅ View employees
- ✅ View statistics
- ✅ **Add new employees**
- ❌ Cannot edit employees
- ❌ Cannot delete employees
- ❌ Cannot manage users

**Use Case:** HR assistants who can add new hires but cannot modify existing records.

---

### 3. **Manager** (Level 3)
**Mid-Level Management**
- ✅ View employees
- ✅ View statistics
- ✅ Add new employees
- ✅ **Edit existing employees**
- ❌ Cannot delete employees
- ❌ Cannot manage users

**Use Case:** Department managers who can update employee information but cannot remove employees.

---

### 4. **Senior Manager** (Level 4)
**Senior Management**
- ✅ View employees
- ✅ View statistics
- ✅ Add new employees
- ✅ Edit existing employees
- ✅ **Delete employees**
- ❌ Cannot manage users

**Use Case:** Senior managers with full employee management capabilities.

---

### 5. **Administrator** (Level 5)
**Top Level - Full Access**
- ✅ View employees
- ✅ View statistics
- ✅ Add new employees
- ✅ Edit existing employees
- ✅ Delete employees
- ✅ **Manage users**
- ✅ **Assign roles**

**Use Case:** System administrators with complete control over the system and user management.

---

## 🔑 Permissions Matrix

| Permission | Viewer | Employee | Manager | Senior | Admin |
|------------|--------|----------|---------|--------|-------|
| **View Employees** | ✅ | ✅ | ✅ | ✅ | ✅ |
| **View Statistics** | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Add Employees** | ❌ | ✅ | ✅ | ✅ | ✅ |
| **Edit Employees** | ❌ | ❌ | ✅ | ✅ | ✅ |
| **Delete Employees** | ❌ | ❌ | ❌ | ✅ | ✅ |
| **Manage Users** | ❌ | ❌ | ❌ | ❌ | ✅ |
| **Assign Roles** | ❌ | ❌ | ❌ | ❌ | ✅ |

---

## 🚀 How It Works

### First User is Admin

When you create your **first account**, you automatically become an **Administrator** with full access!

All subsequent users start as **Viewers** and must be promoted by an Admin.

### Role Assignment

Only **Administrators** can change user roles:
1. Login as Admin
2. Click **"Manage Users"** button (only visible to Admins)
3. View all users and their current roles
4. Change any user's role (except your own)
5. Changes take effect immediately

### UI Adapts to Permissions

The interface automatically shows/hides features based on your role:
- **Add Employee button** - Only if you have `Employees.Add` permission
- **Edit buttons** - Only if you have `Employees.Edit` permission
- **Delete buttons** - Only if you have `Employees.Delete` permission
- **Manage Users button** - Only if you have `Users.Manage` permission

---

## 📋 Permission Details

### Employees.View
- View employee list (grid and table views)
- View individual employee details
- Access employee information

### Employees.Add
- Click "Add Employee" button
- Fill out new employee form
- Create new employee records

### Employees.Edit
- Click "Edit" button on employee cards
- Modify existing employee information
- Update employee records

### Employees.Delete
- Click "Delete" button on employee cards
- Remove employees from the system
- Permanent deletion (with confirmation)

### Statistics.View
- View dashboard statistics
- See total employees, average salary, average age
- View department counts

### Users.Manage
- View all system users
- See user roles and permissions
- Access user management interface

### Roles.Assign
- Change user roles
- Promote or demote users
- Assign management levels

---

## 🎨 UI Features

### Role Display

Your role is displayed in the header:
```
Welcome,
John Doe
🛡️ Administrator
```

### Conditional Buttons

Buttons appear only if you have permission:
- **Viewer**: No action buttons visible
- **Employee**: Only "Add Employee" button
- **Manager**: "Add Employee" + Edit buttons on cards
- **Senior**: "Add Employee" + Edit + Delete buttons
- **Admin**: All buttons + "Manage Users"

### Permission-Based Views

**Grid View:**
- Cards show only available actions
- No buttons if you're a Viewer
- Edit/Delete buttons appear based on permissions

**Table View:**
- Actions column hidden if no edit/delete permissions
- Individual buttons shown based on permissions

---

## 🔌 API Endpoints

### Role Management Endpoints

#### GET /api/auth/users
Get all users (Admin only)

**Response:**
```json
[
  {
    "id": 1,
    "username": "admin",
    "email": "admin@example.com",
    "fullName": "Admin User",
    "role": "Administrator",
    "roleLevel": 5,
    "permissions": [
      "Employees.View",
      "Employees.Add",
      "Employees.Edit",
      "Employees.Delete",
      "Statistics.View",
      "Users.Manage",
      "Roles.Assign"
    ]
  }
]
```

#### POST /api/auth/change-role
Change user role (Admin only)

**Request:**
```json
{
  "userId": 2,
  "newRole": 3
}
```

**Role Values:**
- 1 = Viewer
- 2 = Employee
- 3 = Manager
- 4 = Senior
- 5 = Admin

#### GET /api/auth/roles
Get available roles

**Response:**
```json
[
  {
    "role": 1,
    "name": "Viewer",
    "description": "Can only view employee information",
    "level": 1,
    "permissions": ["Employees.View", "Statistics.View"]
  },
  ...
]
```

### Protected Employee Endpoints

All employee endpoints now check permissions:
- **GET /api/employees** - Requires `Employees.View`
- **POST /api/employees** - Requires `Employees.Add`
- **PUT /api/employees/{id}** - Requires `Employees.Edit`
- **DELETE /api/employees/{id}** - Requires `Employees.Delete`
- **GET /api/employees/stats** - Requires `Statistics.View`

---

## 🧪 Testing Roles

### Create Test Users

1. **Login as Admin** (first user)
2. **Create additional accounts** (they start as Viewers)
3. **Change their roles** via Manage Users
4. **Logout and login** as different users
5. **Test permissions** for each role

### Test Scenarios

**As Viewer:**
- ✅ Can see employee list
- ✅ Can see statistics
- ❌ No Add/Edit/Delete buttons visible
- ❌ Cannot access user management

**As Employee:**
- ✅ Can see employee list
- ✅ Can add new employees
- ❌ Cannot edit existing employees
- ❌ Cannot delete employees

**As Manager:**
- ✅ Can see employee list
- ✅ Can add new employees
- ✅ Can edit existing employees
- ❌ Cannot delete employees

**As Senior:**
- ✅ Full employee management
- ❌ Cannot manage users

**As Admin:**
- ✅ Everything!

---

## 💾 Data Storage

### User Data with Roles

`users.json` now includes role information:
```json
[
  {
    "id": 1,
    "username": "admin",
    "email": "admin@example.com",
    "passwordHash": "...",
    "fullName": "Admin User",
    "role": 5,
    "createdAt": "2025-10-14T11:30:00Z",
    "lastLogin": "2025-10-14T11:35:00Z"
  }
]
```

**Role Field Values:**
- 1 = Viewer
- 2 = Employee
- 3 = Manager
- 4 = Senior Manager
- 5 = Administrator

---

## 🔒 Security Features

### Permission Checking

**Backend:**
- Every endpoint checks user permissions
- Returns 403 Forbidden if insufficient permissions
- Validates user role from JWT token

**Frontend:**
- UI elements hidden if no permission
- API calls include authentication token
- Auto-redirect on 403 Forbidden

### Role Protection

- Users cannot change their own role
- Only Admins can assign roles
- First user automatically becomes Admin
- Role changes logged in user records

---

## 📊 Use Cases

### Small Company
- **Owner**: Administrator
- **HR Manager**: Senior Manager
- **Department Heads**: Manager
- **HR Assistants**: Employee
- **Accountants**: Viewer

### Large Enterprise
- **IT Admins**: Administrator
- **C-Level Executives**: Senior Manager
- **Department Managers**: Manager
- **HR Staff**: Employee
- **Auditors/Contractors**: Viewer

### Startup
- **Founders**: Administrator
- **Team Leads**: Manager
- **All Staff**: Employee

---

## ⚙️ Configuration

### Changing Default Role

Edit `AuthService.cs` to change the default role for new users:

```csharp
// Current: New users start as Viewer
Role = isFirstUser ? UserRole.Admin : UserRole.Viewer

// Change to Employee:
Role = isFirstUser ? UserRole.Admin : UserRole.Employee
```

### Adding Custom Permissions

1. Add permission to `Permissions` class in `Models/Role.cs`
2. Assign to roles in `RoleHelper.RoleDefinitions`
3. Check permission in controllers
4. Update UI to use permission

---

## 🎯 Best Practices

### For Administrators

1. **Assign appropriate roles** - Give users minimum necessary permissions
2. **Review regularly** - Audit user roles periodically
3. **Document decisions** - Keep track of why users have certain roles
4. **Remove access** - Downgrade or remove users who leave

### For Developers

1. **Always check permissions** - Never trust frontend-only checks
2. **Use permission constants** - Don't hardcode permission strings
3. **Log role changes** - Track who changed what
4. **Test all roles** - Verify each role works correctly

---

## 🚨 Common Issues

### Issue: Can't see Add Employee button
**Solution:** Your role doesn't have `Employees.Add` permission. Contact an Admin to upgrade your role.

### Issue: Getting 403 Forbidden errors
**Solution:** You don't have permission for that action. Check your role and permissions.

### Issue: Can't change user roles
**Solution:** Only Administrators can change roles. You need Admin access.

### Issue: First user isn't Admin
**Solution:** Delete `users.json` and create a new first user. The first user is always Admin.

---

## 📈 Future Enhancements

Possible improvements:
- [ ] Custom roles with specific permission combinations
- [ ] Department-based access control
- [ ] Temporary role assignments
- [ ] Role change history/audit log
- [ ] Bulk role assignments
- [ ] Role templates
- [ ] Permission inheritance
- [ ] Time-based permissions

---

## 📝 Quick Reference

### Role Hierarchy
```
Admin (5) → Full Access
   ↓
Senior (4) → + Delete
   ↓
Manager (3) → + Edit
   ↓
Employee (2) → + Add
   ↓
Viewer (1) → View Only
```

### Permission Codes
```
Employees.View       - View employee list
Employees.Add        - Add new employees
Employees.Edit       - Edit existing employees
Employees.Delete     - Delete employees
Statistics.View      - View statistics
Users.Manage         - View all users
Roles.Assign         - Change user roles
```

---

## ✅ Testing Checklist

- [ ] First user becomes Admin
- [ ] New users start as Viewer
- [ ] Admin can see Manage Users button
- [ ] Admin can change user roles
- [ ] Admin cannot change own role
- [ ] Viewer cannot see action buttons
- [ ] Employee can see Add button only
- [ ] Manager can see Add and Edit buttons
- [ ] Senior can see all action buttons
- [ ] Permissions enforced on backend
- [ ] UI updates based on role
- [ ] 403 errors handled correctly

---

## 🎉 You're All Set!

Your Employee Management System now has enterprise-grade role-based access control!

### Quick Start:
1. ✅ Login as your first user (automatically Admin)
2. 👥 Create additional user accounts
3. 🔐 Assign appropriate roles via Manage Users
4. 🧪 Test different permission levels
5. 🚀 Deploy with confidence!

---

**Created:** 2025-10-14  
**Version:** 1.0  
**Status:** ✅ Fully Functional  
**Security:** Enterprise-Grade RBAC
