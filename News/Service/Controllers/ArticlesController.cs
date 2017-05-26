using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Persistence;

namespace Service.Controllers
{
    public class ArticlesController : ApiController
    {
        private NewsModel db = new NewsModel();

        // GET: api/Articles
        public IHttpActionResult GetArticles()
        {
            try
            {
                return Ok(db.Articles.ToList().Select(article => new Article
                {
                    Id = article.Id,
                    Title = article.Title,
                    Summary = article.Summary,
                    Content = article.Content,
                    PublishedAt = article.PublishedAt,
                    Author = article.Author,
                    isLead = article.isLead,
                }));
            }
            catch
            {
                return InternalServerError();
            }
        }

        // GET: api/Articles/5
        [ResponseType(typeof(Article))]
        public IHttpActionResult GetArticle(int id)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // PUT: api/Articles/5
        [ResponseType(typeof(void))]
        [Authorize]
        public IHttpActionResult PutArticle(int id, Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != article.Id)
            {
                return BadRequest();
            }

            db.Entry(article).State = EntityState.Modified;

            if (article.isLead)
            {
                ChangeLeadArticle(article.Id);
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Articles
        [ResponseType(typeof(Article))]
        [Authorize]
        public IHttpActionResult PostArticle(Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Articles.Add(new Article
            {
                Id = article.Id,
                Title = article.Title,
                Summary = article.Summary,
                Content = article.Content,
                Author = article.Author,
                PublishedAt = article.PublishedAt,
                isLead = article.isLead
            });

            if (article.isLead)
            {
                ChangeLeadArticle(article.Id);
            }

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = article.Id }, article);
        }

        // DELETE: api/Articles/5
        [ResponseType(typeof(Article))]
        [Authorize]
        public IHttpActionResult DeleteArticle(int id)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return NotFound();
            }

            db.Articles.Remove(article);
            db.SaveChanges();

            return Ok(article);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArticleExists(int id)
        {
            return db.Articles.Count(e => e.Id == id) > 0;
        }

        private void ChangeLeadArticle(Int32 ArticleId)
        {
            var articles = db.Articles.Where(article => article.isLead && article.Id != ArticleId);

            foreach (var article in articles)
            {
                db.Entry(article).State = EntityState.Modified;

                article.isLead = false;
            }

            db.SaveChanges();
        }
    }
}