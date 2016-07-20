using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Modèle
{
    class Traitement
    {
        private Dictionary<string, Capteur> DicoCapteurs;
        private DateTime time;

        public double seuilCo2Présence = 500;

        public Traitement()
        {
            DicoCapteurs = chargerDicoCapteurs("capteurs.xtim");
            chargerDonnées(DicoCapteurs);
        }

        public Capteur getCapteurByNom(string nom)
        {
            foreach (var capteur in DicoCapteurs.Values)
            {
                if (capteur.Nom.Equals(nom))
                {
                    return capteur;
                }
            }
            return null;
        }

        public Capteur getCapteurByDesc(string desc)
        {
            foreach (var capteur in DicoCapteurs.Values)
            {
                if (capteur.Description.Equals(desc))
                {
                    return capteur;
                }
            }
            return null;
        }

        // return true quand le taux de co2 dans la cuisine est sup au seuil
        public bool isSomeoneInKitchen(DateTime date)
        {
            List<Mesure> mesures = getCapteurByNom("co2cuisine").MesureList;
            foreach(var mesure in mesures)
            {
                if (mesure.Date.Year == date.Year && mesure.Date.Month == date.Month && mesure.Date.Day == date.Day)
                {
                    if (mesure.Valeur <= seuilCo2Présence)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // return true quand le taux de co2 dans la chambre d'alain est sup au seuil
        public bool isAlainInRoom(DateTime date)
        {
            List<Mesure> mesures = getCapteurByNom("co2chambrealain").MesureList;
            foreach (var mesure in mesures)
            {
                if (mesure.Date.Year == date.Year && mesure.Date.Month == date.Month && mesure.Date.Day == date.Day)
                {
                    if (mesure.Valeur <= seuilCo2Présence)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // return true quand le taux de co2 dans la chambre de beatrice est sup au seuil
        public bool isBeatriceInRoom(DateTime date)
        {
            List<Mesure> mesures = getCapteurByNom("co2chambrebeatrice").MesureList;
            foreach (var mesure in mesures)
            {
                if (mesure.Date.Year == date.Year && mesure.Date.Month == date.Month && mesure.Date.Day == date.Day)
                {
                    if (mesure.Valeur <= seuilCo2Présence)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // return true si la température ou l'humidité augmente
        public bool isSomeoneCooking(DateTime date)
        {
            List<Mesure> mesuresT = getCapteurByNom("temperaturecuisine").MesureList;
            List<Mesure> mesuresH = getCapteurByNom("humiditycuisine").MesureList;
            if (isSomeoneInKitchen(date))
            {
             //  if()
            }
            return false;
        }

        // return le nom le la personne qqui cuisine
        public string whosCooking(DateTime date)
        {
            if (isSomeoneCooking(date))
            {
                if (isAlainInRoom(date))
                {
                    return "Beatrice";
                }
                else if (isBeatriceInRoom(date))
                {
                    return "Alain";
                }
                else
                {
                    return "Impossible de connaitre le cuisto";
                }
            }
            return "Personne ne cuisine";
        }

        // si l'humidité augmente, on considère que le plat et préparé à l'eau/vapeur
        // si la température augmente, on considère que le plat est fait au four  
        public void mealType()
        {
            //if (isSomeoneCooking())
            {

            }
        }

        // si le co2 augmente fortement dans la salle (plus que la normale FIXER seuil), on considère que des invités sont présents
        public bool areFriendsInHouse()
        {
            return false;
        }

        public bool mealForFriends(DateTime date)
        {
            if(isSomeoneCooking(date) && areFriendsInHouse())
            {
                return true;
            }
            return false;
        }

        // si le temps passé à cuisiner dépasse une certaine valeur (à fixer) on considère que le repas préparé est "élaboré"
        public void complexMeal(DateTime date)
        {
            if (isSomeoneCooking(date))
            {

            }
        }

        // déterminer à quelle heure sont pris les repas
        public void launchHour(DateTime date)
        {
            if (isSomeoneInKitchen(date))
            {

            }
        }

        public List<List<DateTime>> augmentationCo2(DateTime date)
        {
            List<Mesure> mesures = getCapteurByNom("co2cuisine").MesureList;
            List<DateTime> mesuresCroissantesConsecutives = new List<DateTime>();
            List<List<DateTime>> mesuresCroissantes = new List<List<DateTime>>();

            Mesure precedente = null;
            foreach (var mesure in mesures)
            {
                if (precedente != null)
                {
                    if (mesure.Date.Year == date.Year && mesure.Date.Month == date.Month && mesure.Date.Day == date.Day)
                    {
                        if (mesure.Valeur > precedente.Valeur + 40)
                        {
                            mesuresCroissantesConsecutives.Add(mesure.Date);
                        }
                        else
                        {
                            if (mesuresCroissantesConsecutives.Count != 0)
                            {
                                mesuresCroissantes.Add(mesuresCroissantesConsecutives);
                                mesuresCroissantesConsecutives = new List<DateTime>();
                            }
                        }
                    }
                }
                precedente = mesure;
            }
            //mesuresCroissantes.Add(mesuresCroissantesConsecutives);
            return mesuresCroissantes;
        }



        public List<List<DateTime>> diminutionCo2(DateTime date)
        {
            List<Mesure> mesures = getCapteurByNom("co2cuisine").MesureList;
            List<DateTime> mesuresDecroissantesConsecutives = new List<DateTime>();
            List<List<DateTime>> mesuresDecroissantes = new List<List<DateTime>>();

            Mesure precedente = null;
            foreach (var mesure in mesures)
            {
                if (precedente != null)
                {
                    if ((mesure.Valeur < precedente.Valeur - 40) && (isSomeoneInKitchen(date)))
                    {
                        mesuresDecroissantesConsecutives.Add(mesure.Date);
                    }
                    else
                    {
                        if (mesuresDecroissantesConsecutives.Count != 0)
                        {
                            mesuresDecroissantes.Add(mesuresDecroissantesConsecutives);
                            mesuresDecroissantesConsecutives = new List<DateTime>();
                        }
                    }
                }
                precedente = mesure;
            }
            //mesuresCroissantes.Add(mesuresCroissantesConsecutives);
            return mesuresDecroissantes;
        }

        // ---------------------------------------------------------------------------------------------------------------------------------


        #region CHARGEMENT DES DONNEES
        private Dictionary<String, Capteur> chargerDicoCapteurs(string cheminFichier)
        {
            IEnumerable<Capteur> capteurs = Utilitaire.LireFichierXML(cheminFichier);
            Dictionary<String, Capteur> DicoCapteurs = new Dictionary<string, Capteur>();

            foreach (var capteur in capteurs)
            {
                DicoCapteurs.Add(capteur.Nom, capteur);
            }

            return DicoCapteurs;
        }

        private Dictionary<String, Capteur> chargerDonnées(Dictionary<String, Capteur> dico)
        {
            string dirPath = "netatmotest";
            List<string> files = new List<string>(Directory.EnumerateFiles(dirPath));
            foreach (var file in files)
            {
                dico = Utilitaire.LireFichierDT(file, dico);
            }
            return dico;
        }

        public IEnumerable<Capteur> getListeCapteurs()
        {
            IEnumerable<Capteur> listeCapteurs = DicoCapteurs.Values;
            return listeCapteurs;
        }
        #endregion
    }
}
