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
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace WebAPIDemo.Controllers
{
    public class ValuesController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["webapi_conn"].ConnectionString);
        //Employee emp = new Employee();
        [HttpGet]
        public Object GetToken(Login login)
        {
            string RespMessage;
            string Token;
            List<Employee> Data = new List<Employee>();
            string key = "vietstar_28032023"; //secret key which will be used later during validation
            var issuer = "http://vietstar.com";  //normally this will be your site url

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            SqlDataAdapter adapterLogin = new SqlDataAdapter("AA_GetLogin", con);
            adapterLogin.SelectCommand.CommandType = CommandType.StoredProcedure;
            adapterLogin.SelectCommand.Parameters.AddWithValue("@Username", login.Username);
            adapterLogin.SelectCommand.Parameters.AddWithValue("@Password", login.Password);
            DataTable loginData = new DataTable();
            adapterLogin.Fill(loginData);
            if (loginData.Rows.Count > 0)
            {
                SqlDataAdapter adapterGet = new SqlDataAdapter("AA_GetEmployeeById", con);
                adapterGet.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapterGet.SelectCommand.Parameters.AddWithValue("@Id", login.Username);
                DataTable userData = new DataTable();
                adapterGet.Fill(userData);
                if (userData.Rows.Count > 0)
                {
                    for (int i = 0; i < userData.Rows.Count; i++)
                    {
                        Employee DataEmp = new Employee();
                        DataEmp.Id = userData.Rows[i]["idEmployee"].ToString();
                        DataEmp.Name = userData.Rows[i]["nameEmployee"].ToString();
                        DataEmp.Address = userData.Rows[i]["address"].ToString();
                        DataEmp.Center = userData.Rows[i]["center"].ToString();
                        DataEmp.Type = Convert.ToInt16(userData.Rows[i]["otc"]);
                        DataEmp.Password = userData.Rows[i]["password"].ToString();
                        Data.Add(DataEmp);
                    }
                }

                //create a list of claims, keep claims name short   
                var permClaims = new List<Claim>();
                permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                permClaims.Add(new Claim("saoviet", "123456789"));

                //create security token object by giving required parameters    
                var token = new JwtSecurityToken(issuer, //issure    
                                issuer,  //audience    
                                permClaims,
                                expires: DateTime.Now.AddDays(1),
                                signingCredentials: credentials);
                var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
                RespMessage = "Đăng nhập thành công";
                Token = jwt_token;
            }
            else
            {
                RespMessage = "Tên đăng nhập hoặc mật khẩu không hợp lệ";
                Token = null;
                Data = null;
            }
            return new
            {
                RespMessage,
                Token,
                Data
            };

        }

        //[HttpPost]
        //public String GetName1() {
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var identity = User.Identity as ClaimsIdentity;
        //        if (identity != null)
        //        {
        //            IEnumerable<Claim> claims = identity.Claims;
        //        }
        //        return "Valid";
        //    }
        //    else
        //    {
        //        return "Invalid";
        //    }
        //}

        //[Authorize]
        //[HttpPost]
        //public Object GetName2()
        //{
        //    var identity = User.Identity as ClaimsIdentity;
        //    if (identity != null)
        //    {
        //        IEnumerable<Claim> claims = identity.Claims;
        //        var name = claims.Where(p => p.Type == "userid").FirstOrDefault()?.Value;
        //        return new
        //        {
        //            data = name
        //        };

        //    }
        //    return null;
        //}

        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["webapi_conn"].ConnectionString);
        //Employee emp = new Employee();
        //// GET api/values
        ///// <summary>
        ///// Lấy toàn bộ thông tin nhân viên
        ///// </summary>
        ///// <returns></returns>
        //public List<Employee> Get()
        //{
        //    SqlDataAdapter adapter = new SqlDataAdapter("AA_GetEmployee", con);
        //    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
        //    DataTable dt = new DataTable();
        //    adapter.Fill(dt);
        //    List<Employee> listEmployee = new List<Employee>();
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i< dt.Rows.Count; i++)
        //        {
        //            Employee emp = new Employee();
        //            emp.Id = dt.Rows[i]["idEmployee"].ToString();
        //            emp.Name = dt.Rows[i]["nameEmployee"].ToString();
        //            emp.Address = dt.Rows[i]["address"].ToString();
        //            emp.Center = dt.Rows[i]["center"].ToString();
        //            emp.Type = Convert.ToInt16(dt.Rows[i]["otc"]);
        //            listEmployee.Add(emp);
        //        }
        //    }
        //    if (listEmployee.Count > 0)
        //    {
        //        return listEmployee;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        // GET api/values/
        /// <summary>
        /// Lấy thông tin nhân viên theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public Employee GetInformationEmployee(string id)
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
                emp.Password = dt.Rows[0]["password"].ToString();
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

        //// POST api/values
        ///// <summary>
        ///// Thêm nhân viên mới
        ///// </summary>
        ///// <param name="employee"></param>
        ///// <returns></returns>
        //public string Post(Employee employee)
        //{
        //    String msg = "";
        //    if (employee != null)
        //    {
        //        SqlCommand cmd = new SqlCommand("AA_AddEmployee", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Id", employee.Id);
        //        cmd.Parameters.AddWithValue("@Name", employee.Name);
        //        cmd.Parameters.AddWithValue("@Address", employee.Address);
        //        cmd.Parameters.AddWithValue("@Center", employee.Center);
        //        cmd.Parameters.AddWithValue("@Type", employee.Type);

        //        con.Open();
        //        int i = cmd.ExecuteNonQuery();
        //        con.Close();

        //        if (i > 0) 
        //        {
        //            msg = "Data is inserted";
        //        }
        //        else
        //        {
        //            msg = "Error";
        //        }
        //    }
        //    return msg;
        //}

        //// PUT api/values/5
        ///// <summary>
        ///// Cập nhật thông tin nhân viên
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="employee"></param>
        ///// <returns></returns>
        //public string Put(string id, Employee employee)
        //{
        //    String msg = "";
        //    if (employee != null)
        //    {
        //        SqlCommand cmd = new SqlCommand("AA_UpdateEmployeeById", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Id", id);
        //        cmd.Parameters.AddWithValue("@Name", employee.Name);
        //        cmd.Parameters.AddWithValue("@Address", employee.Address);
        //        cmd.Parameters.AddWithValue("@Center", employee.Center);
        //        cmd.Parameters.AddWithValue("@Type", employee.Type);

        //        con.Open();
        //        int i = cmd.ExecuteNonQuery();
        //        con.Close();

        //        if (i > 0)
        //        {
        //            msg = "Data is updated";
        //        }
        //        else
        //        {
        //            msg = "Error";
        //        }
        //    }
        //    return msg;
        //}

        //// DELETE api/values/5
        ///// <summary>
        ///// Xóa nhân viên
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public string Delete(string id)
        //{
        //    String msg = "";
        //    SqlCommand cmd = new SqlCommand("AA_DeleteEmployeeById", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@Id", id);

        //    con.Open();
        //    int i = cmd.ExecuteNonQuery();
        //    con.Close();

        //    if (i > 0)
        //    {
        //        msg = "Data is deleted";
        //    }
        //    else
        //    {
        //        msg = "Error";
        //    }
        //    return msg;
        //}
    }
}
