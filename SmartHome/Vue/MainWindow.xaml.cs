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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartHome.Vue
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.VM;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            App.VM.clearGraphe();
        }

        private void comboBoxLieu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.VM.comboBoxSelectionChanged();
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            App.VM.calendarSelectedDateChanged((Calendar) sender);
        }

        private void btnTpsCuisine_Click(object sender, RoutedEventArgs e)
        {
            App.VM.timeSpentCooking();
        }

        private void btnquit_Click(object sender, RoutedEventArgs e)
        {
            App.VM.quitterAppli();
        }

        private void PlotView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
