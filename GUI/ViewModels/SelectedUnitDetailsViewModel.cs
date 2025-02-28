using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using BoH.Interfaces;
using BoH.Models;

namespace BoH.GUI.ViewModels
{
    public partial class SelectedUnitDetailsViewModel : ViewModelBase
    {
        [ObservableProperty]
        private IUnit? _selectedUnit;

        [ObservableProperty]
        private ObservableCollection<IAbility> _passiveAbilities = new();

        public string Description => SelectedUnit != null
            ? $"Юнит: {SelectedUnit.UnitName}\n" +
              $"HP: {SelectedUnit.Hp}\n" +
              $"Защита: {SelectedUnit.Defence}\n" +
              $"Фаза: {SelectedUnit.CurrentTurnPhase}\n" +
              $"Статус: {(SelectedUnit.IsDead ? "Мертв" : "Жив")}\n" +
              $"Число бросаемых кубиков: {SelectedUnit.DamageDices}"
            : "Нет выбранного юнита";

        // Этот метод будет вызван автоматически при изменении SelectedUnit
        partial void OnSelectedUnitChanged(IUnit? value)
        {
            if (value != null)
            {
                // Обновляем список пассивных способностей (фильтруем способности, где IsActive == false)
                PassiveAbilities = new ObservableCollection<IAbility>(
                    value.Abilities.Where(a => !a.IsActive)
                );
            }
            else
            {
                PassiveAbilities.Clear();
            }
            OnPropertyChanged(nameof(Description));
        }
    }
}