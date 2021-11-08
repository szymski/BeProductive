using System.ComponentModel.DataAnnotations;
using BeProductive.Modules.Common.Domain;
using BeProductive.Modules.Rituals.Domain;
using Microsoft.AspNetCore.Identity;

namespace BeProductive.Modules.Users.Domain;

public class User : IdentityUser<int>
{
    public override string UserName { get; set; }

    public string FullName { get; set; }
    public List<Goal> Goals { get; set; }
    public List<Ritual> Rituals { get; set; }

    public override string ToString()
        => $"User {{ {nameof(Id)} = {Id}, {nameof(UserName)} = {UserName} }}";
}