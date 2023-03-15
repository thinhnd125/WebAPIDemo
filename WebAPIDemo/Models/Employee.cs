using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIDemo.Models
{
    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Center { get; set; }
        public int Type { get; set; }
    }
}