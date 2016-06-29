using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Modèle
{
    public class Mesure
    {
        public DateTime Date { get; set; }
        public double Valeur { get; set; }

        public Mesure(DateTime date, double valeur)
        {
            this.Date = date;
            this.Valeur = valeur;
        }

        public Mesure(double uneValeur, DateTime date)
        {
            this.Valeur = uneValeur;
            this.Date = date;
        }

        public override string ToString()
        {
            return $"date = {Date}; valeur = {Valeur}";
        }
    }
}
