namespace ForumAggregator.UnitTests;

using System;
using System.Linq;
using ForumAggregator.Domain.ForumRegistry;
using ForumAggregator.Domain.Shared.Interfaces;

public class ForumTests
{
    [Fact]
    public void OwnerBecomesModerator()
    {
        Guid userId = Guid.NewGuid();
        Forum forum = new Forum(userId, "Test Forum", "This is the Test Forum, welcome.");
        Moderator? ownerModeratorCanBeNull = forum.GetModeratorByUserId(userId);
        
        Assert.True(ownerModeratorCanBeNull != null);

        if (ownerModeratorCanBeNull != null)
        {
            Moderator ownerModerator = (Moderator) ownerModeratorCanBeNull;
            Assert.All<EAuthority>(Enum.GetValues<EAuthority>().ToList<EAuthority>(), x => Assert.True(ownerModerator.CheckForAuthority(x)));
        }
    }

    [Fact]
    public void OwnerEditForumName()
    {
        Guid userId = Guid.NewGuid();
        Forum forum = Forum.Load(
            Guid.NewGuid(),
            userId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{new Moderator(userId, Enum.GetValues<EAuthority>())}
            ),
            new BlackListedCollection()
        );

        string newForumName = "Lorem Ipsum";
        IDomainResult<bool> result = forum.EditName(userId, newForumName);
        
        Assert.True(result.Value, result.Result);
        Assert.True(forum.Name.Equals(newForumName));
    }

    [Fact]
    public void OwnerEditForumDescription()
    {
        Guid userId = Guid.NewGuid();
        Forum forum = Forum.Load(
            Guid.NewGuid(),
            userId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{new Moderator(userId, Enum.GetValues<EAuthority>())}
            ),
            new BlackListedCollection()
        );

        string newForumDescription = "Some new description I don't know";
        IDomainResult<bool> result = forum.EditDescription(userId, newForumDescription);
        
        Assert.True(result.Value, result.Result);
        Assert.True(forum.Description.Equals(newForumDescription));
    }

    [Fact]
    public void OwnerShouldDeleteForum()
    {
        Guid userId = Guid.NewGuid();
        Forum forum = Forum.Load(
            Guid.NewGuid(),
            userId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{new Moderator(userId, Enum.GetValues<EAuthority>())}
            ),
            new BlackListedCollection()
        );

        IDomainResult<bool> result = forum.Remove(userId);

        Assert.True(result.Value, result.Result);
        Assert.True(forum.Deleted);
    }

    [Fact]
    public void OwnerShouldNotDeleteForum()
    {
        Guid userId = Guid.NewGuid();
        Guid otherUserId = Guid.NewGuid();

        Forum forum = Forum.Load(
            Guid.NewGuid(),
            userId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    new Moderator(userId, Enum.GetValues<EAuthority>()),
                    new Moderator(otherUserId, new List<EAuthority>{EAuthority.DeleteForum}),
                }
            ),
            new BlackListedCollection()
        );

        IDomainResult<bool> result = forum.Remove(userId);

        Assert.False(result.Value, result.Result);
        Assert.False(forum.Deleted);
    }

    [Fact]
    public void OwnerShoudDeleteModerator()
    {
        Guid userId = Guid.NewGuid();
        Guid otherUserId = Guid.NewGuid();
        Moderator otherModerator = new Moderator(otherUserId, new List<EAuthority>{EAuthority.DeleteForum});

        Forum forum = Forum.Load(
            Guid.NewGuid(),
            userId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    new Moderator(userId, Enum.GetValues<EAuthority>()),
                    otherModerator
                }
            ),
            new BlackListedCollection()
        );

        IDomainResult<bool> result = forum.RemoveModerator(userId, otherModerator.Id);

        Assert.True(result.Value, result.Result);
        Assert.True(forum.GetModeratorByUserId(otherUserId) == null, "Moderator should not be considered in searches.");
    }

    [Fact]
    public void ModeratorHasNoAuthorityToRemoveModerator()
    {
        Guid ownerUserId = Guid.NewGuid();
        Guid moderatorUserId = Guid.NewGuid();
        Guid otherModeratorUserId = Guid.NewGuid();
        
        Moderator moderator = new Moderator(
            moderatorUserId, 
            new List<EAuthority>{
                EAuthority.AlterForumDescription, 
                EAuthority.BlockFromComment
            }
        );
        
        Moderator otherModerator = new Moderator(
            otherModeratorUserId, 
            new List<EAuthority>{
                EAuthority.BlockFromPost, 
                EAuthority.BlockFromComment
            }
        );

        Forum forum = Forum.Load(
            Guid.NewGuid(),
            ownerUserId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    new Moderator(ownerUserId, Enum.GetValues<EAuthority>()),
                    moderator,
                    otherModerator
                }
            ),
            new BlackListedCollection()
        );

        IDomainResult<bool> result = forum.RemoveModerator(moderatorUserId, otherModerator.Id);

        Assert.False(result.Value, result.Result);
        Assert.False(forum.GetModeratorByUserId(otherModeratorUserId) == null, "Moderator should be considered in searches.");
    }

    [Fact]
    public void ModeratorShouldAddModerator()
    {
        Guid ownerUserId = Guid.NewGuid();
        Guid moderatorUserId = Guid.NewGuid();
        Guid otherModeratorUserId = Guid.NewGuid();

        Moderator moderator = new Moderator(
            moderatorUserId, 
            new List<EAuthority>{
                EAuthority.AlterForumDescription,
                EAuthority.BlockFromComment,
                EAuthority.AddModerator
            }
        );

        Forum forum = Forum.Load(
            Guid.NewGuid(),
            moderatorUserId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    new Moderator(ownerUserId, Enum.GetValues<EAuthority>()),
                    moderator
                }
            ),
            new BlackListedCollection()
        );

        IDomainResult<bool> result = forum.AddModerator(
            moderatorUserId, 
            otherModeratorUserId, 
            new List<EAuthority>{
                EAuthority.AlterForumDescription,
                EAuthority.BlockFromComment,
            }
        );

        Assert.True(result.Value, result.Result);
        Assert.True(forum.GetModeratorByUserId(otherModeratorUserId) != null, "Moderator should have been created.");
    }

    [Fact]
    public void ModeratorShouldNotAddModerator()
    {
        Guid ownerUserId = Guid.NewGuid();
        Guid moderatorUserId = Guid.NewGuid();
        Guid otherModeratorUserId = Guid.NewGuid();

        Moderator moderator = new Moderator(
            moderatorUserId, 
            new List<EAuthority>{
                EAuthority.AlterForumDescription,
                EAuthority.BlockFromComment,
                EAuthority.AddModerator
            }
        );

        Forum forum = Forum.Load(
            Guid.NewGuid(),
            moderatorUserId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    new Moderator(ownerUserId, Enum.GetValues<EAuthority>()),
                    moderator
                }
            ),
            new BlackListedCollection()
        );

        IDomainResult<bool> result = forum.AddModerator(
            moderatorUserId, 
            otherModeratorUserId, 
            new List<EAuthority>{
                EAuthority.AlterForumDescription,
                EAuthority.BlockFromComment,
                EAuthority.DeleteForum
            }
        );

        Assert.False(result.Value, result.Result);
        Assert.False(forum.GetModeratorByUserId(otherModeratorUserId) != null, "Moderator should not have been created.");
    }

    [Fact]
    public void ModeratorShouldNotAddModerator2()
    {
        Guid ownerUserId = Guid.NewGuid();
        Guid moderatorUserId = Guid.NewGuid();
        Guid otherModeratorUserId = Guid.NewGuid();

        Moderator moderator = new Moderator(
            moderatorUserId, 
            new List<EAuthority>{
                EAuthority.AlterForumDescription,
                EAuthority.BlockFromComment
            }
        );

        Forum forum = Forum.Load(
            Guid.NewGuid(),
            moderatorUserId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    new Moderator(ownerUserId, Enum.GetValues<EAuthority>()),
                    moderator
                }
            ),
            new BlackListedCollection()
        );

        IDomainResult<bool> result = forum.AddModerator(
            moderatorUserId, 
            otherModeratorUserId, 
            new List<EAuthority>{
                EAuthority.AlterForumDescription,
                EAuthority.BlockFromComment
            }
        );

        Assert.False(result.Value, result.Result);
        Assert.False(forum.GetModeratorByUserId(otherModeratorUserId) != null, "Moderator should not have been created.");
    }

    [Fact]
    public void ModeratorShouldUpdateModerator()
    {
        Guid ownerUserId = Guid.NewGuid();
        Guid moderatorUserId = Guid.NewGuid();
        Guid otherModeratorUserId = Guid.NewGuid();

         Moderator moderator = new Moderator(
            moderatorUserId, 
            new List<EAuthority>{
                EAuthority.BlockFromComment,
                EAuthority.AlterForumName,
                EAuthority.AlterForumDescription,
                EAuthority.AddModerator,
                EAuthority.AlterModerator
            }
        );

        Moderator otherModerator = new Moderator(
            otherModeratorUserId, 
            new List<EAuthority>{                
                EAuthority.BlockFromComment
            }
        );

         Forum forum = Forum.Load(
            Guid.NewGuid(),
            moderatorUserId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    new Moderator(ownerUserId, Enum.GetValues<EAuthority>()),
                    moderator,
                    otherModerator
                }
            ),
            new BlackListedCollection()
        );

        ICollection<EAuthority> authorities = otherModerator.GetAuthorities();
        authorities.Add(EAuthority.AlterForumName);
        authorities.Add(EAuthority.AlterForumDescription);
        
        IDomainResult<bool> result = forum.UpdateModerator(moderatorUserId, otherModerator.Id, authorities);

        Assert.True(result.Value, result.Result);
        Assert.All<EAuthority>(authorities, x => Assert.True(otherModerator.CheckForAuthority(x)));
    }

    [Fact]
    public void ModeratorShouldNotUpdateModerator()
    {
        Guid ownerUserId = Guid.NewGuid();
        Guid moderatorUserId = Guid.NewGuid();
        Guid otherModeratorUserId = Guid.NewGuid();

         Moderator moderator = new Moderator(
            moderatorUserId, 
            new List<EAuthority>{
                EAuthority.BlockFromComment,
                EAuthority.AlterForumName,
                EAuthority.AlterForumDescription,
                EAuthority.AddModerator,
                EAuthority.AlterModerator
            }
        );

        Moderator otherModerator = new Moderator(
            otherModeratorUserId, 
            new List<EAuthority>{                
                EAuthority.BlockFromComment
            }
        );

         Forum forum = Forum.Load(
            Guid.NewGuid(),
            moderatorUserId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    new Moderator(ownerUserId, Enum.GetValues<EAuthority>()),
                    moderator,
                    otherModerator
                }
            ),
            new BlackListedCollection()
        );

        ICollection<EAuthority> authorities = otherModerator.GetAuthorities();
        authorities.Add(EAuthority.AlterForumName);
        authorities.Add(EAuthority.AlterForumDescription);
        authorities.Add(EAuthority.DeleteModerator);

        IDomainResult<bool> result = forum.UpdateModerator(moderatorUserId, otherModerator.Id, authorities);        
        Assert.False(result.Value, result.Result);

        bool notMissing = true;
        foreach(EAuthority authority in authorities)
            notMissing = notMissing && otherModerator.CheckForAuthority(authority);
        Assert.False(notMissing, "The moderator should have the same authorities from before the operation");
    }
}