namespace ForumAggregator.Domain.ForumRegistry;

public enum EAuthority
{
    BlockFromComment = 100,
    BlockFromPost = 101,
    AlterForumName = 102,
    AlterForumDescription = 103,
    AlterModerator = 104,
    DeleteForum = 105,
    DeleteModerator = 106,
    DeleteComment = 107,
    DeletePost = 108,
    AddModerator = 109
}