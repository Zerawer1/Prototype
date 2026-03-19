using System;
using System.Collections.Generic;

namespace PrototypeDemo;

internal sealed class UserProfile
{
    public UserProfile(Guid id, string username, Preferences preferences, List<string> permissions)
    {
        Id = id;
        Username = username;
        Preferences = preferences;
        Permissions = permissions;
    }

    public Guid Id { get; }
    public string Username { get; private set; }
    public Preferences Preferences { get; }
    public List<string> Permissions { get; }

    public UserProfile ShallowClone(string? newUsername = null)
    {
        var clone = (UserProfile)MemberwiseClone();
        if (!string.IsNullOrWhiteSpace(newUsername))
        {
            clone.Username = newUsername;
        }

        return clone;
    }

    public UserProfile DeepClone(string? newUsername = null)
    {
        return new UserProfile(
            id: Guid.NewGuid(),
            username: newUsername ?? Username,
            preferences: Preferences.DeepClone(),
            permissions: new List<string>(Permissions));
    }

    public override string ToString()
    {
        var permissions = Permissions.Count == 0 ? "-" : string.Join(",", Permissions);

        return $"UserProfile(Username={Username}, Lang={Preferences.Language}, Theme={Preferences.Theme}, Notif=[Email={Preferences.Notifications.Email},Push={Preferences.Notifications.Push},Sms={Preferences.Notifications.Sms}], Perms=[{permissions}])";
    }
}
