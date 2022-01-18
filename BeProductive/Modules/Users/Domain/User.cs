using System.ComponentModel.DataAnnotations;
using BeProductive.Modules.Common.Domain;
using BeProductive.Modules.Rewards.Domain;
using BeProductive.Modules.Rituals.Domain;
using Microsoft.AspNetCore.Identity;

namespace BeProductive.Modules.Users.Domain;

public class User : IdentityUser<int>
{
    public override string UserName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public DateTime LastSignedInAt { get; set; } = DateTime.UtcNow; 

    public string FullName { get; set; }
    public List<Goal> Goals { get; set; }
    public List<Ritual> Rituals { get; set; }
    public List<PointClaimEvent> PointEvents { get; set; }

    public override string ToString()
        => $"User {{ {nameof(Id)} = {Id}, {nameof(UserName)} = {UserName} }}";
}