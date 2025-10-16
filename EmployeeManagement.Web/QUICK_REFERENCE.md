# 📖 Employee Management System - Quick Reference Guide

**Version:** 2.0  
**Last Updated:** October 16, 2025  
**Status:** 40% Complete (4/10 Phases)

---

## 🚀 Quick Start

### Start Application
```powershell
cd EmployeeManagement.Web
dotnet restore
dotnet run
```

### Access Points
- **Web UI:** http://localhost:5000
- **API Docs:** http://localhost:5000/swagger

### Default Admin Login
```
Username: admin
Password: admin123
```

---

## 📊 System Overview

### Current Capabilities
✅ 39 API Endpoints  
✅ 5 Services  
✅ 4 Controllers  
✅ 17+ Models  
✅ 5 Data Files  
✅ Complete RBAC (5 roles)  

### Features Available
1. Employee Management (CRUD + Search/Filter/Sort)
2. User Management (CRUD + Role Assignment)
3. Payslip Management (Generate + Download PDF)
4. Leave Management (Request + Approval Workflow)
5. Data Export (CSV/Excel/PDF)
6. Data Import (CSV with validation)

---

## 🔐 User Roles & Permissions

| Role | Level | Capabilities |
|------|-------|--------------|
| **Viewer** | 1 | View only |
| **Employee** | 2 | + Add employees, request leaves |
| **Manager** | 3 | + Edit employees, approve leaves, generate payslips |
| **Senior Manager** | 4 | + Delete employees, bulk payslips |
| **Administrator** | 5 | + Manage users, delete all records |

---

## 📚 API Endpoints Reference

### Authentication (8 endpoints)
```
POST   /api/auth/register
POST   /api/auth/login
GET    /api/auth/me
GET    /api/auth/validate
GET    /api/auth/users
POST   /api/auth/create-user
POST   /api/auth/change-role
DELETE /api/auth/delete-user/{id}
```

### Employees (12 endpoints)
```
GET    /api/employees
GET    /api/employees/{id}
POST   /api/employees
PUT    /api/employees/{id}
DELETE /api/employees/{id}
GET    /api/employees/stats
GET    /api/employees/search?searchTerm=X&department=Y&sortBy=Z
GET    /api/employees/departments
GET    /api/employees/export/csv
GET    /api/employees/export/excel
GET    /api/employees/export/pdf
POST   /api/employees/import/csv
```

### Payslips (8 endpoints)
```
GET    /api/payslips
GET    /api/payslips/{id}
GET    /api/payslips/employee/{employeeId}
POST   /api/payslips/generate/{employeeId}
POST   /api/payslips/generate-bulk
GET    /api/payslips/{id}/download
DELETE /api/payslips/{id}
GET    /api/payslips/stats
```

### Leaves (11 endpoints)
```
GET    /api/leaves
GET    /api/leaves/{id}
GET    /api/leaves/employee/{employeeId}
GET    /api/leaves/pending
POST   /api/leaves/employee/{employeeId}
POST   /api/leaves/{id}/approve
POST   /api/leaves/{id}/reject
POST   /api/leaves/{id}/cancel?employeeId=X
DELETE /api/leaves/{id}
GET    /api/leaves/balance/{employeeId}?year=2025
GET    /api/leaves/stats
```

---

## 💡 Common Tasks

### 1. Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}
```

### 2. Search Employees
```http
GET /api/employees/search?searchTerm=john&department=Engineering&sortBy=salary&sortDescending=true
Authorization: Bearer <token>
```

### 3. Export to Excel
```http
GET /api/employees/export/excel
Authorization: Bearer <token>
```

### 4. Generate Payslip
```http
POST /api/payslips/generate/1001
Authorization: Bearer <token>
Content-Type: application/json

{
  "payPeriodStart": "2025-10-01",
  "payPeriodEnd": "2025-10-31",
  "bonusAmount": 500,
  "additionalDeductions": 0,
  "notes": "Monthly salary + performance bonus"
}
```

### 5. Request Leave
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

### 6. Approve Leave
```http
POST /api/leaves/1/approve
Authorization: Bearer <token>
Content-Type: application/json

{
  "approve": true,
  "notes": "Approved. Enjoy!"
}
```

---

## 📋 Data Files Location

All data files are auto-generated in the project root:

```
EmployeeManagement.Web/
├── users.json              # User accounts
├── employees.json          # Employee records
├── payslips.json          # Payslip records
├── leave_requests.json    # Leave requests
└── leave_balances.json    # Leave balances
```

---

## 🎯 Leave Types

| ID | Type | Balance Deduction |
|----|------|-------------------|
| 1 | Vacation | Yes (vacation days) |
| 2 | Sick | Yes (sick days) |
| 3 | Unpaid | No |
| 4 | Personal | Yes (personal days) |
| 5 | Maternity | No |
| 6 | Paternity | No |
| 7 | Bereavement | No |
| 8 | Study | No |

### Default Annual Allocations
- **Vacation:** 20 days
- **Sick:** 10 days
- **Personal:** 5 days

---

## 🎨 Leave Status Codes

| Code | Status | Description |
|------|--------|-------------|
| 1 | Pending | Waiting for approval |
| 2 | Approved | Manager approved |
| 3 | Rejected | Manager rejected |
| 4 | Cancelled | Employee cancelled |

---

## 💰 Payslip Calculations

```
Monthly Gross Salary = Annual Salary ÷ 12
Tax = Monthly Gross × 20%
Net Salary = Gross + Bonuses - Tax - Deductions
```

**Example:**
- Annual Salary: $75,000
- Monthly Gross: $6,250
- Tax (20%): $1,250
- Bonus: $500
- Deductions: $50
- **Net Salary: $5,450**

---

## 🔍 Search Query Examples

### Search by name
```
/api/employees/search?searchTerm=john
```

### Filter by department
```
/api/employees/search?department=Engineering
```

### Sort by salary (descending)
```
/api/employees/search?sortBy=salary&sortDescending=true
```

### Combined query
```
/api/employees/search?searchTerm=john&department=Engineering&sortBy=name&sortDescending=false
```

### Sort options
- `name` - Sort by first/last name
- `age` - Sort by age
- `salary` - Sort by salary
- `department` - Sort by department

---

## 📤 Export Formats

### CSV Export
- Plain text format
- Compatible with Excel
- Headers included
- Use: `/api/employees/export/csv`

### Excel Export
- XLSX format
- Professional formatting
- Styled headers (blue background)
- Auto-adjusted columns
- Use: `/api/employees/export/excel`

### PDF Export
- Professional layout
- Landscape orientation
- Company branding
- Complete data table
- Use: `/api/employees/export/pdf`

---

## 📥 CSV Import Format

### Required Columns
```csv
Id,FirstName,LastName,Age,Department,Salary,Street,City,State,Zip
```

### Example CSV
```csv
Id,FirstName,LastName,Age,Department,Salary,Street,City,State,Zip
1001,John,Doe,30,Engineering,75000,123 Main St,New York,NY,10001
1002,Jane,Smith,28,Sales,65000,456 Oak Ave,Boston,MA,02101
```

### Validation Rules
- First name and last name required
- Age must be 18-100
- Salary must be positive
- ID must be unique

---

## 🛠️ Troubleshooting

### Issue: Cannot login
**Solution:** Use default accounts (admin/admin123, jemo/jemo123, testuser/test123)

### Issue: 401 Unauthorized
**Solution:** Include valid JWT token in Authorization header: `Bearer <token>`

### Issue: 403 Forbidden
**Solution:** Your role lacks permission. Contact admin for role upgrade.

### Issue: Port already in use
**Solution:** Stop other processes on port 5000 or change port in `launchSettings.json`

### Issue: Data not persisting
**Solution:** Check write permissions in project directory. Verify JSON files exist.

---

## 📊 Testing Checklist

### Phase 1: Search & Filter
- [ ] Search by employee name
- [ ] Filter by department
- [ ] Sort by salary
- [ ] Combined search + filter + sort

### Phase 2: Export/Import
- [ ] Export to CSV
- [ ] Export to Excel (verify formatting)
- [ ] Export to PDF (verify layout)
- [ ] Import valid CSV
- [ ] Import invalid CSV (check errors)

### Phase 3: Payslips
- [ ] Generate single payslip
- [ ] Generate bulk payslips
- [ ] Download payslip PDF
- [ ] Verify salary calculations
- [ ] View payslip statistics

### Phase 4: Leaves
- [ ] Create leave request
- [ ] View pending requests
- [ ] Approve leave
- [ ] Reject leave
- [ ] Check leave balance
- [ ] Verify balance deduction

---

## 🎓 Documentation Files

| File | Purpose |
|------|---------|
| `README.md` | Main documentation |
| `FINAL_SUMMARY.md` | Complete implementation summary |
| `PROGRESS_REPORT.md` | Detailed progress tracking |
| `IMPLEMENTATION_GUIDE.md` | Implementation details |
| `FEATURES_COMPLETED.md` | Completed features overview |
| `PHASE3_PAYSLIP_COMPLETE.md` | Payslip system details |
| `PHASE4_LEAVE_MANAGEMENT_COMPLETE.md` | Leave system details |
| `REMAINING_PHASES_ROADMAP.md` | Future work roadmap |
| `QUICK_REFERENCE.md` | This file |

---

## 🚀 What's Next?

### Completed (40%)
✅ Phase 1: Search, Filter & Sort  
✅ Phase 2: Export/Import Features  
✅ Phase 3: Payslip Management  
✅ Phase 4: Leave Management  

### Remaining (60%)
⏳ Phase 5: Email & Password Recovery  
⏳ Phase 6: Audit Logs & Profiles  
⏳ Phase 7: Database Migration (High Priority)  
⏳ Phase 8: Advanced Features  
⏳ Phase 9: Security & Testing (High Priority)  
⏳ Phase 10: Production Deployment  

---

## 📞 Quick Reference Card

```
╔═══════════════════════════════════════════════════════╗
║         EMPLOYEE MANAGEMENT SYSTEM v2.0               ║
╠═══════════════════════════════════════════════════════╣
║                                                       ║
║  URL: http://localhost:5000                          ║
║  Swagger: http://localhost:5000/swagger              ║
║                                                       ║
║  Admin Login: admin / admin123                       ║
║                                                       ║
║  Features:                                           ║
║  ✅ Employee Management (CRUD + Search)              ║
║  ✅ Payslip Generation (PDF)                         ║
║  ✅ Leave Management (Approval Workflow)             ║
║  ✅ Data Export (CSV/Excel/PDF)                      ║
║  ✅ Data Import (CSV)                                ║
║  ✅ User Management (5 Roles)                        ║
║                                                       ║
║  API Endpoints: 39                                   ║
║  Documentation Files: 13+                            ║
║  Implementation: 40% Complete                        ║
║                                                       ║
╚═══════════════════════════════════════════════════════╝
```

---

**Keep this file handy for quick reference!** 📌

---

*Last Updated: October 16, 2025*
