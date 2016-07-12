using SmartHome.VueModèle;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SmartHome
{

    public partial class App : Application
    {
        public static MainViewModel VM = new MainViewModel();
    }
}
