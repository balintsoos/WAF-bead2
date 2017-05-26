using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Persistence;

namespace Service.Models
{
    public class NewsService : INewsService
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

        public IEnumerable<Image> Images
        {
            get
            {
                return _entities.Images;
            }
        }

        public Image GetImage(Int32? imageId)
        {
            if (imageId == null)
                return null;

            return _entities.Images.FirstOrDefault(image => image.Id == imageId);
        }

        public Boolean AddArticle(Article article)
        {
            _entities.Articles.Add(new Article
            {
                Id = article.Id,
                Title = article.Title,
                Summary = article.Summary,
                Content = article.Content,
                Author = article.Author,
                PublishedAt = article.PublishedAt,
                isLead = article.isLead
            });

            try
            {
                _entities.SaveChanges();
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }

        public Boolean AddImage(Image image)
        {
            _entities.Images.Add(new Image
            {
                Id = image.Id,
                ImageLarge = image.ImageLarge,
                ImageSmall = image.ImageSmall,
                ArticleId = image.ArticleId
            });

            try
            {
                _entities.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public Boolean DeleteArticle(Article article)
        {
            _entities.Articles.Remove(article);

            try
            {
                _entities.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public Boolean DeleteImage(Image image)
        {
            return true;
        }

        public Boolean EditArticle(Article article)
        {
            return true;
        }
    }
}