namespace WpfEmail
{
    using System.ComponentModel;
    using System.Windows.Input;

    public partial class EmailViewModel
    {
        public class BreedViewModel : INotifyPropertyChanged
        {
            public string Caption { get; set; }
            public bool IsSelected { get; set; }
            public ICommand RemoveMeCommand { get; set; }
            public ICommand InsertMeCommand { get; set; }
            public ICommand CrossMeCommand { get; set; }
            public ICommand ChangeMeCommand { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;


            public void SimplePropertyChanged(string property)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
