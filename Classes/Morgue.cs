using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Windows;

//TODO: add a window containing info about a hero after a double click on listview item
//TODO: add windows and buttons for charts
//TODO: add application icon

namespace DarkestDungeonMorgueGUI {
    public class Morgue {

        public IDictionary<HeroClass, Uri> ClassPortraits;

        private ObservableCollection<HeroQuirk> Positives;
        private ObservableCollection<HeroQuirk> Negatives;
        private ObservableCollection<HeroDisease> _Diseases;
        private ObservableCollection<string> _CausesOfDeath;
        private ObservableCollection<HeroDeath> heroes;

        public ObservableCollection<HeroDeath> FallenHeroes {
            get => this.heroes;
            set => this.heroes = value;
        }

        public ObservableCollection<HeroQuirk> NegativeQuirks {
            get => this.Negatives;
            set => this.Negatives = value;
        }

        public ObservableCollection<HeroQuirk> PositiveQuirks {
            get => this.Positives;
            set => this.Positives = value;
        }

        public ObservableCollection<HeroDisease> Diseases {
            get => this._Diseases;
            set => this._Diseases = value;
        }

        public ObservableCollection<string> CausesOfDeath {
            get => this._CausesOfDeath;
            set => this._CausesOfDeath = value;
        }

        private static Morgue Instance = null;

        private Morgue() {
            this.FallenHeroes = new ObservableCollection<HeroDeath>();
            this.CausesOfDeath = new ObservableCollection<string>();
            this.Diseases = new ObservableCollection<HeroDisease>();
            this.PositiveQuirks = new ObservableCollection<HeroQuirk>();
            this.NegativeQuirks = new ObservableCollection<HeroQuirk>();
            this.ClassPortraits = new ConcurrentDictionary<HeroClass, Uri>();
            this.LoadImages();
            this.LoadData();
        }

        public static Morgue GetInstance() => Instance;

        public static void InitInstance() {
            if (Instance != null) return;
            Instance = new Morgue();
        }

        #region LOADING AND SAVING METHODS

        private void LoadImages() {
            string[] paths = Directory.GetFiles(@"../../HeroPortraits");
            string[] heroClasses = Enum.GetNames(typeof(HeroClass));
            Parallel.ForEach(paths, p => {
                var heroEnum = (HeroClass)Enum.Parse(
                    typeof(HeroClass), 
                    heroClasses.
                        Where(s => s.ToLower().Trim() == Path.GetFileNameWithoutExtension(p).ToLower().Trim()).
                        ElementAt(0)
                );
                var uri = new Uri(p, UriKind.Relative);
                this.ClassPortraits.Add(heroEnum, uri);
            });
        }

        private void LoadData() {
            ThreadPool.QueueUserWorkItem(o => LoadPositiveQuirks());
            ThreadPool.QueueUserWorkItem(o => LoadNegativeQuirks());
            ThreadPool.QueueUserWorkItem(o => LoadMorgue());
            ThreadPool.QueueUserWorkItem(o => LoadCausesOfDeath());
            ThreadPool.QueueUserWorkItem(o => LoadDiseases());
        }

        private void LoadPositiveQuirks() {
            using (var reader = new StreamReader(@"../../Data/PositiveQuirks.json")) {
                ObservableCollection<HeroQuirk> l = JsonConvert.DeserializeObject<ObservableCollection<HeroQuirk>>(reader.ReadToEnd());
                foreach (var obj in l) {
                    Application.Current.Dispatcher.Invoke(() => this.PositiveQuirks.Add(obj));
                }
            }
        }

        private void LoadNegativeQuirks() {
            using (var reader = new StreamReader(@"../../Data/NegativeQuirks.json")) {
                ObservableCollection<HeroQuirk> l = JsonConvert.DeserializeObject<ObservableCollection<HeroQuirk>>(reader.ReadToEnd());
                foreach (var obj in l) {
                    Application.Current.Dispatcher.Invoke(() => this.NegativeQuirks.Add(obj));
                }
            }
        }

        private void LoadDiseases() {
            using (var reader = new StreamReader(@"../../Data/Diseases.json")) {
                ObservableCollection<HeroDisease> l = JsonConvert.DeserializeObject<ObservableCollection<HeroDisease>>(reader.ReadToEnd());
                foreach (var obj in l) {
                    Application.Current.Dispatcher.Invoke(() => this.Diseases.Add(obj));
                }
            }
        }

        private void LoadCausesOfDeath() {
            using (var reader = new StreamReader(@"../../Data/CausesOfDeath.json")) {
                ObservableCollection<string> l = JsonConvert.DeserializeObject<ObservableCollection<string>>(reader.ReadToEnd());
                foreach (var obj in l) {
                    Application.Current.Dispatcher.Invoke(() => this.CausesOfDeath.Add(obj));
                }
            }
        }

        public void SaveMorgue() {
            using (var fs = new FileStream(@"../../Data/Morgue.json", FileMode.Create)) {
                using (var writer = new StreamWriter(fs)) {
                    writer.Write(JsonConvert.SerializeObject(FallenHeroes, Formatting.Indented));
                }
            }
        }

        private void LoadMorgue() {
            using (var reader = new StreamReader(@"../../Data/Morgue.json")) {
                ObservableCollection<HeroDeath> l = JsonConvert.DeserializeObject<ObservableCollection<HeroDeath>>(reader.ReadToEnd());
                foreach (var obj in l) {
                    Application.Current.Dispatcher.Invoke(() => this.FallenHeroes.Add(obj));
                    obj.ImagePath = this.ClassPortraits[obj.HeroClass];
                }
            }
        }
        #endregion

        #region COLLECTION METHODS
        public void AddHero(HeroDeath hd) => this.FallenHeroes.Add(hd);

        public void SortFallenHeroes(HeroesSortType sortType) {
            List<HeroDeath> tempHeroesList = new List<HeroDeath>(this.FallenHeroes);
            switch (sortType) {

                case HeroesSortType.Level:
                    tempHeroesList = tempHeroesList.OrderByDescending(h => h.HeroLevel).ToList();
                    break;

                case HeroesSortType.Name:
                    tempHeroesList = tempHeroesList.OrderBy(h => h.HeroName).ToList();
                    break;

                case HeroesSortType.Class:
                    tempHeroesList = tempHeroesList.OrderBy(h => h.HeroClass).ToList();
                    break;

                default:
                    throw new ArgumentException("Value wasn't defined in an enum!");
            }
            this.FallenHeroes.Clear();
            this.FallenHeroes.AddRange(tempHeroesList);
        }
        #endregion
    }
}
