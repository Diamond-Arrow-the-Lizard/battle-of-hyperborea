﻿using System;
using System.ComponentModel;
using System.Runtime.Loader;

namespace BoH.GUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public GameBoardViewModel GameBoardVm { get; }
    public AbilitiesViewModel AbilitiesVm { get; }
    public SelectedUnitDetailsViewModel SelectedUnitDetailsVm { get; }
    
    public MainWindowViewModel(GameBoardViewModel gameBoardVm, 
        AbilitiesViewModel abilitiesVm,
        SelectedUnitDetailsViewModel selectedUnitDetailsVm)
    {
        GameBoardVm = gameBoardVm;
        AbilitiesVm = abilitiesVm;
        SelectedUnitDetailsVm = selectedUnitDetailsVm;

        GameBoardVm.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(GameBoardVm.SelectedUnit))
            {
                SelectedUnitDetailsVm.SelectedUnit = GameBoardVm.SelectedUnit;
            }
        };
        
        GameBoardVm.OnUnitSelectedForAbilities += unit => AbilitiesVm.UpdateAbilities(unit);
        AbilitiesVm.AbilityChanged += ability =>
        {
            GameBoardVm.SelectedAbility = ability;
            if(ability != null) GameBoardVm.UpdateScan(ability);
        };
    }
    
}
