using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WebAPIDemo.Models;
using Newtonsoft.Json;

namespace WebAPIDemo.Controllers
{
    public class ValuesController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["webapi_conn"].ConnectionString);
        Employee emp = new Employee();
        // GET api/values
        public List<Employee> Get()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("AA_GetEmployee", con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Employee> listEmployee = new List<Employee>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i< dt.Rows.Count; i++)
                {
                    Employee emp = new Employee();
                    emp.Id = dt.Rows[i]["Id"].ToString();
                    emp.Name = dt.Rows[i]["Name"].ToString();
                    emp.Address = dt.Rows[i]["Address"].ToString();
                    emp.Center = dt.Rows[i]["Center"].ToString();
                    emp.Type = Convert.ToInt16(dt.Rows[i]["Type"]);
                    listEmployee.Add(emp);
                }
                if (listEmployee.Count > 0)
                {
                    return listEmployee;
                }
                else 
                { 
                    return null;
                }
            }
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public string Post(Employee employee)
        {
            String msg = "";
            if (employee != null)
            {
                SqlCommand cmd = new SqlCommand("AA_AddEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", employee.Id);
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Address", employee.Address);
                cmd.Parameters.AddWithValue("@Center", employee.Center);
                cmd.Parameters.AddWithValue("@Type", employee.Type);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i > 0) 
                {
                    msg = "Data is inserted";
                }
                else
                {
                    msg = "Error";
                }
            }
            return msg;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
