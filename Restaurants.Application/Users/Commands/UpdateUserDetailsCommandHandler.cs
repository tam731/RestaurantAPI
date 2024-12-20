using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Users.Commands
{
    internal class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger,
        IUserContext userContext,
        IUserStore<User> userStore) : IRequestHandler<UpdateUserDetailsCommand>
    {
        public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();
            logger.LogInformation("Updating user: {userId},with {@Request}", user!.Id, request);

            var dbUser=await userStore.FindByIdAsync(user!.Id,cancellationToken);

            if (dbUser == null) throw new NotFoundException(nameof(dbUser),user!.Id);
            dbUser.Nationality= request.Nationality;
            dbUser.DateOfBirth= request.DateOfBirth;

            await userStore.UpdateAsync(dbUser,cancellationToken);
        }
    }
}
