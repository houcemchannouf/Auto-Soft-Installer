/***************** En-tête du module ******************\
 * Nom du module :   Software.cs
 * Projet        :   Développement d'un service Windows
 * Développeurs  :   Abdelkafi Ahmed & Channouf Houcem
 * 
 * Ce fichier définit la classe Software.
 * 
 * Cette classe sert à la sérialisation des éléments
 * <Software> des fichiers Xml, elle implémente aussi
 * la fonction CompareTo de l'interface IComparable.
 * 
\******************************************************/

#region Directives 'Using'

using System;

#endregion

namespace Auto_Soft_Installer
{
    /// <summary>
    ///     Cette classe sert à la sérialisation des éléments
    ///     des fichiers Xml, elle implémente aussi
    ///     la fonction CompareTo de l'interface IComparable.
    /// </summary>
    public class Software : IComparable
    {
        #region Méthodes

        /// <summary>
        ///     Redéfinition de la fonction CompareTo de l'interface IComparable,
        ///     retourne 1 si les deux objets sont égaux et 0 sinon .
        /// </summary>
        /// <returns> Integer </returns>
        public int CompareTo(object obj)
        {
            var software = (Software) obj;
            return Name == software.Name && SetupFile == software.SetupFile ? 1 : 0;
        }

        #endregion

        #region Propriétés

        /// <summary>
        ///     Nom du logiciel
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Nom du fichier d'installation
        /// </summary>
        public string SetupFile { get; set; }

        #endregion
    }
}