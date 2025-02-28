using System;
using System.ComponentModel;

namespace BoH.GUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public GameBoardViewModel GameBoardVm { get; }
    public AbilitiesViewModel AbilitiesVm { get; }
    
    public MainWindowViewModel(GameBoardViewModel gameBoardVm, AbilitiesViewModel abilitiesVm)
    {
        GameBoardVm = gameBoardVm;
        AbilitiesVm = abilitiesVm;
        
        GameBoardVm.OnUnitSelectedForAbilities += unit => AbilitiesVm.UpdateAbilities(unit);
        AbilitiesVm.AbilityChanged += ability =>
        {
            GameBoardVm.SelectedAbility = ability;
            if(ability != null) GameBoardVm.UpdateScan(ability);
        };
    }
    
}
