const { useState, useEffect } = React;

const API_BASE = '/api/employees';
const AUTH_BASE = '/api/auth';

// Auth Helper Functions
const getToken = () => localStorage.getItem('token');
const setToken = (token) => localStorage.setItem('token', token);
const removeToken = () => localStorage.removeItem('token');
const getUser = () => {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
};
const setUserData = (user) => localStorage.setItem('user', JSON.stringify(user));
const removeUser = () => localStorage.removeItem('user');

const authFetch = async (url, options = {}) => {
    const token = getToken();
    const headers = {
        'Content-Type': 'application/json',
        ...options.headers,
    };
    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }
    return fetch(url, { ...options, headers });
};

// Login Component
function LoginForm({ onLogin, onSwitchToSignup }) {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);
        try {
            const response = await fetch(`${AUTH_BASE}/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username, password }),
            });
            const data = await response.json();
            if (data.success) {
                setToken(data.token);
                setUserData(data.user);
                onLogin(data.user);
            } else {
                setError(data.message || 'Login failed');
            }
        } catch (error) {
            setError('An error occurred during login');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-purple-600 to-violet-700 px-4">
            <div className="max-w-md w-full bg-white rounded-2xl shadow-2xl p-8">
                <div className="text-center mb-8">
                    <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-purple-600 to-violet-600 rounded-full mb-4">
                        <i className="fas fa-users text-white text-2xl"></i>
                    </div>
                    <h2 className="text-3xl font-bold text-gray-800">Welcome Back</h2>
                    <p className="text-gray-600 mt-2">Sign in to Employee Management System</p>
                </div>
                {error && (
                    <div className="mb-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded-lg">
                        <i className="fas fa-exclamation-circle mr-2"></i>{error}
                    </div>
                )}
                <form onSubmit={handleSubmit} className="space-y-6">
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2">
                            <i className="fas fa-user mr-2"></i>Username
                        </label>
                        <input type="text" value={username} onChange={(e) => setUsername(e.target.value)}
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                            placeholder="Enter your username" required />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2">
                            <i className="fas fa-lock mr-2"></i>Password
                        </label>
                        <input type="password" value={password} onChange={(e) => setPassword(e.target.value)}
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                            placeholder="Enter your password" required />
                    </div>
                    <button type="submit" disabled={loading}
                        className="w-full bg-gradient-to-r from-purple-600 to-violet-600 text-white py-3 rounded-lg font-semibold hover:from-purple-700 hover:to-violet-700 transition-all duration-200 shadow-lg disabled:opacity-50">
                        {loading ? <><i className="fas fa-spinner fa-spin mr-2"></i>Signing in...</> : <><i className="fas fa-sign-in-alt mr-2"></i>Sign In</>}
                    </button>
                </form>
                <div className="mt-6 text-center">
                    <p className="text-gray-600">Don't have an account? <button onClick={onSwitchToSignup} className="text-purple-600 hover:text-purple-700 font-semibold">Sign Up</button></p>
                </div>
            </div>
        </div>
    );
}

// Signup Component
function SignupForm({ onSignup, onSwitchToLogin }) {
    const [formData, setFormData] = useState({ username: '', email: '', password: '', confirmPassword: '', fullName: '' });
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        if (formData.password !== formData.confirmPassword) {
            setError('Passwords do not match');
            return;
        }
        if (formData.password.length < 6) {
            setError('Password must be at least 6 characters long');
            return;
        }
        setLoading(true);
        try {
            const response = await fetch(`${AUTH_BASE}/register`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username: formData.username, email: formData.email, password: formData.password, fullName: formData.fullName }),
            });
            const data = await response.json();
            if (data.success) {
                setToken(data.token);
                setUserData(data.user);
                onSignup(data.user);
            } else {
                setError(data.message || 'Registration failed');
            }
        } catch (error) {
            setError('An error occurred during registration');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-purple-600 to-violet-700 px-4 py-8">
            <div className="max-w-md w-full bg-white rounded-2xl shadow-2xl p-8">
                <div className="text-center mb-8">
                    <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-purple-600 to-violet-600 rounded-full mb-4">
                        <i className="fas fa-user-plus text-white text-2xl"></i>
                    </div>
                    <h2 className="text-3xl font-bold text-gray-800">Create Account</h2>
                    <p className="text-gray-600 mt-2">Join Employee Management System</p>
                </div>
                {error && (
                    <div className="mb-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded-lg">
                        <i className="fas fa-exclamation-circle mr-2"></i>{error}
                    </div>
                )}
                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2"><i className="fas fa-user mr-2"></i>Full Name</label>
                        <input type="text" name="fullName" value={formData.fullName} onChange={handleChange}
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                            placeholder="Enter your full name" required />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2"><i className="fas fa-user-circle mr-2"></i>Username</label>
                        <input type="text" name="username" value={formData.username} onChange={handleChange}
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                            placeholder="Choose a username" required />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2"><i className="fas fa-envelope mr-2"></i>Email</label>
                        <input type="email" name="email" value={formData.email} onChange={handleChange}
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                            placeholder="Enter your email" required />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2"><i className="fas fa-lock mr-2"></i>Password</label>
                        <input type="password" name="password" value={formData.password} onChange={handleChange}
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                            placeholder="Create a password (min 6 characters)" required />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2"><i className="fas fa-lock mr-2"></i>Confirm Password</label>
                        <input type="password" name="confirmPassword" value={formData.confirmPassword} onChange={handleChange}
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                            placeholder="Confirm your password" required />
                    </div>
                    <button type="submit" disabled={loading}
                        className="w-full bg-gradient-to-r from-purple-600 to-violet-600 text-white py-3 rounded-lg font-semibold hover:from-purple-700 hover:to-violet-700 transition-all duration-200 shadow-lg disabled:opacity-50">
                        {loading ? <><i className="fas fa-spinner fa-spin mr-2"></i>Creating Account...</> : <><i className="fas fa-user-plus mr-2"></i>Sign Up</>}
                    </button>
                </form>
                <div className="mt-6 text-center">
                    <p className="text-gray-600">Already have an account? <button onClick={onSwitchToLogin} className="text-purple-600 hover:text-purple-700 font-semibold">Sign In</button></p>
                </div>
            </div>
        </div>
    );
}

// Main App Component with Authentication
function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [currentUser, setCurrentUser] = useState(null);
    const [showSignup, setShowSignup] = useState(false);
    const [employees, setEmployees] = useState([]);
    const [stats, setStats] = useState(null);
    const [loading, setLoading] = useState(true);
    const [showModal, setShowModal] = useState(false);
    const [editingEmployee, setEditingEmployee] = useState(null);
    const [view, setView] = useState('grid');

    useEffect(() => {
        const token = getToken();
        const user = getUser();
        if (token && user) {
            setIsAuthenticated(true);
            setCurrentUser(user);
        } else {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        if (isAuthenticated) {
            fetchEmployees();
            fetchStats();
        }
    }, [isAuthenticated]);

    const handleLogin = (user) => {
        setCurrentUser(user);
        setIsAuthenticated(true);
        setShowSignup(false);
    };

    const handleSignup = (user) => {
        setCurrentUser(user);
        setIsAuthenticated(true);
        setShowSignup(false);
    };

    const handleLogout = () => {
        removeToken();
        removeUser();
        setIsAuthenticated(false);
        setCurrentUser(null);
        setEmployees([]);
        setStats(null);
    };

    const fetchEmployees = async () => {
        try {
            const response = await authFetch(API_BASE);
            if (response.status === 401) {
                handleLogout();
                return;
            }
            const data = await response.json();
            setEmployees(data);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching employees:', error);
            setLoading(false);
        }
    };

    const fetchStats = async () => {
        try {
            const response = await authFetch(`${API_BASE}/stats`);
            if (response.status === 401) {
                handleLogout();
                return;
            }
            const data = await response.json();
            setStats(data);
        } catch (error) {
            console.error('Error fetching stats:', error);
        }
    };

    if (!isAuthenticated) {
        if (showSignup) {
            return <SignupForm onSignup={handleSignup} onSwitchToLogin={() => setShowSignup(false)} />;
        }
        return <LoginForm onLogin={handleLogin} onSwitchToSignup={() => setShowSignup(true)} />;
    }

    const handleAddEmployee = () => {
        setEditingEmployee(null);
        setShowModal(true);
    };

    const handleEditEmployee = (employee) => {
        setEditingEmployee(employee);
        setShowModal(true);
    };

    const handleDeleteEmployee = async (id) => {
        if (!confirm('Are you sure you want to delete this employee?')) return;

        try {
            const response = await authFetch(`${API_BASE}/${id}`, {
                method: 'DELETE',
            });

            if (response.ok) {
                fetchEmployees();
                fetchStats();
            } else if (response.status === 401) {
                handleLogout();
            }
        } catch (error) {
            console.error('Error deleting employee:', error);
        }
    };

    const handleSaveEmployee = async (employee) => {
        try {
            const url = editingEmployee ? `${API_BASE}/${editingEmployee.id}` : API_BASE;
            const method = editingEmployee ? 'PUT' : 'POST';

            const response = await authFetch(url, {
                method,
                body: JSON.stringify(employee),
            });

            if (response.ok) {
                setShowModal(false);
                fetchEmployees();
                fetchStats();
            } else if (response.status === 401) {
                handleLogout();
            } else {
                const error = await response.text();
                alert(error);
            }
        } catch (error) {
            console.error('Error saving employee:', error);
            alert('Error saving employee');
        }
    };

    return (
        <div className="min-h-screen bg-gray-50">
            {/* Header */}
            <header className="gradient-bg text-white shadow-lg">
                <div className="container mx-auto px-4 py-6">
                    <div className="flex items-center justify-between">
                        <div className="flex items-center space-x-3">
                            <i className="fas fa-users text-3xl"></i>
                            <h1 className="text-3xl font-bold">Employee Management System</h1>
                        </div>
                        <div className="flex items-center space-x-4">
                            <div className="text-right mr-4">
                                <p className="text-sm opacity-90">Welcome,</p>
                                <p className="font-semibold">{currentUser?.fullName || currentUser?.username}</p>
                                <p className="text-xs opacity-75">
                                    <i className="fas fa-shield-alt mr-1"></i>{currentUser?.role}
                                </p>
                            </div>
                            {currentUser?.permissions?.includes('Employees.Add') && (
                                <button
                                    onClick={handleAddEmployee}
                                    className="bg-white text-purple-600 px-6 py-2 rounded-lg font-semibold hover:bg-gray-100 transition flex items-center space-x-2"
                                >
                                    <i className="fas fa-plus"></i>
                                    <span>Add Employee</span>
                                </button>
                            )}
                            <a
                                href="/leaves.html"
                                className="bg-green-500 text-white px-6 py-2 rounded-lg font-semibold hover:bg-green-600 transition flex items-center space-x-2"
                            >
                                <i className="fas fa-umbrella-beach"></i>
                                <span>Leaves</span>
                            </a>
                            <a
                                href="/payslips.html"
                                className="bg-yellow-500 text-white px-6 py-2 rounded-lg font-semibold hover:bg-yellow-600 transition flex items-center space-x-2"
                            >
                                <i className="fas fa-file-invoice-dollar"></i>
                                <span>Payslips</span>
                            </a>
                            {currentUser?.permissions?.includes('Users.Manage') && (
                                <button
                                    onClick={() => window.location.href = '/user-management.html'}
                                    className="bg-blue-500 text-white px-6 py-2 rounded-lg font-semibold hover:bg-blue-600 transition flex items-center space-x-2"
                                >
                                    <i className="fas fa-users-cog"></i>
                                    <span>Manage Users</span>
                                </button>
                            )}
                            <button
                                onClick={handleLogout}
                                className="bg-red-500 text-white px-6 py-2 rounded-lg font-semibold hover:bg-red-600 transition flex items-center space-x-2"
                            >
                                <i className="fas fa-sign-out-alt"></i>
                                <span>Logout</span>
                            </button>
                        </div>
                    </div>
                </div>
            </header>

            {/* Stats Dashboard */}
            {stats && <StatsSection stats={stats} />}

            {/* View Toggle */}
            <div className="container mx-auto px-4 py-4">
                <div className="flex justify-end space-x-2">
                    <button
                        onClick={() => setView('grid')}
                        className={`px-4 py-2 rounded-lg ${view === 'grid' ? 'bg-purple-600 text-white' : 'bg-white text-gray-700'}`}
                    >
                        <i className="fas fa-th-large"></i> Grid
                    </button>
                    <button
                        onClick={() => setView('table')}
                        className={`px-4 py-2 rounded-lg ${view === 'table' ? 'bg-purple-600 text-white' : 'bg-white text-gray-700'}`}
                    >
                        <i className="fas fa-table"></i> Table
                    </button>
                </div>
            </div>

            {/* Employee List */}
            <main className="container mx-auto px-4 py-8">
                {loading ? (
                    <div className="text-center py-12">
                        <i className="fas fa-spinner fa-spin text-4xl text-purple-600"></i>
                        <p className="mt-4 text-gray-600">Loading employees...</p>
                    </div>
                ) : employees.length === 0 ? (
                    <div className="text-center py-12 bg-white rounded-lg shadow">
                        <i className="fas fa-users text-6xl text-gray-300"></i>
                        <p className="mt-4 text-xl text-gray-600">No employees found</p>
                        <button
                            onClick={handleAddEmployee}
                            className="mt-4 bg-purple-600 text-white px-6 py-2 rounded-lg hover:bg-purple-700"
                        >
                            Add Your First Employee
                        </button>
                    </div>
                ) : view === 'grid' ? (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {employees.map(employee => (
                            <EmployeeCard
                                key={employee.id}
                                employee={employee}
                                onEdit={handleEditEmployee}
                                onDelete={handleDeleteEmployee}
                                canEdit={currentUser?.permissions?.includes('Employees.Edit')}
                                canDelete={currentUser?.permissions?.includes('Employees.Delete')}
                            />
                        ))}
                    </div>
                ) : (
                    <EmployeeTable
                        employees={employees}
                        onEdit={handleEditEmployee}
                        onDelete={handleDeleteEmployee}
                        canEdit={currentUser?.permissions?.includes('Employees.Edit')}
                        canDelete={currentUser?.permissions?.includes('Employees.Delete')}
                    />
                )}
            </main>

            {/* Modal */}
            {showModal && (
                <EmployeeModal
                    employee={editingEmployee}
                    onClose={() => setShowModal(false)}
                    onSave={handleSaveEmployee}
                />
            )}
        </div>
    );
}

// Stats Section Component
function StatsSection({ stats }) {
    return (
        <div className="bg-white border-b">
            <div className="container mx-auto px-4 py-6">
                <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
                    <StatCard
                        icon="fa-users"
                        title="Total Employees"
                        value={stats.totalEmployees}
                        color="blue"
                    />
                    <StatCard
                        icon="fa-dollar-sign"
                        title="Average Salary"
                        value={`$${stats.averageSalary.toFixed(2)}`}
                        color="green"
                    />
                    <StatCard
                        icon="fa-calendar"
                        title="Average Age"
                        value={stats.averageAge.toFixed(1)}
                        color="purple"
                    />
                    <StatCard
                        icon="fa-building"
                        title="Departments"
                        value={stats.departmentCount}
                        color="orange"
                    />
                </div>
            </div>
        </div>
    );
}

// Stat Card Component
function StatCard({ icon, title, value, color }) {
    const colors = {
        blue: 'bg-blue-100 text-blue-600',
        green: 'bg-green-100 text-green-600',
        purple: 'bg-purple-100 text-purple-600',
        orange: 'bg-orange-100 text-orange-600',
    };

    return (
        <div className="bg-gray-50 rounded-lg p-4 flex items-center space-x-4">
            <div className={`${colors[color]} p-3 rounded-lg`}>
                <i className={`fas ${icon} text-2xl`}></i>
            </div>
            <div>
                <p className="text-sm text-gray-600">{title}</p>
                <p className="text-2xl font-bold text-gray-800">{value}</p>
            </div>
        </div>
    );
}

// Employee Card Component
function EmployeeCard({ employee, onEdit, onDelete, canEdit, canDelete }) {
    return (
        <div className="bg-white rounded-lg shadow-md p-6 card-hover">
            <div className="flex items-start justify-between mb-4">
                <div className="flex items-center space-x-3">
                    <div className="bg-purple-100 text-purple-600 w-12 h-12 rounded-full flex items-center justify-center text-xl font-bold">
                        {employee.firstName[0]}{employee.lastName[0]}
                    </div>
                    <div>
                        <h3 className="text-lg font-semibold text-gray-800">
                            {employee.firstName} {employee.lastName}
                        </h3>
                        <p className="text-sm text-gray-500">ID: {employee.id}</p>
                    </div>
                </div>
                <span className="bg-purple-100 text-purple-600 px-3 py-1 rounded-full text-xs font-semibold">
                    {employee.department}
                </span>
            </div>

            <div className="space-y-2 mb-4">
                <div className="flex items-center text-sm text-gray-600">
                    <i className="fas fa-birthday-cake w-5"></i>
                    <span>{employee.age} years old</span>
                </div>
                <div className="flex items-center text-sm text-gray-600">
                    <i className="fas fa-money-bill-wave w-5"></i>
                    <span className="font-semibold text-green-600">${employee.salary.toLocaleString()}</span>
                </div>
                <div className="flex items-start text-sm text-gray-600">
                    <i className="fas fa-map-marker-alt w-5 mt-1"></i>
                    <span>{employee.address.street}, {employee.address.city}, {employee.address.state} {employee.address.zip}</span>
                </div>
            </div>

            {(canEdit || canDelete) && (
                <div className="flex space-x-2 pt-4 border-t">
                    {canEdit && (
                        <button
                            onClick={() => onEdit(employee)}
                            className="flex-1 bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600 transition"
                        >
                            <i className="fas fa-edit"></i> Edit
                        </button>
                    )}
                    {canDelete && (
                        <button
                            onClick={() => onDelete(employee.id)}
                            className="flex-1 bg-red-500 text-white py-2 rounded-lg hover:bg-red-600 transition"
                        >
                            <i className="fas fa-trash"></i> Delete
                        </button>
                    )}
                </div>
            )}
        </div>
    );
}

// Employee Table Component
function EmployeeTable({ employees, onEdit, onDelete, canEdit, canDelete }) {
    return (
        <div className="bg-white rounded-lg shadow-md overflow-hidden">
            <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gray-50">
                    <tr>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">ID</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Name</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Age</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Department</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Salary</th>
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Location</th>
                        {(canEdit || canDelete) && (
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
                        )}
                    </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                    {employees.map(employee => (
                        <tr key={employee.id} className="hover:bg-gray-50">
                            <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{employee.id}</td>
                            <td className="px-6 py-4 whitespace-nowrap">
                                <div className="flex items-center">
                                    <div className="bg-purple-100 text-purple-600 w-8 h-8 rounded-full flex items-center justify-center text-xs font-bold mr-3">
                                        {employee.firstName[0]}{employee.lastName[0]}
                                    </div>
                                    <div className="text-sm font-medium text-gray-900">
                                        {employee.firstName} {employee.lastName}
                                    </div>
                                </div>
                            </td>
                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{employee.age}</td>
                            <td className="px-6 py-4 whitespace-nowrap">
                                <span className="px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-purple-100 text-purple-800">
                                    {employee.department}
                                </span>
                            </td>
                            <td className="px-6 py-4 whitespace-nowrap text-sm text-green-600 font-semibold">
                                ${employee.salary.toLocaleString()}
                            </td>
                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                {employee.address.city}, {employee.address.state}
                            </td>
                            {(canEdit || canDelete) && (
                                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium space-x-2">
                                    {canEdit && (
                                        <button
                                            onClick={() => onEdit(employee)}
                                            className="text-blue-600 hover:text-blue-900"
                                        >
                                            <i className="fas fa-edit"></i>
                                        </button>
                                    )}
                                    {canDelete && (
                                        <button
                                            onClick={() => onDelete(employee.id)}
                                            className="text-red-600 hover:text-red-900"
                                        >
                                            <i className="fas fa-trash"></i>
                                        </button>
                                    )}
                                </td>
                            )}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

// Employee Modal Component
function EmployeeModal({ employee, onClose, onSave }) {
    const [formData, setFormData] = useState(employee || {
        id: '',
        firstName: '',
        lastName: '',
        age: '',
        department: '',
        salary: '',
        address: {
            street: '',
            city: '',
            state: '',
            zip: ''
        }
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        if (name.startsWith('address.')) {
            const addressField = name.split('.')[1];
            setFormData({
                ...formData,
                address: {
                    ...formData.address,
                    [addressField]: value
                }
            });
        } else {
            setFormData({
                ...formData,
                [name]: name === 'id' || name === 'age' ? parseInt(value) || '' : 
                        name === 'salary' ? parseFloat(value) || '' : value
            });
        }
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSave(formData);
    };

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
            <div className="bg-white rounded-lg shadow-xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
                <div className="gradient-bg text-white p-6 rounded-t-lg">
                    <h2 className="text-2xl font-bold">
                        {employee ? 'Edit Employee' : 'Add New Employee'}
                    </h2>
                </div>

                <form onSubmit={handleSubmit} className="p-6 space-y-6">
                    {/* Basic Information */}
                    <div>
                        <h3 className="text-lg font-semibold text-gray-800 mb-4">Basic Information</h3>
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Employee ID <span className="text-red-500">*</span>
                                </label>
                                <input
                                    type="number"
                                    name="id"
                                    value={formData.id}
                                    onChange={handleChange}
                                    disabled={!!employee}
                                    required
                                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent disabled:bg-gray-100"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Age <span className="text-red-500">*</span>
                                </label>
                                <input
                                    type="number"
                                    name="age"
                                    value={formData.age}
                                    onChange={handleChange}
                                    required
                                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    First Name <span className="text-red-500">*</span>
                                </label>
                                <input
                                    type="text"
                                    name="firstName"
                                    value={formData.firstName}
                                    onChange={handleChange}
                                    required
                                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Last Name <span className="text-red-500">*</span>
                                </label>
                                <input
                                    type="text"
                                    name="lastName"
                                    value={formData.lastName}
                                    onChange={handleChange}
                                    required
                                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Department <span className="text-red-500">*</span>
                                </label>
                                <input
                                    type="text"
                                    name="department"
                                    value={formData.department}
                                    onChange={handleChange}
                                    required
                                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Salary <span className="text-red-500">*</span>
                                </label>
                                <input
                                    type="number"
                                    step="0.01"
                                    name="salary"
                                    value={formData.salary}
                                    onChange={handleChange}
                                    required
                                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                                />
                            </div>
                        </div>
                    </div>

                    {/* Address Information */}
                    <div>
                        <h3 className="text-lg font-semibold text-gray-800 mb-4">Address</h3>
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <div className="md:col-span-2">
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Street <span className="text-red-500">*</span>
                                </label>
                                <input
                                    type="text"
                                    name="address.street"
                                    value={formData.address.street}
                                    onChange={handleChange}
                                    required
                                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    City <span className="text-red-500">*</span>
                                </label>
                                <input
                                    type="text"
                                    name="address.city"
                                    value={formData.address.city}
                                    onChange={handleChange}
                                    required
                                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    State <span className="text-red-500">*</span>
                                </label>
                                <input
                                    type="text"
                                    name="address.state"
                                    value={formData.address.state}
                                    onChange={handleChange}
                                    required
                                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                                />
                            </div>
                            <div className="md:col-span-2">
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    ZIP Code <span className="text-red-500">*</span>
                                </label>
                                <input
                                    type="text"
                                    name="address.zip"
                                    value={formData.address.zip}
                                    onChange={handleChange}
                                    required
                                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                                />
                            </div>
                        </div>
                    </div>

                    {/* Action Buttons */}
                    <div className="flex justify-end space-x-3 pt-4 border-t">
                        <button
                            type="button"
                            onClick={onClose}
                            className="px-6 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50 transition"
                        >
                            Cancel
                        </button>
                        <button
                            type="submit"
                            className="px-6 py-2 bg-purple-600 text-white rounded-lg hover:bg-purple-700 transition"
                        >
                            {employee ? 'Update Employee' : 'Add Employee'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

// Render the app
ReactDOM.render(<App />, document.getElementById('root'));
