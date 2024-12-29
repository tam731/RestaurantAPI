using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands.AssignUseRole;
using Restaurants.Application.Users.Commands.UnassignUseRole;
using Restaurants.Application.Users.Commands.UpdateUserDatails;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class IdentityController(IMediator mediator):ControllerBase
    {
        [HttpPatch("user")]
        [Authorize]
        public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPost("user-role")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AssignUseRole(AssignUseRoleCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("user-role")]
        [Authorize(Roles =UserRoles.Admin)]
        public async Task<IActionResult> UnassignUseRole(UnassignUseRoleCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }
    }
}
