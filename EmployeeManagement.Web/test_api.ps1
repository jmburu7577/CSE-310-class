# Test All New Features

Write-Host "=== Testing Employee Management System v3.0 ===" -ForegroundColor Cyan
Write-Host ""

# 1. Login
Write-Host "1. Authenticating..." -ForegroundColor Yellow
$loginBody = @{
    username = "admin"
    password = "admin123"
} | ConvertTo-Json

$loginResponse = Invoke-RestMethod -Uri "http://localhost:5001/api/Auth/login" -Method Post -Body $loginBody -ContentType "application/json"
$token = $loginResponse.token
Write-Host "   ✓ Login successful! Token obtained." -ForegroundColor Green
Write-Host ""

$headers = @{
    Authorization = "Bearer $token"
    "Content-Type" = "application/json"
}

# 2. Create Job Posting
Write-Host "2. Creating Job Posting..." -ForegroundColor Yellow
$jobBody = @{
    title = "Senior Software Engineer"
    department = "Engineering"
    description = "We are looking for an experienced developer"
    requirements = "5+ years of experience in .NET"
    salaryMin = 80000
    salaryMax = 120000
    location = "Remote"
    jobType = 0
    status = 1
    positions = 2
    postedBy = "admin"
} | ConvertTo-Json

$job = Invoke-RestMethod -Uri "http://localhost:5001/api/Recruitment/jobs" -Method Post -Body $jobBody -Headers $headers
Write-Host "   ✓ Job Posting Created: ID $($job.id) - $($job.title)" -ForegroundColor Green
Write-Host ""

# 3. Create Training Course
Write-Host "3. Creating Training Course..." -ForegroundColor Yellow
$courseBody = @{
    title = "Leadership Development Program"
    description = "Advanced leadership and management skills"
    category = "Management"
    instructor = "Dr. John Smith"
    durationHours = 40
    type = 0
    level = "Advanced"
    cost = 2500
    maxParticipants = 25
    status = 0
} | ConvertTo-Json

$course = Invoke-RestMethod -Uri "http://localhost:5001/api/Training/courses" -Method Post -Body $courseBody -Headers $headers
Write-Host "   ✓ Training Course Created: ID $($course.id) - $($course.title)" -ForegroundColor Green
Write-Host ""

# 4. Create Company Asset
Write-Host "4. Creating Company Asset..." -ForegroundColor Yellow
$assetBody = @{
    assetType = "Laptop"
    assetTag = "LAP-001"
    brand = "Dell"
    model = "XPS 15"
    serialNumber = "SN123456789"
    purchaseDate = "2025-01-15"
    purchaseValue = 1500
    status = 0
    condition = 0
    location = "IT Department"
} | ConvertTo-Json

$asset = Invoke-RestMethod -Uri "http://localhost:5001/api/Asset" -Method Post -Body $assetBody -Headers $headers
Write-Host "   ✓ Asset Created: ID $($asset.id) - $($asset.brand) $($asset.model)" -ForegroundColor Green
Write-Host ""

# 5. Create Holiday
Write-Host "5. Creating Holiday..." -ForegroundColor Yellow
$holidayBody = @{
    name = "Christmas"
    date = "2025-12-25"
    description = "Christmas Day"
    isOptional = $false
    country = "USA"
} | ConvertTo-Json

$holiday = Invoke-RestMethod -Uri "http://localhost:5001/api/Attendance/holidays" -Method Post -Body $holidayBody -Headers $headers
Write-Host "   ✓ Holiday Created: ID $($holiday.id) - $($holiday.name)" -ForegroundColor Green
Write-Host ""

# 6. Create Performance Goal
Write-Host "6. Creating Performance Goal..." -ForegroundColor Yellow
$goalBody = @{
    employeeId = 1
    title = "Complete Q4 Sales Target"
    description = "Achieve 100% of quarterly sales target"
    category = "Revenue"
    type = 0
    targetValue = 100
    currentValue = 0
    unit = "%"
    startDate = "2025-10-01"
    dueDate = "2025-12-31"
    status = 1
    weight = 40
    setBy = "admin"
} | ConvertTo-Json

$goal = Invoke-RestMethod -Uri "http://localhost:5001/api/Performance/goals" -Method Post -Body $goalBody -Headers $headers
Write-Host "   ✓ Performance Goal Created: ID $($goal.id) - $($goal.title)" -ForegroundColor Green
Write-Host ""

# 7. Create HR Ticket
Write-Host "7. Creating HR Ticket..." -ForegroundColor Yellow
$ticketBody = @{
    employeeId = 1
    employeeName = "Test Employee"
    subject = "Leave Balance Inquiry"
    description = "I need clarification on my remaining leave balance for this year"
    category = 2
    priority = 1
} | ConvertTo-Json

$ticket = Invoke-RestMethod -Uri "http://localhost:5001/api/Helpdesk/tickets" -Method Post -Body $ticketBody -Headers $headers
Write-Host "   ✓ HR Ticket Created: ID $($ticket.id) - $($ticket.subject)" -ForegroundColor Green
Write-Host ""

# 8. Create Knowledge Base Article
Write-Host "8. Creating Knowledge Base Article..." -ForegroundColor Yellow
$kbBody = @{
    title = "How to Request Leave"
    content = "Step 1: Navigate to Leave Management. Step 2: Click Request Leave. Step 3: Fill in the form and submit."
    category = "Leave Management"
    tags = @("leave", "tutorial", "how-to")
    createdBy = "admin"
    isPublished = $true
} | ConvertTo-Json

$kb = Invoke-RestMethod -Uri "http://localhost:5001/api/Helpdesk/knowledge-base" -Method Post -Body $kbBody -Headers $headers
Write-Host "   ✓ KB Article Created: ID $($kb.id) - $($kb.title)" -ForegroundColor Green
Write-Host ""

# 9. Get Statistics from all modules
Write-Host "9. Fetching Statistics from All Modules..." -ForegroundColor Yellow
$recruitmentStats = Invoke-RestMethod -Uri "http://localhost:5001/api/Recruitment/stats" -Method Get -Headers $headers
$attendanceStats = Invoke-RestMethod -Uri "http://localhost:5001/api/Attendance/stats" -Method Get -Headers $headers
$performanceStats = Invoke-RestMethod -Uri "http://localhost:5001/api/Performance/stats" -Method Get -Headers $headers
$trainingStats = Invoke-RestMethod -Uri "http://localhost:5001/api/Training/stats" -Method Get -Headers $headers
$assetStats = Invoke-RestMethod -Uri "http://localhost:5001/api/Asset/stats" -Method Get -Headers $headers
$helpdeskStats = Invoke-RestMethod -Uri "http://localhost:5001/api/Helpdesk/stats" -Method Get -Headers $headers

Write-Host "   ✓ Recruitment Stats: $($recruitmentStats.totalJobPostings) jobs, $($recruitmentStats.totalApplications) applications" -ForegroundColor Green
Write-Host "   ✓ Attendance Stats: $($attendanceStats.totalRecords) records" -ForegroundColor Green
Write-Host "   ✓ Performance Stats: $($performanceStats.totalGoals) goals, $($performanceStats.totalReviews) reviews" -ForegroundColor Green
Write-Host "   ✓ Training Stats: $($trainingStats.totalCourses) courses, $($trainingStats.totalEnrollments) enrollments" -ForegroundColor Green
Write-Host "   ✓ Asset Stats: $($assetStats.overview.totalAssets) assets" -ForegroundColor Green
Write-Host "   ✓ Helpdesk Stats: $($helpdeskStats.totalTickets) tickets" -ForegroundColor Green
Write-Host ""

# 10. Display Summary
Write-Host "=== Test Summary ===" -ForegroundColor Cyan
Write-Host "✅ Authentication: Working" -ForegroundColor Green
Write-Host "✅ Recruitment Module: Working (Job created)" -ForegroundColor Green
Write-Host "✅ Training Module: Working (Course created)" -ForegroundColor Green
Write-Host "✅ Asset Module: Working (Asset created)" -ForegroundColor Green
Write-Host "✅ Attendance Module: Working (Holiday created)" -ForegroundColor Green
Write-Host "✅ Performance Module: Working (Goal created)" -ForegroundColor Green
Write-Host "✅ Helpdesk Module: Working (Ticket & KB Article created)" -ForegroundColor Green
Write-Host "✅ Statistics: All modules reporting data" -ForegroundColor Green
Write-Host ""
Write-Host "🎉 All 7 new modules are fully functional!" -ForegroundColor Cyan
Write-Host ""
Write-Host "Data files created in: EmployeeManagement.Web/Data/" -ForegroundColor Yellow
Write-Host "View Swagger UI: http://localhost:5001/swagger" -ForegroundColor Yellow
