/***************** En-tête du module ******************\
 * Nom du module :   Fichier.cs
 * Projet        :   Développement d'un service Windows
 * Développeurs  :   Abdelkafi Ahmed & Channouf Houcem
 * 
 * Le fichier définit la classe Files dont héritent 
 * les deux classes Xml et SoftwareSetup 
 * (Nommée Files pour ne pas confondre avec
 * la classe File du Framework .NET)
 * 
\******************************************************/

namespace Auto_Soft_Installer
{
    /// <summary>
    ///     Classe Files dont héritent les deux classes
    ///     Xml et SoftwareSetup
    ///     (Nommée Files pour ne pas confondre avec
    ///     la classe File du Framework .NET)
    /// </summary>
    internal class Files
    {
        #region Constructeur

        /// <summary>
        ///     Constructeur
        /// </summary>
        protected Files()
        {
        }

        #endregion

        #region Propriétés

        /// <summary>
        ///     Nom du fichier
        /// </summary>
        protected string Name { get; set; }

        /// <summary>
        ///     Chemin local du fichier
        /// </summary>
        protected string LocalPath { get; set; }

        #endregion
    }
}