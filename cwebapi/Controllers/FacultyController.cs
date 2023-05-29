using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models.Faculty;


namespace cwebapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public FacultyController(IConfiguration configuration , IWebHostEnvironment env)
        {
            _configuration=configuration;
            _env=env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query=@"
                           select FacultyId , FacultyName , DepartmentName ,
                           convert(varchar(10),DateOfJoining,120) as DateOfJoining , PhotoFileName
                           from 
                           dbo.Faculty   
                          ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CollegeAppCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query,myCon))
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
        public JsonResult Post(Faculty fclty)
        {
            string query=@"
                           insert into dbo.Faculty
                           (FacultyName , DepartmentName , DateOfJoining , PhotoFileName)
                           values(@FacultyName , @DepartmentName , @DateOfJoining , @PhotoFileName)   
                          ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CollegeAppCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query,myCon))
                {
                    myCommand.Parameters.AddWithValue("@FacultyName",fclty.FacultyName);
                    myCommand.Parameters.AddWithValue("@DepartmentName",fclty.DepartmentName);
                    myCommand.Parameters.AddWithValue("@DateOfJoining",fclty.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName",fclty.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);              
        }

        [HttpPut]
        public JsonResult Put(Faculty fclty)
        {
            string query=@"
                           update  dbo.Faculty
                           set FacultyName = @FacultyName,
                           DepartmentName = @DepartmentName,
                           DateOfJoining = @DateOfJoining,
                           PhotoFileName = @PhotoFileName
                           where FacultyId=@FacultyId   
                          ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CollegeAppCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query,myCon))
                {
                    myCommand.Parameters.AddWithValue("@FacultyId",dep.FacultyId);
                    myCommand.Parameters.AddWithValue("@FacultyName",dep.FacultyName);
                    myCommand.Parameters.AddWithValue("@DepartmentName",fclty.DepartmentName);
                    myCommand.Parameters.AddWithValue("@DateOfJoining",fclty.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName",fclty.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);              
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query=@"
                           delete from dbo.Faculty
                           where FacultyId=@FacultyId   
                          ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CollegeAppCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query,myCon))
                {
                    myCommand.Parameters.AddWithValue("@FacultyId",id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);              
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
               var httpRequest = Request.Form;
               var postedFile = httpRequest.Files[0];
               string fileName = postedFile.name;
               var physicalPath = _env.ContentRootPath+"/Resources/"+fileName;

               using (var stream = new FileStream(physicalPath,FileMode.Create))
               {
                  postedFile.CopyTo(stream); 
               }

               return new JsonResult(fileName); 
            }

            catch (System.Exception)
            {
                return new JsonResult("gautam1.png");
                throw;
            }
        }


    }
}