using HotelManagementSoftware.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Threading.Tasks;
using HotelManagementSoftware.Utils;

namespace HotelManagementSoftware.Business
{
    public class EmployeeBusiness
    {
        // Currently logged in employee
        public Employee? CurrentEmployee { get; set; } = null;

        /// <summary>
        /// Login using a username and password
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <returns>True if login succeeds</returns>
        public async Task<bool> Login(string userName, string password)
        {
            using (var db = new Database())
            {
                Employee? employee = await db.Employees.FirstOrDefaultAsync(i => i.UserName == userName);
                if (employee == null)
                    return false;
                if (employee.Salt == null || employee.HashedPassword == null)
                    return false;
                byte[] salt = Convert.FromBase64String(employee.Salt);
                string hashedPassword = GetHashedPassword(password, salt);
                if (employee.HashedPassword == hashedPassword)
                {
                    CurrentEmployee = employee;
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Get all employees.
        /// </summary>
        /// <returns>List of employees</returns>
        public async Task<List<Employee>> GetAllEmloyees()
        {
            using (var db = new Database())
            {
                return await db.Employees.ToListAsync();
            }
        }

        /// <summary>
        /// Get an employee by ID.
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee</returns>
        public async Task<Employee?> GetEmployeeById(int id)
        {
            using (var db = new Database())
            {
                return await db.Employees
                    .FirstOrDefaultAsync(i => i.EmployeeId == id);
            }
        }

        /// <summary>
        /// Get an employee by CMND number.
        /// </summary>
        /// <param name="cmnd">CMND number</param>
        /// <returns>Employee</returns>
        public async Task<Employee?> GetEmployeeByCmndNumber(string cmnd)
        {
            using (var db = new Database())
            {
                return await db.Employees
                    .FirstOrDefaultAsync(i => i.Cmnd == cmnd);
            }
        }

        /// <summary>
        /// Get an employee by phone number.
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <returns>Employee</returns>
        public async Task<Employee?> GetEmployeeByPhoneNumber(string phoneNumber)
        {
            using (var db = new Database())
            {
                return await db.Employees
                    .FirstOrDefaultAsync(i => i.PhoneNumber == phoneNumber);
            }
        }

        /// <summary>
        /// Get employees with name containing the search term.
        /// </summary>
        /// <param name="searchTerm">Name search term</param>
        /// <returns>Employee</returns>
        public async Task<List<Employee>> GetEmployeesByName(string searchTerm)
        {
            using (var db = new Database())
            {
                return await db.Employees
                    .Where(i => i.Name.Contains(searchTerm))
                    .ToListAsync();
            }
        }

        /// <summary>
        /// Create a new employee.
        /// </summary>
        /// <param name="employee">Employee</param>
        /// <param name="password">Login password of the employee</param>
        public async Task CreateEmployee(Employee employee, string password)
        {
            ValidateEmployee(employee);
            using (var db = new Database())
            {
                byte[] salt = GetNewSalt();
                employee.HashedPassword = GetHashedPassword(password, salt);
                employee.Salt = Convert.ToBase64String(salt);

                db.Employees.Add(employee);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateException err)
                {
                    if (err.InnerException != null
                        && err.InnerException.Message.Contains("duplicate"))
                    {
                        if (err.InnerException.Message.Contains("IdNumber"))
                            throw new ArgumentException("There is already a customer with the same identity number");
                        else if (err.InnerException.Message.Contains("PhoneNumber"))
                            throw new ArgumentException("There is already a customer with the same phone number");
                    }
                    else
                        throw err;
                }
            }
        }

        /// <summary>
        /// Edit an employee.
        /// </summary>
        /// <param name="employee">Updated employee</param>
        public async Task EditEmployee(Employee employee)
        {
            ValidateEmployee(employee);
            using (var db = new Database())
            {
                db.Employees.Update(employee);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateException err)
                {
                    if (err.InnerException != null
                        && err.InnerException.Message.Contains("duplicate"))
                    {
                        if (err.InnerException.Message.Contains("IdNumber"))
                            throw new ArgumentException("There is already a customer with the same identity number");
                        else if (err.InnerException.Message.Contains("PhoneNumber"))
                            throw new ArgumentException("There is already a customer with the same phone number");
                    }
                    else
                        throw err;
                }
            }
        }

        /// <summary>
        /// Change employee's login password.
        /// </summary>
        /// <param name="employee">Employee</param>
        /// <param name="password">New password</param>
        public async Task ChangePassword(Employee employee, string password)
        {
            using (var db = new Database())
            {
                byte[] salt = GetNewSalt();
                employee.HashedPassword = GetHashedPassword(password, salt);
                employee.Salt = Convert.ToBase64String(salt);
                db.Employees.Update(employee);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Delete an employee.
        /// </summary>
        /// <param name="employee">Employee to delete</param>
        public async Task DeleteEmployee(Employee employee)
        {
            using (var db = new Database())
            {
                db.Employees.Remove(employee);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Validate employee's information before adding or updating.
        /// </summary>
        /// <param name="employee">Employee to validate</param>
        /// <exception cref="ArgumentException">Validation failure</exception>
        public void ValidateEmployee(Employee employee)
        {
            if (employee.Name == "")
                throw new ArgumentException("Name cannot be empty");
            if (employee.UserName == "")
                throw new ArgumentException("User name cannot be empty");
            if (employee.BirthDate > DateTime.Now.AddYears(-18))
                throw new ArgumentException("Age cannot be less than 18 years old");
            if (!ValidationUtils.ValidateCmnd(employee.Cmnd))
                throw new ArgumentException("CMND number must have 9 or 12 numbers");
            if (!ValidationUtils.ValidatePhoneNumber(employee.PhoneNumber, "VN"))
                throw new ArgumentException("Phone number is invalid");
            if (employee.Address == "")
                throw new ArgumentException("Address cannot be empty");
            if (employee.Email != null && !ValidationUtils.ValidateEmail(employee.Email))
                throw new ArgumentException("Email is invalid");
        }

        /// <summary>
        /// Get salt for password hasing.
        /// </summary>
        /// <returns>Salt string as byte array</returns>
        public byte[] GetNewSalt()
        {
            return RandomNumberGenerator.GetBytes(128 / 8);
        }

        /// <summary>
        /// Get hashed password from a password and salt.
        /// </summary>
        /// <param name="password">Plain text password to hash</param>
        /// <param name="salt">Salt</param>
        /// <returns></returns>
        public string GetHashedPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }
    }
}
