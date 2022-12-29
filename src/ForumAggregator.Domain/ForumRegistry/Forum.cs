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

    private void AssingOwnerAsModerator ()
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
        Moderator? aux = ModeratorCollection.GetModeratorByUserId(editor);
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
        Moderator? aux = ModeratorCollection.GetModeratorByUserId(editor);
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

    public bool CanDeleteForum (Guid deleter)
    {
        Moderator? aux = ModeratorCollection.GetModeratorByUserId(deleter);
        if (aux != null)
        {
            Moderator mod = (Moderator) aux;
            return mod.CheckForAuthority(EAuthority.DeleteForum);
        }
        return false;
    }

    public ForumServiceResult AddModerator (Guid actorUserId, Guid newModeratorUserId, ICollection<EAuthority> authorities)    
    {
        Moderator? aux = ModeratorCollection.GetModeratorByUserId(actorUserId);
        if (aux != null)
        {
            Moderator mod = (Moderator) aux;
            if (!mod.CheckForAuthority(EAuthority.AddModerator))
            {
                return new ForumServiceResult()
                {
                    Value = false,
                    Result = "Actor user has no Authority to add a Moderator."
                };
            }

            Moderator newModerator = new Moderator(newModeratorUserId, authorities);
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
            Result = "Actor User is not a Moderator."
        };
    }

    public ForumServiceResult UpdateModerator (Guid actorUserId, Guid moderatorId, ICollection<EAuthority> newAuthorities)    
    {
        Moderator? aux = ModeratorCollection.GetModeratorByUserId(actorUserId);
        if (aux != null)
        {
            Moderator mod = (Moderator) aux;
            if (!mod.CheckForAuthority(EAuthority.AlterModerator))
            {
                return new ForumServiceResult()
                {
                    Value = false,
                    Result = "Moderator has no Authority to alter others Authorities."
                };
            }

            Moderator? updated = ModeratorCollection.GetModerator(moderatorId);            
            if (updated != null)
            {
                ModeratorCollection.UpdateModerator(moderatorId, newAuthorities);
                return new ForumServiceResult()
                {
                    Value = true,
                    Result = "Moderator successfully updated."
                };
            }            
            
            return new ForumServiceResult()
            {
                Value = false,
                Result = "Moderator to be updated not found."
            };
        }

        return new ForumServiceResult()
        {
            Value = false,
            Result = "Actor User is not a Moderator."
        };
    }

    public ForumServiceResult RemoveModerator (Guid actor, Guid moderatorId)
    {
        Moderator? aux = ModeratorCollection.GetModeratorByUserId(actor);
        if (aux != null)
        {
            Moderator mod = (Moderator) aux;
            if (!mod.CheckForAuthority(EAuthority.DeleteModerator))
            {
                return new ForumServiceResult()
                {
                    Value = false,
                    Result = "Actor User has no Authority to remove a Moderator."
                };
            }

            Moderator? deleted = ModeratorCollection.GetModerator(moderatorId);
            if (deleted != null)
            {
                ModeratorCollection.RemoveModerator((Moderator)deleted);
                return new ForumServiceResult()
                {
                    Value = true,
                    Result = "Moderator has been successfully removed."
                };
            } 

            return new ForumServiceResult()
            {
                Value = false,
                Result = "Moderator to be removed not found."
            };       
        }

        return new ForumServiceResult()
        {
            Value = false,
            Result = "Actor User is not a Moderator."
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