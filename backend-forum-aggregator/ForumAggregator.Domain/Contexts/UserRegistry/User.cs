using System;
using ForumAggregator.Domain.Shared.Interfaces;

namespace ForumAggregator.Domain.UserRegistry;

public class User: IEntity
{
    // Fields & Properties

    public Guid Id { get; init; }

    public string Name { get; private set; } = default!;

    public string Password { get; private set; } = default!;

    public string Email { get; private set; } = default!;

    public bool Deleted {get; private set; }

    private User() {}

    // Constructors

    public User (string name, string email, string password)
    {
        Id = Guid.NewGuid();
        Name = name.ToLower();
        Password = password;
        Email = email.ToLower();
        Deleted = false;
    }

    // Methods

    public UserResult EditName(Guid editor, string newName)
    {
        if (Deleted)
            return DeletedResult();

        if (editor == Id)
        {
            Name = newName;
            return new UserResult()
            {
                Value = true,
                Result = string.Empty
            };
        }

        return new UserResult()
        {
            Value = false,
            Result = "Unauthorized access to other User's account."
        };
    }

    public UserResult EditEmail (Guid editor, string newEmail)
    {
        if (Deleted)
            return DeletedResult();

        if (editor == Id)
        {
            Email = newEmail;
            return new UserResult()
            {
                Value = true,
                Result = string.Empty
            };
        }

        return new UserResult()
        {
            Value = false,
            Result = "Unauthorized access to other User's account."
        };
    }

    public UserResult EditPassword (Guid editor, string newPassword)
    {
        if (Deleted)
            return DeletedResult();
        
        if (editor != Id)
            return new UserResult() { Value = false, Result = "Unauthorized access to other User's account." };

        if (newPassword == Password)
            return new UserResult() { Value = false, Result = "The new Password can not be equal to the old one." };
        
        Password = newPassword;

        return new UserResult() { Value = true, Result = string.Empty };
    }

    public UserResult Delete(Guid actorUserId)
    {
        if (Deleted)
            return DeletedResult();
        
        if (actorUserId != Id)
            return new UserResult() { Value = false, Result = "Unauthorized access to other User's account." };
        
        Deleted = true;

        return new UserResult() { Value = true, Result = string.Empty };
    }

    private UserResult DeletedResult ()
    {
        return new UserResult()
        {
            Value = false,
            Result = "User has already been deleted."
        };
    }

    public static User Load (Guid id, string name, string password, string email, bool deleted)
    {
        return new User()
        {
            Id = id,
            Name = name.ToLower(),
            Password = password,
            Email = email,
            Deleted = deleted
        };
    }
}
