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
        private static void FtpConnection()
        {
            _ftpWebRequest.Credentials = new NetworkCredential(UserName, Password);
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
            try
            {
                //  Création requete de connexion FTP
                _ftpWebRequest = (FtpWebRequest) WebRequest.Create("ftp://" + ServerIp + "/" + fileName);

                //  Connexion au serveur FTP
                FtpConnection();

                // Type de la requete FTP : RETR qui représente le téléchargement depuis le serveur
                _ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                //  Retourne la réponse du serveur
                using (var ftpWebResponse = (FtpWebResponse) _ftpWebRequest.GetResponse())
                {
                    using (var fluxFtp = ftpWebResponse.GetResponseStream())
                    {
                        using (
                            var fluxFichierLocal = new FileStream(DownloadDirectory + "\\" + fileName,
                                FileMode.Create))
                        {
                            var byteBuffer = new byte[BufferSize];
                            if (fluxFtp != null)
                            {
                                var bytesRead = fluxFtp.Read(byteBuffer, 0, BufferSize);

                                //  Telecharge le fichier en ecrivant les données du buffer jusqu'a ce que le transfert est terminé
                                try
                                {
                                    while (bytesRead > 0)
                                    {
                                        fluxFichierLocal.Write(byteBuffer, 0, bytesRead);
                                        bytesRead = fluxFtp.Read(byteBuffer, 0, BufferSize);
                                    }
                                }
                                catch (Exception)
                                {
                                    const string message =
                                        "Erreur lors du téléchargement du fichier à partir du serveur FTP";
                                    Library.Library.LogFileWriter(message);
                                    return false;
                                }
                            }
                            //  Fermeture des flux
                            //fluxFichierLocal.Close();
                        }
                        //if (fluxFtp != null) fluxFtp.Close();
                    }
                    //ftpWebResponse.Close();
                }
                _ftpWebRequest = null;
            }
            catch (Exception)
            {
                const string message =
                    "Impossible de se connecter au serveur FTP, Contactez votre administrateur réseau !";
                Library.Library.LogFileWriter(message);
                return false;
            }
            return true;
        }

        #endregion
    }
}