using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _configuation;

        public AuthController(IConfiguration configuration) {
            _configuation = configuration;
        }

        [HttpGet]
        [Route("SignIn")]
        public JsonResult SignIn()
        {
            string query = "select * from dbo.Users";
            DataTable table = new DataTable();
            string sqlDataSource = _configuation.GetConnectionString("DBConnection");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        [Route("SignUp")]
        public JsonResult SignUp([FromForm] string newUser)
        {
            string query = "insert into dbo.Users values(@newUser)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuation.GetConnectionString("DBConnection");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@newUser", newUser);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public JsonResult DeleteUser(int id)
        {
            string query = "delete from dbo.Users where id=@id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuation.GetConnectionString("DBConnection");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Delete Successfully");
        }
    }
}
