namespace ForumAggregator.Domain.Shared.Entities.Moderator;

public enum EAuthority
{
    BlockFromComment = 100,
    BlockFromPost = 101,
    AlterForumName = 102,
    AlterForumDescription = 103,
    DeleteForum = 104,
    DeleteModerator = 105,
    DeleteComment = 106,
    DeletePost = 107,
    AddModerator = 108
}