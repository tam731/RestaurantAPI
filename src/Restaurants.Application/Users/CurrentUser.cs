namespace Restaurants.Application.Users;

public record CurrentUser(string Id, string Email, IEnumerable<string> Roles,string? Nationality,DateOnly? DateOfBirth)
//public class CurrentUser
{
    //public string Id { get; set; }
    //public string Email { get; set; }
    //public IEnumerable<string> Roles { get; set; }
    //public string? Nationality { get; set; }
    //public DateOnly? DateOfBirth { get; set; }
    //public CurrentUser(string id, string email, IEnumerable<string> roles, string? nationality, DateOnly? dateOfBirth)
    //{
    //    Id = id;
    //    Email = email;
    //    Roles = roles;
    //    Nationality = nationality;
    //    DateOfBirth = dateOfBirth;
    //}
    public bool IsInRole(string role)=>Roles.Contains(role);
}