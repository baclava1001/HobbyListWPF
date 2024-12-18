using HobbyListWPF.Commands;
using HobbyListWPF.Models;
using HobbyListWPF.MVVM;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace HobbyListWPF.ViewModels
{
    /// <summary>
    /// This class is a viewmodel containing everything the view needs to display data. The view's components bind to its properties and methods.
    /// </summary>
    internal class HobbysForDisplayVM : ViewModelBase
    {
        // Create an ICommand (RelayCommand) instance and send in the relevant method as an argument
        public ICommand AddCommand => new RelayCommand(execute => AddHobby(), canExecute => CanAddNewHobby());
        public ICommand DeleteCommand => new RelayCommand(execute => DeleteHobby(), canExecute => CanDeleteHobby());
        public ICommand EditCommand => new RelayCommand(execute => EditHobby(), canExecute => CanEditHobby());
        public ICommand UpdateCommand => new RelayCommand(execute => UpdateHobby(), canExecute => CanUpdateHobby());
        public ICommand SearchCommand => new RelayCommand(execute => SearchHobby(), canExecute => CanSearchHobby());
        public ICommand ClearSearchCommand => new RelayCommand(execute => ClearSearch());
        
        // Instance of a data collection of the model class type, that provides notifications when it is updated
        private ObservableCollection<Hobby> _hobbies = new();
        // Getter and setter for the hobbys field
        public ObservableCollection<Hobby> Hobbies
		{
			get
            {
                return _hobbies;
            }
			set
            {
                _hobbies = value;
                // Event handler triggered when the Hobbies property is updated
                OnPropertyChanged();
            }
		}

        // Wraps the original collection data in a filterable collection
        private ICollectionView _filteredHobbies;
        public ICollectionView FilteredHobbies
        {
            get
            {
                return _filteredHobbies;
            }
            set
            {
                _filteredHobbies = value;
                // Event handler triggered when the FilteredHobbies property is updated
                OnPropertyChanged();
            }
        }

        private Hobby _selectedHobby;
        // Getter and setter for selected hobbies in the listview
        public Hobby SelectedHobby
        {
            get
            {
                return _selectedHobby;
            }
            set
            {
                _selectedHobby = value;
                // Event handler triggered when the SelectedHobby property is updated
                OnPropertyChanged();
            }
        }

        private Hobby _newHobby = new();
        // Getter and setter for hobby in the Textboxes
        public Hobby NewHobby
        {
            get
            {
                return _newHobby;
            }
            set
            {
                _newHobby = value;
                // Event handler triggered when the NewHobby property is updated
                OnPropertyChanged();
            }
        }

        private string _searchString = "";
        // Getter and setter for string in the Search textbox
        public string SearchString
        {
            get
            {
                return _searchString;
            }
            set
            {
                _searchString = value;
                // Event handler triggered when the SearchString property is updated
                OnPropertyChanged();
            }
        }


        // Constructor for the viewmodel adds some data to the _hobbies list
        public HobbysForDisplayVM()
		{
            // Seed initial data
            Hobbies.Add(new Hobby() { Name = "Knyppling", About = "Skapa vacker spets av lintråd.", IsFun = false });
            Hobbies.Add(new Hobby() { Name = "Rollspel", About = "Dröm dig bort till en magisk tid.", IsFun = true });
            Hobbies.Add(new Hobby() { Name = "Bågskytte", About = "Fokus och lugn kombinerat med styrka och smidighet.", IsFun = true });

            // Bases FilteredHobbies off the original Hobbies collection
            FilteredHobbies = CollectionViewSource.GetDefaultView(Hobbies);
            FilteredHobbies.Filter = FilterHobby;
        }


        private void AddHobby()
        {
            if(NewHobby is not null)
            {
                Hobbies.Add(NewHobby);
                NewHobby = new();
                SelectedHobby = Hobbies.Last();
            }
        }

        private bool CanAddNewHobby() =>
            !string.IsNullOrWhiteSpace(NewHobby.Name)
            && !string.IsNullOrWhiteSpace(NewHobby.About);

        private void EditHobby()
        {
            if (SelectedHobby is not null && NewHobby is not null)
            {
                int index = Hobbies.IndexOf(SelectedHobby);
                if (index >=0)
                {
                    NewHobby = SelectedHobby;
                }
            }
        }

        private bool CanEditHobby() => SelectedHobby is not null
            && !string.IsNullOrWhiteSpace(SelectedHobby.Name)
            && !string.IsNullOrWhiteSpace(SelectedHobby.About);


        private void UpdateHobby()
        {
            if (SelectedHobby is not null && NewHobby is not null)
            {
                int index = Hobbies.IndexOf(SelectedHobby);
                if(index >= 0)
                {
                    Hobbies[index] = NewHobby;
                    NewHobby = new();
                }
            }
        }

        private bool CanUpdateHobby() => SelectedHobby is not null
            && !string.IsNullOrWhiteSpace(NewHobby.Name)
            && !string.IsNullOrWhiteSpace(NewHobby.About)
            && !string.IsNullOrWhiteSpace(SelectedHobby.Name)
            && !string.IsNullOrWhiteSpace(SelectedHobby.About);

        private void DeleteHobby()
        {
            if (SelectedHobby is not null)
            {
                Hobbies.Remove(SelectedHobby);
                SelectedHobby = new();
            }
        }

        private bool CanDeleteHobby() => SelectedHobby is not null
            && !string.IsNullOrWhiteSpace(SelectedHobby.Name)
            && !string.IsNullOrWhiteSpace(SelectedHobby.About);

        // Logic used by ICollectionView
        private bool FilterHobby(Object obj)
        {
            if (obj is not Hobby hobby) return false;

            if (string.IsNullOrWhiteSpace(SearchString)) return true; // No filter applied

            return hobby.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                   hobby.About.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                   hobby.IsFun.ToString().Contains(SearchString, StringComparison.OrdinalIgnoreCase);
        }

        private void SearchHobby()
        {
            // Trigger filtering manually
            FilteredHobbies.Refresh();
        }

        private bool CanSearchHobby() => !string.IsNullOrWhiteSpace(SearchString);

        private void ClearSearch()
        {
            SearchString = "";
            FilteredHobbies.Refresh();
        }
    }
}
