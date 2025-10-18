# 🧪 Manual Testing Guide - Copy & Paste Ready

**Test URL:** http://localhost:5001/swagger

---

## Step 1: Login (REQUIRED FIRST!)

### 1.1 Expand `POST /api/Auth/login`
### 1.2 Click "Try it out"
### 1.3 Copy this into the Request body:

```json
{
  "username": "admin",
  "password": "admin123"
}
```

### 1.4 Click "Execute"
### 1.5 Copy the `token` from the response
### 1.6 Click the "Authorize" button (top of page)
### 1.7 Paste the token (Swagger will add "Bearer" automatically)
### 1.8 Click "Authorize" then "Close"

**✅ You're now authenticated!**

---

## 🎯 Test All New Modules (Copy-Paste Examples)

### 2. Recruitment Module

#### Create Job Posting
**Endpoint:** `POST /api/Recruitment/jobs`

```json
{
  "title": "Senior Software Engineer",
  "department": "Engineering",
  "description": "Join our growing tech team",
  "requirements": "5+ years .NET experience",
  "salaryMin": 80000,
  "salaryMax": 120000,
  "location": "Remote",
  "jobType": 0,
  "status": 1,
  "positions": 2,
  "postedBy": "admin"
}
```

#### Create Application
**Endpoint:** `POST /api/Recruitment/applications`

```json
{
  "jobPostingId": 1,
  "applicantName": "Jane Smith",
  "email": "jane.smith@example.com",
  "phone": "555-1234",
  "resumePath": "/resumes/jane-smith.pdf",
  "coverLetter": "I am excited to apply for this position",
  "status": 0,
  "reviewedBy": "",
  "notes": ""
}
```

#### Schedule Interview
**Endpoint:** `POST /api/Recruitment/interviews`

```json
{
  "applicationId": 1,
  "applicantName": "Jane Smith",
  "scheduledDate": "2025-10-25T10:00:00",
  "interviewType": "Video",
  "interviewer": "John Manager",
  "location": "Zoom",
  "status": 0,
  "notes": "Technical interview"
}
```

#### View Stats
**Endpoint:** `GET /api/Recruitment/stats`

---

### 3. Attendance Module

#### Clock In
**Endpoint:** `POST /api/Attendance/clock-in`
**Parameters:**
- employeeId: `1`
- location: `Office`

#### Create Holiday
**Endpoint:** `POST /api/Attendance/holidays`

```json
{
  "name": "Christmas Day",
  "date": "2025-12-25",
  "description": "Christmas Holiday",
  "isOptional": false,
  "country": "USA"
}
```

#### Create Timesheet
**Endpoint:** `POST /api/Attendance/timesheets`

```json
{
  "employeeId": 1,
  "weekStart": "2025-10-13",
  "weekEnd": "2025-10-19",
  "entries": [
    {
      "date": "2025-10-14",
      "hours": "08:00:00",
      "project": "Project Alpha",
      "task": "Development",
      "description": "Backend API development"
    },
    {
      "date": "2025-10-15",
      "hours": "08:00:00",
      "project": "Project Alpha",
      "task": "Testing",
      "description": "Unit testing"
    }
  ],
  "status": 0
}
```

#### View Stats
**Endpoint:** `GET /api/Attendance/stats`

---

### 4. Performance Module

#### Create Performance Goal
**Endpoint:** `POST /api/Performance/goals`

```json
{
  "employeeId": 1,
  "title": "Complete Q4 Sales Target",
  "description": "Achieve 100% of quarterly sales target",
  "category": "Revenue",
  "type": 0,
  "targetValue": 100,
  "currentValue": 25,
  "unit": "%",
  "startDate": "2025-10-01",
  "dueDate": "2025-12-31",
  "status": 1,
  "weight": 40,
  "setBy": "admin"
}
```

#### Create Review
**Endpoint:** `POST /api/Performance/reviews`

```json
{
  "employeeId": 1,
  "reviewDate": "2025-10-18",
  "reviewPeriodStart": "Q3 2025",
  "reviewPeriodEnd": "Q3 2025",
  "type": 1,
  "reviewerId": "admin",
  "reviewerName": "Manager Name",
  "criteria": [
    {
      "name": "Quality of Work",
      "description": "Consistently delivers high-quality work",
      "rating": 5,
      "comments": "Excellent performance"
    },
    {
      "name": "Teamwork",
      "description": "Collaborates effectively",
      "rating": 4,
      "comments": "Good team player"
    }
  ],
  "overallRating": 4.5,
  "strengths": "Technical skills, problem-solving",
  "areasForImprovement": "Communication skills",
  "comments": "Strong performer",
  "status": 0
}
```

#### Submit 360 Feedback
**Endpoint:** `POST /api/Performance/feedback`

```json
{
  "employeeId": 1,
  "reviewCycle": 1,
  "reviewerType": "Peer",
  "reviewerId": "2",
  "reviewerName": "John Colleague",
  "questions": [
    {
      "question": "How well does the employee collaborate?",
      "rating": 5,
      "response": "Always willing to help"
    },
    {
      "question": "What are their strengths?",
      "response": "Great technical skills and leadership"
    }
  ],
  "isAnonymous": false
}
```

#### View Stats
**Endpoint:** `GET /api/Performance/stats`

---

### 5. Training Module

#### Create Course
**Endpoint:** `POST /api/Training/courses`

```json
{
  "title": "Leadership Development Program",
  "description": "Advanced leadership and management skills",
  "category": "Management",
  "instructor": "Dr. John Smith",
  "durationHours": 40,
  "type": 0,
  "level": "Advanced",
  "cost": 2500,
  "maxParticipants": 25,
  "prerequisites": ["Basic Management Course"],
  "status": 0
}
```

#### Create Schedule
**Endpoint:** `POST /api/Training/schedules`

```json
{
  "courseId": 1,
  "courseName": "Leadership Development Program",
  "startDate": "2025-11-01",
  "endDate": "2025-11-15",
  "location": "Conference Room A",
  "instructor": "Dr. John Smith",
  "enrolledEmployees": [],
  "availableSeats": 25,
  "status": 0
}
```

#### Enroll Employee
**Endpoint:** `POST /api/Training/enrollments`

```json
{
  "employeeId": 1,
  "scheduleId": 1,
  "status": 0
}
```

#### Add Certification
**Endpoint:** `POST /api/Training/certifications`

```json
{
  "employeeId": 1,
  "name": "AWS Certified Solutions Architect",
  "issuingOrganization": "Amazon Web Services",
  "certificationNumber": "AWS-12345",
  "issueDate": "2025-01-15",
  "expiryDate": "2028-01-15",
  "certificateUrl": "https://aws.amazon.com/verify/12345",
  "isVerified": true,
  "status": 0
}
```

#### Add Skill
**Endpoint:** `POST /api/Training/skills`

```json
{
  "employeeId": 1,
  "skillName": "C# Programming",
  "category": "Technical",
  "proficiency": 3,
  "yearsOfExperience": 5,
  "lastUsed": "2025-10-18",
  "isPrimary": true,
  "verifiedBy": "Manager"
}
```

#### View Stats
**Endpoint:** `GET /api/Training/stats`

---

### 6. Asset Management Module

#### Create Asset
**Endpoint:** `POST /api/Asset`

```json
{
  "assetType": "Laptop",
  "assetTag": "LAP-001",
  "brand": "Dell",
  "model": "XPS 15",
  "serialNumber": "SN123456789",
  "purchaseDate": "2025-01-15",
  "purchaseValue": 1500,
  "status": 0,
  "condition": 0,
  "location": "IT Department",
  "notes": "High performance laptop"
}
```

#### Assign Asset
**Endpoint:** `POST /api/Asset/assign`

```json
{
  "assetId": 1,
  "employeeId": 1,
  "employeeName": "Employee Name",
  "conditionOnAssignment": 1,
  "assignedBy": "admin"
}
```

#### Add Maintenance Record
**Endpoint:** `POST /api/Asset/maintenance`

```json
{
  "assetId": 1,
  "maintenanceDate": "2025-10-18",
  "maintenanceType": "Upgrade",
  "description": "RAM upgrade from 16GB to 32GB",
  "cost": 150,
  "performedBy": "IT Department",
  "vendor": "Dell",
  "nextMaintenanceDate": "2026-04-18"
}
```

#### View Inventory
**Endpoint:** `GET /api/Asset/inventory`

#### View Stats
**Endpoint:** `GET /api/Asset/stats`

---

### 7. Helpdesk Module

#### Create Ticket
**Endpoint:** `POST /api/Helpdesk/tickets`

```json
{
  "employeeId": 1,
  "employeeName": "John Doe",
  "subject": "Payslip Inquiry",
  "description": "I need a copy of my payslip for last month",
  "category": 0,
  "priority": 1,
  "comments": [],
  "attachments": []
}
```

#### Add Comment to Ticket
**Endpoint:** `POST /api/Helpdesk/tickets/{id}/comments`
**Path Parameter:** id = 1

```json
{
  "author": "HR Assistant",
  "comment": "We will send you the payslip by email",
  "isInternal": false
}
```

#### Create Knowledge Base Article
**Endpoint:** `POST /api/Helpdesk/knowledge-base`

```json
{
  "title": "How to Request Leave",
  "content": "Step 1: Login to the system. Step 2: Navigate to Leave Management. Step 3: Click 'Request Leave'. Step 4: Fill in the form with dates and reason. Step 5: Submit for approval.",
  "category": "Leave Management",
  "tags": ["leave", "tutorial", "how-to"],
  "createdBy": "admin",
  "viewCount": 0,
  "isPublished": true
}
```

#### Search Knowledge Base
**Endpoint:** `GET /api/Helpdesk/knowledge-base/search`
**Parameter:** term = `leave`

#### View Stats
**Endpoint:** `GET /api/Helpdesk/stats`

---

### 8. Offboarding Module

#### Submit Resignation
**Endpoint:** `POST /api/Offboarding/resignations`

```json
{
  "employeeId": 1,
  "employeeName": "John Doe",
  "submissionDate": "2025-10-18",
  "lastWorkingDay": "2025-11-18",
  "reason": "Career advancement opportunity",
  "type": 0,
  "status": 0,
  "noticePeriodDays": 30,
  "isServed": false,
  "comments": "Thank you for the opportunity"
}
```

#### Create Exit Interview
**Endpoint:** `POST /api/Offboarding/exit-interviews`

```json
{
  "employeeId": 1,
  "employeeName": "John Doe",
  "interviewDate": "2025-11-15",
  "interviewedBy": "HR Manager",
  "questions": [
    {
      "question": "What prompted you to leave?",
      "response": "Better opportunity elsewhere",
      "rating": 3
    },
    {
      "question": "How satisfied were you with your role?",
      "response": "Generally satisfied",
      "rating": 4
    }
  ],
  "overallSatisfaction": 4,
  "wouldRecommendCompany": true,
  "wouldConsiderReturning": true,
  "additionalFeedback": "Great company culture",
  "status": 0
}
```

#### Initiate Clearance
**Endpoint:** `POST /api/Offboarding/clearances`

```json
{
  "employeeId": 1,
  "employeeName": "John Doe",
  "items": [
    {
      "department": "IT",
      "itemDescription": "Return laptop and access card",
      "isCleared": false
    },
    {
      "department": "HR",
      "itemDescription": "Complete exit paperwork",
      "isCleared": false
    },
    {
      "department": "Finance",
      "itemDescription": "Clear all expenses",
      "isCleared": false
    }
  ],
  "isFullyCleared": false
}
```

#### Calculate Final Settlement
**Endpoint:** `POST /api/Offboarding/settlements`

```json
{
  "employeeId": 1,
  "employeeName": "John Doe",
  "outstandingSalary": 5000,
  "unusedLeaveEncashment": 1500,
  "bonus": 2000,
  "deductions": 500,
  "status": 0,
  "processedBy": "admin"
}
```

#### View Stats
**Endpoint:** `GET /api/Offboarding/stats`

---

## 📊 View All Statistics

Run these to see aggregated data across all modules:

1. `GET /api/Recruitment/stats`
2. `GET /api/Attendance/stats`
3. `GET /api/Performance/stats`
4. `GET /api/Training/stats`
5. `GET /api/Asset/stats`
6. `GET /api/Helpdesk/stats`
7. `GET /api/Offboarding/stats`
8. `GET /api/Employees/stats`
9. `GET /api/Payslips/stats`
10. `GET /api/Leaves/stats`

---

## 🎯 Test Workflow Example

### Complete Employee Lifecycle Test:

1. **Hire:** Create job → Create application → Schedule interview → Create offer
2. **Onboard:** Create onboarding tasks → Assign assets
3. **Manage:** Clock in → Create goals → Enroll in training → Add skills
4. **Exit:** Submit resignation → Schedule exit interview → Initiate clearance → Calculate settlement

---

## ✅ Success Indicators

After testing, check:
- ✅ Data folder created: `EmployeeManagement.Web/Data/`
- ✅ JSON files created for each module
- ✅ Statistics show your test data
- ✅ No errors in console/Swagger

---

**🎉 You now have a fully functional enterprise HRMS with 169 API endpoints!**

**Support Documentation:**
- Full feature list: `NEW_FEATURES_ADDED.md`
- API reference: `API_QUICK_REFERENCE.md`
- Complete checklist: `COMPLETE_FEATURE_LIST.md`
