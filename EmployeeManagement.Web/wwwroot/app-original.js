const { useState, useEffect } = React;

const API_BASE = '/api/employees';

// Main App Component
function App() {
    const [employees, setEmployees] = useState([]);
    const [stats, setStats] = useState(null);
    const [loading, setLoading] = useState(true);
    const [showModal, setShowModal] = useState(false);
    const [editingEmployee, setEditingEmployee] = useState(null);
    const [view, setView] = useState('grid'); // 'grid' or 'table'

    useEffect(() => {
        fetchEmployees();
        fetchStats();
    }, []);

    const fetchEmployees = async () => {
        try {
            const response = await fetch(API_BASE);
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
            const response = await fetch(`${API_BASE}/stats`);
            const data = await response.json();
            setStats(data);
        } catch (error) {
            console.error('Error fetching stats:', error);
        }
    };

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
            const response = await fetch(`${API_BASE}/${id}`, {
                method: 'DELETE',
            });

            if (response.ok) {
                fetchEmployees();
                fetchStats();
            }
        } catch (error) {
            console.error('Error deleting employee:', error);
        }
    };

    const handleSaveEmployee = async (employee) => {
        try {
            const url = editingEmployee ? `${API_BASE}/${editingEmployee.id}` : API_BASE;
            const method = editingEmployee ? 'PUT' : 'POST';

            const response = await fetch(url, {
                method,
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(employee),
            });

            if (response.ok) {
                setShowModal(false);
                fetchEmployees();
                fetchStats();
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
                        <button
                            onClick={handleAddEmployee}
                            className="bg-white text-purple-600 px-6 py-2 rounded-lg font-semibold hover:bg-gray-100 transition flex items-center space-x-2"
                        >
                            <i className="fas fa-plus"></i>
                            <span>Add Employee</span>
                        </button>
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
                            />
                        ))}
                    </div>
                ) : (
                    <EmployeeTable
                        employees={employees}
                        onEdit={handleEditEmployee}
                        onDelete={handleDeleteEmployee}
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
function EmployeeCard({ employee, onEdit, onDelete }) {
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

            <div className="flex space-x-2 pt-4 border-t">
                <button
                    onClick={() => onEdit(employee)}
                    className="flex-1 bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600 transition"
                >
                    <i className="fas fa-edit"></i> Edit
                </button>
                <button
                    onClick={() => onDelete(employee.id)}
                    className="flex-1 bg-red-500 text-white py-2 rounded-lg hover:bg-red-600 transition"
                >
                    <i className="fas fa-trash"></i> Delete
                </button>
            </div>
        </div>
    );
}

// Employee Table Component
function EmployeeTable({ employees, onEdit, onDelete }) {
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
                        <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
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
                            <td className="px-6 py-4 whitespace-nowrap text-sm font-medium space-x-2">
                                <button
                                    onClick={() => onEdit(employee)}
                                    className="text-blue-600 hover:text-blue-900"
                                >
                                    <i className="fas fa-edit"></i>
                                </button>
                                <button
                                    onClick={() => onDelete(employee.id)}
                                    className="text-red-600 hover:text-red-900"
                                >
                                    <i className="fas fa-trash"></i>
                                </button>
                            </td>
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
