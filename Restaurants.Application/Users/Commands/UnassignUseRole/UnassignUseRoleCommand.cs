using MediatR;

namespace Restaurants.Application.Users.Commands.UnassignUseRole;

public class UnassignUseRoleCommand: IRequest
{
    public string UserEmail { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}
