namespace ForumAggregator.Infraestructure.Repository;

using Microsoft.EntityFrameworkCore;
using ForumAggregator.Domain.Shared.Interfaces;
using AutoMapper;
using System.Linq;
using ForumAggregator.Infraestructure.DbContext;
using System;

public class ForumRepository : IForumRepository
{
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public ForumRepository(DatabaseContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Domain.ForumRegistry.Forum? Get(Guid forumId)
    {
        Models.Forum? forum = _dbContext.Forums
            .Include(forum => forum.BlackList)
            .Include(forum => forum.Moderators)
            .ThenInclude(moderator => moderator.ModeratorAuthorities)
            .Where(forum => forum.Id == forumId)
            .FirstOrDefault();

        if (forum == null)
            return null;

        var domainForum = _mapper.Map<Domain.ForumRegistry.Forum>(forum);
        var domainModerators = _mapper.Map<ICollection<Infraestructure.Models.Moderator>, ICollection<Domain.ForumRegistry.Moderator>>(forum.Moderators).ToList();
        var domainBlackList = _mapper.Map<ICollection<Infraestructure.Models.BlackListed>, ICollection<Domain.ForumRegistry.BlackListed>>(forum.BlackList).ToList();

        var resultForum = Domain.ForumRegistry.Forum.Load(
            domainForum.Id,
            domainForum.OwnerId,
            domainForum.Name,
            domainForum.Description,
            domainForum.Deleted,
            Domain.ForumRegistry.ModeratorCollection.Load(domainModerators),
            Domain.ForumRegistry.BlackListedCollection.Load(domainBlackList)
        );

        return resultForum;
    }

    public Domain.ForumRegistry.Forum? GetByName(string forumName)
    {
        Models.Forum? forum = _dbContext.Forums
            .Include(forum => forum.BlackList)
            .Include(forum => forum.Moderators)
            .ThenInclude(moderator => moderator.ModeratorAuthorities)
            .FirstOrDefault(forum => forum.Name == forumName);

        if (forum == null)
            return null;

        var domainForum = _mapper.Map<Domain.ForumRegistry.Forum>(forum);
        var domainModerators = _mapper.Map<ICollection<Infraestructure.Models.Moderator>, ICollection<Domain.ForumRegistry.Moderator>>(forum.Moderators).ToList();
        var domainBlackList = _mapper.Map<ICollection<Infraestructure.Models.BlackListed>, ICollection<Domain.ForumRegistry.BlackListed>>(forum.BlackList).ToList();

        var resultForum = Domain.ForumRegistry.Forum.Load(
            domainForum.Id,
            domainForum.OwnerId,
            domainForum.Name,
            domainForum.Description,
            domainForum.Deleted,
            Domain.ForumRegistry.ModeratorCollection.Load(domainModerators),
            Domain.ForumRegistry.BlackListedCollection.Load(domainBlackList)
        );

        return resultForum;
    }

    public bool Save(Domain.ForumRegistry.Forum forum)
    {
        var forumExist = _dbContext.Forums.FirstOrDefault(x => x.Id == forum.Id);

        if (forumExist == null)
        {
            var newForum = _mapper.Map<Infraestructure.Models.Forum>(forum);

            newForum.Moderators = _mapper.Map<
                ICollection<Domain.ForumRegistry.Moderator>, ICollection<Infraestructure.Models.Moderator>
            >(forum.ModeratorCollection.Moderators).ToList();

            newForum.BlackList = _mapper.Map<
                ICollection<Domain.ForumRegistry.BlackListed>, ICollection<Infraestructure.Models.BlackListed>
            >(forum.BlackListedCollection.BlackList).ToList();

            _dbContext.Forums.Add(newForum);
        }
        else
        {
            forumExist.Name = forum.Name;
            forumExist.Description = forum.Description;
            forumExist.Deleted = forum.Deleted;

            _dbContext.Forums.Update(forumExist);
        }

        return _dbContext.SaveChanges() > 0;
    }

    public bool SaveModerator(Guid forumId, Domain.ForumRegistry.Moderator moderator)
    {
        var trackedModerator = _dbContext.Moderators
            .Include(x => x.ModeratorAuthorities)
            .FirstOrDefault(x => x.Id == moderator.Id);

        if (trackedModerator == null)
        {
            var newModerator = _mapper.Map<Models.Moderator>(moderator);
            newModerator.ForumId = forumId;
            _dbContext.Moderators.Add(newModerator);
        }
        else
        {
            if (moderator.Deleted == true)
            {
                trackedModerator.Deleted = true;
                _dbContext.Moderators.Update(trackedModerator);
            }
            else 
            {
                // TODO: only save authorities that have changed.
                List<Domain.ForumRegistry.EAuthority> authorities = moderator.Authorities.ToList();

                foreach (var auth in trackedModerator.ModeratorAuthorities)
                {
                    authorities.RemoveAt(authorities.IndexOf(auth.Authority));
                }

                foreach (var auth in authorities)
                {
                    _dbContext.ModeratorAuthorities.Add(
                        new Models.ModeratorAuthority()
                        {
                            ModeratorId = moderator.Id,
                            Authority = auth
                        }
                    );
                }
            }
        }

        return _dbContext.SaveChanges() > 0;
    }

    public bool SaveBlackListed(Guid forumId, Domain.ForumRegistry.BlackListed blackListed)
    {
        var trackedBlackListed = _dbContext.BlackList
            .FirstOrDefault(x => x.Id == blackListed.Id);
        
        if (trackedBlackListed == null)
        {
            var newBlackListed = _mapper.Map<Models.BlackListed>(blackListed);
            newBlackListed.ForumId = forumId;
            
            _dbContext.BlackList.Add(newBlackListed);
        }
        else
        {
            trackedBlackListed.CanComment = blackListed.CanComment;
            trackedBlackListed.CanPost = blackListed.CanPost;
            trackedBlackListed.Deleted = blackListed.Deleted;

            _dbContext.BlackList.Update(trackedBlackListed);
        }

        return _dbContext.SaveChanges() > 0;
    }

    public ICollection<Domain.ForumRegistry.Forum> GetAll()
    {
        return _dbContext.Forums.Select(x => _mapper.Map<Domain.ForumRegistry.Forum>(x)).ToList();
    }

    private string PrintForum(Domain.ForumRegistry.Forum forum)
    {
        string forumString = $"Forum Id: {forum.Id}\nForum OwnerId: {forum.OwnerId}\nForum Name: {forum.Name}\nForum Description: {forum.Description}\nDeleted: {forum.Deleted}\n";
        foreach (var moderator in forum.ModeratorCollection.Moderators)
        {
            forumString += $"\tModerator Id: {moderator.Id}\n\tModerator UserId: {moderator.UserId}\n";
            foreach (var authority in moderator.Authorities)
            {
                forumString += $"\t\tAuthority: {authority.ToString()}\n";
            }
        }
        foreach (var blackListed in forum.BlackListedCollection.BlackList)
        {
            forumString += $"\tBlackListed Id: {blackListed.Id}\n\tBlackListed UserId: {blackListed.UserId}\n\tBlackListed CanComment: {blackListed.CanComment}\n\tBlackListed CanPost: {blackListed.CanPost}\n";
        }

        Console.WriteLine(forumString);

        return forumString;
    }

    private string PrintForum(Infraestructure.Models.Forum forum)
    {
        string forumString = $"Forum Id: {forum.Id}\nForum OwnerId: {forum.OwnerId}\nForum Name: {forum.Name}\nForum Description: {forum.Description}\nDeleted: {forum.Deleted}\n";
        foreach (var moderator in forum.Moderators)
        {
            forumString += $"\tModerator Id: {moderator.Id}\n\tModerator UserId: {moderator.UserId}\n";
            foreach (var authority in moderator.ModeratorAuthorities)
            {
                forumString += $"\t\tAuthority: {authority.Authority.ToString()}\n";
            }
        }
        // foreach(var blackListed in forum.BlackList)
        // {
        // forumString += $"\tBlackListed Id: {blackListed.Id}\n\tBlackListed UserId: {blackListed.UserId}\n\tBlackListed CanComment: {blackListed.CanComment}\n\tBlackListed CanPost: {blackListed.CanPost}\n";
        // }

        Console.WriteLine(forumString);

        return forumString;
    }
}