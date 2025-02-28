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
        
        public string LogText => string.Join(Environment.NewLine, LogMessages);

        public BattleLogViewModel(ITurnManager turnManager, IGameController gameController,
            IPlayer[] players)
        {
            foreach (var player in players)
            {
                foreach (var unit in player.Units)
                {
                    unit.OnAttack += Notify_UnitAttacked;
                    unit.OnDeath += Notify_UnitDead;
                    unit.OnHealed += Notify_UnitHealed;
                    unit.OnMove += Notify_UnitMoved;
                    unit.OnTakingDamage += Notify_UnitRecievedDamage;
                    unit.OnStunned += Notify_UnitStunned;
                    foreach (var ability in unit.Abilities)
                    {
                        ability.OnCooldown += Notify_AbilityOnCooldown;
                        ability.OnAbilityUsed += Notify_AbilityUsed;
                        ability.OnAbilityFailed += Notify_AbilityFailed;
                    }

                }
            }

            turnManager.OnTurnEnd += Notify_TurnEnded;
            turnManager.OnTurnStart += Notify_TurnStarted;
            turnManager.OnTurnStateChanged += Notify_UnitChangedTurnPhase;
            turnManager.OnUnitSelected += Notify_UnitSelected;
            gameController.OnPlayerWinning += Notify_PlayerWon;

        }

        private void AddLogMessage(string message)
        {
            LogMessages.Add($"{DateTime.Now:HH:mm:ss}: {message}");
            OnPropertyChanged(nameof(LogText));
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
            AddLogMessage($"Юнит атаковал: {unit.UnitName} (урон: {amount})");
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

        public void Notify_TurnStarted(IPlayer player)
        {
            AddLogMessage($"Начинается ход команды {player.Team}");
        }
        public void Notify_TurnEnded(IPlayer player)
        {
            AddLogMessage($"Закончен ход команды {player.Team}");
        }

        public void Notify_UnitSelected(IUnit unit)
        {
            AddLogMessage($"Выбран юнит: {unit.UnitName}");
        }

        public void Notify_PlayerWon(IPlayer player)
        {
            AddLogMessage($"|------------!!! Команда {player.Team} победила !!!------------|");
        }
    }

