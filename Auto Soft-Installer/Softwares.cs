/***************** En-tête du module ******************\
 * Nom du module :   Softwares.cs
 * Projet        :   Développement d'un service Windows
 * Développeurs  :   Abdelkafi Ahmed & Channouf Houcem
 * 
 * Ce fichier définit la classe Softwares .
 * 
 * Cette classe sert à la sérialisation des éléments
 * <Softwares> des fichiers Xml et l'écriture des objets
 * de types Softwares dans les fichiers XML
 * Contient aussi une liste d'objets de type Software.
 * 
\******************************************************/

#region Directives 'Using'

using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

#endregion

namespace Auto_Soft_Installer
{
    /// <summary>
    ///     Utilisé dans la sérialisation des fichiers XML (conversion des fichiers XML en objets)
    /// </summary>
    public class Softwares
    {
        #region Propriétés

        /// <summary>
        ///     Liste d'objet Logiciel, Utilisé dans la sérialisation des fichiers XML
        ///     (conversion des fichiers XML en objets)
        /// </summary>
        [XmlElement("Software")] public List<Software> softwares = new List<Software>();

        #endregion

        #region Méthodes

        /// <summary>
        ///     Ecriture de l'objet softwares dans le fichier XML
        ///     dont le chemin est passé en paramétre
        /// </summary>
        /// <param name="destinationPath"></param>
        public void XmlWriter(string destinationPath)
        {
            var serializer = new XmlSerializer(type: typeof (Softwares));
            using (var writer = new StreamWriter(path: destinationPath))
            {
                serializer.Serialize(textWriter: writer, o: this);
            }
        }

        #endregion
    }
}