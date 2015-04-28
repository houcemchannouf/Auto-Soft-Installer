/***************** En-tête du module ******************\
 * Nom du module :   Xml.cs
 * Projet        :   Développement d'un service Windows
 * Développeurs  :   Abdelkafi Ahmed & Channouf Houcem
 * 
 * Ce fichier définit la classe Xml.
 * 
 * Cette classe sert a charger un fichier Xml à partir
 * d'un chemin, le convertir en objet de type Logiciels.
 * 
\******************************************************/

#region Directives 'Using'

using System;
using System.IO;
using System.Xml.Serialization;

#endregion

namespace Auto_Soft_Installer
{
    internal class Xml : Files
    {
        #region Constructeur

        public Xml(string localPath, string name)
        {
            if (name != null) Name = name;
            if (localPath != null) LocalPath = localPath;
        }

        #endregion

        #region Méthodes

        /// <summary>
        ///     Convertit le contenu du fihier XML en objet Logiciels, et le retourne .
        /// </summary>
        /// <returns> Logiciels </returns>
        internal Softwares ObjectConversion()
        {
            Softwares xmlData = null;
            try
            {
                var deserializer = new XmlSerializer(typeof (Softwares));
                using (var reader = new StreamReader(LocalPath))
                {
                    var obj = deserializer.Deserialize(reader);
                    xmlData = (Softwares) obj;
                    reader.Close();
                }
            }
            catch (Exception)
            {
                const string message = "Fichier XML introuvable ou fichier XML corrompu (Vérifiez Syntaxe)";
                Library.Library.LogFileWriter(message);
            }
            return xmlData;
        }

        #endregion
    }
}