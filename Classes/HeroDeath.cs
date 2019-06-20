using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DarkestDungeonMorgueGUI {
    public class HeroDeath {


        private HeroClass heroClass;
        private string heroName;
        private IList<HeroQuirk> positiveQuirks;
        private IList<HeroQuirk> negativeQuirks;
        private IList<HeroDisease> heroDiseases;
        private HeroLevel heroLevel;
        private string causeOfDeath;

        private Uri imagePath;

        private Affliction? affliction;
        private Virtue? virtue;

        [JsonIgnore]
        public Uri ImagePath {
            get => this.imagePath;
            set => this.imagePath = value;
        }

        public HeroClass HeroClass {
            get => this.heroClass;
            set => this.heroClass = value;
        }       

        public string HeroName {
            get => this.heroName;
            set => this.heroName = value;
        }

        public HeroLevel HeroLevel {
            get => heroLevel;
            set => heroLevel = value;
        }

        public IList<HeroDisease> Diseases {
            get => this.heroDiseases;
            set => this.heroDiseases = value;
        }

        public Affliction? Affliction {
            get => affliction;
            set => affliction = value;
        }

        public Virtue? Virtue {
            get => virtue;
            set => virtue = value;
        }

        public string CauseOfDeath {
            get => causeOfDeath;
            set => causeOfDeath = value;
        }

        public IList<HeroQuirk> PositiveQuirks {
            get => positiveQuirks;
            set => positiveQuirks = value;
        }

        public IList<HeroQuirk> NegativeQuirks {
            get => negativeQuirks;
            set => negativeQuirks = value;
        }

        public HeroDeath() {}

    }
}
