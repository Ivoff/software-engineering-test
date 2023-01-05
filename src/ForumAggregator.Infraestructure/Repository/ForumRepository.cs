namespace ForumAggregator.Infraestructure.Repository;

using Microsoft.EntityFrameworkCore;
using ForumAggregator.Domain.Shared.Interfaces;
using AutoMapper;
using System.Linq;
using ForumAggregator.Infraestructure.DbContext;
using System;

public class ForumRepository: IForumRepository
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
            .Where(forum => forum.Name == forumName)
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

            _dbContext.Add(newForum);
        }
        else
        {
            forumExist.Name = forum.Name;
            forumExist.Description = forum.Description;
            forumExist.Deleted = forum.Deleted;
            
            forumExist.Moderators = _mapper.Map<
                ICollection<Domain.ForumRegistry.Moderator>, ICollection<Infraestructure.Models.Moderator>
            >(forum.ModeratorCollection.Moderators).ToList();

            forumExist.BlackList = _mapper.Map<
                ICollection<Domain.ForumRegistry.BlackListed>, ICollection<Infraestructure.Models.BlackListed>
            >(forum.BlackListedCollection.BlackList).ToList();

            _dbContext.Update(forumExist);
        }

        return _dbContext.SaveChanges() > 0;
    }

    private void PrintForum(Domain.ForumRegistry.Forum forum)
    {
        string forumString = $"{forum.Id}\n{forum.OwnerId}\n{forum.Name}\n{forum.Description}\n{forum.Deleted}\n";
        foreach(var moderator in forum.ModeratorCollection.Moderators)
        {
            forumString += $"\t{moderator.Id}\n\t{moderator.UserId}\n";
            foreach(var authority in moderator.Authorities)
            {
                forumString += $"\t\t{authority.ToString()}\n";
            }
        }
        foreach(var blackListed in forum.BlackListedCollection.BlackList)
        {
            forumString += $"\t{blackListed.Id}\n\t{blackListed.UserId}\n\t{blackListed.CanComment}\n\t{blackListed.CanPost}\n";
        }
        
        Console.WriteLine(forumString);
    }
}