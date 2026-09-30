using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public DateTime HireDate { get; set; }

        public bool IsActive { get; set; }

        public User User { get; set; } = null!;

        public Department Department { get; set; } = null!;
    }
}
