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

        public struct HeroInfo {
            public HeroClass @class;
            public string name;
            public HeroLevel level;
            public string deathCause;
            public Affliction affliction;
            public Virtue virtue;

            public HeroInfo(HeroDeath hd) {
                this.@class = hd.HeroClass;
                this.name = hd.HeroName;
                this.level = hd.HeroLevel;
                this.deathCause = hd.CauseOfDeath;
                this.affliction = hd.Affliction == null ? global::Affliction.None : hd.Affliction.GetValueOrDefault();
                this.virtue = hd.Virtue == null ? global::Virtue.None : hd.Virtue.GetValueOrDefault();
            }

            public override string ToString() {
                StringBuilder sb = new StringBuilder();
                sb.Append(this.name).Append("\n");
                sb.Append(Enum.GetName(typeof(HeroLevel), level)).Append(" ");
                sb.Append(Enum.GetName(typeof(HeroClass), @class)).Append("\n");
                sb.Append("Cause of death: ").Append(this.deathCause).Append("\n");
                sb.Append("Affliction: ").Append(Enum.GetName(typeof(Affliction), this.affliction)).Append("\n");
                sb.Append("Virtue: ").Append(Enum.GetName(typeof(Virtue), this.virtue)).Append("\n");
                return sb.ToString();
            }
        }

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

        public HeroInfo GetHeroInfo() => new HeroInfo(this);
    }
}
