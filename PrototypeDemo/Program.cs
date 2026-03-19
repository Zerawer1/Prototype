using System;
using System.Collections.Generic;

namespace PrototypeDemo;

internal static class Program
{
    private static void Main()
    {
        RunDemo();
    }

    private static void RunDemo()
    {
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

        PrintTemplates(marketingTemplate, supportTemplate);
        DemonstrateShallowClone(marketingTemplate);
        DemonstrateDeepClone(supportTemplate);
        DemonstratePrototypeCatalog(marketingTemplate, supportTemplate);
    }

    private static void PrintTemplates(UserProfile marketingTemplate, UserProfile supportTemplate)
    {
        Console.WriteLine("=== Прототипы (шаблонные профили) ===");
        Console.WriteLine(marketingTemplate);
        Console.WriteLine(supportTemplate);
        Console.WriteLine();
    }

    private static void DemonstrateShallowClone(UserProfile marketingTemplate)
    {
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
    }

    private static void DemonstrateDeepClone(UserProfile supportTemplate)
    {
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
    }

    private static void DemonstratePrototypeCatalog(UserProfile marketingTemplate, UserProfile supportTemplate)
    {
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
    }
}
