/***************** En-tête du module ******************\
 * Nom du module :   SoftwareSetup.cs
 * Projet        :   Développement d'un service Windows
 * Développeurs  :   Abdelkafi Ahmed & Channouf Houcem
 * 
 * Le fichier définit la classe SoftwareSetup qui hérite
 * de la classe Files.
 * 
 * Cette classe sert à décompresser les archives
 * d'installations téléchargés et d'executer les
 * fichiers d'installation.
 * 
\******************************************************/

#region Directives 'Using'

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

#endregion

namespace Auto_Soft_Installer
{
    internal class SoftwareSetup : Files
    {
        #region Constructeur

        public SoftwareSetup(string setupFile, string localDirectory, string name)
        {
            LocalPath = localDirectory;
            Name = name;
            SetupFile = setupFile;
            XtractionDirectory = localDirectory + "\\" + name.Replace(".zip", "");
        }

        #endregion

        #region Propriétés

        //  Nom du fichier d'installation
        private string SetupFile { get; set; }

        //  Chemin du dossier d'extraction du fichier ZIP
        private string XtractionDirectory { get; set; }

        #endregion

        #region Méthodes

        /// <summary>
        ///     Extraire le fichier ZIP dans le dossier d'extraction.
        /// </summary>
        public void Unzip()
        {
            //  Si le fichier n'existe pas, ne rien faire
            if (!File.Exists(path: Name)) return;
            //  Verifie si le dossier existe déjà ou non
            if (Directory.Exists(path: XtractionDirectory))
            {
                //  Si oui, le supprimer avec tout son contenu
                Directory.Delete(path: XtractionDirectory, recursive: true);
            }
            ZipFile.ExtractToDirectory(sourceArchiveFileName: LocalPath + "\\" + Name, destinationDirectoryName: XtractionDirectory);
        }

        /// <summary>
        ///     Lancer le fichier d'installation,
        ///     retourne TRUE si l'installation réussi et FALSE sinon.
        /// </summary>
        /// <returns> Boolean </returns>
        public bool Setup()
        {
            var softwareInformation = new ProcessStartInfo(fileName: XtractionDirectory + "\\" + SetupFile);
            try
            {
                // Lancement du processus
                using (var software = Process.Start(startInfo: softwareInformation))
                {
                    if (software != null)
                    {
                        // Attente de l'arret de l'installation
                        software.WaitForExit();

                        //  Raffraichie les attributs du Process
                        if (software.HasExited) software.Refresh();
                    }

                    if (software != null && software.ExitCode == 0) return true;
                }
            }
            catch (Exception)
            {
                var softwareNames = new List<string>(collection: XtractionDirectory.Split(Convert.ToChar(value: "\\")));
                var message = "Erreur lors de l'installation du logiciel ( " +
                              softwareNames[index: softwareNames.Count - 1] + " )";
                Library.Library.LogFileWriter(message: message);
                Library.Library.MessageBoxDisplayer(message: message);
                return false;
            }
            //if (software != null) software.Close();
            return false;
        }

        #endregion
    }
}