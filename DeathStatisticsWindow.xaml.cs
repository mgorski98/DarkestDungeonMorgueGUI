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
    /// Interaction logic for DeathStatisticsWindow.xaml
    /// </summary>
    public partial class DeathStatisticsWindow : Window {

        private LevelDeathStatisticsPlot levelPlot;
        private ClassDeathStatisticsPlot classPlot;

        public DeathStatisticsWindow() {
            InitializeComponent();
            this.InitializeViews();
        }

        private void InitializeViews() {
            this.levelPlot = new LevelDeathStatisticsPlot();
            this.classPlot = new ClassDeathStatisticsPlot();
            this.ClassPlot.Model = this.classPlot.ClassPlotModel;
            this.LevelPlot.Model = this.levelPlot.LevelPlotModel;
        }
    }
}
