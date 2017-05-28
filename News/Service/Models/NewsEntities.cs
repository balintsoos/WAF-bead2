using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Persistence;

namespace Service.Models
{
    public class NewsEntities : DbContext, INewsEntities
    {
        public NewsEntities()
            : base("name=NewsModel")
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Image> Images { get; set; }
    }
}