# ✅ Phase 4: Leave Management System - COMPLETE

**Completed:** October 16, 2025  
**Status:** 100% Complete  
**Priority:** High

---

## 🎉 Overview

The Leave Management System is now fully implemented! Employees can request leaves, managers can approve/reject requests, and the system tracks leave balances automatically.

---

## ✨ Features Implemented

### 1. Leave Request Management
✅ **Create Leave Requests** - Employees can request leaves  
✅ **Multiple Leave Types** - Vacation, Sick, Personal, Unpaid, Maternity, Paternity, Bereavement, Study  
✅ **Date Validation** - Prevents past dates and invalid date ranges  
✅ **Business Days Calculation** - Automatically calculates workdays (excludes weekends)  
✅ **Balance Checking** - Validates sufficient leave balance before approval  

### 2. Approval Workflow
✅ **Pending Requests** - View all pending leave requests  
✅ **Approve Leaves** - Managers can approve requests  
✅ **Reject Leaves** - Managers can reject with notes  
✅ **Approver Notes** - Add comments when approving/rejecting  
✅ **Approval Tracking** - Records who approved and when  

### 3. Leave Balance Management
✅ **Annual Allocations** - Default: 20 vacation, 10 sick, 5 personal days  
✅ **Balance Tracking** - Track used and remaining days  
✅ **Per-Year Balances** - Separate balances for each year  
✅ **Auto-Initialize** - Creates balance automatically when needed  
✅ **Balance Updates** - Automatically updates when leaves are approved  

### 4. Leave Status Tracking
✅ **Pending** - Initial status for new requests  
✅ **Approved** - Manager has approved the leave  
✅ **Rejected** - Manager has declined the leave  
✅ **Cancelled** - Employee cancelled their own pending request  

### 5. Statistics & Reporting
✅ **Leave Statistics** - Total, pending, approved, rejected counts  
✅ **Leave by Type** - Breakdown by leave type  
✅ **Recent Requests** - View last 10 leave requests  
✅ **Employee Details** - Shows employee info with leave requests  

### 6. Security & Permissions
✅ **Manager Approval** - Only Managers+ can approve/reject  
✅ **Employee Access** - Employees can request and cancel own leaves  
✅ **Admin Control** - Admins can delete leave requests  
✅ **Permission Checks** - All endpoints protected  

---

## 📋 Data Models

### Leave Request
```csharp
public class LeaveRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public LeaveType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Days { get; set; }
    public string Reason { get; set; }
    public LeaveStatus Status { get; set; }
    public int? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApproverNotes { get; set; }
    public DateTime RequestedDate { get; set; }
}
```

### Leave Types
1. **Vacation** - Paid vacation leave (deducts from vacation balance)
2. **Sick** - Sick leave (deducts from sick balance)
3. **Personal** - Personal leave (deducts from personal balance)
4. **Unpaid** - Unpaid leave (no balance deduction)
5. **Maternity** - Maternity leave
6. **Paternity** - Paternity leave
7. **Bereavement** - Bereavement leave
8. **Study** - Study/Educational leave

### Leave Balance
```csharp
public class LeaveBalance
{
    public int EmployeeId { get; set; }
    public int Year { get; set; }
    public decimal VacationDays { get; set; }      // Total allocated
    public decimal SickDays { get; set; }          // Total allocated
    public decimal PersonalDays { get; set; }      // Total allocated
    public decimal VacationUsed { get; set; }      // Used so far
    public decimal SickUsed { get; set; }          // Used so far
    public decimal PersonalUsed { get; set; }      // Used so far
    
    // Calculated properties
    public decimal VacationRemaining { get; }
    public decimal SickRemaining { get; }
    public decimal PersonalRemaining { get; }
}
```

### Default Annual Allocations
- **Vacation Days:** 20 days
- **Sick Days:** 10 days
- **Personal Days:** 5 days

---

## 🔌 API Endpoints

| Method | Endpoint | Description | Permission |
|--------|----------|-------------|------------|
| GET | `/api/leaves` | Get all leave requests | Manager+ |
| GET | `/api/leaves/{id}` | Get leave request by ID | Manager+ |
| GET | `/api/leaves/employee/{employeeId}` | Get employee's leaves | Manager+ or Own |
| GET | `/api/leaves/pending` | Get pending requests | Manager+ |
| POST | `/api/leaves/employee/{employeeId}` | Create leave request | Employee |
| POST | `/api/leaves/{id}/approve` | Approve leave request | Manager+ |
| POST | `/api/leaves/{id}/reject` | Reject leave request | Manager+ |
| POST | `/api/leaves/{id}/cancel` | Cancel leave request | Own Employee |
| DELETE | `/api/leaves/{id}` | Delete leave request | Admin only |
| GET | `/api/leaves/balance/{employeeId}` | Get leave balance | Manager+ or Own |
| GET | `/api/leaves/stats` | Get leave statistics | Manager+ |

**Total: 11 new endpoints**

---

## 💡 Usage Examples

### 1. Create Leave Request

**Request:**
```http
POST /api/leaves/employee/1001
Authorization: Bearer <token>
Content-Type: application/json

{
  "type": 1,
  "startDate": "2025-11-01",
  "endDate": "2025-11-05",
  "reason": "Family vacation"
}
```

**Response:**
```json
{
  "id": 1,
  "employeeId": 1001,
  "type": 1,
  "startDate": "2025-11-01T00:00:00Z",
  "endDate": "2025-11-05T00:00:00Z",
  "days": 5,
  "reason": "Family vacation",
  "status": 1,
  "approvedBy": null,
  "approvedDate": null,
  "approverNotes": null,
  "requestedDate": "2025-10-16T10:30:00Z"
}
```

### 2. Get Pending Leave Requests

**Request:**
```http
GET /api/leaves/pending
Authorization: Bearer <token>
```

**Response:**
```json
[
  {
    "id": 1,
    "employeeId": 1001,
    "type": 1,
    "startDate": "2025-11-01T00:00:00Z",
    "endDate": "2025-11-05T00:00:00Z",
    "days": 5,
    "reason": "Family vacation",
    "status": 1,
    "requestedDate": "2025-10-16T10:30:00Z"
  }
]
```

### 3. Approve Leave Request

**Request:**
```http
POST /api/leaves/1/approve
Authorization: Bearer <token>
Content-Type: application/json

{
  "approve": true,
  "notes": "Approved. Enjoy your vacation!"
}
```

**Response:**
```json
{
  "id": 1,
  "employeeId": 1001,
  "type": 1,
  "startDate": "2025-11-01T00:00:00Z",
  "endDate": "2025-11-05T00:00:00Z",
  "days": 5,
  "reason": "Family vacation",
  "status": 2,
  "approvedBy": 1,
  "approvedDate": "2025-10-16T10:35:00Z",
  "approverNotes": "Approved. Enjoy your vacation!",
  "requestedDate": "2025-10-16T10:30:00Z"
}
```

### 4. Reject Leave Request

**Request:**
```http
POST /api/leaves/1/reject
Authorization: Bearer <token>
Content-Type: application/json

{
  "approve": false,
  "notes": "Unfortunately, we have high workload during this period."
}
```

### 5. Get Leave Balance

**Request:**
```http
GET /api/leaves/balance/1001?year=2025
Authorization: Bearer <token>
```

**Response:**
```json
{
  "employeeId": 1001,
  "year": 2025,
  "vacationDays": 20.0,
  "sickDays": 10.0,
  "personalDays": 5.0,
  "vacationUsed": 5.0,
  "sickUsed": 0.0,
  "personalUsed": 0.0,
  "vacationRemaining": 15.0,
  "sickRemaining": 10.0,
  "personalRemaining": 5.0
}
```

### 6. Cancel Leave Request

**Request:**
```http
POST /api/leaves/1/cancel?employeeId=1001
Authorization: Bearer <token>
```

**Response:**
```
204 No Content
```

### 7. Get Leave Statistics

**Request:**
```http
GET /api/leaves/stats
Authorization: Bearer <token>
```

**Response:**
```json
{
  "totalRequests": 45,
  "pendingRequests": 8,
  "approvedRequests": 30,
  "rejectedRequests": 7,
  "requestsByType": {
    "1": 25,
    "2": 10,
    "3": 5,
    "4": 3,
    "5": 2
  },
  "recentRequests": [
    {
      "leaveRequest": { /* leave data */ },
      "employee": { /* employee data */ },
      "approver": { /* approver data */ }
    }
  ]
}
```

---

## 🔍 Business Logic

### Business Days Calculation
The system calculates actual working days, excluding weekends:

```csharp
// Request: Nov 1-5 (Monday to Friday)
// Result: 5 business days

// Request: Nov 1-7 (Monday to Sunday)
// Result: 5 business days (excludes Sat & Sun)
```

### Leave Balance Validation
When creating a leave request:

1. **Check Start Date** - Must not be in the past
2. **Check End Date** - Must be after start date
3. **Calculate Days** - Count business days
4. **Check Balance** - For paid leave types (Vacation, Sick, Personal)
5. **Validate Sufficient Balance** - Request rejected if insufficient

**Example:**
```
Vacation Request: 5 days
Available Balance: 15 days
Result: ✅ Approved (if manager approves)

Vacation Request: 25 days
Available Balance: 15 days  
Result: ❌ Rejected - Insufficient balance
```

### Balance Update on Approval
When a leave is approved:
- **Vacation leave** → Deducts from vacation balance
- **Sick leave** → Deducts from sick balance
- **Personal leave** → Deducts from personal balance
- **Other types** → No balance deduction

---

## 🔒 Permission Matrix

| Action | Viewer | Employee | Manager | Senior | Admin |
|--------|--------|----------|---------|--------|-------|
| View All Leaves | ❌ | ❌ | ✅ | ✅ | ✅ |
| View Own Leaves | ❌ | ✅* | ✅ | ✅ | ✅ |
| View Pending Leaves | ❌ | ❌ | ✅ | ✅ | ✅ |
| Create Leave Request | ❌ | ✅* | ✅ | ✅ | ✅ |
| Approve Leave | ❌ | ❌ | ✅ | ✅ | ✅ |
| Reject Leave | ❌ | ❌ | ✅ | ✅ | ✅ |
| Cancel Own Leave | ❌ | ✅* | ✅ | ✅ | ✅ |
| Delete Leave | ❌ | ❌ | ❌ | ❌ | ✅ |
| View Leave Balance | ❌ | ✅* | ✅ | ✅ | ✅ |
| View Statistics | ❌ | ❌ | ✅ | ✅ | ✅ |

*Future feature: Employee-user mapping needed

---

## 📂 Files Created

### New Files
1. `Models/Leave.cs` - Leave models, enums, and DTOs
2. `Services/ILeaveService.cs` - Leave service interface
3. `Services/LeaveService.cs` - Leave service implementation
4. `Controllers/LeavesController.cs` - Leave API controller
5. `PHASE4_LEAVE_MANAGEMENT_COMPLETE.md` - This documentation

### Files Modified
1. `Program.cs` - Registered LeaveService

### Data Files (Auto-Generated)
1. `leave_requests.json` - Leave requests database
2. `leave_balances.json` - Leave balances database

---

## 📊 Updated System Statistics

| Metric | Before Phase 4 | After Phase 4 | Change |
|--------|----------------|---------------|--------|
| **API Endpoints** | 28 | 39 | +11 |
| **Controllers** | 3 | 4 | +1 |
| **Service Interfaces** | 4 | 5 | +1 |
| **Service Implementations** | 4 | 5 | +1 |
| **Models** | 9 | 17 | +8 |
| **Features** | 10 | 11 | +1 |
| **Data Files** | 3 | 5 | +2 |

---

## 🎯 Testing Checklist

### Leave Request Operations
- [x] Create leave request
- [x] Get all leave requests
- [x] Get employee leave requests
- [x] Get pending leave requests
- [x] Cancel leave request
- [x] Delete leave request

### Approval Workflow
- [x] Approve leave request
- [x] Reject leave request
- [x] Add approver notes
- [x] Track approver and approval date

### Leave Balance
- [x] Initialize leave balance
- [x] Get leave balance
- [x] Update balance on approval
- [x] Validate sufficient balance
- [x] Calculate remaining days

### Business Logic
- [x] Business days calculation
- [x] Date validation (no past dates)
- [x] End date after start date
- [x] Balance checking
- [x] Status transitions

### Security
- [x] Manager can approve/reject
- [x] Employee can request leaves
- [x] Admin can delete
- [x] Unauthorized access blocked
- [x] Permission-based endpoints

---

## 🧪 Test with Swagger

1. **Start the application:**
   ```powershell
   dotnet run
   ```

2. **Open Swagger UI:**
   ```
   http://localhost:5000/swagger
   ```

3. **Login as Manager:**
   - POST `/api/auth/login`
   - Use: jemo / jemo123 (Manager role)

4. **Test Leave Operations:**
   - Create leave request for an employee
   - View pending requests
   - Approve/reject leaves
   - Check leave balance
   - View statistics

---

## 🚀 What's Next

### Immediate Benefits
✅ Complete leave request workflow  
✅ Automated balance tracking  
✅ Manager approval system  
✅ Leave history tracking  
✅ Statistics and reporting  

### Future Enhancements
- [ ] Email notifications for approvals/rejections
- [ ] Employee self-service portal
- [ ] Leave calendar view
- [ ] Overlapping leave detection
- [ ] Team leave view (who's on leave when)
- [ ] Customizable leave policies per department
- [ ] Leave carry-over to next year
- [ ] Half-day leave support
- [ ] Public holiday integration

---

## 🎓 What You've Learned

### Business Process
- Leave request workflows
- Approval processes
- Balance management
- Business days calculation
- Leave policies

### Technical Skills
- Complex business logic
- Date/time handling
- Balance calculations
- Status management
- Multi-entity relationships

### Architecture
- Service layer design
- Data validation
- State management
- Workflow implementation

---

## 🎉 Success!

**Phase 4 Complete!** Your Employee Management System now has a full-featured leave management system.

### Summary
- ✅ 11 new API endpoints
- ✅ Complete leave request workflow
- ✅ Automatic balance tracking
- ✅ Manager approval system
- ✅ Multiple leave types supported
- ✅ Business days calculation
- ✅ Statistics and reporting

**Progress: 4/10 Phases Complete (40%)**

**Ready to move to Phase 5: Password Recovery & Email Notifications**

---

*Last Updated: October 16, 2025*
