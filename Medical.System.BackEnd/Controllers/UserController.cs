using Medical.System.Core.DTOs;
using Medical.System.Core.Models.Catalogs;
using Medical.System.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Medical.System.BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController(ICatalogsService catalogsService)
        {
            CatalogsService = catalogsService;
        }

        public ICatalogsService CatalogsService { get; }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserDto userDto)
        {
            var user = new User
            {
                // Generate new ObjectId
                Id = ObjectId.GenerateNewId().ToString(),
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password, // remember to hash this before saving in a real scenario!
            };

            CatalogsService.CreateUserAsync(user);

            // Return a 201 Created response, with the created user in the body, you can replace 'user' with a ReadUserDto if you want to hide the password.
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(string id)
        {
            var user = CatalogsService.GetUserByIdAsync(id);

            if (user == null)
        {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
