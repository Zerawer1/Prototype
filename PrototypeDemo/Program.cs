using System;
using System.Collections.Generic;

var marketingTemplate = new UserProfile(
    id: Guid.NewGuid(),
    username: "template_marketing",
    preferences: new Preferences(
        language: "ru-RU",
        theme: Theme.Dark,
        notifications: new NotificationSettings(email: true, push: true, sms: false)),
    permissions: new List<string> { "view_reports", "export" });

var supportTemplate = new UserProfile(
    id: Guid.NewGuid(),
    username: "template_support",
    preferences: new Preferences(
        language: "ru-RU",
        theme: Theme.Light,
        notifications: new NotificationSettings(email: true, push: false, sms: false)),
    permissions: new List<string> { "view_tickets", "comment" });

Console.WriteLine("=== Прототипы (шаблонные профили) ===");
Console.WriteLine(marketingTemplate);
Console.WriteLine(supportTemplate);
Console.WriteLine();

Console.WriteLine("=== 1) Поверхностное клонирование (ShallowClone) ===");
var shallow = marketingTemplate.ShallowClone(newUsername: "maria_marketing");
Console.WriteLine("До изменений:");
Console.WriteLine($"Прототип: {marketingTemplate}");
Console.WriteLine($"Клон:     {shallow}");
Console.WriteLine();

shallow.Preferences.Theme = Theme.Light;
shallow.Preferences.Notifications.Push = false;
shallow.Permissions.Add("publish_campaigns");

Console.WriteLine("После изменений в клоне:");
Console.WriteLine($"Прототип: {marketingTemplate}");
Console.WriteLine($"Клон:     {shallow}");
Console.WriteLine("(Видно, что вложенные объекты/коллекции общие, поэтому прототип тоже изменился)");
Console.WriteLine();

Console.WriteLine("=== 2) Глубокое клонирование (DeepClone) ===");
var deep = supportTemplate.DeepClone(newUsername: "ivan_support");
Console.WriteLine("До изменений:");
Console.WriteLine($"Прототип: {supportTemplate}");
Console.WriteLine($"Клон:     {deep}");
Console.WriteLine();

deep.Preferences.Theme = Theme.Dark;
deep.Preferences.Notifications.Push = true;
deep.Permissions.Add("close_tickets");

Console.WriteLine("После изменений в клоне:");
Console.WriteLine($"Прототип: {supportTemplate}");
Console.WriteLine($"Клон:     {deep}");
Console.WriteLine("(Прототип не изменился, потому что DeepClone создал независимые копии)");
Console.WriteLine();

Console.WriteLine("=== 3) Prototype как фабрика профилей ===");
var profileCatalog = new Dictionary<string, UserProfile>
{
    ["marketing"] = marketingTemplate.DeepClone(),
    ["support"] = supportTemplate.DeepClone(),
};

var userA = profileCatalog["marketing"].DeepClone(newUsername: "user_a");
userA.Preferences.Language = "en-US";

var userB = profileCatalog["support"].DeepClone(newUsername: "user_b");
userB.Preferences.Notifications.Email = false;

Console.WriteLine(userA);
Console.WriteLine(userB);

internal enum Theme
{
    Light,
    Dark,
}

internal sealed class NotificationSettings
{
    public NotificationSettings(bool email, bool push, bool sms)
    {
        Email = email;
        Push = push;
        Sms = sms;
    }

    public bool Email { get; set; }
    public bool Push { get; set; }
    public bool Sms { get; set; }

    public NotificationSettings DeepClone() => new(email: Email, push: Push, sms: Sms);
}

internal sealed class Preferences
{
    public Preferences(string language, Theme theme, NotificationSettings notifications)
    {
        Language = language;
        Theme = theme;
        Notifications = notifications;
    }

    public string Language { get; set; }
    public Theme Theme { get; set; }
    public NotificationSettings Notifications { get; }

    public Preferences DeepClone() => new(language: Language, theme: Theme, notifications: Notifications.DeepClone());
}

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
            clone.Username = newUsername;

        return clone;
    }

    public UserProfile DeepClone(string? newUsername = null)
    {
        var clone = new UserProfile(
            id: Guid.NewGuid(),
            username: newUsername ?? Username,
            preferences: Preferences.DeepClone(),
            permissions: new List<string>(Permissions));

        return clone;
    }

    public override string ToString()
    {
        var perms = Permissions.Count == 0 ? "-" : string.Join(",", Permissions);
        return $"UserProfile(Username={Username}, Lang={Preferences.Language}, Theme={Preferences.Theme}, Notif=[Email={Preferences.Notifications.Email},Push={Preferences.Notifications.Push},Sms={Preferences.Notifications.Sms}], Perms=[{perms}])";
    }
}
