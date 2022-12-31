namespace ForumAggregator.Domain.ForumRegistry;

using ForumAggregator.Domain.Shared.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

public class Moderator : IEntity
{
    // Fields & Properties
    
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    private ICollection<EAuthority> Authorities { get; init; } = default!;

    public bool Deleted { get; private set; } = default!;

    // Constructors

    private Moderator () {}

    public Moderator(Guid userId, ICollection<EAuthority> authorities)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Authorities = authorities;
        Deleted = false;
    }

    // Methods

    public bool CheckForAuthority(EAuthority authority)
    {
        EAuthority aux = Authorities.FirstOrDefault(x => x == authority);
        return aux == authority;
    }

    public ICollection<EAuthority> GetAuthorities()
    {
        return new List<EAuthority>(Authorities);
    }

    public void AddAuthority(EAuthority authority)
    {
        Authorities.Add(authority);
    }

    public void AddAuthorities(ICollection<EAuthority> authorities)
    {
        foreach (EAuthority authority in authorities)
        {
            Authorities.Add(authority);
        }
    }

    public bool RemoveAuthority(EAuthority authority)
    {
        return Authorities.Remove(authority);
    }

    public void ClearAuthorities()
    {
        Authorities.Clear();
    }

    public void Delete()
    {
        Deleted = true;
    }

    public static Moderator Load(Guid moderatorId, Guid userId, bool deleted, ICollection<EAuthority> authorities)
    {
        return new Moderator()
        {
            Id = moderatorId,
            UserId = userId,
            Authorities = authorities,
            Deleted = deleted
        };
    }
}