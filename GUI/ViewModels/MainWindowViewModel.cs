namespace BoH.GUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public GameBoardViewModel GameBoardVm { get; }
    
    public MainWindowViewModel(GameBoardViewModel gameBoardVm)
    {
        GameBoardVm = gameBoardVm;
    }
}
