using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SmartHome.Modèle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.VueModèle
{
    public class MainViewModel
    {
        //yDictionary<String, Capteur> DicoCapteurs = null;

        public PlotModel MyModel { get; private set; }

        public MainViewModel()
        {
            this.MyModel = new PlotModel { Title = "!--------!" };

            // Create two line series (markers are hidden by default)
            var series1 = new LineSeries { Title = "Series 1", MarkerType = MarkerType.Circle };
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(10, 18));
            series1.Points.Add(new DataPoint(20, 12));
            series1.Points.Add(new DataPoint(30, 8));
            series1.Points.Add(new DataPoint(40, 15));

            var series2 = new LineSeries { Title = "Series 2", MarkerType = MarkerType.Square };
            series2.Points.Add(new DataPoint(0, 4));
            series2.Points.Add(new DataPoint(10, 12));
            series2.Points.Add(new DataPoint(20, 16));
            series2.Points.Add(new DataPoint(30, 25));
            series2.Points.Add(new DataPoint(40, 5));

            // Add the series to the plot model
            this.MyModel.Series.Add(series1);
            this.MyModel.Series.Add(series2);
        }


    }
}
