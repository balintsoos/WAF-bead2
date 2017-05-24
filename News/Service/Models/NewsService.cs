using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Persistence;

namespace Service.Models
{
    public class NewsService
    {
        private INewsEntities _entities;

        public NewsService(INewsEntities entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            _entities = entities;
        }

        public IEnumerable<Article> Articles
        {
            get
            {
                return _entities.Articles;
            }
        }

        public Article GetArticle(Int32? articleId)
        {
            if (articleId == null)
                return null;

            return _entities.Articles.FirstOrDefault(article => article.Id == articleId);
        }
    }
}