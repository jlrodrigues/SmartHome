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
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using OxyPlot.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartHome.VueModèle
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string str = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }

        public PlotModel MyModel { get; private set; }
        public ObservableCollection<String> itemsComboBox { get; set; }
        public String selectedItem { get; set; }

        public DateTime selectedDate { get; set; }

        public string TextCuisto
        {
            get
            {
                return textCuisto;
            }

            set
            {
                textCuisto = value;
                NotifyPropertyChanged("TextCuisto");
            }
        }

        private String textCuisto;

        private Traitement traitement;

        public MainViewModel()
        {
            this.MyModel = new PlotModel { Title = "Représentation" };

            traitement = new Traitement();
            
            selectedDate = new DateTime(2014, 2, 1);
            fillComboBox();
        }

        public void fillComboBox()
        {
            itemsComboBox = new ObservableCollection<string>();

            IEnumerable<Capteur> listeCapteurs = traitement.getListeCapteurs();

            foreach (var capteur in listeCapteurs)
            {
                itemsComboBox.Add(capteur.Description);
            }
        }
        

        public void afficherUnGraphe()
        {
            if (selectedItem != null)
            {
                Capteur capteur = traitement.getCapteurByDesc(selectedItem);
                
                var lineSerie = new LineSeries()
                {
                    StrokeThickness = 2,
                    MarkerSize = 3,
                    CanTrackerInterpolatePoints = false,
                    Title = selectedItem,
                    Smooth = false,
                    RenderInLegend = false
                };


                foreach (var data in capteur.MesureList)
                {

                    if (data.Date.Year == selectedDate.Year && data.Date.Month == selectedDate.Month && data.Date.Day == selectedDate.Day)
                    {
                        lineSerie.Points.Add
                        (
                            new DataPoint(Axis.ToDouble(data.Date), data.Valeur)
                        );
                    }
                }
                this.MyModel.Axes.Add(new LinearAxis()
                {
                    Position = AxisPosition.Left,
                    Title = "Valeurs capteurs",
                    PositionAtZeroCrossing = true
                });
                this.MyModel.Title = " Représentation Graphique "+capteur.Description;

                this.MyModel.Axes.Add(new DateTimeAxis()
                {
                    Position = AxisPosition.Bottom,
                    Title = "Heure",
                    StringFormat = "HH:mm",
                    MinorIntervalType = DateTimeIntervalType.Hours,
                    IntervalType = DateTimeIntervalType.Hours,
                });

                this.MyModel.Series.Add(lineSerie);
                this.MyModel.InvalidatePlot(true);

                
            }
            else
            {
                MessageBox.Show("Veuillez selectionner un Lieu");
            }
        }

        internal void quitterAppli()
        {
            Application.Current.Shutdown();
        }

        public void creationGraphe(DateTime date)
        {
            this.MyModel.Axes.Clear();
            this.MyModel.Series.Clear();

            if (selectedItem != null)
            {
                foreach (var capteur in traitement.getListeCapteurs())
                {
                    var lineSerie = new LineSeries()
                    {
                        StrokeThickness = 2,
                        MarkerSize = 3,
                        CanTrackerInterpolatePoints = false,
                        Title = capteur.Nom,
                        Smooth = false,
                        RenderInLegend = false
                    };


                    foreach (var data in capteur.MesureList)
                    {

                        if (data.Date.Year == date.Year && data.Date.Month == date.Month && data.Date.Day == date.Day)
                        {
                            lineSerie.Points.Add
                            (
                                new DataPoint(Axis.ToDouble(data.Date), data.Valeur)
                            );
                        }
                    }
                    this.MyModel.Axes.Add(new LinearAxis()
                    {
                        Position = AxisPosition.Left,
                        Minimum = 0,
                        Maximum = 3000,
                        Title = "Valeurs capteurs",
                        PositionAtZeroCrossing = true
                    });
                    this.MyModel.Title = " Représentation Graphique de tout les capteurs présents dans cette pièce";

                    this.MyModel.Axes.Add(new DateTimeAxis()
                    {
                        Position = AxisPosition.Bottom,
                        Title = "Heure",
                        StringFormat = "HH:mm",
                        MajorGridlineStyle = LineStyle.Solid,
                        // MinorGridlineStyle = LineStyle.Automatic,
                        MinorIntervalType = DateTimeIntervalType.Hours,
                        IntervalType = DateTimeIntervalType.Hours,
                        IntervalLength = 80,
                        PositionAtZeroCrossing = true
                    });

                    this.MyModel.Series.Add(lineSerie);
                    this.MyModel.InvalidatePlot(true);

                }
            }
            else
            {
                MessageBox.Show("Veuillez selectionner un Lieu");
            }
        }

        public void afficherLigneSeuil()
        {
            if (selectedItem != null)
            {
                if (selectedItem.Contains("Co2"))
                {
                    this.MyModel.Annotations.Add(new LineAnnotation
                    {
                        Type = LineAnnotationType.Horizontal,
                        Y = traitement.seuilCo2Présence,
                        Text = "Seuil de présence",
                        Color = OxyColors.Pink
                    });
                }

                this.MyModel.InvalidatePlot(true);
            }
        }


        #region EVENTS
        internal void comboBoxSelectionChanged()
        {
            Console.WriteLine("ITEM SELECTED : " + selectedItem);
        }

        internal void calendarSelectedDateChanged(Calendar calendrier)
        {
            if (calendrier.SelectedDate != null)
            {
                //creationGraphe(calendrier.SelectedDate.Value);
                afficherUnGraphe();
                afficherLigneSeuil();
            }
        }

        internal void clearGraphe()
        {
            Console.WriteLine("BUTTON CLICKED");
            this.MyModel.Axes.Clear();
            this.MyModel.Series.Clear();
            this.MyModel.InvalidatePlot(true);
        }


        // return le temps passé à cuisiner --> stop time quand la température et/ou l'humidité diminue
        public void timeSpentCooking()
        {
            Console.WriteLine("Btn clicked");
            List<List<DateTime>> listMesure = traitement.augmentationCo2(selectedDate);
            List<List<DateTime>> listMesureDe = traitement.diminutionCo2(selectedDate);
            DateTime descente;
            bool flag = false;

            foreach (var i in listMesureDe)
            {
                descente = i.First();
                foreach (var mesures in listMesure)
                {
                    double first = DateTimeAxis.ToDouble(mesures.First());
                    double last = DateTimeAxis.ToDouble(descente);
                    this.MyModel.Annotations.Add(new LineAnnotation
                    {
                        Type = LineAnnotationType.Vertical,
                        X = first,
                        Text = " Début",
                        Color = OxyColors.Chocolate
                    });
                    this.MyModel.Annotations.Add(new LineAnnotation
                    {
                        Type = LineAnnotationType.Vertical,
                        X = last,
                        Text = "Fin",
                        Color = OxyColors.DarkSalmon
                    });
                }
            }
           TextCuisto =  traitement.whosCooking(listMesure.First().First());
            this.MyModel.InvalidatePlot(true);
        }

        #endregion
    }
}
