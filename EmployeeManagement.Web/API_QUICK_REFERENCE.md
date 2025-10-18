# 🚀 API Quick Reference Guide

**Base URL:** `http://localhost:5000`  
**Swagger UI:** `http://localhost:5000/swagger`

---

## 🔐 Authentication

All endpoints require JWT authentication. First login to get a token:

```http
POST /api/auth/login
{
  "username": "admin",
  "password": "admin123"
}
```

Copy the token and use it in Swagger by clicking "Authorize" button.

---

## 📋 Complete API Endpoint List

### 1️⃣ **Recruitment Module** (`/api/recruitment`)

#### Job Postings
```http
GET    /api/recruitment/jobs                    # Get all jobs
GET    /api/recruitment/jobs/{id}               # Get specific job
POST   /api/recruitment/jobs                    # Create job posting
PUT    /api/recruitment/jobs/{id}               # Update job
DELETE /api/recruitment/jobs/{id}               # Delete job
```

#### Applications
```http
GET    /api/recruitment/applications            # Get all applications
GET    /api/recruitment/applications/{id}       # Get specific application
POST   /api/recruitment/applications            # Submit application
PUT    /api/recruitment/applications/{id}/status # Update status
```

#### Interviews
```http
GET    /api/recruitment/interviews              # Get all interviews
POST   /api/recruitment/interviews              # Schedule interview
PUT    /api/recruitment/interviews/{id}         # Update interview
```

#### Offers
```http
GET    /api/recruitment/offers                  # Get all offers
POST   /api/recruitment/offers                  # Create offer letter
PUT    /api/recruitment/offers/{id}/status      # Update offer status
```

#### Onboarding
```http
GET    /api/recruitment/onboarding/employee/{employeeId} # Get tasks
POST   /api/recruitment/onboarding              # Create task
PUT    /api/recruitment/onboarding/{id}/complete # Complete task
```

#### Statistics
```http
GET    /api/recruitment/stats                   # Recruitment statistics
```

---

### 2️⃣ **Attendance Module** (`/api/attendance`)

#### Clock In/Out
```http
GET    /api/attendance                          # Get all attendance
GET    /api/attendance/employee/{employeeId}    # Get employee attendance
POST   /api/attendance/clock-in?employeeId={id}&location={loc}
POST   /api/attendance/clock-out?employeeId={id}
```

#### Timesheets
```http
GET    /api/attendance/timesheets               # Get all timesheets
GET    /api/attendance/timesheets/employee/{employeeId}
POST   /api/attendance/timesheets               # Create timesheet
PUT    /api/attendance/timesheets/{id}/submit   # Submit timesheet
PUT    /api/attendance/timesheets/{id}/approve  # Approve timesheet
```

#### Holidays
```http
GET    /api/attendance/holidays                 # Get all holidays
POST   /api/attendance/holidays                 # Create holiday
```

#### Overtime
```http
GET    /api/attendance/overtime                 # Get overtime records
POST   /api/attendance/overtime                 # Create overtime request
PUT    /api/attendance/overtime/{id}/approve    # Approve overtime
```

#### Statistics
```http
GET    /api/attendance/stats?startDate={date}&endDate={date}
```

---

### 3️⃣ **Performance Module** (`/api/performance`)

#### Goals & KPIs
```http
GET    /api/performance/goals                   # Get all goals
GET    /api/performance/goals/employee/{employeeId}
POST   /api/performance/goals                   # Create goal
PUT    /api/performance/goals/{id}              # Update goal
```

#### Reviews
```http
GET    /api/performance/reviews                 # Get all reviews
GET    /api/performance/reviews/employee/{employeeId}
POST   /api/performance/reviews                 # Create review
PUT    /api/performance/reviews/{id}            # Update review
```

#### 360 Feedback
```http
GET    /api/performance/feedback                # Get all feedback
GET    /api/performance/feedback/employee/{employeeId}
POST   /api/performance/feedback                # Submit feedback
```

#### Appraisals
```http
GET    /api/performance/appraisals              # Get all appraisals
GET    /api/performance/appraisals/employee/{employeeId}
POST   /api/performance/appraisals              # Create appraisal
PUT    /api/performance/appraisals/{id}/approve # Approve appraisal
```

#### Statistics
```http
GET    /api/performance/stats
```

---

### 4️⃣ **Training Module** (`/api/training`)

#### Courses
```http
GET    /api/training/courses                    # Get all courses
POST   /api/training/courses                    # Create course
PUT    /api/training/courses/{id}               # Update course
```

#### Schedules
```http
GET    /api/training/schedules                  # Get all schedules
POST   /api/training/schedules                  # Create schedule
```

#### Enrollments
```http
GET    /api/training/enrollments                # Get all enrollments
GET    /api/training/enrollments/employee/{employeeId}
POST   /api/training/enrollments                # Enroll employee
PUT    /api/training/enrollments/{id}/complete  # Complete training
```

#### Certifications
```http
GET    /api/training/certifications             # Get all certifications
GET    /api/training/certifications/employee/{employeeId}
POST   /api/training/certifications             # Add certification
```

#### Skills
```http
GET    /api/training/skills/employee/{employeeId} # Get skills
POST   /api/training/skills                     # Add skill
```

#### Skill Gap Analysis
```http
GET    /api/training/skill-gap/{employeeId}     # Get analysis
POST   /api/training/skill-gap                  # Create analysis
```

#### Statistics
```http
GET    /api/training/stats
```

---

### 5️⃣ **Asset Module** (`/api/asset`)

#### Assets
```http
GET    /api/asset                               # Get all assets
GET    /api/asset/{id}                          # Get specific asset
GET    /api/asset/employee/{employeeId}         # Get employee assets
POST   /api/asset                               # Create asset
PUT    /api/asset/{id}                          # Update asset
DELETE /api/asset/{id}                          # Delete asset
```

#### Assignments
```http
GET    /api/asset/assignments                   # Get all assignments
GET    /api/asset/assignments/employee/{employeeId}
POST   /api/asset/assign                        # Assign asset
POST   /api/asset/{assetId}/return              # Return asset
```

#### Maintenance
```http
GET    /api/asset/{assetId}/maintenance         # Get maintenance history
POST   /api/asset/maintenance                   # Add maintenance record
```

#### Disposal
```http
GET    /api/asset/disposals                     # Get disposals
POST   /api/asset/dispose                       # Dispose asset
```

#### Inventory
```http
GET    /api/asset/inventory                     # Get inventory overview
GET    /api/asset/stats                         # Asset statistics
```

---

### 6️⃣ **Helpdesk Module** (`/api/helpdesk`)

#### Tickets
```http
GET    /api/helpdesk/tickets                    # Get all tickets
GET    /api/helpdesk/tickets/{id}               # Get specific ticket
GET    /api/helpdesk/tickets/employee/{employeeId}
POST   /api/helpdesk/tickets                    # Create ticket
PUT    /api/helpdesk/tickets/{id}/status        # Update status
PUT    /api/helpdesk/tickets/{id}/assign        # Assign ticket
POST   /api/helpdesk/tickets/{id}/comments      # Add comment
PUT    /api/helpdesk/tickets/{id}/resolve       # Resolve ticket
```

#### Knowledge Base
```http
GET    /api/helpdesk/knowledge-base             # Get all articles
GET    /api/helpdesk/knowledge-base/search?term={term}
POST   /api/helpdesk/knowledge-base             # Create article
PUT    /api/helpdesk/knowledge-base/{id}        # Update article
```

#### Statistics
```http
GET    /api/helpdesk/stats
```

---

### 7️⃣ **Offboarding Module** (`/api/offboarding`)

#### Resignations
```http
GET    /api/offboarding/resignations            # Get all resignations
GET    /api/offboarding/resignations/{id}       # Get specific resignation
POST   /api/offboarding/resignations            # Submit resignation
PUT    /api/offboarding/resignations/{id}/status # Update status
```

#### Terminations
```http
GET    /api/offboarding/terminations            # Get all terminations
POST   /api/offboarding/terminations            # Create termination
```

#### Exit Interviews
```http
GET    /api/offboarding/exit-interviews         # Get all interviews
GET    /api/offboarding/exit-interviews/{id}    # Get specific interview
POST   /api/offboarding/exit-interviews         # Create interview
PUT    /api/offboarding/exit-interviews/{id}    # Update interview
```

#### Clearances
```http
GET    /api/offboarding/clearances              # Get all clearances
GET    /api/offboarding/clearances/employee/{employeeId}
POST   /api/offboarding/clearances              # Initiate clearance
PUT    /api/offboarding/clearances/{clearanceId}/items/{itemIndex}
```

#### Settlements
```http
GET    /api/offboarding/settlements             # Get all settlements
GET    /api/offboarding/settlements/employee/{employeeId}
POST   /api/offboarding/settlements             # Calculate settlement
PUT    /api/offboarding/settlements/{id}/pay    # Process payment
```

#### Statistics
```http
GET    /api/offboarding/stats
```

---

### 8️⃣ **Payslips Module** (`/api/payslips`) - Existing

```http
GET    /api/payslips                            # Get all payslips
GET    /api/payslips/{id}                       # Get payslip by ID
GET    /api/payslips/employee/{employeeId}      # Get employee payslips
POST   /api/payslips/generate/{employeeId}      # Generate single payslip
POST   /api/payslips/generate-bulk              # Bulk generate
GET    /api/payslips/{id}/download              # Download PDF
DELETE /api/payslips/{id}                       # Delete payslip
GET    /api/payslips/stats                      # Statistics
```

---

### 9️⃣ **Leaves Module** (`/api/leaves`) - Existing

```http
GET    /api/leaves                              # Get all leaves
GET    /api/leaves/{id}                         # Get leave by ID
GET    /api/leaves/employee/{employeeId}        # Get employee leaves
GET    /api/leaves/pending                      # Get pending requests
POST   /api/leaves/employee/{employeeId}        # Create leave request
POST   /api/leaves/{id}/approve                 # Approve leave
POST   /api/leaves/{id}/reject                  # Reject leave
POST   /api/leaves/{id}/cancel                  # Cancel leave
DELETE /api/leaves/{id}                         # Delete leave
GET    /api/leaves/balance/{employeeId}         # Get balance
GET    /api/leaves/stats                        # Statistics
```

---

### 🔟 **Employees Module** (`/api/employees`) - Enhanced

```http
GET    /api/employees                           # Get all employees
GET    /api/employees/{id}                      # Get employee by ID
POST   /api/employees                           # Create employee
PUT    /api/employees/{id}                      # Update employee
DELETE /api/employees/{id}                      # Delete employee
GET    /api/employees/stats                     # Statistics
GET    /api/employees/search                    # Search/filter
GET    /api/employees/departments               # Get departments
GET    /api/employees/export/csv                # Export CSV
GET    /api/employees/export/excel              # Export Excel
GET    /api/employees/export/pdf                # Export PDF
POST   /api/employees/import/csv                # Import CSV
```

---

### 1️⃣1️⃣ **Authentication Module** (`/api/auth`) - Existing

```http
POST   /api/auth/register                       # Register user
POST   /api/auth/login                          # Login
GET    /api/auth/me                             # Get current user
GET    /api/auth/validate                       # Validate token
GET    /api/auth/users                          # Get all users (Admin)
POST   /api/auth/create-user                    # Create user (Admin)
POST   /api/auth/change-role                    # Change role (Admin)
DELETE /api/auth/delete-user/{id}               # Delete user (Admin)
```

---

## 📊 Statistics Endpoints Summary

All modules provide statistics:

```http
GET /api/recruitment/stats
GET /api/attendance/stats
GET /api/performance/stats
GET /api/training/stats
GET /api/asset/stats
GET /api/helpdesk/stats
GET /api/offboarding/stats
GET /api/payslips/stats
GET /api/leaves/stats
GET /api/employees/stats
```

---

## 🧪 Sample Test Workflow

### 1. Login
```json
POST /api/auth/login
{
  "username": "admin",
  "password": "admin123"
}
```

### 2. Create Job Posting
```json
POST /api/recruitment/jobs
{
  "title": "Senior Software Engineer",
  "department": "Engineering",
  "description": "Looking for experienced developer",
  "requirements": "5+ years experience",
  "salaryMin": 80000,
  "salaryMax": 120000,
  "location": "Remote",
  "jobType": 0,
  "status": 1,
  "positions": 2
}
```

### 3. Clock In
```http
POST /api/attendance/clock-in?employeeId=1001&location=Office
```

### 4. Create Performance Goal
```json
POST /api/performance/goals
{
  "employeeId": 1001,
  "title": "Complete Project X",
  "description": "Deliver project by Q4",
  "type": 0,
  "targetValue": 100,
  "currentValue": 0,
  "unit": "%",
  "startDate": "2025-01-01",
  "dueDate": "2025-12-31",
  "weight": 30,
  "setBy": "Manager"
}
```

### 5. Enroll in Training
```json
POST /api/training/enrollments
{
  "employeeId": 1001,
  "scheduleId": 1
}
```

### 6. Assign Asset
```json
POST /api/asset/assign
{
  "assetId": 1,
  "employeeId": 1001,
  "employeeName": "John Doe",
  "conditionOnAssignment": 1,
  "assignedBy": "Admin"
}
```

### 7. Create Helpdesk Ticket
```json
POST /api/helpdesk/tickets
{
  "employeeId": 1001,
  "employeeName": "John Doe",
  "subject": "Leave Balance Inquiry",
  "description": "Need clarification on leave balance",
  "category": 2,
  "priority": 1
}
```

---

## 📈 Total Endpoint Count

| Module | Endpoints |
|--------|-----------|
| Authentication | 8 |
| Employees | 12 |
| Payslips | 8 |
| Leaves | 11 |
| **Recruitment** | **28** |
| **Attendance** | **17** |
| **Performance** | **19** |
| **Training** | **19** |
| **Asset** | **16** |
| **Helpdesk** | **13** |
| **Offboarding** | **18** |
| **TOTAL** | **169** |

---

## 🎯 Quick Tips

1. **Always authenticate first** - Get JWT token via `/api/auth/login`
2. **Use Swagger UI** - Interactive testing at `http://localhost:5000/swagger`
3. **Check stats endpoints** - Great for testing data flow
4. **Test workflows** - Try complete scenarios (hire → onboard → manage → exit)
5. **Data persists** - All data saved to JSON files in `Data/` folder

---

## 🔍 Troubleshooting

### 401 Unauthorized
- Ensure you've logged in and added the Bearer token
- Token format: `Bearer <your-token-here>`

### 404 Not Found
- Check endpoint URL and parameters
- Ensure entity exists (e.g., employee ID)

### 500 Internal Server Error
- Check server console for error details
- Verify request body matches model structure

---

**Happy Testing! 🚀**

All 169 endpoints are ready for use!
