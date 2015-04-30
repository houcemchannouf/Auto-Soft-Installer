/***************** En-tête du module ******************\
 * Nom du module :   Program.cs
 * Projet        :   Développement d'un service Windows
 * Développeurs  :   Abdelkafi Ahmed & Channouf Houcem
 * 
 * Le fichier définit Point d'entrée principal 
 * de l'application.
 * 
\******************************************************/

#region Directives 'Using'

using System;
using System.IO;
using System.Threading;

#endregion

namespace Auto_Soft_Installer
{
    /// <summary>
    ///     Point d'entrée principal de l'application.
    /// </summary>
    public static class Program
    {
        #region Méthodes

        /// <summary>
        ///     Méthode Main()
        /// </summary>
        [STAThread]
        public static void Main()
        {
            //  Ajoute le programme à la liste des programmes de démarrage
            //  du systéme d'exploitation
            Library.Library.AddToStartupPrograms(name: "Software Auto-install");

            //  Ajouter les permissions Full Control aux dossier ENIT et Auto Soft-Installer
            //  dans le dossier de donnés d'application ProgramData
            Library.Library.SetFolderPermission(
                directory: Directory.GetParent(Library.Library.GetAppDataPath()).ToString());
            Library.Library.SetFolderPermission(directory: Library.Library.GetAppDataPath());

            var sleepPeriod = Convert.ToInt32(value: Library.Library.SettingsReader(key: "sleepPeriod"));

            //  Instanciation d'un objet de la classe CoreApplication
            var app = new CoreApplication();

            //  Tant que l'application ne s'est pas 
            //  arrêté correctement on boucle
            while (!app.IsFinished)
            {
                //  S'il y a eu une exception, 
                //  on met le processus en pause pendant une durée
                //  sleepPeriod (secondes)
                if (app.AnyException)
                {
                    Library.Library.LogFileWriter(message: "Exception levée, reprise du service après " + sleepPeriod +
                                                           " secondes");

                    //  Le paramètre de la méthode Sleep 
                    //  de la classe Thread à pour unité
                    //  le milliseconde donc on doit multiplier
                    //  par 1000 pour obtenir un temps en seconde
                    Thread.Sleep(millisecondsTimeout: sleepPeriod*1000);
                }

                //  Et on execute l'application
                app.Run();
            }
        }

        #endregion

    }
}