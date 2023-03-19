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
        /// <summary>
        /// Lấy toàn bộ thông tin nhân viên
        /// </summary>
        /// <returns></returns>
        public List<Employee> Get()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("AA_GetEmployee", con);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Employee> listEmployee = new List<Employee>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i< dt.Rows.Count; i++)
                {
                    Employee emp = new Employee();
                    emp.Id = dt.Rows[i]["idEmployee"].ToString();
                    emp.Name = dt.Rows[i]["nameEmployee"].ToString();
                    emp.Address = dt.Rows[i]["address"].ToString();
                    emp.Center = dt.Rows[i]["center"].ToString();
                    emp.Type = Convert.ToInt16(dt.Rows[i]["otc"]);
                    listEmployee.Add(emp);
                }
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

        // GET api/values/5
        /// <summary>
        /// Lấy thông tin nhân viên theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Employee Get(string id)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("AA_GetEmployeeById", con);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand.Parameters.AddWithValue("@Id", id);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            Employee emp = new Employee();
            if (dt.Rows.Count > 0)
            {
                emp.Id = dt.Rows[0]["idEmployee"].ToString();
                emp.Name = dt.Rows[0]["nameEmployee"].ToString();
                emp.Address = dt.Rows[0]["address"].ToString();
                emp.Center = dt.Rows[0]["center"].ToString();
                emp.Type = Convert.ToInt16(dt.Rows[0]["otc"]);
            }
            if (emp != null)
            {
                return emp;
            }
            else
            {
                return null;
            }
        }

        // POST api/values
        /// <summary>
        /// Thêm nhân viên mới
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        public string Put(string id, Employee employee)
        {
            String msg = "";
            if (employee != null)
            {
                SqlCommand cmd = new SqlCommand("AA_UpdateEmployeeById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Address", employee.Address);
                cmd.Parameters.AddWithValue("@Center", employee.Center);
                cmd.Parameters.AddWithValue("@Type", employee.Type);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i > 0)
                {
                    msg = "Data is updated";
                }
                else
                {
                    msg = "Error";
                }
            }
            return msg;
        }

        // DELETE api/values/5
        /// <summary>
        /// Xóa nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Delete(string id)
        {
            String msg = "";
            SqlCommand cmd = new SqlCommand("AA_DeleteEmployeeById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i > 0)
            {
                msg = "Data is deleted";
            }
            else
            {
                msg = "Error";
            }
            return msg;
        }
    }
}
