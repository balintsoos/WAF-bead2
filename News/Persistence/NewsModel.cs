using System.Data.Entity;

namespace Persistence
{
    public class NewsModel : DbContext
    {
        public NewsModel()
            : base("name=NewsModel")
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Image> Images { get; set; }
    }
}
