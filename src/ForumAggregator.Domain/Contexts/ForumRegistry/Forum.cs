namespace ForumAggregator.Domain.ForumRegistry;

using System;
using System.Collections.Generic;
using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.Shared.Entities.Moderator;

public class Forum : IEntity, IAggregateRoot
{
    // Fields & Properties
    
    public Guid Id { get; init; }

    public Guid OwnerId {get; init; }

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public bool Deleted { get; private set; } = default!;

    private ModeratorCollection ModeratorCollection { get; init; } = default!;

    // Constructors

    private Forum () {}

    public Forum (Guid ownerId, string name, string description)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Name = name;
        Description = description;
        ModeratorCollection = new ModeratorCollection();
        AssingOwnerAsModerator();
        Deleted = false;
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

    public ForumResult EditName (Guid editor, string newName)
    {
        if (Deleted)
            return DeletedResult();

        Moderator? aux = ModeratorCollection.GetModeratorByUserId(editor);
        if (aux != null)
        {
            Moderator mod = (Moderator) aux;
            if (mod.CheckForAuthority(EAuthority.AlterForumName))
            {
                Name = newName;
                return new ForumResult()
                {
                    Value = true,
                    Result = string.Empty
                };
            }

            return new ForumResult()
            {
                Value = false,
                Result = "Actor User has no Authority to edit Name."
            };
        }

        return new ForumResult()
        {
            Value = false,
            Result = "Actor User is not a Moderator."
        };
    }

    public ForumResult EditDescription (Guid editor, string newDescription)
    {
        if (Deleted)
            return DeletedResult();

        Moderator? aux = ModeratorCollection.GetModeratorByUserId(editor);
        if (aux != null)
        {
            Moderator mod = (Moderator) aux;
            if (mod.CheckForAuthority(EAuthority.AlterForumName))
            {
                Description = newDescription;
                return new ForumResult()
                {
                    Value = true,
                    Result = string.Empty
                };
            }

            return new ForumResult()
            {
                Value = false,
                Result = "Actor User has no Authority to edit Description."
            };
        }

        return new ForumResult()
        {
            Value = false,
            Result = "Actor User is not a Moderator."
        };
    }

    public ForumResult CanDeleteForum (Guid deleter)
    {
        if (Deleted)
            return DeletedResult();

        Moderator? aux = ModeratorCollection.GetModeratorByUserId(deleter);
        if (aux != null)
        {
            Moderator mod = (Moderator) aux;
            bool value = mod.CheckForAuthority(EAuthority.DeleteForum);
            return new ForumResult()
            {
                Value = value,
                Result = value ? string.Empty : "Actor User has no authority to delete the Forum."
            };
        }

        return new ForumResult()
        {
            Value = false,
            Result = "Actor User is not a Moderator."
        };
    }

    public ForumResult AddModerator (Guid actorUserId, Guid newModeratorUserId, ICollection<EAuthority> authorities)    
    {
        if (Deleted)
            return DeletedResult();

        Moderator? aux = ModeratorCollection.GetModeratorByUserId(actorUserId);
        if (aux != null)
        {
            Moderator mod = (Moderator) aux;
            if (!mod.CheckForAuthority(EAuthority.AddModerator))
            {
                return new ForumResult()
                {
                    Value = false,
                    Result = "Actor user has no Authority to add a Moderator."
                };
            }

            Moderator newModerator = new Moderator(newModeratorUserId, authorities);
            ModeratorCollection.AddModerator(newModerator);            
            return new ForumResult()
            {
                Value = true,
                Result = "Moderator successfully added."
            };
        }
        
        return new ForumResult()
        {
            Value = false,
            Result = "Actor User is not a Moderator."
        };
    }

    public ForumResult UpdateModerator (Guid actorUserId, Guid moderatorId, ICollection<EAuthority> newAuthorities)    
    {
        if (Deleted)
            return DeletedResult();

        Moderator? aux = ModeratorCollection.GetModeratorByUserId(actorUserId);
        if (aux != null)
        {
            Moderator mod = (Moderator) aux;
            if (!mod.CheckForAuthority(EAuthority.AlterModerator))
            {
                return new ForumResult()
                {
                    Value = false,
                    Result = "Moderator has no Authority to alter others Authorities."
                };
            }

            Moderator? updated = ModeratorCollection.GetModerator(moderatorId);            
            if (updated != null)
            {
                ModeratorCollection.UpdateModerator(moderatorId, newAuthorities);
                return new ForumResult()
                {
                    Value = true,
                    Result = "Moderator successfully updated."
                };
            }            
            
            return new ForumResult()
            {
                Value = false,
                Result = "Moderator to be updated not found."
            };
        }

        return new ForumResult()
        {
            Value = false,
            Result = "Actor User is not a Moderator."
        };
    }

    public ForumResult RemoveModerator (Guid actor, Guid moderatorId)
    {
        if (Deleted)
            return DeletedResult();

        Moderator? aux = ModeratorCollection.GetModeratorByUserId(actor);
        if (aux != null)
        {
            Moderator mod = (Moderator) aux;
            if (!mod.CheckForAuthority(EAuthority.DeleteModerator))
            {
                return new ForumResult()
                {
                    Value = false,
                    Result = "Actor User has no Authority to remove a Moderator."
                };
            }

            Moderator? deleted = ModeratorCollection.GetModerator(moderatorId);
            if (deleted != null)
            {
                ModeratorCollection.RemoveModerator((Moderator)deleted);
                return new ForumResult()
                {
                    Value = true,
                    Result = "Moderator has been successfully removed."
                };
            } 

            return new ForumResult()
            {
                Value = false,
                Result = "Moderator to be removed not found."
            };       
        }

        return new ForumResult()
        {
            Value = false,
            Result = "Actor User is not a Moderator."
        };
    }

    private ForumResult DeletedResult()
    {
        return new ForumResult()
        {
            Value = false,
            Result = "Forum has been removed."
        };
    }

    public static Forum Load (Guid forumId, Guid ownerId, string name, string description, bool deleted, ModeratorCollection moderatorCollection)    
    {
        return new Forum()
        {
            Id = forumId,
            OwnerId = ownerId,
            Name = name,
            Description = description,
            ModeratorCollection = moderatorCollection,
            Deleted = deleted
        };
    }
}