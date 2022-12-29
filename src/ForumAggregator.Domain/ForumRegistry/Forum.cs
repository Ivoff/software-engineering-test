namespace ForumAggregator.Domain.Generic.ForumRegistry;

using System;
using System.Collections.Generic;
using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.Shared.Entities.Moderator;

public class Forum : IEntity, IAggregateRoot
{
    // Fields & Properties

    private Guid _forum_id;
    public Guid Id { 
        get => _forum_id; 
        init => _forum_id = value;
    }

    public Guid OwnerId {get; init; }

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    private ModeratorCollection ModeratorCollection { get; init; } = default!;

    // Constructors

    private Forum () {}

    public Forum (Guid ownerId, string name, string description)
    {
        _forum_id = Guid.NewGuid();
        OwnerId = ownerId;
        Name = name;
        Description = description;
        ModeratorCollection = new ModeratorCollection();
        AssingOwnerAsModerator();
    }

    // Methods

    private void AssingOwnerAsModerator()
    {
        EAuthority[] authorities = {
            EAuthority.BlockFromComment,
            EAuthority.BlockFromPost,
            EAuthority.AlterForumDescription,
            EAuthority.DeleteForum,
            EAuthority.DeleteModerator,
            EAuthority.DeleteComment,
            EAuthority.DeletePost,
            EAuthority.AddModerator
        };
        
        Moderator newModerator = new Moderator(OwnerId, authorities);

        ModeratorCollection.AddModerator(newModerator);
    }

    public bool EditName (Guid editor, string newName)
    {
        Moderator? aux = ModeratorCollection.GetModerator(editor);
        if (aux != null)
        {
            Moderator mod = (Moderator) aux;
            if (mod.CheckForAuthority(EAuthority.AlterForumName))
            {
                Name = newName;
                return true;
            }
        }
        return false;
    }

    public bool EditDescription (Guid editor, string newDescription)
    {
        Moderator? aux = ModeratorCollection.GetModerator(editor);
        if (aux != null)
        {
            Moderator mod = (Moderator) aux;
            if (mod.CheckForAuthority(EAuthority.AlterForumName))
            {
                Description = newDescription;
                return true;
            }
        }
        return false;
    }

    public bool CanDelete(Guid deleter)
    {
        Moderator? aux = ModeratorCollection.GetModerator(deleter);
        if (aux != null)
        {
            Moderator mod = (Moderator) aux;
            return mod.CheckForAuthority(EAuthority.DeleteForum);
        }
        return false;
    }

    public ForumServiceResult AddModerator (Guid actor, Guid userIdNewModerator, ICollection<EAuthority> authorities)    
    {
        Moderator? aux = ModeratorCollection.GetModerator(actor);
        if (aux != null)
        {
            Moderator newModerator = new Moderator(userIdNewModerator, authorities);
            ModeratorCollection.AddModerator(newModerator);
            return new ForumServiceResult()
            {
                Value = true,
                Result = "Moderator successfully added."
            };
        }
        
        return new ForumServiceResult()
        {
            Value = false,
            Result = "User is not a Moderato."
        };
    }

    public ForumServiceResult UpdateModerator (Guid actor, Guid moderatorId, ICollection<EAuthority> authorities)    
    {
        Moderator? aux = ModeratorCollection.GetModerator(actor);
        if (aux != null)
        {
            ModeratorCollection.UpdateModerator(moderatorId, authorities);
            return new ForumServiceResult()
            {
                Value = true,
                Result = "Moderator successfully updated."
            };
        }

        return new ForumServiceResult()
        {
            Value = false,
            Result = "User is not a Moderator."
        };
    }

    public static Forum Load (Guid forumId, Guid ownerId, string name, string description, ModeratorCollection moderatorCollection)    
    {
        return new Forum()
        {
            Id = forumId,
            OwnerId = ownerId,
            Name = name,
            Description = description,
            ModeratorCollection = moderatorCollection            
        };
    }
}