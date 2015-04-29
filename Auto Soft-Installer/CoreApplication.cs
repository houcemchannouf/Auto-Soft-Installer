/***************** En-tête du module ******************\
 * Nom du module :   CoreApplication.cs
 * Projet        :   Développement d'un service Windows
 * Développeurs  :   Abdelkafi Ahmed & Channouf Houcem
 * 
 * Le fichier définit la classe CoreApplication qui
 * est le noyau de notre application.
 * 
\******************************************************/

#region Directives 'Using'

using System.IO;

#endregion

namespace Auto_Soft_Installer
{
    /// <summary>
    ///     Le fichier définit la classe CoreService qui
    ///     est le coeur de l'application
    ///     Cette application se connecte à un serveur FTP, télécharge
    ///     la liste des logiciels disponibles, et installe les
    ///     logiciels non installés sur le client.
    /// </summary>
    internal class CoreApplication
    {
        #region Constructeur

        /// <summary>
        ///     Constructeur de la classe CoreService.
        /// </summary>
        public CoreApplication()
        {
            AnyException = false;
            IsFinished = false;
            _serverXmlFileName = Library.Library.SettingsReader(key: "serverXmlFileName");
            _localXmlFileName = Library.Library.SettingsReader(key: "localXmlFileName");
            _downloadDirectory = Library.Library.SettingsReader(key: "downloadDirectory");
            _localXmlFile = new Xml(localPath: _downloadDirectory + "\\" + _localXmlFileName, name: _localXmlFileName);
            _serverXmlFile = new Xml(localPath: _downloadDirectory + "\\" + _serverXmlFileName, name: _serverXmlFileName);
        }

        #endregion

        #region Champs

        private static string _serverXmlFileName;
        private static string _localXmlFileName;
        private static string _downloadDirectory;
        private static Xml _localXmlFile;
        private static Xml _serverXmlFile;

        #endregion

        #region Propriétés

        internal bool AnyException { get; private set; } //  TRUE si il y a une exception, FALSE sinon
        internal bool IsFinished { get; private set; } //  TRUE si le programme se termine correctement, FALSE sinon

        #endregion

        #region Méthodes

        /// <summary>
        ///     Lancement de toute l'application
        ///     à partir de cette méthode.
        /// </summary>
        internal void Run()
        {
            Library.Library.LogFileWriter(message: "Program launched");

            //  Téléchargement du fichier XML à partir du serveur FTP
            if (!Ftp.Download(fileName: _serverXmlFileName))
                OnException();
            else
            {
                //  Contient la liste des logiciels disponibles sur le client
                var localElements = _localXmlFile.ObjectConversion();

                //  Contient la liste des logiciels disponibles sur le serveur
                var serverElements = _serverXmlFile.ObjectConversion();

                //  Si une des deux listes est NULL => arrêt immédiat du programme
                if (serverElements == null || localElements == null) OnException();
                else
                {
                    //  A la sortie de la boucle, seuls les éléments non présents dans la liste des 
                    //  logiciels installés (Client) seront présents dans la liste des logiciels disponibles (Serveur)
                    for (var i = 0; i < localElements.softwares.Count; i++)
                    {
                        for (var j = 0; j < serverElements.softwares.Count; j++)
                        {
                            if (localElements.softwares[index: i].CompareTo(obj: serverElements.softwares[j]) == 1)
                            {
                                //  il sera supprimé de la liste des logiciels disponibles sur le serveur
                                serverElements.softwares.RemoveAt(index: j);
                            }
                        }
                    }

                    //  Si le contenu des deux fichiers est le même
                    //  Le programme s'arrête
                    for (var i = 0; i < serverElements.softwares.Count; i++)
                    {
                        //  Telechargement du fichier d'installation compressé
                        Ftp.Download(fileName: serverElements.softwares[index: i].Name);

                        var I = new SoftwareSetup(setupFile: serverElements.softwares[index: i].SetupFile, localDirectory: _downloadDirectory + "\\", name: serverElements.softwares[index: i].Name);

                        //  Decompression des fichier d'installation
                        I.Unzip();

                        //  Si l'installation réussi
                        if (I.Setup())
                        {
                            //  Ajouter les éléments restants dans la liste des logiciels 
                            //  disponibles sur le serveur à la liste des logiciels installés 
                            //  (après installation)
                            localElements.softwares.Add(item: serverElements.softwares[index: i]);

                            //  Effacer les fichiers d'installation
                            Directory.Delete(path: _downloadDirectory + "\\" + serverElements.softwares[index: i].Name.Replace(oldValue: ".zip", newValue: ""), recursive: true);
                        }

                        //  Effacer l'archive téléchargé
                        File.Delete(path: _downloadDirectory + "\\" + serverElements.softwares[index: i].Name);
                    }

                    //  Ecriture dans le fichier XML local
                    localElements.XmlWriter(destinationPath: _downloadDirectory + "\\" + _localXmlFileName);

                    //  Programme executé sans erreurs ni exceptions (avec risque de logiciel non installé
                    //  car ça dépend des fichiers d'installations)
                    Library.Library.LogFileWriter(message: "Program finished");
                    IsFinished = true;
                }

                //  Supprime le fichier logiciels.xml téléchargé du serveur FTP
                File.Delete(path: _downloadDirectory + "\\" + _serverXmlFileName);
            }
        }

        /// <summary>
        ///     Dans le cas où une exception est levée,
        ///     AnyException = TRUE
        ///     et
        ///     IsFinished = FALSE
        ///     pour signaler au programme principale qu'une exception est levée
        ///     et donc suspension de l'application en cours.
        /// </summary>
        private void OnException()
        {
            AnyException = true;
            IsFinished = false;
        }

        #endregion
    }
}