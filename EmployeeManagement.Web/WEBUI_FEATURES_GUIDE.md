# 🎉 Web UI Features Guide - Leave & Payslip Management

## ✅ What's New

Your Employee Management System now has **full Leave Management and Payslip Management** features accessible directly from the Web UI!

---

## 🌐 Accessing the Features

### **Main Application**
**URL:** http://localhost:5000

From the main page, you'll see new navigation buttons in the header:
- 🏖️ **Leaves** (Green button) - Manage leave requests
- 💰 **Payslips** (Yellow button) - Generate and download payslips

---

## 🏖️ Leave Management Features

### **Access:** http://localhost:5000/leaves.html

### **What You Can Do:**

#### 1. **View Leave Statistics**
- Total Requests
- Pending Requests
- Approved Requests
- Rejected Requests

#### 2. **Request New Leave**
- Click "Request Leave" button
- Select employee
- Choose leave type (Vacation, Sick, Personal, etc.)
- Set start and end dates
- Enter reason
- Submit request

#### 3. **Manage Leave Requests**
- **Approve** pending requests (adds manager notes)
- **Reject** requests (requires rejection reason)
- **View Balance** - Check remaining leave days for any employee
- **Filter by Status** - View Pending, Approved, or Rejected leaves

#### 4. **View Leave Balance**
- Shows vacation days (used/remaining)
- Shows sick days (used/remaining)
- Shows personal days (used/remaining)
- Updates automatically when leaves are approved

### **Leave Types Available:**
1. Vacation
2. Sick
3. Unpaid
4. Personal
5. Maternity
6. Paternity
7. Bereavement
8. Study

---

## 💰 Payslip Management Features

### **Access:** http://localhost:5000/payslips.html

### **What You Can Do:**

#### 1. **View Payslip Statistics**
- Total Payslips Generated
- Total Amount Paid
- Average Net Salary

#### 2. **Generate New Payslips**
- Click "Generate Payslip" button
- Select employee
- Choose pay period:
  - Current Month (automatic dates)
  - Last Month (automatic dates)
  - Custom Dates (set manually)
- Add optional bonus amount
- Add optional additional deductions
- Add notes
- Generate payslip

#### 3. **View Payslips**
- See all payslips with details:
  - Gross Salary
  - Tax (20% automatic)
  - Bonuses
  - Deductions
  - **Net Salary** (highlighted)
- **Search** by employee name or ID
- **Filter by Month** - View payslips for specific months

#### 4. **Download Payslips**
- Click "Download PDF" on any payslip
- Professional PDF with:
  - Company header
  - Employee details
  - Salary breakdown table
  - Net salary highlighted
  - Notes section
  - Professional footer

#### 5. **View Details**
- Click "View Details" for complete breakdown
- Shows all calculations

---

## 🎯 User Guide

### **For Employees:**
1. Login to the system
2. Click **"Leaves"** to request time off
3. Click **"Payslips"** to view and download your payslips
4. Download PDF payslips for your records

### **For Managers:**
1. Login to the system
2. Navigate to **Leaves** section
3. Review pending leave requests
4. Approve or reject with notes
5. Check employee leave balances

### **For HR/Payroll:**
1. Navigate to **Payslips** section
2. Generate payslips for employees
3. Review generated payslips
4. Employees can download their own payslips

---

## 📊 Key Features

### **Leave Management:**
✅ Request leaves for any employee  
✅ Approve/reject with manager notes  
✅ Automatic business days calculation  
✅ Leave balance tracking (vacation, sick, personal)  
✅ Balance validation (prevents over-booking)  
✅ Real-time statistics  
✅ Filter by status  

### **Payslip Management:**
✅ Generate individual payslips  
✅ Automatic salary calculations  
✅ Tax calculation (20%)  
✅ Bonus and deduction support  
✅ Professional PDF generation  
✅ Download payslips anytime  
✅ Search and filter capabilities  
✅ Month-wise filtering  

---

## 🚀 Quick Start

### **1. Start the Application**
```powershell
cd EmployeeManagement.Web
dotnet run
```

### **2. Open Your Browser**
Navigate to: http://localhost:5000

### **3. Login**
- Username: `admin`
- Password: `admin123`

### **4. Try the New Features**

**Request a Leave:**
1. Click **"Leaves"** button (green)
2. Click **"Request Leave"**
3. Fill in the form
4. Submit

**Generate a Payslip:**
1. Click **"Payslips"** button (yellow)
2. Click **"Generate Payslip"**
3. Select employee
4. Choose "Current Month"
5. Add bonus if needed
6. Generate

**Download a Payslip:**
1. On any payslip card, click **"Download PDF"**
2. PDF will download automatically
3. Open and view professional payslip!

---

## 📱 Navigation

### **From Any Page:**

**Top Navigation Bar:**
- 👥 **Employees** - Back to employee management
- 🏖️ **Leaves** - Leave management
- 💰 **Payslips** - Payslip management
- ⚙️ **Manage Users** - User management (admins only)
- 🚪 **Logout** - Sign out

---

## 🎨 UI Features

### **Responsive Design**
- Works on desktop, tablet, and mobile
- Clean, modern interface
- Easy navigation

### **Visual Indicators**
- **Color-coded status badges:**
  - 🟡 Yellow = Pending
  - 🟢 Green = Approved
  - 🔴 Red = Rejected
  - ⚫ Gray = Cancelled

- **Leave types with icons**
- **Salary breakdowns with colors:**
  - Green = Income (Gross, Bonuses)
  - Red = Deductions (Tax, Other)

### **Statistics Dashboard**
- Real-time counts
- Visual cards
- Quick overview

---

## 💡 Tips & Best Practices

### **Leave Management:**
1. Always check balance before requesting leaves
2. Add clear reasons for leave requests
3. Managers should provide feedback in approval/rejection notes
4. Plan leaves in advance

### **Payslip Management:**
1. Generate payslips at the end of each pay period
2. Add notes for clarity (e.g., "Performance bonus included")
3. Keep bonuses and deductions accurate
4. Download PDFs for record-keeping

---

## 🔒 Security

- All features require authentication
- JWT token-based security
- Role-based access control
- Secure PDF downloads

---

## 🎉 Summary

You now have a **complete HR management system** with:
- ✅ Employee Management
- ✅ Leave Management (Request, Approve, Track)
- ✅ Payslip Generation (Create, Download PDF)
- ✅ User Management
- ✅ Role-Based Access Control
- ✅ Professional Documentation

**Everything is accessible from your browser at http://localhost:5000!**

---

## 📞 Need Help?

- Check the **QUICK_REFERENCE.md** for API details
- Review **FINAL_SUMMARY.md** for complete system overview
- Test endpoints in **Swagger**: http://localhost:5000/swagger

**Enjoy your enhanced Employee Management System!** 🚀
