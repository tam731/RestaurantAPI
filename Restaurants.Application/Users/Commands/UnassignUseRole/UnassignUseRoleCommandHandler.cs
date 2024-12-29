using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users.Commands.AssignUseRole;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Users.Commands.UnassignUseRole;

public class UnassignUseRoleCommandHandler(ILogger<UnassignUseRoleCommandHandler> logger,
  UserManager<User> userManager,
  RoleManager<IdentityRole> roleManager) : IRequestHandler<UnassignUseRoleCommand>
{
    public async Task Handle(UnassignUseRoleCommand request, CancellationToken cancellationToken)
{
    logger.LogInformation("Assigning role {RoleName} to user {UserEmail}", request.RoleName, request.UserEmail);

    var user = await userManager.FindByEmailAsync(request.UserEmail)
         ?? throw new NotFoundException(nameof(User), request.UserEmail);
    var role = await roleManager.FindByNameAsync(request.RoleName)
         ?? throw new NotFoundException(nameof(IdentityRole), request.RoleName);

    await userManager.RemoveFromRoleAsync(user, role.Name!);
}

}