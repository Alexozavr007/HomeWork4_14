using System.ComponentModel;
using System.Windows.Input;

namespace HomeWork4_14_2;

public enum EState {
    Unknown = 0,
    Connected = 1,
    Connecting = 2,
    Disconnected = 3,
    Disconnecting = 4
}

public class DBStateModel : INotifyPropertyChanged {

    private string _totalLog = "";
    private EState _currentstate = EState.Unknown;
    public  Timer timer;

    public EState CurrentState {
        get { return _currentstate; } 
        set { 
            _currentstate = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentState)));
            CommandManager.InvalidateRequerySuggested();
        } 
    }

    public ICommand ConnectCommand { get; set; }
    public ICommand DisconnectCommand { get; set; }
    public string TotalLog {
        get { return _totalLog; }
        set {
            _totalLog = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalLog)));
        }
    }

    public DBStateModel() {
        CurrentState = EState.Disconnected;

        ConnectCommand = new RelayCommand(
            execute: async (btnParameter) => {
                CurrentState = EState.Connecting;
                WriteToTotalLog("Conecting.....");

                await Task.Delay(3000);
                CurrentState = EState.Connected;
                WriteToTotalLog("Conected");

                if (timer == null)
                    timer = new Timer(DataReceived, null, 3000, 3000);
                else
                    timer.Change(3000, 3000);
            },
            canExecute: (x) => (
                CurrentState != EState.Connected && 
                CurrentState != EState.Connecting &&
                CurrentState != EState.Disconnecting
            ));

        DisconnectCommand = new RelayCommand(
            execute: async (btnParametr) => {
                timer?.Change(Timeout.Infinite,Timeout.Infinite);

                CurrentState = EState.Disconnecting;
                WriteToTotalLog("Disconnecting.....");

                await Task.Delay(3000);
                CurrentState = EState.Disconnected;
                WriteToTotalLog("Disconected");
            },
            canExecute: (x) => (
                CurrentState != EState.Disconnected &&
                CurrentState != EState.Connecting &&
                CurrentState != EState.Disconnecting
            ));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void DataReceived(object? a) {
        WriteToTotalLog("Data received");
    }

    public void WriteToTotalLog(string msg) {
        TotalLog += (DateTime.Now.ToString("HH:mm:ss:fff") + " >>>> " + msg + "\r\n");
    }
}
