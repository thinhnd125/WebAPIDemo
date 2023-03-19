using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    public class CustomerController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["webapi_conn"].ConnectionString);
        Customer cus = new Customer();
        // GET api/values
        /// <summary>
        /// Lấy toàn bộ thông tin khach hang
        /// </summary>
        /// <returns></returns>
        public void GetKH()
        {

        }
    }
}
