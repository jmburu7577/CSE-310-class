# 🛠️ Setup Instructions

## Employee Management System - Web Application

Complete setup guide to get your web application running.

---

## ✅ Prerequisites

### Required
- **.NET 8.0 SDK** or later
  - Download: https://dotnet.microsoft.com/download
  - Check version: `dotnet --version`

### Optional (for development)
- **Visual Studio 2022** (Community, Professional, or Enterprise)
- **Visual Studio Code** with C# extension
- **Git** for version control

---

## 🚀 Installation Steps

### Step 1: Verify .NET Installation

Open Command Prompt and check:
```cmd
dotnet --version
```

You should see: `8.0.x` or higher

If not installed:
1. Visit https://dotnet.microsoft.com/download
2. Download .NET 8.0 SDK
3. Install and restart your terminal

---

### Step 2: Navigate to Project Directory

```cmd
cd C:\Users\BYU\Desktop\CSE-310-class-1\EmployeeManagement.Web
```

---

### Step 3: Restore Dependencies (Optional)

This happens automatically, but you can run manually:
```cmd
dotnet restore
```

---

### Step 4: Build the Project (Optional)

```cmd
dotnet build
```

Expected output:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

---

### Step 5: Run the Application

```cmd
dotnet run
```

Expected output:
```
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shutdown.
```

---

### Step 6: Open in Browser

Navigate to:
```
http://localhost:5000
```

You should see the Employee Management System interface!

---

## 🎯 Quick Start Commands

### Run Application
```cmd
cd EmployeeManagement.Web
dotnet run
```

### Build Only
```cmd
dotnet build
```

### Clean Build
```cmd
dotnet clean
dotnet build
```

### Run with Specific Port
Edit `Properties/launchSettings.json`:
```json
"applicationUrl": "http://localhost:5001"
```

---

## 🌐 Accessing the Application

### Web Interface
```
http://localhost:5000
```

### API Documentation (Swagger)
```
http://localhost:5000/swagger
```

### API Endpoints
```
http://localhost:5000/api/employees
```

---

## 📁 Project Structure Verification

After setup, you should have:

```
EmployeeManagement.Web/
├── Controllers/
│   └── EmployeesController.cs     ✓
├── Models/
│   └── Employee.cs                ✓
├── Services/
│   ├── IEmployeeService.cs        ✓
│   └── EmployeeService.cs         ✓
├── Properties/
│   └── launchSettings.json        ✓
├── wwwroot/
│   ├── index.html                 ✓
│   └── app.js                     ✓
├── Program.cs                     ✓
├── appsettings.json              ✓
├── EmployeeManagement.Web.csproj  ✓
└── Documentation files            ✓
```

---

## 🔧 Development Setup

### Using Visual Studio

1. **Open Project**
   - File → Open → Project/Solution
   - Select `EmployeeManagement.Web.csproj`

2. **Run**
   - Press `F5` (Debug mode)
   - Or `Ctrl+F5` (Without debugging)

3. **Browser Opens Automatically**

### Using Visual Studio Code

1. **Open Folder**
   ```cmd
   code EmployeeManagement.Web
   ```

2. **Install C# Extension**
   - Open Extensions (Ctrl+Shift+X)
   - Search "C#"
   - Install "C# for Visual Studio Code"

3. **Run**
   - Press `F5`
   - Or use terminal: `dotnet run`

### Using Command Line

```cmd
cd EmployeeManagement.Web
dotnet run
```

---

## 🧪 Testing the Setup

### 1. Check Web Interface

Visit `http://localhost:5000`

You should see:
- ✅ Purple gradient header
- ✅ "Employee Management System" title
- ✅ Statistics dashboard (showing 0s initially)
- ✅ "Add Employee" button
- ✅ Empty state message

### 2. Test Add Employee

1. Click "Add Employee"
2. Fill in the form:
   - ID: 1
   - First Name: John
   - Last Name: Doe
   - Age: 30
   - Department: Engineering
   - Salary: 75000
   - Street: 123 Main St
   - City: New York
   - State: NY
   - ZIP: 10001
3. Click "Add Employee"
4. Employee card should appear!

### 3. Check API

Visit `http://localhost:5000/swagger`

You should see:
- ✅ Swagger UI
- ✅ List of API endpoints
- ✅ Ability to test endpoints

### 4. Test API Endpoint

In Swagger:
1. Click on `GET /api/employees`
2. Click "Try it out"
3. Click "Execute"
4. You should see your added employee in JSON format

---

## 🐛 Troubleshooting

### Issue: "dotnet: command not found"

**Solution:**
1. Install .NET SDK from https://dotnet.microsoft.com/download
2. Restart terminal
3. Verify: `dotnet --version`

---

### Issue: Port 5000 Already in Use

**Error:**
```
Failed to bind to address http://localhost:5000
```

**Solution 1:** Stop other applications using port 5000

**Solution 2:** Change port in `Properties/launchSettings.json`:
```json
{
  "profiles": {
    "http": {
      "applicationUrl": "http://localhost:5001"
    }
  }
}
```

---

### Issue: Browser Doesn't Open Automatically

**Solution:**
Manually open browser and navigate to:
```
http://localhost:5000
```

---

### Issue: Build Errors

**Solution:**
```cmd
dotnet clean
dotnet restore
dotnet build
```

---

### Issue: Changes Not Appearing

**Solution:**
1. Stop the application (Ctrl+C)
2. Rebuild: `dotnet build`
3. Run again: `dotnet run`
4. Hard refresh browser (Ctrl+F5)

---

### Issue: Data Not Saving

**Check:**
1. Application has write permissions
2. Check console for errors
3. Look for `employees.json` in application directory

**Solution:**
Run as administrator if needed

---

### Issue: CORS Errors in Browser Console

**Check:**
`Program.cs` has CORS configuration:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
                       .AllowAnyHeader()
                       .AllowAnyMethod());
});

app.UseCors("AllowFrontend");
```

---

## 📊 Verify Installation Checklist

- [ ] .NET SDK installed (`dotnet --version` works)
- [ ] Project files present (13 files)
- [ ] Can build project (`dotnet build` succeeds)
- [ ] Can run project (`dotnet run` starts server)
- [ ] Can access web interface (http://localhost:5000)
- [ ] Can add employee (form works)
- [ ] Can view employees (cards display)
- [ ] Can edit employee (modal opens)
- [ ] Can delete employee (confirmation works)
- [ ] Statistics update (dashboard shows correct numbers)
- [ ] Swagger works (http://localhost:5000/swagger)
- [ ] API responds (endpoints return data)

---

## 🎯 Next Steps After Setup

### 1. Explore the Interface
- Add multiple employees
- Try grid and table views
- Edit and delete employees
- Watch statistics update

### 2. Test the API
- Use Swagger UI
- Test all endpoints
- See JSON responses

### 3. Examine the Code
- Read through controllers
- Study React components
- Understand data flow

### 4. Customize
- Change colors
- Add new fields
- Create new features

---

## 📚 Additional Resources

### Documentation
- **README.md** - Full project documentation
- **QUICK_START.md** - Quick start guide
- **FEATURES.md** - Feature descriptions
- **MIGRATION_GUIDE.md** - Console vs Web comparison

### Learning Resources
- ASP.NET Core Docs: https://docs.microsoft.com/aspnet/core
- React Tutorial: https://react.dev/learn
- TailwindCSS: https://tailwindcss.com/docs
- REST API Design: https://restfulapi.net/

---

## 🔒 Security Notes

### For Development
- ✅ Running on localhost
- ✅ No authentication needed
- ✅ CORS configured for local development

### For Production
Consider adding:
- [ ] HTTPS/SSL
- [ ] Authentication (JWT, OAuth)
- [ ] Authorization (role-based)
- [ ] Input validation
- [ ] Rate limiting
- [ ] Database instead of JSON file
- [ ] Environment variables for secrets

---

## 🚀 Deployment Preparation

### Local Network Access

To access from other devices on your network:

1. **Find your IP address:**
   ```cmd
   ipconfig
   ```
   Look for IPv4 Address (e.g., 192.168.1.100)

2. **Update launchSettings.json:**
   ```json
   "applicationUrl": "http://0.0.0.0:5000"
   ```

3. **Access from other devices:**
   ```
   http://192.168.1.100:5000
   ```

### Cloud Deployment

**Azure:**
```cmd
az webapp up --name my-employee-app --resource-group myResourceGroup
```

**Docker:**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY bin/Release/net8.0/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "EmployeeManagement.Web.dll"]
```

---

## ✅ Setup Complete!

If you've completed all steps successfully, you now have:

✅ **Working web application**  
✅ **Beautiful user interface**  
✅ **Functional API**  
✅ **Complete documentation**  
✅ **Ready for development**  

---

## 🎉 You're Ready!

Start the application:
```cmd
dotnet run
```

Open browser:
```
http://localhost:5000
```

**Enjoy your Employee Management System!** 🚀

---

**Last Updated**: 2025-10-14  
**Version**: 1.0  
**Status**: ✅ Ready to Use
