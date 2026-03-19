namespace PrototypeDemo;

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
