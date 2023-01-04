namespace ForumAggregator.Domain.ForumRegistry;

using System;
using System.Linq;
using System.Collections.Generic;
using ForumAggregator.Domain.Shared.Interfaces;

public class Forum : IEntity, IAggregateRoot
{
    // Fields & Properties
    
    public Guid Id { get; init; }

    public Guid OwnerId {get; init; }

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public bool Deleted { get; private set; } = default!;

    private ModeratorCollection ModeratorCollection { get; init; } = default!;

    private BlackListedCollection BlackListedCollection { get; init; } = default!;

    // Constructors

    private Forum () {}

    public Forum (Guid ownerId, string name, string description)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Name = name;
        Description = description;
        ModeratorCollection = new ModeratorCollection();
        BlackListedCollection = new BlackListedCollection();
        Deleted = false;

        AssingOwnerAsModerator();        
    }

    // Methods

    private void AssingOwnerAsModerator ()
    {
        EAuthority[] authorities = Enum.GetValues<EAuthority>();
        
        var newModerator = new Moderator(OwnerId, authorities);

        ModeratorCollection.AddModerator(newModerator);
    }

    public ForumResult EditName (Guid editor, string newName)
    {
        if (Deleted)
            return DeletedResult();

        Moderator? aux = ModeratorCollection.GetModeratorByUserId(editor);
        if (aux != null)
        {
            var mod = aux!;
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
            var mod = aux!;
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

    public ForumResult Remove (Guid deleter)
    {
        if (Deleted)
            return DeletedResult();

        Moderator? aux = ModeratorCollection.GetModeratorByUserId(deleter);
        if (aux != null)
        {
            if (ModeratorCollection.GetModeratorsWith(EAuthority.DeleteForum).Count != 1)
            {
                return new ForumResult()
                {
                    Value = false,
                    Result = "Actor User is not the only one that can delete the Forum, therefore it will not be done."
                };
            }

            var mod = aux!;            
            bool value = mod.CheckForAuthority(EAuthority.DeleteForum);
            
            if (value == false)
                return new ForumResult() { Value = value, Result = "Actor User has no authority to delete the Forum." };

            bool moderatorDeleted = true;
            string moderatorResult = string.Empty;
            bool blackListedDeleted = true;
            string blackListedResult = string.Empty;
                
            foreach(var moderator in ModeratorCollection.GetAllModerators())
            {
                var result = ModeratorCollection.RemoveModerator(moderator);
                moderatorDeleted = moderatorDeleted && result.Value;
                moderatorResult = !string.IsNullOrWhiteSpace(result.Result) ? result.Result : moderatorResult;
            }

            foreach(var blacklisted in BlackListedCollection.GetAllBlackListed())
            {
                var result = BlackListedCollection.Remove(blacklisted);
                blackListedDeleted = moderatorDeleted && result.Value;
                blackListedResult = !string.IsNullOrWhiteSpace(result.Result) ? result.Result : blackListedResult;
            }

            if (moderatorDeleted == false)
                return new ForumResult() {Value = false, Result = "Error while deleting Mdoerators." };

            if (blackListedDeleted == false)
                return new ForumResult() {Value = false, Result = "Error while deleting BlackList." };

            Deleted = true;
            return new ForumResult() { Value = true, Result = string.Empty };
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
            Moderator mod = aux!;
            if (!mod.CheckForAuthority(EAuthority.AddModerator))
            {
                return new ForumResult()
                {
                    Value = false,
                    Result = "Actor User has no Authority to add a Moderator."
                };
            }

            bool hasNecessaryAuthorities = true;
            foreach(var authority in authorities)
                hasNecessaryAuthorities = hasNecessaryAuthorities && mod.CheckForAuthority(authority);
            
            if (hasNecessaryAuthorities == false)
            {
                return new ForumResult(){
                    Value = false,
                    Result = "Actor user has to possess all Authorities that are being given."
                };
            }

            var newModerator = new Moderator(newModeratorUserId, authorities);
            ModeratorCollection.AddModerator(newModerator);            
            return new ForumResult()
            {
                Value = true,
                Result = string.Empty
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
            var mod = aux!;
            if (!mod.CheckForAuthority(EAuthority.AlterModerator))
            {
                return new ForumResult()
                {
                    Value = false,
                    Result = "Moderator has no Authority to alter others Authorities."
                };
            }

            bool hasNecessaryAuthorities = true;
            foreach(var authority in newAuthorities)
                hasNecessaryAuthorities = hasNecessaryAuthorities && mod.CheckForAuthority(authority);
            
            if (hasNecessaryAuthorities == false)
            {
                return new ForumResult(){
                    Value = false,
                    Result = "Actor User has to possess all Authorities that are being given."
                };
            }

            Moderator? updated = ModeratorCollection.GetModerator(moderatorId);            
            if (updated != null)
            {
                var result = ModeratorCollection.UpdateModerator(moderatorId, newAuthorities);
                return new ForumResult()
                {
                    Value = result.Value,
                    Result = result.Result
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

    public ForumResult RemoveModerator (Guid actorUserId, Guid moderatorId)
    {
        if (Deleted)
            return DeletedResult();

        Moderator? aux = ModeratorCollection.GetModeratorByUserId(actorUserId);
        if (aux != null)
        {
            Moderator mod = aux!;
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
                var result = ModeratorCollection.RemoveModerator((Moderator)deleted);
                return new ForumResult()
                {
                    Value = result.Value,
                    Result = result.Result
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

    public Moderator? GetModerator (Guid moderatorId)
    {
        if (Deleted)
            return null;
        
        return ModeratorCollection.GetModerator(moderatorId);
    }

    public Moderator? GetModeratorByUserId (Guid userId)
    {
        if (Deleted)
            return null;

        return ModeratorCollection.GetModeratorByUserId(userId);
    }

    public ForumResult AddBlackListed (Guid actorUserId, Guid blackListedUserId, bool? canComment, bool? canPost)
    {
        if (Deleted)
            return DeletedResult();

        Moderator? aux = ModeratorCollection.GetModeratorByUserId(actorUserId);
        if (aux != null)
        {
            BlackListed? alreadyExist = BlackListedCollection.GetByUserId(blackListedUserId);
            if (alreadyExist != null)
                return new ForumResult () { Value = false, Result = "User is already BlackListed." };

            Moderator mod = aux!;
            bool successful = false;

            if (canComment != null && canPost != null && mod.CheckForAuthority(EAuthority.BlockFromComment) && mod.CheckForAuthority(EAuthority.BlockFromPost))
            {
                BlackListedCollection.Add(new BlackListed(blackListedUserId, (bool) canComment, (bool) canPost));
                successful = true;
            }                
            else if (canComment != null && canPost == null && mod.CheckForAuthority(EAuthority.BlockFromComment))
            {
                BlackListedCollection.Add(new BlackListed(blackListedUserId, (bool) canComment, true));
                successful = true;
            }                
            else if (canPost != null && canComment == null && mod.CheckForAuthority(EAuthority.BlockFromPost))
            {
                BlackListedCollection.Add(new BlackListed(blackListedUserId, true, (bool) canPost));
                successful = true;
            }                

            return new ForumResult()
            {
                Value = successful,
                Result = successful ? string.Empty : "Actor User does not have the Authority to proceed with the actions."
            };
        }

        return new ForumResult()
        {
            Value = false,
            Result = "Actor User is not a Moderator."
        };
    }

    public ForumResult UpdateBlackListedCanComment (Guid actorUserId, Guid blackListedUserId, bool canComment)
    {
        if (Deleted)
            return DeletedResult();

        Moderator? aux = ModeratorCollection.GetModeratorByUserId(actorUserId);
        if (aux != null)
        {
            BlackListed? blackListedUserExist = BlackListedCollection.GetByUserId(blackListedUserId);
            if (blackListedUserExist == null)
                return new ForumResult () { Value = false, Result = "User to be updated is not BlackListed." };
            
            BlackListed blackListedUser = (BlackListed) blackListedUserExist;
            Moderator mod = aux!;

            if (mod.CheckForAuthority(EAuthority.BlockFromComment))
            {
                IDomainResult<bool> result = BlackListedCollection.Update(blackListedUserId, canComment, blackListedUser.CanPost);
                return new ForumResult()
                {
                    Value = result.Value,
                    Result = result.Result
                };
            }

            return new ForumResult()
            {
                Value = false,
                Result = "Actor User has no Authority to block other Users from comment."
            };
        }

        return new ForumResult()
        {
            Value = false,
            Result = "Actor User is not a Moderator."
        };
    }

    public ForumResult UpdateBlackListedCanPost (Guid actorUserId, Guid blackListedUserId, bool canPost)
    {
         if (Deleted)
            return DeletedResult();

        Moderator? aux = ModeratorCollection.GetModeratorByUserId(actorUserId);
        if (aux != null)
        {
            BlackListed? blackListedUserExist = BlackListedCollection.GetByUserId(blackListedUserId);
            if (blackListedUserExist == null)
                return new ForumResult () { Value = false, Result = "User to be updated is not BlackListed." };
            
            BlackListed blackListedUser = (BlackListed) blackListedUserExist;
            Moderator mod = aux!;

            if (mod.CheckForAuthority(EAuthority.BlockFromPost))
            {
                IDomainResult<bool> result = BlackListedCollection.Update(blackListedUserId, canPost, blackListedUser.CanComment);
                return new ForumResult()
                {
                    Value = result.Value,
                    Result = result.Result
                };
            }

            return new ForumResult()
            {
                Value = false,
                Result = "Actor User has no Authority to block other Users from comment."
            };
        }

        return new ForumResult()
        {
            Value = false,
            Result = "Actor User is not a Moderator."
        };
    }

    public ForumResult RemoveBlackListed(Guid actorUserId, Guid blackListedUserId)
    {
        if (Deleted)
            return DeletedResult();
        
        Moderator? aux = ModeratorCollection.GetModeratorByUserId(actorUserId);
        if (aux != null)
        {
            BlackListed? blackListedUserExist = BlackListedCollection.GetByUserId(blackListedUserId);            
            if (blackListedUserExist == null)
                return new ForumResult () { Value = false, Result = "User to be removed is not Blacklisted." };

            Moderator mod = aux!;
            BlackListed blackListedUser = (BlackListed) blackListedUserExist;

            if (mod.CheckForAuthority(EAuthority.BlockFromComment) && mod.CheckForAuthority(EAuthority.BlockFromPost))
            {
                IDomainResult<bool> result = BlackListedCollection.Remove(blackListedUser);
                return new ForumResult()
                {
                    Value = result.Value,
                    Result = result.Result
                };
            }

            return new ForumResult()
            {
                Value = false,
                Result = "Actor User has no Authority to remove ther Users from the BlackList."
            };
        }

        return new ForumResult()
        {
            Value = false,
            Result = "Actor User is not a Moderator."
        };
    }

    public BlackListed? GetBlackListedByUserId(Guid userId)
    {
        if (Deleted)
            return null;
        
        return BlackListedCollection.GetByUserId(userId);
    }

    private ForumResult DeletedResult()
    {
        return new ForumResult()
        {
            Value = false,
            Result = "Forum has been removed."
        };
    }

    public static Forum Load (Guid forumId, Guid ownerId, string name, string description, bool deleted, ModeratorCollection moderatorCollection, BlackListedCollection blackListedCollection)
    {
        return new Forum()
        {
            Id = forumId,
            OwnerId = ownerId,
            Name = name,
            Description = description,
            ModeratorCollection = moderatorCollection,
            BlackListedCollection = blackListedCollection,
            Deleted = deleted
        };
    }
}