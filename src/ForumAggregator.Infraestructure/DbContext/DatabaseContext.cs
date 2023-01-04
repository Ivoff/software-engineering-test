namespace ForumAggregator.Infraestructure.DbContext;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using ForumAggregator.Infraestructure.Models;


public class DatabaseContext: DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Forum> Forums { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<BlackListed> BlackList { get; set; } = null!;
    public DbSet<Moderator> Moderators { get; set; } = null!;
    public DbSet<ModeratorAuthority> ModeratorAuthorities { get; set; } = null!;

    public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(b => b.CreatedAt).HasDefaultValueSql("Now()");
        
        modelBuilder.Entity<Forum>().Property(b => b.CreatedAt).HasDefaultValueSql("Now()");
        
        modelBuilder.Entity<Post>().Property(b => b.CreatedAt).HasDefaultValueSql("Now()");
        
        modelBuilder.Entity<Comment>().Property(b => b.CreatedAt).HasDefaultValueSql("Now()");
        
        modelBuilder.Entity<BlackListed>().Property(b => b.CreatedAt).HasDefaultValueSql("Now()");
        
        modelBuilder.Entity<Moderator>().Property(b => b.CreatedAt).HasDefaultValueSql("Now()");
    }   

}