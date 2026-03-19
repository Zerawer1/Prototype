namespace PrototypeDemo;

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
