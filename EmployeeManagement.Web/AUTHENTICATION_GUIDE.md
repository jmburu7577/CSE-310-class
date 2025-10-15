# 🔐 Authentication System Guide

## Overview

Your Employee Management System now includes a **fully functional authentication system** with login and signup capabilities!

---

## ✨ Features Added

### Backend (API)
- ✅ **JWT Token Authentication** - Secure token-based auth
- ✅ **User Registration** - Create new accounts
- ✅ **User Login** - Authenticate existing users
- ✅ **Password Hashing** - SHA256 encrypted passwords
- ✅ **Token Validation** - Automatic token verification
- ✅ **Protected Routes** - Employee API requires authentication
- ✅ **User Storage** - JSON file-based user database

### Frontend (UI)
- ✅ **Login Page** - Beautiful login form
- ✅ **Signup Page** - User registration form
- ✅ **Session Management** - LocalStorage token persistence
- ✅ **Auto-redirect** - Redirects to login if unauthorized
- ✅ **User Display** - Shows logged-in user name
- ✅ **Logout Button** - Clear session and return to login
- ✅ **Form Validation** - Client-side validation
- ✅ **Error Handling** - User-friendly error messages

---

## 🚀 How to Use

### 1. First Time Setup - Create an Account

When you open the application at **http://localhost:5000**, you'll see the **Login Page**.

Since you don't have an account yet:

1. Click **"Sign Up"** button at the bottom
2. Fill in the registration form:
   - **Full Name**: Your full name (e.g., "John Doe")
   - **Username**: Choose a unique username (e.g., "johndoe")
   - **Email**: Your email address (e.g., "john@example.com")
   - **Password**: At least 6 characters
   - **Confirm Password**: Must match password
3. Click **"Sign Up"** button
4. You'll be automatically logged in and redirected to the main app!

### 2. Logging In

After creating an account, you can log in:

1. Enter your **Username**
2. Enter your **Password**
3. Click **"Sign In"**
4. You'll be redirected to the Employee Management dashboard

### 3. Using the Application

Once logged in:
- Your name appears in the top-right corner
- You can manage employees (add, edit, delete)
- All API requests include your authentication token
- Your session persists even if you refresh the page

### 4. Logging Out

Click the **"Logout"** button in the top-right corner:
- Your session will be cleared
- You'll be redirected to the login page
- You'll need to log in again to access the app

---

## 🔒 Security Features

### Password Security
- Passwords are hashed using **SHA256** before storage
- Plain text passwords are never stored
- Password minimum length: 6 characters

### Token Security
- **JWT (JSON Web Tokens)** for authentication
- Tokens expire after **7 days**
- Tokens are stored securely in browser LocalStorage
- All API requests include the token in Authorization header

### API Protection
- All employee endpoints require authentication
- Unauthorized requests return **401 Unauthorized**
- Frontend automatically redirects to login on 401

---

## 📁 Files Created/Modified

### Backend Files Created
```
Models/User.cs                  - User data models
Services/IAuthService.cs        - Authentication service interface
Services/AuthService.cs         - Authentication business logic
Controllers/AuthController.cs   - Login/Signup API endpoints
```

### Backend Files Modified
```
Program.cs                      - Added JWT authentication middleware
EmployeeManagement.Web.csproj   - Added JWT Bearer package
Controllers/EmployeesController.cs - Added [Authorize] attribute
```

### Frontend Files Modified
```
wwwroot/app.js                  - Added login/signup components
```

### Data Files (Auto-created)
```
users.json                      - User database (created on first signup)
employees.json                  - Employee database
```

---

## 🔌 API Endpoints

### Authentication Endpoints

#### POST /api/auth/register
Register a new user account.

**Request Body:**
```json
{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "password123",
  "fullName": "John Doe"
}
```

**Response (Success):**
```json
{
  "success": true,
  "message": "Registration successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "johndoe",
    "email": "john@example.com",
    "fullName": "John Doe"
  }
}
```

#### POST /api/auth/login
Login with existing credentials.

**Request Body:**
```json
{
  "username": "johndoe",
  "password": "password123"
}
```

**Response (Success):**
```json
{
  "success": true,
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "johndoe",
    "email": "john@example.com",
    "fullName": "John Doe"
  }
}
```

#### GET /api/auth/me
Get current user information (requires authentication).

**Headers:**
```
Authorization: Bearer <token>
```

**Response:**
```json
{
  "id": 1,
  "username": "johndoe",
  "email": "john@example.com",
  "fullName": "John Doe"
}
```

#### GET /api/auth/validate
Validate if token is still valid (requires authentication).

**Response:**
```json
{
  "valid": true,
  "message": "Token is valid"
}
```

### Employee Endpoints (Now Protected)

All employee endpoints now require authentication:
- GET /api/employees
- GET /api/employees/{id}
- POST /api/employees
- PUT /api/employees/{id}
- DELETE /api/employees/{id}
- GET /api/employees/stats

**All requests must include:**
```
Authorization: Bearer <your-jwt-token>
```

---

## 🧪 Testing with Swagger

1. Open **http://localhost:5000/swagger**
2. You'll see the new **Auth** endpoints
3. To test protected endpoints:
   - First, call **POST /api/auth/register** or **POST /api/auth/login**
   - Copy the `token` from the response
   - Click the **"Authorize"** button at the top
   - Enter: `Bearer <your-token>`
   - Click **"Authorize"**
   - Now you can test all employee endpoints!

---

## 💾 Data Storage

### users.json
Located in the project root directory.

**Format:**
```json
[
  {
    "id": 1,
    "username": "johndoe",
    "email": "john@example.com",
    "passwordHash": "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=",
    "fullName": "John Doe",
    "createdAt": "2025-10-14T11:30:00Z",
    "lastLogin": "2025-10-14T11:35:00Z"
  }
]
```

---

## 🎨 UI Screenshots

### Login Page
- Clean, modern design
- Purple gradient background
- Username and password fields
- "Sign Up" link at bottom
- Error messages displayed clearly

### Signup Page
- Full name, username, email fields
- Password and confirm password
- Validation messages
- "Sign In" link to return to login

### Main Application (Authenticated)
- User name displayed in header
- Logout button in top-right
- All employee management features available

---

## 🔧 Configuration

### JWT Settings (in Program.cs)

You can customize JWT settings in `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyForJWTTokenGeneration123456789",
    "Issuer": "EmployeeManagementApp",
    "Audience": "EmployeeManagementApp"
  }
}
```

**Note:** The default key is used if not specified in appsettings.json.

---

## ⚠️ Common Issues & Solutions

### Issue: "Invalid username or password"
**Solution:** Check that you're using the correct username and password. Usernames are case-insensitive.

### Issue: "Username already exists"
**Solution:** Choose a different username during registration.

### Issue: "Passwords do not match"
**Solution:** Ensure password and confirm password fields match exactly.

### Issue: Automatically logged out
**Solution:** Your token may have expired (7 days). Log in again.

### Issue: Can't access employee data
**Solution:** Make sure you're logged in. Check that the token is valid.

---

## 🎯 Validation Rules

### Username
- Required field
- Must be unique
- Case-insensitive

### Email
- Required field
- Must be valid email format
- Must be unique

### Password
- Required field
- Minimum 6 characters
- No maximum length
- Hashed before storage

### Full Name
- Required field
- No length restrictions

---

## 🔐 Best Practices

### For Users
1. **Use strong passwords** - Mix letters, numbers, and symbols
2. **Don't share credentials** - Keep your login info private
3. **Logout when done** - Especially on shared computers
4. **Remember your password** - No password recovery yet

### For Developers
1. **Change the JWT Key** - Use a strong, unique key in production
2. **Use HTTPS** - Always use SSL in production
3. **Add password recovery** - Implement email-based recovery
4. **Add rate limiting** - Prevent brute force attacks
5. **Use stronger hashing** - Consider bcrypt or Argon2

---

## 📊 User Management

### Viewing Users
Users are stored in `users.json` in the project root.

### Resetting Password
Currently, to reset a password:
1. Stop the application
2. Open `users.json`
3. Delete the user entry
4. Restart the application
5. User can register again

### Deleting Users
1. Stop the application
2. Open `users.json`
3. Remove the user object from the array
4. Save the file
5. Restart the application

---

## 🚀 Future Enhancements

Possible improvements:
- [ ] Password recovery via email
- [ ] Email verification
- [ ] Two-factor authentication (2FA)
- [ ] Remember me functionality
- [ ] Password strength indicator
- [ ] User profile management
- [ ] Admin role and permissions
- [ ] Activity logging
- [ ] Session timeout warnings
- [ ] Database storage (instead of JSON)

---

## 📝 Quick Reference

### Login Credentials Format
```
Username: your-username
Password: your-password
```

### Token Format
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### LocalStorage Keys
```
token  - JWT authentication token
user   - User information (JSON)
```

---

## ✅ Testing Checklist

- [ ] Can register a new account
- [ ] Can login with credentials
- [ ] Can see user name in header
- [ ] Can access employee management
- [ ] Can add/edit/delete employees
- [ ] Can logout successfully
- [ ] Session persists on page refresh
- [ ] Redirects to login when unauthorized
- [ ] Error messages display correctly
- [ ] Password validation works

---

## 🎉 You're All Set!

Your Employee Management System now has a complete authentication system!

### Quick Start:
1. ✅ Application is running at **http://localhost:5000**
2. 📝 Click **"Sign Up"** to create your first account
3. 🔐 Login with your credentials
4. 👥 Start managing employees!

---

**Created:** 2025-10-14  
**Version:** 1.0  
**Status:** ✅ Fully Functional  
**Security:** JWT Token-based Authentication
