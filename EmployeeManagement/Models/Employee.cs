using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public string id { get; set; }

        [DisplayName("Employee Name")]
        public string employee_name { get; set; }
        [DisplayName("Employee Salary")]
        public string employee_salary { get; set; }
        [DisplayName("Employee Age")]
        public string employee_age { get; set; }
        [DisplayName("Employee Photo")]
        public string profile_image { get; set; }

    }

    public class AllEmployees
    {
        public string status { get; set; }
        public List<Employee> data { get; set; }
        public string message { get; set; } 

    }
}