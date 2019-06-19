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
    class LevelDeathStatisticsPlot
    {
        private PlotModel levelPlot;

        public PlotModel LevelPlotModel {
            get => this.levelPlot;
            set => this.levelPlot = value;
        }

        public LevelDeathStatisticsPlot() {
            this.SetUpViewModel();
        }

        private void SetUpViewModel() {
            levelPlot = new PlotModel() { Title = "Death statistics by level" };

            IDictionary<HeroLevel, uint> kills = new Dictionary<HeroLevel, uint>();

            foreach (var hero in Morgue.GetInstance().FallenHeroes) {
                uint count = 0;
                try {
                    count = kills[hero.HeroLevel];
                    kills.Add(hero.HeroLevel, ++count);
                } catch (KeyNotFoundException) {
                    kills.Add(hero.HeroLevel, 1u);
                }
            }

            IDictionary<string, uint> mappedKills = kills.ToDictionary(e => Enum.GetName(typeof(HeroLevel), e.Key), e => e.Value);

            BarSeries barSeries = new BarSeries() {
                ItemsSource = mappedKills.Select(e => new BarItem() { Value = e.Value })
            };

            CategoryAxis axis = new CategoryAxis() {
                ItemsSource = mappedKills.Keys,
                Key = "Hero levels",
                Position = AxisPosition.Bottom
            };

            levelPlot.Axes.Add(axis);
            levelPlot.Series.Add(barSeries);
        }
    }
}
