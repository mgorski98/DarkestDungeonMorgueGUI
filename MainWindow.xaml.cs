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

namespace DarkestDungeonMorgueGUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Morgue.GetInstance();
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
                this.CausesOfDeathListView.SelectedItems.Clear();
            });
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
            negativeQuirks = new List<HeroQuirk>();
            positiveQuirks = new List<HeroQuirk>();
            diseases = new List<HeroDisease>();
            foreach (var quirk in NegativeQuirksListView.SelectedItems) {
                HeroQuirk _quirk = (quirk as HeroQuirk);
                if (_quirk == null) continue;
                negativeQuirks.Add(_quirk);
            }

            foreach (var quirk in PositiveQuirksListView.SelectedItems) {
                HeroQuirk _quirk = (quirk as HeroQuirk);
                if (_quirk == null) continue;
                positiveQuirks.Add(_quirk);
            }

            foreach (var disease in DiseasesListView.SelectedItems) {
                HeroDisease dis = (disease as HeroDisease);
                if (dis == null) continue;
                diseases.Add(dis);
            }

            string causeOfDeath = CausesOfDeathListView.SelectedItem as string;
            if (causeOfDeath == null) {
                MessageBox.Show("Please select a cause of death for your hero!");
                return null;
            }

            Affliction? affliction = AfflictionComboBox.SelectedIndex == -1 ? null : AfflictionComboBox.SelectedItem as Affliction?;
            Virtue? virtue = VirtueComboBox.SelectedIndex == -1 ? null : VirtueComboBox.SelectedItem as Virtue?;

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
    }
}
