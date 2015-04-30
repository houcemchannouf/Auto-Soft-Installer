/***************** En-tête du module ******************\
 * Nom du module :   Xml.cs
 * Projet        :   Développement d'un service Windows
 * Développeurs  :   Abdelkafi Ahmed & Channouf Houcem
 * 
 * Ce fichier définit la classe Xml.
 * 
 * Cette classe sert a charger un fichier Xml à partir
 * d'un chemin, le convertir en objet de type Softwares.
 * 
\******************************************************/

#region Directives 'Using'

using System;
using System.IO;
using System.Xml.Serialization;

#endregion

namespace Auto_Soft_Installer
{
    /// <summary>
    ///     Cette classe sert a charger un fichier Xml à partir
    ///     d'un chemin, le convertir en objet de type Softwares.
    /// </summary>
    public class Xml : Files
    {
        #region Constructeur

        /// <summary>
        ///     Constructeur
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="name"></param>
        public Xml(string localPath, string name)
        {
            Name = name;
            LocalPath = localPath;
        }

        #endregion

        #region Méthodes

        /// <summary>
        ///     Convertit le contenu du fihier XML en objet Logiciels, et le retourne .
        /// </summary>
        /// <returns> Logiciels </returns>
        public Softwares ObjectConversion()
        {
            Softwares xmlData = null;
            try
            {
                var deserializer = new XmlSerializer(type: typeof (Softwares));
                using (var reader = new StreamReader(path: LocalPath))
                {
                    var obj = deserializer.Deserialize(textReader: reader);
                    xmlData = (Softwares) obj;
                    //reader.Close();
                }
            }
            catch (Exception)
            {
                const string message = "Fichier XML introuvable ou fichier XML corrompu (Vérifiez Syntaxe)";
                Library.Library.LogFileWriter(message: message);
            }
            return xmlData;
        }

        #endregion
    }
}