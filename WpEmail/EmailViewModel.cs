namespace WpfEmail
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public partial class EmailViewModel : INotifyPropertyChanged
    {
        private EmialModel _model;

        private ObservableCollection<BreedViewModel> _species;
        public ObservableCollection<BreedViewModel> Keyboard { get => _species; set => _species = value; }

        private BreedViewModel _selectedBreed;
        private int? _selectedBreedId;

        private string _completedText;
        public string CompletedText
        {
            get => _completedText;
            set
            {
                _completedText = value;
                SimplePropertyChanged("CompletedText");
            }
        }

        public ICommand NewLifeCommand { get; set; }
        public ICommand EvolveCommand { get; set; }
        public ICommand ToGenePoolCommand { get; set; }

        public ICommand EvolutionCompleteCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnMainBreedChaged(object o, EventArgs a)
        {
            CompletedText = _model.GetMainBreedName();
        }
        public void OnOtherBreedsChaged(object o, EventArgs a)
        {
            _species.Clear();
            for (int i = _model.GetBreedCount()-1; i>=0; --i)
            {
                var breed = new BreedViewModel();
                int nr = i;
                breed.Caption = _model.GetBreedName(nr);
                breed.RemoveMeCommand = new SimpleCommand(_ => _model.KillBreed(nr));
                breed.InsertMeCommand = new SimpleCommand(_ => _model.CloneBreed(nr));
                breed.ChangeMeCommand = new SimpleCommand(_ => _model.EvolveBreed(nr));
                breed.CrossMeCommand = new SimpleCommand(_ =>
                {
                    if (_selectedBreedId == null)
                    {
                        _selectedBreedId = nr;
                        _selectedBreed = breed;
                        breed.IsSelected = true;
                        breed.SimplePropertyChanged("IsSelected");
                    }
                    else if (_selectedBreedId == nr)
                    {
                        _selectedBreedId = null;
                        _selectedBreed = null;
                        breed.IsSelected = false;
                        breed.SimplePropertyChanged("IsSelected");
                    }
                    else
                    {
                        var crossTarget = _selectedBreedId;
                        _selectedBreed.IsSelected = false;
                        _selectedBreedId = null;
                        _selectedBreed = null;
                        _model.CrossBreeds(nr, crossTarget.Value);
                    }
                });
                _species.Add(breed);
            }
            SimplePropertyChanged("Keyboard");
        }

        public EmailViewModel()
        {
            Keyboard = new ObservableCollection<BreedViewModel>();

            _model = new EmialModel();
            _model.OnMainBreedChaged += OnMainBreedChaged;
            _model.OnOtherBreedsChanged += OnOtherBreedsChaged;

            ToGenePoolCommand = new SimpleCommand(
                x => _model.SaveMainBreed(),
                x => _model.CanSaveMainBreed()
            );
            EvolveCommand = new SimpleCommand(
                x => _model.EvolveMainBreed(),
                x => _model.CanEvolveMainBreed()
            );
            NewLifeCommand = new SimpleCommand(x => { _model.ComplicateMainBreed(); });

            EvolutionCompleteCommand = new SimpleCommand(x =>
            {
                MessageBoxResult result = MessageBox.Show($"Is your email is:\n{_model.GetMainBreedName()}\nIs that correct?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);
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
    }
}
