namespace ForumAggregator.Domain.Shared.Entities.Moderator;

using ForumAggregator.Domain.Shared.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

public class Moderator : IEntity
{
    // Fields & Properties
    
    private Guid _moderator_id;
    public Guid Id 
    { 
        get => _moderator_id; 
        init => _moderator_id = value; 
    }

    public Guid UserId { get; init; }

    private ICollection<EAuthority> Authorities { get; init; } = default!;

    // Constructors

    private Moderator () {}

    public Moderator(Guid userId, ICollection<EAuthority> authorities)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Authorities = authorities;
    }

    // Methods

    public bool CheckForAuthority(EAuthority authority)
    {
        EAuthority aux = Authorities.FirstOrDefault(x => x == authority);
        return aux == authority;
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

    public static Moderator Load(Guid moderatorId, Guid userId, ICollection<EAuthority> authorities)
    {
        return new Moderator()
        {
            Id = moderatorId,
            UserId = userId,
            Authorities = authorities
        };
    }
}