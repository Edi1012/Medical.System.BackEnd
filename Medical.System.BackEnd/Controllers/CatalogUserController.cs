using Medical.System.Core.DTOs;
using Medical.System.Core.Models.Catalogs;
using Medical.System.Core.Services.Interfaces;
using Medical.System.Core.Validator.Catalogs.User;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Medical.System.BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatalogUserController : ControllerBase
    {
        private readonly CreateUserValidator _createUserValidator;

        public CatalogUserController(ICatalogsService catalogsService)
        {
            CatalogsService = catalogsService;
            _createUserValidator = new CreateUserValidator(CatalogsService);
        }

        public ICatalogsService CatalogsService { get; }




        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/user/Create
        ///     {
        ///        "userName": "johndoe",
        ///        "password": "Password1!"
        ///     }
        ///
        /// </remarks>
        /// <param name="userDto">A CreateUserDto object that contains the details of the new user to be created.</param>
        /// <returns>A newly created User</returns>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If the validation fails</response>  
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            var user = new User
            {
                // Generate new ObjectId
                Id = ObjectId.GenerateNewId().ToString(),
                UserName = userDto.UserName,
                Password = userDto.Password, // remember to hash this before saving in a real scenario!
            };


            var validationResult = await _createUserValidator.ValidateAsync(userDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await CatalogsService.CreateUserAsync(user);

            //// Return a 201 Created response, with the created user in the body, you can replace 'user' with a ReadUserDto if you want to hide the password.
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
