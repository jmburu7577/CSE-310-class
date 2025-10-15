# 🔧 Troubleshooting Guide

## Application Not Displaying at http://localhost:5000

### Quick Fix Steps

#### Step 1: Stop All Running Instances
Open PowerShell in the project directory and run:
```powershell
Get-Process | Where-Object {$_.ProcessName -eq "dotnet"} | Stop-Process -Force
```

#### Step 2: Start the Application
**Option A: Using the Batch File (Recommended)**
1. Navigate to: `c:\Users\BYU\Desktop\CSE-310-class-1\EmployeeManagement.Web`
2. Double-click `START_APP.bat`
3. Wait for the message: "Now listening on: http://localhost:5000"
4. Open browser to http://localhost:5000

**Option B: Using PowerShell**
1. Open PowerShell
2. Navigate to project directory:
   ```powershell
   cd c:\Users\BYU\Desktop\CSE-310-class-1\EmployeeManagement.Web
   ```
3. Run:
   ```powershell
   dotnet run --urls "http://localhost:5000"
   ```
4. Wait for startup message
5. Open browser to http://localhost:5000

**Option C: Using Visual Studio Code**
1. Open the project in VS Code
2. Press F5 or click Run > Start Debugging
3. Application will start automatically

#### Step 3: Verify Application is Running
Open PowerShell and check:
```powershell
netstat -ano | findstr :5000
```

You should see:
```
TCP    0.0.0.0:5000    0.0.0.0:0    LISTENING    [PID]
```

---

## Common Issues

### Issue 1: Port Already in Use
**Symptoms:** Error message "Address already in use"

**Solution:**
```powershell
# Find process using port 5000
netstat -ano | findstr :5000

# Stop the process (replace PID with actual process ID)
Stop-Process -Id [PID] -Force

# Start application again
dotnet run
```

### Issue 2: Build Errors
**Symptoms:** Application won't start, build fails

**Solution:**
```powershell
# Clean and rebuild
dotnet clean
dotnet build

# If still fails, restore packages
dotnet restore
dotnet build
```

### Issue 3: Browser Shows "Can't Reach This Page"
**Symptoms:** Browser can't connect to localhost:5000

**Solution:**
1. Verify application is running (check PowerShell window)
2. Look for message: "Now listening on: http://localhost:5000"
3. If not running, start with `dotnet run`
4. Try accessing: http://127.0.0.1:5000 instead

### Issue 4: Application Starts But Shows Blank Page
**Symptoms:** Page loads but nothing displays

**Solution:**
1. Check browser console (F12) for errors
2. Verify wwwroot/index.html exists
3. Clear browser cache (Ctrl+Shift+Delete)
4. Try incognito/private browsing mode

### Issue 5: Users.json or Employees.json Missing
**Symptoms:** Errors about missing files

**Solution:**
Files are created automatically on first run. If issues persist:
```powershell
# Delete existing files
Remove-Item users.json -ErrorAction SilentlyContinue
Remove-Item employees.json -ErrorAction SilentlyContinue

# Restart application - files will be recreated
dotnet run
```

---

## Step-by-Step Startup Guide

### Method 1: Simple Batch File (Easiest)

1. **Locate the batch file:**
   - Path: `c:\Users\BYU\Desktop\CSE-310-class-1\EmployeeManagement.Web\START_APP.bat`

2. **Double-click START_APP.bat**
   - A command window will open
   - You'll see build output
   - Wait for: "Now listening on: http://localhost:5000"

3. **Open your browser:**
   - Navigate to: http://localhost:5000
   - You should see the login page

4. **Login with admin account:**
   - Username: `admin` (or `jemo` or `testuser`)
   - Password: `admin123` (or `jemo123` or `test123`)

### Method 2: PowerShell Command Line

1. **Open PowerShell:**
   - Press Windows + X
   - Select "Windows PowerShell" or "Terminal"

2. **Navigate to project:**
   ```powershell
   cd c:\Users\BYU\Desktop\CSE-310-class-1\EmployeeManagement.Web
   ```

3. **Run the application:**
   ```powershell
   dotnet run --urls "http://localhost:5000"
   ```

4. **Wait for startup:**
   - Look for: "Now listening on: http://localhost:5000"
   - Look for: "Application started. Press Ctrl+C to shut down."

5. **Open browser:**
   - Go to: http://localhost:5000

6. **To stop:**
   - Press Ctrl+C in PowerShell window

### Method 3: Visual Studio Code

1. **Open VS Code**

2. **Open folder:**
   - File > Open Folder
   - Select: `c:\Users\BYU\Desktop\CSE-310-class-1\EmployeeManagement.Web`

3. **Start debugging:**
   - Press F5
   - Or click Run > Start Debugging
   - Or click the play button in the Run panel

4. **Browser opens automatically:**
   - Application launches
   - Browser opens to http://localhost:5000

---

## Verification Checklist

Before accessing the application, verify:

- [ ] PowerShell/Command window is open
- [ ] You see "Now listening on: http://localhost:5000"
- [ ] No error messages in the console
- [ ] Port 5000 is in use (check with netstat)
- [ ] Browser is pointed to http://localhost:5000 (not https)

---

## Expected Startup Output

When the application starts correctly, you should see:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: c:\Users\BYU\Desktop\CSE-310-class-1\EmployeeManagement.Web
```

You may also see:
```
info: EmployeeManagement.Web.Services.AuthService[0]
      Default admin account created - Username: admin, Password: admin123
info: EmployeeManagement.Web.Services.AuthService[0]
      Default admin account created - Username: jemo, Password: jemo123
info: EmployeeManagement.Web.Services.AuthService[0]
      Default admin account created - Username: testuser, Password: test123
```

---

## Quick Commands Reference

### Start Application
```powershell
dotnet run
```

### Start on Specific Port
```powershell
dotnet run --urls "http://localhost:5000"
```

### Build Application
```powershell
dotnet build
```

### Clean Build
```powershell
dotnet clean
dotnet build
```

### Check Port Usage
```powershell
netstat -ano | findstr :5000
```

### Stop All Dotnet Processes
```powershell
Get-Process dotnet | Stop-Process -Force
```

---

## Browser Access

### Correct URLs
✅ http://localhost:5000  
✅ http://127.0.0.1:5000  

### Incorrect URLs
❌ https://localhost:5000 (note the 's')  
❌ localhost:5000 (missing http://)  
❌ http://localhost:5173 (wrong port)  

---

## Default Admin Accounts

Once the application is running, login with:

**Account 1:**
- Username: `admin`
- Password: `admin123`

**Account 2:**
- Username: `jemo`
- Password: `jemo123`

**Account 3:**
- Username: `testuser`
- Password: `test123`

All three accounts have full Administrator access!

---

## Still Having Issues?

### Check These:

1. **Is .NET installed?**
   ```powershell
   dotnet --version
   ```
   Should show: 9.0.x or 8.0.x

2. **Is the project file intact?**
   ```powershell
   Test-Path EmployeeManagement.Web.csproj
   ```
   Should return: True

3. **Are dependencies installed?**
   ```powershell
   dotnet restore
   ```

4. **Is there a firewall blocking port 5000?**
   - Check Windows Firewall settings
   - Temporarily disable to test

5. **Try a different port:**
   ```powershell
   dotnet run --urls "http://localhost:5001"
   ```
   Then access: http://localhost:5001

---

## Contact Information

If you continue to have issues:

1. Check the console output for error messages
2. Look in the browser console (F12) for JavaScript errors
3. Verify all files are present in wwwroot folder
4. Try rebuilding: `dotnet clean && dotnet build`

---

**Last Updated:** 2025-10-14  
**Application:** Employee Management System  
**Default Port:** 5000  
**Framework:** .NET 9.0
