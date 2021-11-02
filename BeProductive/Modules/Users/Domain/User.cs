using System.ComponentModel.DataAnnotations;
using BeProductive.Modules.Common.Domain;
using Microsoft.AspNetCore.Identity;

namespace BeProductive.Modules.Users.Domain;

public class User : IdentityUser<int>
{
    public override string UserName { get; set; }

    public string FullName { get; set; }

    public override string ToString()
        => $"User {{ {nameof(Id)} = {Id}, {nameof(UserName)} = {UserName} }}";
}