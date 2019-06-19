using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkestDungeonMorgueGUI {
    public class HeroQuirk {
        private string quirkName;
        private QuirkType quirkType;
        private string quirkDescription;

        public string QuirkName {
            get => this.quirkName;
            set => this.quirkName = value;
        }

        public QuirkType QType {
            get => this.quirkType;
            set => this.quirkType = value;
        }

        public string QuirkDescription {
            get => this.quirkDescription;
            set => this.quirkDescription = value;
        }

        public HeroQuirk() {}

        public HeroQuirk(HeroQuirk other) {
            this.quirkName = other.quirkName;
            this.quirkDescription = other.quirkDescription;
            this.quirkType = other.quirkType;
        }

        public override string ToString() {
            return String.Format("{0} - {1}", this.QuirkName, this.QuirkDescription);
        }
    }
}
