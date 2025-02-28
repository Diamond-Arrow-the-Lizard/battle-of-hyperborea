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

        // Метод обновления способностей для нового выбранного юнита.
        public void UpdateAbilities(IUnit unit)
        {
            if (unit == null) return;
            ActiveAbilities = new ObservableCollection<IAbility>(unit.Abilities);
            SelectedAbility = ActiveAbilities.FirstOrDefault();
        }
    }
}