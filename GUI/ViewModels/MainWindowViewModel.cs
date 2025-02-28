namespace BoH.GUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public GameBoardViewModel GameBoardVm { get; }
    public AbilitiesViewModel AbilitiesVm { get; }
    
    public MainWindowViewModel(GameBoardViewModel gameBoardVm, AbilitiesViewModel abilitiesVm)
    {
        GameBoardVm = gameBoardVm;
        AbilitiesVm = abilitiesVm;
    }
}
