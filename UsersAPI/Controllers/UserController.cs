using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Linq.Expressions;
using UsersAPI.Models;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<User> users = await SelectAllUsers(connection);
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            try
            {
                using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                var user = await connection.QueryFirstAsync("Select * from users where id=@Id", new { Id = userId });

                
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message)
                 ;
             
            }
           
       
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into users(name, email, username, password) values(@Name, @Email, @Username,@Password )", user);
            return Ok(await SelectAllUsers(connection));
        }

        [HttpPut]
        public async Task<ActionResult<List<User>>> UpdateUser(User user)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update users set name=@Name,email=@Email, username=@Username, password=@Password where id=@Id", user);
            return Ok(await SelectAllUsers(connection));
        }
        [HttpDelete("{userId}")]
        public async Task<ActionResult<List<User>>> DeleteUser(int userId)
        {
            try
            {
                using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                await connection.ExecuteAsync("delete from users where id = @Id", new { Id = userId });
                return Ok(await SelectAllUsers(connection));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
                
            }
           
            
        }
        private static async Task<IEnumerable<User>> SelectAllUsers(SqlConnection connection)
        {
            return await connection.QueryAsync<User>("Select * from users");
        }
    }
}
