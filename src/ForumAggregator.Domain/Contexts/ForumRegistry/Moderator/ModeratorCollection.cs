namespace ForumAggregator.Domain.ForumRegistry;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class ModeratorCollection
{
    // Fields & Properties

    // IMPORTANT: This is public so it can be checked in tests, is should never be accessed directly
    public ICollection<Moderator> Moderators { get; init; }

    // Constructors

    public ModeratorCollection ()
    {
        Moderators = new List<Moderator>();
    }

    // Methods

    public Moderator? GetModeratorByUserId (Guid userId)
    {
        return Moderators.FirstOrDefault(x => x.UserId == userId && x.Deleted == false);
    }

    public Moderator? GetModerator (Guid moderatorId)
    {
        return Moderators.FirstOrDefault(x => x.Id == moderatorId && x.Deleted == false);
    }

    public ICollection<Moderator> GetModeratorsWith (EAuthority authority)
    {
        return Moderators.Where(x => x.CheckForAuthority(authority)).ToList<Moderator>();
    }

    public IReadOnlyCollection<Moderator> GetAllModerators()
    {
        return new ReadOnlyCollection<Moderator>(Moderators.Where(x => x.Deleted == false).ToList());
    }

    public void AddModerator (Moderator newModerator)
    {
        Moderators.Add(newModerator);
    }

    public ModeratorResult RemoveModerator (Moderator deletedModerator)
    {
        return deletedModerator.Delete();
    }

    public ModeratorResult UpdateModerator (Guid moderatorId, ICollection<EAuthority> authorities)
    {
        var currModerator = Moderators.First(x => x.Id == moderatorId && x.Deleted == false);
        var result = currModerator.ClearAuthorities();
        currModerator.AddAuthorities(authorities);

        return result;
    }

    public static ModeratorCollection Load (ICollection<Moderator> moderators)
    {
        return new ModeratorCollection() 
        { 
            Moderators = moderators
        };        
    }

}