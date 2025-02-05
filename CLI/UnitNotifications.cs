namespace BoH.CLI;

using BoH.Interfaces;

public class ConsoleUnitNotifications : IUnitNotifications
{

    public void ShowUnitStats(IUnit unit)
    {
        string infoMessage = "";
        infoMessage += $"Фаза хода юнита: {unit.CurrentTurnPhase}\n";
        infoMessage += $"Имя: {unit.UnitName}\n";
        infoMessage += $"Команда: {unit.Team}\n";
        infoMessage += $"Здоровье: {unit.Hp}\n";
        infoMessage += $"Защита: {unit.Defence}\n";
        infoMessage += $"Брошенных кубиков при атаке: {unit.DamageDices}\n";
        infoMessage += $"Может передвигаться: {unit.CanMove()}\n";
        infoMessage += $"Тип Юнита: ";
        switch (unit.UnitType)
        {
            case UnitType.Melee:
                infoMessage += $"Ближний бой\n";
                break;
            case UnitType.Range:
                infoMessage += $"Дальний бой\n";
                infoMessage += $"Дальность атаки: {unit.Range}\n";
                break;
        }
        infoMessage += $"Способности: \n";
        foreach(var i in unit.Abilities)
        {
            infoMessage += $"{i.Name} ";
        }
        infoMessage += "\n";
        Console.WriteLine(infoMessage);
    }
    public void Notify_UnitDead(IUnit unit)
    {
        Console.WriteLine($"{unit.UnitName} убит!");
    }
    public void Notify_UnitStunned(IUnit unit)
    {
        Console.WriteLine($"{unit.UnitName} ошеломлён!");
    }
    public void Notify_UnitMoved(IUnit unit)
    {
        Console.WriteLine($"{unit.UnitName} совершил передвижение");
    }
    public void Notify_UnitAttacked(IUnit unit)
    {
        Console.WriteLine($"{unit.UnitName} совершил атаку");
    }
    public void Notify_UnitRecievedDamage(IUnit unit)
    {
        Console.WriteLine($"{unit.UnitName} получил урон");
    }
    public void Notify_UnitUsedAbility(IUnit unit)
    {
        Console.WriteLine($"{unit.UnitName} Применил способность");
    }
    public void Notify_UnitHealed(IUnit unit)
    {
        Console.WriteLine($"{unit.UnitName} был излечен");
    }
    public void Notify_UnitChangedTurnPhase(IUnit unit)
    {
        Console.WriteLine($"{unit.UnitName} изменил фазу хода");
    }

    public void Notify_UnitSelected(IUnit unit)
    {
        Console.WriteLine($"{unit.UnitName} был выбран\n");
        ShowUnitStats(unit);
    }
}