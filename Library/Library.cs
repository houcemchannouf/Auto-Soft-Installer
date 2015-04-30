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
using System.Security.AccessControl;
using System.Security.Principal;
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
            var logFilePath = Path.Combine(path1: GetAppDataPath() , path2: "Log.txt");
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


        /// <summary>
        ///     retourne le chemin d'accès du dossier de données de l'application
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataPath()
        {
            return Path.Combine(path1: Environment.GetFolderPath(folder: Environment.SpecialFolder.CommonApplicationData), path2: "ENIT", path3: "Auto Soft-Installer");
        }

        /// <summary>
        ///     Change les permissions du dossier de donnée de l'application
        ///     Lecture/Ecriture pour l'utilisateur courant
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static void SetFolderPermission(string directory)
        {
            var info = new DirectoryInfo(directory);
            var self = WindowsIdentity.GetCurrent();
            var directorySecurity = info.GetAccessControl();
            if (self == null) return;
            directorySecurity.AddAccessRule(rule: new FileSystemAccessRule(identity: self.Name, 
                                                                           fileSystemRights: FileSystemRights.FullControl, 
                                                                           inheritanceFlags: InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, 
                                                                           propagationFlags: PropagationFlags.None, 
                                                                           type: AccessControlType.Allow));
            info.SetAccessControl(directorySecurity);
        }

        #endregion
    }
}