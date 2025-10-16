# ✅ Leave Approval Permissions Update

## 🎯 Changes Made

**Senior employees** can now approve and manage leave requests!

---

## 📋 Updated Permissions

### **Previously:**
- Only **Managers** and **Administrators** could approve leaves
- Minimum role required: `Manager`

### **Now:**
- **Senior**, **Manager**, and **Administrator** roles can approve leaves
- Minimum role required: `Senior`

---

## 🔐 Who Can Do What?

### **All Authenticated Users:**
- ✅ Request leave for employees
- ✅ View their own leave requests
- ✅ Cancel their own pending requests

### **Senior Employees and Above (Senior, Manager, Admin):**
- ✅ View all leave requests
- ✅ View pending leave requests
- ✅ **Approve leave requests** ⭐ NEW!
- ✅ **Reject leave requests** ⭐ NEW!
- ✅ View leave balances for any employee
- ✅ View leave statistics
- ✅ View leave requests by employee

### **Managers and Administrators:**
- Same as Senior (no additional leave permissions)

---

## 📊 Role Hierarchy

```
Administrator (5) ─── Can approve leaves ✅
      │
  Manager (4) ────────  Can approve leaves ✅
      │
  Senior (3) ─────────  Can approve leaves ✅ (NEW!)
      │
 Employee (2) ────────  Can only request leaves
      │
   User (1) ──────────  Basic access
```

---

## 🎯 Updated Endpoints

### **Endpoints Now Accessible to Senior+:**

1. **GET /api/leaves** - Get all leave requests
2. **GET /api/leaves/pending** - Get pending requests
3. **GET /api/leaves/{id}** - Get specific request
4. **GET /api/leaves/employee/{employeeId}** - Get employee's requests
5. **POST /api/leaves/{id}/approve** ⭐ - Approve leave
6. **POST /api/leaves/{id}/reject** ⭐ - Reject leave
7. **GET /api/leaves/balance/{employeeId}** - View balance
8. **GET /api/leaves/stats** - View statistics

### **Still Available to All Users:**
- **POST /api/leaves/employee/{employeeId}** - Request leave
- **POST /api/leaves/{id}/cancel** - Cancel own request

---

## 💼 Use Cases

### **Example: Senior Employee Workflow**

1. **Login** as a Senior employee
2. **Navigate to** Leave Management page
3. **View** all pending leave requests
4. **Approve or Reject** requests with notes
5. **Check** employee leave balances

### **Example: Regular Employee Workflow**

1. **Login** as an Employee
2. **Navigate to** Leave Management page
3. **Request** new leave
4. **View** own leave requests
5. **Wait** for Senior/Manager approval

---

## 🧪 Testing

### **Test Senior Employee Approval:**

1. **Create a Senior user** (if not exists):
   ```json
   {
     "username": "senior1",
     "email": "senior@company.com",
     "password": "senior123",
     "fullName": "Senior Employee",
     "role": "Senior"
   }
   ```

2. **Login as regular user** → **Request leave**

3. **Login as senior1** → **Go to Leaves page**

4. **See the pending request**

5. **Click "Approve"** → Success! ✅

---

## 📝 Code Changes Summary

**File:** `Controllers/LeavesController.cs`

**Changed:** All leave management endpoints
- `UserRole.Manager` → `UserRole.Senior`

**Affected Methods:**
- `GetAllLeaveRequests()`
- `GetPendingLeaveRequests()`
- `GetLeaveRequestById()`
- `GetEmployeeLeaveRequests()`
- `ApproveLeaveRequest()` ⭐
- `RejectLeaveRequest()` ⭐
- `GetLeaveBalance()`
- `GetLeaveStatistics()`

---

## ✅ Benefits

✨ **Distributed Authority** - More people can approve leaves  
✨ **Faster Approvals** - Senior employees can approve without waiting for managers  
✨ **Better Workflow** - Reduces bottlenecks in leave approval process  
✨ **Team Empowerment** - Senior team members have more responsibility  

---

## 🚀 Ready to Use!

The changes are **already applied**! Just:
1. Restart your application (if needed)
2. Login as a Senior employee
3. Start approving leaves!

**Server will pick up changes automatically on restart.** 🎉
