namespace BoH.CLI;

using BoH.Interfaces;

public class ConsoleAbilityNotifications : IAbilityNotifications
{
    public void ShowAbilityInfo(IAbility ability)
    {
        string infoMessage = "";
        infoMessage += $"Название способности: {ability.Name}\n";
        infoMessage += $"Описание способности: {ability.Description}\n";
        infoMessage += $"Тип способности: ";
        switch (ability.IsActive)
        {
            case true:
                infoMessage += "Активная\n";
                break;
                case false:
                infoMessage += "Пассивная\n";
                break;
        }
        Console.WriteLine(infoMessage);
    }
    public void Notify_AbilityUsed(IAbility ability)
    {
        string abilityType = "";
        switch (ability.IsActive)
        {
            case true:
                abilityType += "активная";
                break;
                case false:
                abilityType += "пассивная";
                break;
        }

        Console.WriteLine($"Использована {abilityType} способность: {ability.Name}");
    }
    public void Notify_AbilityOnCooldown(IAbility ability)
    {
        Console.WriteLine($"{ability.Name} ещё не готово для применения");
    }
}