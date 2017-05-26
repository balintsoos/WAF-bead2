using System;
using System.Data.Entity;
using Persistence;

namespace Service.Models
{
    public interface INewsEntities : IDisposable
    {
        DbSet<Article> Articles { get; set; }

        DbSet<Image> Images { get; set; }

        Int32 SaveChanges();
    }
}