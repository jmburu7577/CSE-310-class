# 🚀 Quick Start Guide

**Welcome to your Complete HR Management System!**

---

## ⚡ 3-Step Startup

### Step 1: Navigate to Project
```powershell
cd c:\Users\BYU\Desktop\CSE-310-class-1\EmployeeManagement.Web
```

### Step 2: Run Application
```powershell
dotnet run
```

### Step 3: Open Swagger
Open your browser: **http://localhost:5000/swagger**

---

## 🔐 Login Credentials

| Username | Password | Role |
|----------|----------|------|
| admin | admin123 | Administrator |
| jemo | jemo123 | Administrator |
| testuser | test123 | Administrator |

---

## 🧪 Test Your New Features (5 Minutes)

### 1️⃣ Authenticate
```http
POST /api/auth/login
{
  "username": "admin",
  "password": "admin123"
}
```
**Copy the token** → Click "Authorize" button → Paste token

---

### 2️⃣ Create a Job Posting
```http
POST /api/recruitment/jobs
{
  "title": "Software Developer",
  "department": "Engineering",
  "description": "Join our team!",
  "requirements": "3+ years experience",
  "salaryMin": 60000,
  "salaryMax": 90000,
  "location": "Remote",
  "jobType": 0,
  "status": 1,
  "positions": 1,
  "postedBy": "admin"
}
```

---

### 3️⃣ Clock In an Employee
```http
POST /api/attendance/clock-in?employeeId=1001&location=Office
```

---

### 4️⃣ Create Performance Goal
```http
POST /api/performance/goals
{
  "employeeId": 1001,
  "title": "Q4 Sales Target",
  "description": "Achieve 100% of target",
  "category": "Individual",
  "type": 0,
  "targetValue": 100,
  "currentValue": 0,
  "unit": "%",
  "startDate": "2025-10-01",
  "dueDate": "2025-12-31",
  "weight": 40,
  "setBy": "admin"
}
```

---

### 5️⃣ Create Training Course
```http
POST /api/training/courses
{
  "title": "Leadership Training",
  "description": "Advanced leadership skills",
  "category": "Management",
  "instructor": "John Smith",
  "durationHours": 40,
  "type": 0,
  "level": "Intermediate",
  "cost": 1500,
  "maxParticipants": 20,
  "status": 0
}
```

---

### 6️⃣ Assign Company Asset
```http
POST /api/asset/assign
{
  "assetId": 1,
  "employeeId": 1001,
  "employeeName": "Employee Name",
  "conditionOnAssignment": 1,
  "assignedBy": "admin"
}
```

---

### 7️⃣ Create HR Ticket
```http
POST /api/helpdesk/tickets
{
  "employeeId": 1001,
  "employeeName": "Employee Name",
  "subject": "Payslip Inquiry",
  "description": "Need payslip for last month",
  "category": 0,
  "priority": 1
}
```

---

### 8️⃣ View Statistics
Try any of these:
```http
GET /api/recruitment/stats
GET /api/attendance/stats
GET /api/performance/stats
GET /api/training/stats
GET /api/asset/stats
GET /api/helpdesk/stats
```

---

## 📊 What You Have

### 11 Functional Modules
1. ✅ **Recruitment** - Job postings, applications, interviews, offers
2. ✅ **Onboarding** - Task checklists and tracking
3. ✅ **Attendance** - Clock in/out, timesheets, holidays
4. ✅ **Leave Management** - 8 leave types with approval
5. ✅ **Payroll** - Payslip generation and distribution
6. ✅ **Performance** - Goals, KPIs, reviews, appraisals
7. ✅ **Training** - Courses, certifications, skills
8. ✅ **Assets** - Laptop, phone, equipment tracking
9. ✅ **Helpdesk** - Ticketing system and knowledge base
10. ✅ **Offboarding** - Resignations, exit interviews, settlements
11. ✅ **Analytics** - Comprehensive reporting

### 169 API Endpoints
- 28 Recruitment endpoints
- 17 Attendance endpoints
- 19 Performance endpoints
- 19 Training endpoints
- 16 Asset endpoints
- 13 Helpdesk endpoints
- 18 Offboarding endpoints
- Plus existing Employee, Auth, Payslip, Leave endpoints

---

## 📁 Data Storage

All data saved to JSON files in:
```
EmployeeManagement.Web/Data/
```

Files created automatically on first use:
- job_postings.json
- applications.json
- interviews.json
- attendance.json
- timesheets.json
- performance_goals.json
- training_courses.json
- company_assets.json
- hr_tickets.json
- resignations.json
- And 20+ more...

---

## 📖 Documentation

### Full Documentation
- **NEW_FEATURES_ADDED.md** - Complete feature overview
- **API_QUICK_REFERENCE.md** - All 169 endpoints
- **COMPLETE_FEATURE_LIST.md** - Feature checklist
- **FINAL_SUMMARY.md** - Previous implementation summary
- **README.md** - System documentation

### View in Browser
- **Swagger UI:** http://localhost:5000/swagger
- **Web App:** http://localhost:5000

---

## 🎯 Common Workflows

### Hire an Employee
1. Create job posting → `/api/recruitment/jobs`
2. Receive applications → `/api/recruitment/applications`
3. Schedule interviews → `/api/recruitment/interviews`
4. Send offer → `/api/recruitment/offers`
5. Create onboarding tasks → `/api/recruitment/onboarding`
6. Add employee → `/api/employees`

### Manage Performance
1. Set goals → `/api/performance/goals`
2. Track progress → Update goal currentValue
3. Conduct review → `/api/performance/reviews`
4. Gather 360 feedback → `/api/performance/feedback`
5. Process appraisal → `/api/performance/appraisals`

### Track Time
1. Clock in → `/api/attendance/clock-in`
2. Work...
3. Clock out → `/api/attendance/clock-out`
4. Submit timesheet → `/api/attendance/timesheets`
5. Manager approves → `/api/attendance/timesheets/{id}/approve`

### Offboard Employee
1. Submit resignation → `/api/offboarding/resignations`
2. Schedule exit interview → `/api/offboarding/exit-interviews`
3. Initiate clearance → `/api/offboarding/clearances`
4. Calculate settlement → `/api/offboarding/settlements`
5. Process payment → `/api/offboarding/settlements/{id}/pay`

---

## 🔧 Troubleshooting

### Application Won't Start
```powershell
# Restore packages
dotnet restore

# Clean build
dotnet clean
dotnet build

# Run again
dotnet run
```

### Port Already in Use
The app uses port 5000. If busy, stop other services or change port in `Properties/launchSettings.json`

### 401 Unauthorized
Remember to:
1. Login first → `/api/auth/login`
2. Copy the token
3. Click "Authorize" in Swagger
4. Paste token as: `Bearer <your-token>`

### Data Not Persisting
- Check `Data/` folder exists
- Ensure write permissions
- Look for error messages in console

---

## 💡 Pro Tips

1. **Use Swagger** - It's the easiest way to test all endpoints
2. **Check Stats** - Each module has a `/stats` endpoint
3. **Test Workflows** - Try complete scenarios end-to-end
4. **Review Logs** - Console shows all operations
5. **Explore Data** - Check JSON files in `Data/` folder

---

## 🎊 You're Ready!

You now have a **complete enterprise HR system** with:

✅ 169 API endpoints  
✅ 11 functional modules  
✅ 38+ data models  
✅ Full employee lifecycle  
✅ Comprehensive analytics  
✅ Production-ready architecture  

**Start exploring your new features! 🚀**

---

## 📞 Quick Links

- **Swagger:** http://localhost:5000/swagger
- **Web App:** http://localhost:5000
- **Documentation:** See README.md files
- **API Reference:** API_QUICK_REFERENCE.md

---

**Happy HR Management! 🎉**

*All features implemented and ready to use!*
