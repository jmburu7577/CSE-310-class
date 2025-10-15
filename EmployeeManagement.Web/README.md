# Employee Management System - Web Application

A modern, user-friendly web-based employee management system built with ASP.NET Core Web API and React.

## 🌟 Features

### User Interface
- **Modern Design** - Beautiful gradient UI with TailwindCSS
- **Responsive Layout** - Works on desktop, tablet, and mobile
- **Two View Modes** - Grid cards and table view
- **Real-time Stats** - Dashboard with employee statistics
- **Smooth Animations** - Professional hover effects and transitions

### Functionality
- ✅ **Add Employees** - Create new employee records with full details
- ✅ **Edit Employees** - Update existing employee information
- ✅ **Delete Employees** - Remove employees with confirmation
- ✅ **View Employees** - Browse all employees in grid or table format
- ✅ **Statistics Dashboard** - View total employees, average salary, age, and departments
- ✅ **Data Persistence** - Automatic save to JSON file
- ✅ **RESTful API** - Full CRUD operations via API endpoints

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK or later
- A modern web browser (Chrome, Firefox, Edge, Safari)

### Running the Application

1. **Navigate to the project directory**:
   ```bash
   cd EmployeeManagement.Web
   ```

2. **Run the application**:
   ```bash
   dotnet run
   ```

3. **Open your browser**:
   ```
   http://localhost:5000
   ```

That's it! The application will start and open in your default browser.

## 📁 Project Structure

```
EmployeeManagement.Web/
├── Controllers/
│   └── EmployeesController.cs    # API endpoints
├── Models/
│   └── Employee.cs                # Data models
├── Services/
│   ├── IEmployeeService.cs        # Service interface
│   └── EmployeeService.cs         # Business logic
├── wwwroot/
│   ├── index.html                 # Main HTML page
│   └── app.js                     # React application
├── Program.cs                     # Application entry point
└── appsettings.json              # Configuration
```

## 🎨 User Interface

### Dashboard
- **Total Employees** - Count of all employees
- **Average Salary** - Mean salary across all employees
- **Average Age** - Mean age of employees
- **Departments** - Number of unique departments

### Grid View
- Beautiful card-based layout
- Employee avatar with initials
- Quick access to edit and delete
- Department badges
- Salary and location information

### Table View
- Comprehensive data table
- Sortable columns
- Inline actions
- Professional styling

### Add/Edit Modal
- Clean, organized form
- Required field validation
- Separate sections for basic info and address
- Responsive design

## 🔌 API Endpoints

### Base URL: `/api/employees`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/employees` | Get all employees |
| GET | `/api/employees/{id}` | Get employee by ID |
| POST | `/api/employees` | Create new employee |
| PUT | `/api/employees/{id}` | Update employee |
| DELETE | `/api/employees/{id}` | Delete employee |
| GET | `/api/employees/stats` | Get statistics |

### Example API Usage

#### Get All Employees
```http
GET /api/employees
```

#### Create Employee
```http
POST /api/employees
Content-Type: application/json

{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe",
  "age": 30,
  "department": "Engineering",
  "salary": 75000,
  "address": {
    "street": "123 Main St",
    "city": "New York",
    "state": "NY",
    "zip": "10001"
  }
}
```

#### Update Employee
```http
PUT /api/employees/1
Content-Type: application/json

{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe",
  "age": 31,
  "department": "Engineering",
  "salary": 80000,
  "address": {
    "street": "123 Main St",
    "city": "New York",
    "state": "NY",
    "zip": "10001"
  }
}
```

#### Delete Employee
```http
DELETE /api/employees/1
```

## 💾 Data Storage

Employee data is automatically saved to `employees.json` in the application directory. The file is updated whenever you:
- Add a new employee
- Update an existing employee
- Delete an employee

## 🛠️ Development

### Building the Project
```bash
dotnet build
```

### Running in Development Mode
```bash
dotnet run
```

### Accessing Swagger Documentation
When running in development mode, visit:
```
http://localhost:5000/swagger
```

## 🎯 Technology Stack

### Backend
- **ASP.NET Core 8.0** - Web framework
- **C#** - Programming language
- **Swagger/OpenAPI** - API documentation

### Frontend
- **React 18** - UI framework
- **TailwindCSS** - Styling
- **Font Awesome** - Icons
- **Vanilla JavaScript** - No build process needed

## ✨ Key Features Explained

### Responsive Design
The application automatically adapts to different screen sizes:
- **Desktop**: Full grid layout with 3 columns
- **Tablet**: 2 columns
- **Mobile**: Single column

### Real-time Updates
All changes are immediately reflected in the UI:
- Add employee → Card appears instantly
- Edit employee → Changes update immediately
- Delete employee → Card removed with confirmation

### Data Validation
- Required fields are marked with asterisks
- Form validation prevents empty submissions
- ID uniqueness is enforced
- Numeric fields only accept numbers

### User Experience
- **Smooth animations** on hover and interactions
- **Confirmation dialogs** before destructive actions
- **Loading states** while fetching data
- **Error handling** with user-friendly messages
- **Empty states** with helpful prompts

## 🔒 Security Considerations

For production deployment, consider:
- Adding authentication and authorization
- Implementing HTTPS
- Adding input sanitization
- Implementing rate limiting
- Adding CORS restrictions

## 📝 Future Enhancements

Potential improvements:
- [ ] Search and filter functionality
- [ ] Sorting options
- [ ] Pagination for large datasets
- [ ] Export to CSV/Excel
- [ ] Employee photos
- [ ] Department management
- [ ] Salary history tracking
- [ ] Performance reviews
- [ ] User authentication
- [ ] Role-based access control

## 🐛 Troubleshooting

### Port Already in Use
If port 5000 is already in use, modify `launchSettings.json`:
```json
"applicationUrl": "http://localhost:5001"
```

### Data Not Persisting
Check that the application has write permissions to its directory.

### API Errors
Check the console output for detailed error messages.

## 📄 License

This project is for educational purposes.

## 👨‍💻 Author

Created as part of CSE-310 coursework.

---

**Enjoy managing your employees with style!** 🎉
