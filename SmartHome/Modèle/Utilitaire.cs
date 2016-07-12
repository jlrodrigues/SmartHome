using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartHome.Modèle
{
    class Utilitaire
    {
        public static IEnumerable<Capteur> LireFichierXML(string nomFichier)
        {
            Console.WriteLine("LireFichierXML ..");
            XDocument xdoc = XDocument.Load(nomFichier);
            XElement xdocattribute = xdoc.Root;
            if (xdocattribute != null)
            {
                IEnumerable<XElement> xCapteurList = xdocattribute.Elements("capteur");
                foreach (XElement xCapteur in xCapteurList)
                {
                    var nom = xCapteur.Element("id").Value;
                    Capteur unCapteur = new Capteur(nom);

                    unCapteur.Description = xCapteur.Element("description").Value;
                    unCapteur.Lieu = xCapteur.Element("lieu").Value;

                    var box = xCapteur.Element("box").Value;

                    var xGrandeur = xCapteur.Element("grandeur");
                    var nomGrandeur = xGrandeur.Attribute("nom").Value;
                    var unitéGrandeur = xGrandeur.Attribute("unite").Value;
                    var abrevGrandeur = xGrandeur.Attribute("abreviation").Value;

                    if (unCapteur.Lieu.Equals("Cuisine")) //box.Equals("netatmo")) //&& nomGrandeur.Equals("Temperature") || nomGrandeur.Equals("Co2") || nomGrandeur.Equals("Pression"))
                    {
                        unCapteur.Grandeure = new Capteur.Grandeur(nomGrandeur, unitéGrandeur, abrevGrandeur);

                        yield return unCapteur;
                    }
                }
            }
        }

        public static Dictionary<string, Capteur> LireFichierDT(string nomFichier, Dictionary<string, Capteur> DicoCapteurs)
        {
            var données = File.ReadAllLines(nomFichier);

            foreach (var sligne in données)
            {
                //Console.WriteLine(sligne);
                var ligne = sligne;
                // Récupération de la date
                var strDate = ligne.Substring(1, 19);

                ligne = ligne.Substring(22);
                var positionEspace = ligne.IndexOf(' ');
                var strCapteur = ligne.Substring(0, positionEspace);
                var strValeur = ligne.Substring(positionEspace + 1);
                var date = DateTime.Parse(strDate);

                Capteur unCapteur = null;

                if (DicoCapteurs.TryGetValue(strCapteur, out unCapteur))
                {
                    if (unCapteur.Nom.Equals(strCapteur))
                    {
                        var uneValeur = Convert.ToDouble(strValeur);
                        var m = new Mesure(uneValeur, date);
                        unCapteur.ajouterMesure(m);
                    }
                }
            }
            return DicoCapteurs;
        }
    }
}
