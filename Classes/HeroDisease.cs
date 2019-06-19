using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkestDungeonMorgueGUI {
    public class HeroDisease {
        private string diseaseName;
        private string diseaseDescription;

        public string DiseaseName {
            get => diseaseName;
            set => diseaseName = value;
        }

        public string DiseaseDescription {
            get => diseaseDescription;
            set => diseaseDescription = value;
        }

        public override string ToString() => String.Format("{0} - {1}", this.DiseaseName, this.DiseaseDescription);
    }
}
