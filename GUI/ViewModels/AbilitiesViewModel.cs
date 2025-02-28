using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using BoH.Interfaces;
using BoH.Models;

namespace BoH.GUI.ViewModels
{
    public partial class AbilitiesViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<IAbility> _activeAbilities = new();

        [ObservableProperty] private IAbility? _selectedAbility = null;
        
        public event Action<IAbility?>? AbilityChanged;

        public void UpdateAbilities(IUnit unit)
        {
            ActiveAbilities = new ObservableCollection<IAbility>(unit.Abilities.Where(x => x.IsActive));
            SelectedAbility = ActiveAbilities.FirstOrDefault();
        }

        partial void OnSelectedAbilityChanged(IAbility? value)
        {
            AbilityChanged?.Invoke(value);
        }
    }
}