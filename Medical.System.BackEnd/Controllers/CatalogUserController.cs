using FluentValidation;
using Medical.System.Core.Models.DTOs;
using Medical.System.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medical.System.BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatalogUserController : ControllerBase
    {
        public ICatalogsService CatalogsService { get; }

        public CatalogUserController(ICatalogsService catalogsService)
        {
            CatalogsService = catalogsService;
        }

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
                var user = await CatalogsService.CreateUserAsync(userDto);
                return Ok(user);
        }
    }
}
