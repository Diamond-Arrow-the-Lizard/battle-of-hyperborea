using System;
using System.Windows.Input;
using System.Threading.Tasks;

namespace BoH.GUI.ViewModels
{
    /// <summary>
    /// Реализует интерфейс ICommand для работы с командами в MVVM.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool>? _canExecute;

        /// <summary>
        /// Конструктор команды с обязательным выполнением и необязательной проверкой возможности выполнения.
        /// </summary>
        /// <param name="execute">Метод, который будет выполнен при активации команды.</param>
        /// <param name="canExecute">Метод, который определяет, может ли команда быть выполнена.</param>
        public RelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Событие, которое вызывается при изменении состояния CanExecute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Определяет, может ли команда быть выполнена.
        /// </summary>
        /// <param name="parameter">Параметры для выполнения проверки.</param>
        /// <returns>true, если команда может быть выполнена, иначе false.</returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        /// <summary>
        /// Выполняет команду.
        /// </summary>
        /// <param name="parameter">Параметры, передаваемые в метод выполнения.</param>
        public async void Execute(object? parameter)
        {
            await _execute();
        }

        /// <summary>
        /// Сигнализирует об изменении состояния CanExecute.
        /// </summary>
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
