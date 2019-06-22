using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkestDungeonMorgueGUI
{
    class ClassDeathStatisticsPlot
    {
        private PlotModel classPlot;

        public PlotModel ClassPlotModel {
            get => this.classPlot;
            set => this.classPlot = value;
        }

        public ClassDeathStatisticsPlot() {
            this.SetUpViewModel();
        }

        private void SetUpViewModel() {
            classPlot = new PlotModel() { Title = "Death statistics by class" };
            IDictionary<HeroClass, uint> kills = new Dictionary<HeroClass, uint>();
            foreach (var hero in Morgue.GetInstance().FallenHeroes) {
                uint count = 0;
                try {
                    count = kills[hero.HeroClass];
                    kills[hero.HeroClass] = ++count;
                } catch (KeyNotFoundException) {
                    kills.Add(hero.HeroClass, 1u);
                }
            }
            IDictionary<String, uint> mappedKills = kills.
                ToDictionary(e => Enum.GetName(typeof(HeroClass), e.Key).Replace('_', ' '), e => e.Value);



            BarSeries barSeries = new BarSeries() {
                ItemsSource = mappedKills.Select(e => new BarItem() { Value = e.Value })
            };

            CategoryAxis axis = new CategoryAxis() {
                Key = "Hero classes",
                Position = AxisPosition.Left,
                ItemsSource = mappedKills.Keys
            };

            classPlot.Series.Add(barSeries);
            classPlot.Axes.Add(axis);
        }
    }
}
