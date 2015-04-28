/***************** En-tête du module ******************\
 * Nom du module :   Ftp.cs
 * Projet        :   Développement d'un service Windows
 * Développeurs  :   Abdelkafi Ahmed & Channouf Houcem
 * 
 * Le fichier définit la classe Ftp .
 * 
 * Cette classe contient la méthode de connexion au
 * serveur FTP distant, et celle du téléchargement
 * d'un fichier depuis le serveur FTP.
 * 
\******************************************************/

#region Directives 'Using'

using System;
using System.IO;
using System.Net;

#endregion

namespace Auto_Soft_Installer
{
    internal static class Ftp
    {
        #region Constructeur

        static Ftp()
        {
            ServerIp = Library.Library.SettingsReader("serverIp");
            UserName = Library.Library.SettingsReader("userName");
            Password = Library.Library.SettingsReader("password");
            DownloadDirectory = Library.Library.SettingsReader("downloadDirectory");
            BufferSize = Convert.ToInt32(Library.Library.SettingsReader("bufferSize"));
        }

        #endregion

        #region Propriétés

        private static readonly string ServerIp;
        private static readonly string UserName;
        private static readonly string Password;
        private static readonly string DownloadDirectory;
        private static readonly int BufferSize;
        private static FtpWebRequest _ftpWebRequest;

        #endregion

        #region Methodes

        /// <summary>
        ///     Connexion au serveur FTP.
        /// </summary>
        private static void FtpCredential()
        {
            _ftpWebRequest.Credentials = new NetworkCredential(userName: UserName, password: Password);
            _ftpWebRequest.UseBinary = true;
            _ftpWebRequest.UsePassive = true;
            _ftpWebRequest.KeepAlive = true;
        }

        /// <summary>
        ///     Télecharge un fichier à partir du serveur FTP.
        /// </summary>
        /// <param name="fileName"></param>
        public static bool Download(string fileName)
        {
            //  Création requete de connexion FTP
            _ftpWebRequest = (FtpWebRequest) WebRequest.Create(requestUriString: "ftp://" + ServerIp + "/" + fileName);

            //  Connexion au serveur FTP
            FtpCredential();

            // Type de la requete FTP : RETR qui représente le téléchargement depuis le serveur
            _ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            Library.Library.LogFileWriter(message: "Connexion au serveur à l'@" + ServerIp);
            try
            {
                //  Retourne la réponse du serveur
                using (var ftpWebResponse = (FtpWebResponse) _ftpWebRequest.GetResponse())
                {
                    using (var ftpStream = ftpWebResponse.GetResponseStream())
                    {
                        using (
                            var fluxLocalFile = new FileStream(path: DownloadDirectory + "\\" + fileName, mode: FileMode.Create))
                        {
                            var byteBuffer = new byte[BufferSize];
                            if (ftpStream != null)
                            {
                                Library.Library.LogFileWriter(message: "téléchargement du fichier " + fileName);
                                var bytesRead = ftpStream.Read(buffer: byteBuffer, offset: 0, count: BufferSize);

                                //  Telecharge le fichier en ecrivant les données du buffer jusqu'a ce que le transfert est terminé
                                try
                                {
                                    while (bytesRead > 0)
                                    {
                                        fluxLocalFile.Write(array: byteBuffer, offset: 0, count: bytesRead);
                                        bytesRead = ftpStream.Read(buffer: byteBuffer, offset: 0, count: BufferSize);
                                    }
                                }
                                catch (Exception)
                                {
                                    var message = "Erreur lors du téléchargement du fichier " + fileName +
                                                  "à partir du serveur FTP";
                                    Library.Library.LogFileWriter(message: message);
                                    return false;
                                }
                            }
                            //  Fermeture des flux
                            fluxLocalFile.Close();
                        }
                        if (ftpStream != null) ftpStream.Close();
                    }
                    ftpWebResponse.Close();
                }
                _ftpWebRequest = null;
            }
            catch (Exception)
            {
                var message = "Impossible de télécharger le fichier " + fileName;
                Library.Library.LogFileWriter(message: message);
                return false;
            }
            return true;
        }

        #endregion
    }
}