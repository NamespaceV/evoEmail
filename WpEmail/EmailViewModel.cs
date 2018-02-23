namespace WpfEmail
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class EmailViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ButtonViewModel> _keyboard;
        public ObservableCollection<ButtonViewModel> Keyboard { get => _keyboard; set => _keyboard = value; }

        private string _completedText;
        public string CompletedText {
            get => _completedText;
            set
            {
                _completedText = value;
                SimplePropertyChanged("CompletedText");
            }
        }

        public ICommand StoreCommand { get; set; }
        public ICommand InsertCommand { get; set; }
        public ICommand SendCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        System.Random _random = new System.Random();

        public EmailViewModel()
        {
            Keyboard = new ObservableCollection<ButtonViewModel>();
            SimplePropertyChanged("Keyboard");
            CompletedText = "@";
            StoreCommand = new SimpleCommand(x => 
                {
                    var buttonVM = new ButtonViewModel
                    {
                        Caption = CompletedText,
                    };
                    buttonVM.RemoveMeCommand = new SimpleCommand(y => Keyboard.Remove(buttonVM));
                    buttonVM.InsertMeCommand = new SimpleCommand(y => CompletedText += buttonVM.Caption);

                    Keyboard.Add(buttonVM);
                    CompletedText = "";
                },
                x => CompletedText.Length > 0
                );
            InsertCommand = new SimpleCommand(x => {
                var r =_random.Next(28);
                char nextChar = ' ';
                if (r < 26)
                {
                    nextChar = (char)('a' + r);
                }
                else if (r < 27)
                {
                    nextChar = '@';
                }
                else if (r < 28)
                {
                    nextChar = '.';
                }
                CompletedText += nextChar;
                }
            );
            SendCommand = new SimpleCommand(x => {
                MessageBoxResult result = MessageBox.Show($"Is your email is:\n{CompletedText}\nIs that correct?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
            });
        }

        void SimplePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        public class ButtonViewModel
        {
            public string Caption { get; set; }
            public ICommand RemoveMeCommand { get; set; }
            public ICommand InsertMeCommand { get; set; }
        }
    }
}
