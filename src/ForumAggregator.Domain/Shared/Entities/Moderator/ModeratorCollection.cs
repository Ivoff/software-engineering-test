namespace ForumAggregator.Domain.Shared.Entities.Moderator;

using ForumAggregator.Domain.Shared.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

public class ModeratorCollection
{
    // Fields & Properties

    public ICollection<Moderator> Moderators { get; init; }

    // Constructors

    public ModeratorCollection ()
    {
        Moderators = new List<Moderator>();
    }

    // Methods

    public Moderator? GetModeratorByUserId (Guid userId)
    {
        return Moderators.FirstOrDefault(x => x.UserId == userId);
    }

    public Moderator? GetModerator (Guid moderatorId)
    {
        return Moderators.FirstOrDefault(x => x.Id == moderatorId);
    }

    public ICollection<Moderator?> GetModeratorsWith (EAuthority authority)
    {
        return Moderators.Where(x => x.CheckForAuthority(authority)).ToList<Moderator?>();
    }

    public void AddModerator (Moderator newModerator)
    {
        Moderators.Add(newModerator);
    }

    public bool RemoveModerator (Moderator deletedModerator)
    {
        return Moderators.Remove(deletedModerator);
    }

    public void UpdateModerator (Guid moderatorId, ICollection<EAuthority> authorities)
    {
        Moderator currModerator = Moderators.First(x => x.Id == moderatorId);
        Moderators.Remove(currModerator);

        currModerator.ClearAuthorities();
        currModerator.AddAuthorities(authorities);

        Moderators.Add(currModerator);
    }

    public static ModeratorCollection Load (ICollection<Moderator> moderators)
    {
        return new ModeratorCollection() 
        { 
            Moderators = moderators
        };        
    }

}