using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIDemo.Models
{
    public class LoginResponse
    {
        public string RespMessage { get; set; }
        public string Token { get; set; }
        public List<Object> EmployeeList { get; set; }
    }
}