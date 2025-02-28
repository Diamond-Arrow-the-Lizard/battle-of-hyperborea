using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using BoH.Interfaces;
using BoH.Models;

namespace BoH.GUI.ViewModels;

    public partial class BattleLogViewModel : ViewModelBase, IAbilityNotifications, IUnitNotifications
    {
        [ObservableProperty]
        private ObservableCollection<string> logMessages = new();

        public BattleLogViewModel()
        {
             
        }

        private void AddLogMessage(string message)
        {
            LogMessages.Add($"{DateTime.Now:HH:mm:ss}: {message}");
        }

        public void Notify_AbilityUsed(IAbility ability)
        {
            AddLogMessage($"Способность использована: {ability.Name}");
        }

        public void Notify_AbilityOnCooldown(IAbility ability)
        {
            AddLogMessage($"Способность на кулдауне: {ability.Name}");
        }

        public void Notify_AbilityFailed(IAbility ability)
        {
            AddLogMessage($"Способность не сработала: {ability.Name}");
        }

        public void Notify_UnitDead(IUnit unit)
        {
            AddLogMessage($"Юнит погиб: {unit.UnitName}");
        }

        public void Notify_UnitStunned(IUnit unit)
        {
            AddLogMessage($"Юнит оглушён: {unit.UnitName}");
        }

        public void Notify_UnitMoved(IUnit unit)
        {
            AddLogMessage($"Юнит перемещён: {unit.UnitName}");
        }

        public void Notify_UnitAttacked(IUnit unit, int amount)
        {
            AddLogMessage($"Юнит атакован: {unit.UnitName} (урон: {amount})");
        }

        public void Notify_UnitRecievedDamage(IUnit unit, int amount)
        {
            AddLogMessage($"Юнит получил урон: {unit.UnitName} ({amount} ед.)");
        }

        public void Notify_UnitUsedAbility(IUnit unit)
        {
            AddLogMessage($"Юнит применил способность: {unit.UnitName}");
        }

        public void Notify_UnitHealed(IUnit unit, int amount)
        {
            AddLogMessage($"Юнит восстановил здоровье: {unit.UnitName} (+{amount} HP)");
        }

        public void Notify_UnitChangedTurnPhase(IUnit unit)
        {
            AddLogMessage($"Юнит сменил фазу хода: {unit.UnitName} → {unit.CurrentTurnPhase}");
        }

        public void Notify_PlayerWon(IPlayer player)
        {
            AddLogMessage($"|------------!!! Команда {player.Team} победила !!!------------|");
        }
    }

