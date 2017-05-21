using System;

namespace Desktop.Model
{
    /// <summary>
    /// Épület eseményargumentum típusa.
    /// </summary>
    public class ArticleEventArgs : EventArgs
    {
        /// <summary>
        /// Épület azonosító lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 ArticleId { get; set; }
    }
}
