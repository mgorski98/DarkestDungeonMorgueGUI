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
using System.Windows.Shapes;

namespace DarkestDungeonMorgueGUI {
    /// <summary>
    /// Interaction logic for HeroDeathInfoWindow.xaml
    /// </summary>
    public partial class HeroDeathInfoWindow : Window {

        //TODO: think about how info screen should look like (maybe readonly listviews)

        private HeroDeath heroDeathInfo;

        public HeroDeathInfoWindow() {
            InitializeComponent();
        }

        public HeroDeathInfoWindow(HeroDeath d) {
            this.heroDeathInfo = d;
            this.InitializeComponent();
            this.HeroPortrait.Source = new BitmapImage(d.ImagePath);
            var info = d.GetHeroInfo();
            StringBuilder sb = new StringBuilder();
            this.HeroInfoTextBlock.Text = sb.
                Append(info.name).Append("\n").
                Append(info.level.ToString()).Append(" ").
                Append(info.@class.ToString().Replace('_', ' ')).Append("\n").
                Append("Cause of death: ").Append("\n").Append(info.deathCause).
                Append("\n").Append("Virtue: ").Append(info.virtue.ToString()).
                Append("\n").Append("Affliction: ").Append(info.affliction.ToString()).
                ToString();
            this.HeroPositiveQuirks.ItemsSource = d.PositiveQuirks;
            this.HeroNegativeQuirks.ItemsSource = d.NegativeQuirks;
            this.HeroDiseases.ItemsSource = d.Diseases;
        }


    }
}
