using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Modèle
{
    public class Capteur
    {
        public string Nom { get; set; }
        public List<Mesure> MesureList { get; set; }

        public string Description { get; set; }
        public string Lieu { get; set; }
        public Grandeur Grandeure { get; set; }

        public Capteur(string nom, string description, string lieu, Grandeur grandeur)
        { }

        public Capteur(string nom)
        {
            this.Nom = nom;
            this.MesureList = new List<Mesure>();
        }

        public Capteur() { }

        public override string ToString()
        {
            return $"nom = {Nom}; description = {Description}; Lieu = {Lieu}; Grandeur = {Grandeure}; mesure = {MesureList.ToString()} ";
        }

        public void ajouterMesure(Mesure mesure)
        {
            this.MesureList.Add(mesure);
        }


        public class Grandeur
        {
            public string Nom { get; set; }
            public string Unité { get; set; }
            public string Abréviation { get; set; }

            public Grandeur(string nom, string unité, string abreviation)
            {
                this.Nom = nom;
                this.Unité = unité;
                this.Abréviation = abreviation;
            }

            public Grandeur() { }
        }
    }

}
