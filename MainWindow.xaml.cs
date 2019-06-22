using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;
using System.Timers;
using System.Threading;

namespace DarkestDungeonMorgueGUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private const int PositiveQuirksMaxSize = 5;
        private const int NegativeQuirksMaxSize = 5;
        private const int DiseasesMaxSize = 3;

        private ObservableCollection<HeroQuirk> ChosenPositiveQuirks;
        private ObservableCollection<HeroDisease> ChosenHeroDiseases;
        private ObservableCollection<HeroQuirk> ChosenNegativeQuirks;

        public MainWindow() {
            Morgue.InitInstance();
            InitializeComponent();
            this.ChosenHeroDiseases = new ObservableCollection<HeroDisease>();
            this.ChosenNegativeQuirks = new ObservableCollection<HeroQuirk>();
            this.ChosenPositiveQuirks = new ObservableCollection<HeroQuirk>();
            this.InitializeViews();
        }

        private void InitializeViews() {
            Dispatcher.Invoke(() => {
                this.HeroClassesComboBox.ItemsSource = Enum.GetNames(typeof(HeroClass)).Select(s => s.Replace('_', ' '));
                this.HeroLevelsComboBox.ItemsSource = Enum.GetNames(typeof(HeroLevel));
                this.VirtueComboBox.ItemsSource = Enum.GetNames(typeof(Virtue));
                this.AfflictionComboBox.ItemsSource = Enum.GetNames(typeof(Affliction));
                this.HeroLevelsComboBox.SelectedItem = this.HeroLevelsComboBox.Items.GetItemAt(0);
                this.HeroClassesComboBox.SelectedItem = this.HeroClassesComboBox.Items.GetItemAt(0);
                this.ResetCollectionsViews();
                this.HeroesList.ItemsSource = Morgue.GetInstance().FallenHeroes;
                this.ChosenPositiveQuirksListView.ItemsSource = this.ChosenPositiveQuirks;
                this.ChosenNegativeQuirksListView.ItemsSource = this.ChosenNegativeQuirks;
                this.ChosenDiseasesListView.ItemsSource = this.ChosenHeroDiseases;
                this.SortModeComboBox.ItemsSource = Enum.GetValues(typeof(HeroesSortType)).Cast<HeroesSortType>();
            });
        }

        private void ResetControls() {
            this.Dispatcher.Invoke(() => {
                this.HeroLevelsComboBox.SelectedItem = this.HeroLevelsComboBox.Items.GetItemAt(0);
                this.HeroClassesComboBox.SelectedItem = this.HeroClassesComboBox.Items.GetItemAt(0);
                this.HeroNameTextBox.Text = "";
                this.VirtueComboBox.SelectedIndex = -1;
                this.AfflictionComboBox.SelectedIndex = -1;
                this.PositiveQuirksListView.SelectedItems.Clear();
                this.NegativeQuirksListView.SelectedItems.Clear();
                this.DiseasesListView.SelectedItems.Clear();
                this.CausesOfDeathListView.SelectedIndex = -1;
                this.ResetChosenQuirksAndDiseases();
            });
        }

        private void ResetChosenQuirksAndDiseases() {
            this.ChosenNegativeQuirks.Clear();
            this.ChosenPositiveQuirks.Clear();
            this.ChosenHeroDiseases.Clear();
            this.NegativeQuirksListView.SelectedIndex = -1;
            this.PositiveQuirksListView.SelectedIndex = -1;
            this.DiseasesListView.SelectedIndex = -1;
        }

        private void ResetCollectionsViews() {
            this.PositiveQuirksListView.ItemsSource = Morgue.GetInstance().PositiveQuirks;
            this.NegativeQuirksListView.ItemsSource = Morgue.GetInstance().NegativeQuirks;
            this.DiseasesListView.ItemsSource = Morgue.GetInstance().Diseases;
            this.CausesOfDeathListView.ItemsSource = Morgue.GetInstance().CausesOfDeath;
        }

        private HeroDeath GetHeroFromGUI() {
            string heroName = HeroNameTextBox.Text;
            if (String.IsNullOrWhiteSpace(heroName)) {
                MessageBox.Show("Please provide a valid name for your hero!");
                return null;
            }
            HeroClass @class = (HeroClass)Enum.Parse(typeof(HeroClass), (HeroClassesComboBox.SelectedItem as string).Replace(' ', '_'));
            HeroLevel level = (HeroLevel)Enum.Parse(typeof(HeroLevel), HeroLevelsComboBox.SelectedItem as string);
            IList<HeroQuirk> negativeQuirks, positiveQuirks;
            IList<HeroDisease> diseases;
            negativeQuirks = new List<HeroQuirk>(this.ChosenNegativeQuirks);
            positiveQuirks = new List<HeroQuirk>(this.ChosenPositiveQuirks);
            diseases = new List<HeroDisease>(this.ChosenHeroDiseases);

            string causeOfDeath = CausesOfDeathListView.SelectedItem as string;
            if (causeOfDeath == null) {
                MessageBox.Show("Please select a cause of death for your hero!");
                return null;
            }

            Affliction? affliction = AfflictionComboBox.SelectedIndex == -1 ? null : (Affliction?)Enum.Parse(typeof(Affliction), AfflictionComboBox.SelectedItem as string);
            Virtue? virtue = VirtueComboBox.SelectedIndex == -1 ? null : (Virtue?)Enum.Parse(typeof(Virtue), VirtueComboBox.SelectedItem as string);

            HeroDeath hd = new HeroDeath() {
                Affliction = affliction,
                CauseOfDeath = causeOfDeath,
                Diseases = diseases,
                HeroClass = @class,
                HeroLevel = level,
                PositiveQuirks = positiveQuirks,
                NegativeQuirks = negativeQuirks,
                HeroName = heroName,
                Virtue = virtue
            };

            hd.ImagePath = Morgue.GetInstance().ClassPortraits[hd.HeroClass];

            return hd;
        }

        private void AddHeroEvent(object sender, RoutedEventArgs e) {
            HeroDeath hd = this.GetHeroFromGUI();
            if (hd != null) {
                Morgue.GetInstance().AddHero(hd);
                HeroesList.Items.Refresh();
                ResetControls();
            }
        }

        private void AfflictionComboBox_DropDownClosed(object sender, EventArgs e) {
            if (this.AfflictionComboBox.SelectedIndex != -1) {
                this.VirtueComboBox.SelectedIndex = -1;
            }
        }

        private void VirtueComboBox_DropDownClosed(object sender, EventArgs e) {
            if (this.VirtueComboBox.SelectedIndex != -1) {
                this.AfflictionComboBox.SelectedIndex = -1;
            }
        }

        private void SearchTextBoxEvent(object sender, TextChangedEventArgs e) {
            if (ListTabControl.SelectedItem is TabItem tabitem) {
                string header = tabitem.Header as string;
                switch (header) {
                    case "Positive Quirks":
                        PositiveQuirksListView.ItemsSource = Morgue.GetInstance().PositiveQuirks.Where(q => q.QuirkName.ToLower().Contains(SearchTextBox.Text.ToLower().Trim()));
                        break;
                    case "Negative Quirks":
                        NegativeQuirksListView.ItemsSource = Morgue.GetInstance().NegativeQuirks.Where(q => q.QuirkName.ToLower().Contains(SearchTextBox.Text.ToLower().Trim()));
                        break;
                    case "Diseases":
                        DiseasesListView.ItemsSource = Morgue.GetInstance().Diseases.Where(d => d.DiseaseName.ToLower().Contains(SearchTextBox.Text.ToLower().Trim()));
                        break;
                    case "Causes of Death":
                        CausesOfDeathListView.ItemsSource = Morgue.GetInstance().CausesOfDeath.Where(c => c.ToLower().Contains(SearchTextBox.Text.ToLower().Trim()));
                        break;
                }
            }
        }

        private void ListTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            this.Dispatcher.Invoke(() => {
                this.ResetCollectionsViews();
                this.SearchTextBox.Text = "";
            });
        }

        private void AddChosenNegativeQuirk(object sender, MouseButtonEventArgs e) {
            if (ChosenNegativeQuirks.Count >= NegativeQuirksMaxSize) return;
            var items = (sender as ListView)?.SelectedItems;
            if (items != null) {
                foreach (HeroQuirk quirk in items) {
                    if (this.ChosenNegativeQuirks.Contains(quirk)) continue;
                    this.ChosenNegativeQuirks.Add(quirk);
                }
            }
        }

        private void AddChosenPositiveQuirk(object sender, MouseButtonEventArgs e) {
            if (ChosenPositiveQuirks.Count >= PositiveQuirksMaxSize) return;
            var items = (sender as ListView)?.SelectedItems;
            if (items != null) {
                foreach (HeroQuirk quirk in items) {
                    if (this.ChosenPositiveQuirks.Contains(quirk)) continue;
                    this.ChosenPositiveQuirks.Add(quirk);
                }
            }
        }

        private void AddChosenDisease(object sender, MouseButtonEventArgs e) {
            if (ChosenHeroDiseases.Count >= DiseasesMaxSize) return;
            var items = (sender as ListView)?.SelectedItems;
            if (items != null) {
                foreach (HeroDisease item in items) {
                    if (this.ChosenHeroDiseases.Contains(item)) continue;
                    this.ChosenHeroDiseases.Add(item);
                }
            }
        }

        private void ClearChosenQuirksAndDiseasesViews(object sender, RoutedEventArgs e) {
            this.ResetChosenQuirksAndDiseases();
        }

        private void GetKeyDownEvent(object sender, KeyEventArgs e) {
            if (Keyboard.IsKeyDown(Key.S) && Keyboard.IsKeyDown(Key.LeftCtrl)) {
                ThreadPool.QueueUserWorkItem(o => Morgue.GetInstance().SaveMorgue());
                MessageBox.Show("Saving morgue to disk...");
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (this.HeroesList.SelectedItem is HeroDeath item) {
                try {
                    var x = Application.Current.Windows.OfType<HeroDeathInfoWindow>().ElementAt(0);
                } catch (ArgumentOutOfRangeException) {
                    var w = new HeroDeathInfoWindow(item) { Topmost = true };
                    w.Show();
                    System.Timers.Timer t = new System.Timers.Timer() {
                        Interval = 100,
                        AutoReset = false,
                        Enabled = true
                    };
                    t.Elapsed += (source, _e) => Dispatcher.Invoke(() => w.Topmost = false);
                    t.Start();
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            foreach (var window in Application.Current.Windows) {
                try {
                    ((Window)window).Close();
                } catch (Exception) {}
            }
        }

        private void SortModeComboBox_DropDownClosed(object sender, EventArgs e) {
            HeroesSortType sortType;
            try {
                sortType = (HeroesSortType)SortModeComboBox.SelectedItem;
            } catch (Exception) {
                return;
            }
            Morgue.GetInstance().SortFallenHeroes(sortType);
            Keyboard.ClearFocus();
                
            if (this.HeroesList.Items.Count > 0) {
                this.HeroesList.ScrollIntoView(this.HeroesList.Items[0]);
            }
        }

        private void ShowChartsEvent(object sender, RoutedEventArgs e) {
            try {
                var x = Application.Current.Windows.OfType<DeathStatisticsWindow>().ElementAt(0);
            } catch (ArgumentOutOfRangeException) {
                new DeathStatisticsWindow().Show();
            }
        }

        private void SaveChangesEvent(object sender, RoutedEventArgs e) {
            ThreadPool.QueueUserWorkItem(o => Morgue.GetInstance().SaveMorgue());
            MessageBox.Show("Saving fallen heroes...", "Saving", MessageBoxButton.OK);
        }
    }
}
