/***************** En-tête du module ******************\
 * Nom du module :   Library.cs
 * Projet        :   Développement d'un service Windows
 * Développeurs  :   Abdelkafi Ahmed & Channouf Houcem
 * 
 * Ce fichier définit la classe Library.
 * 
 * Cette classe contient les méthodes statiques 
 * utilisées par le service.
 * 
\******************************************************/

#region Directives 'Using'

using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

#endregion

namespace Library
{
    /// <summary>
    ///     Représente les méthodes statiques utilisées par l'application
    /// </summary>
    public static class Library
    {
        #region Méthodes

        /// <summary>
        ///     Ecrire le message dans le fichier log : Log.txt
        /// </summary>
        /// <param name="message"></param>
        public static void LogFileWriter(string message)
        {
            var logFilePath = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments) + @"\Log.txt";
            try
            {
                if (!File.Exists(path: logFilePath))
                {
                    File.Create(path: logFilePath);
                }
                using (var sw = new StreamWriter(path: logFilePath, append: true))
                {
                    sw.WriteLine(value: DateTime.Now + ": " + message);
                    sw.Flush();
                    //sw.Close();
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Retourne la valeur de la clée entrée en paramétre du fichier App.config
        /// </summary>
        /// <param name="key"></param>
        /// <returns> String </returns>
        public static string SettingsReader(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            var result = appSettings[name: key] ?? "clée non trouvée";
            return result;
        }

        /// <summary>
        ///     Affiche un message d'alerte contenant
        ///     la chaine de caractére passée en paramétre
        /// </summary>
        /// <param name="message"></param>
        public static void MessageBoxDisplayer(string message)
        {
            MessageBox.Show(text: message, caption: "Erreur", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);
        }

        /// <summary>
        ///     Ajoute le programme à la liste
        ///     des élément de démarrage
        ///     du systéme d'exploitation
        ///     en ajoutant au registre :
        ///     HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
        ///     une clé contenant le chemin d'accès de notre application
        /// </summary>
        /// <param name="name"></param>
        public static void AddToStartupPrograms(string name)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(name: @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", writable: true))
            {
                if (key == null) return;
                var value = key.GetValue(name: name);
                if (value != null) return;
                key.SetValue(name: "name", value: "\"" + Application.ExecutablePath + "\"");
                //key.Close();
            }
        }

        #endregion
    }
}