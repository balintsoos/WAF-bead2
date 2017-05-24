using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using Desktop.Model;
using Persistence;
using Service;
using Service.Models;

namespace Test
{
    [TestClass]
    public class UnitTest
    {
        private IQueryable<Article> _articleData;

        private Mock<DbSet<Article>> _articleMock;

        private Mock<INewsEntities> _entityMock;

        [TestInitialize]
        public void Initialize()
        {
            // adatok inicializációja
            _articleData = new List<Article>
            {
                new Article
                {
                    Id = 1,
                    Title = "TESTARTICLE",
                    Summary = "TEST",
                    Content = "TEST",
                    Author ="test",
                    isLead = false,
                    PublishedAt = new DateTime(2017, 06, 06)
                }
            }.AsQueryable();

            // entitásmodell inicializációja, amihez le kell generálnunk az adatokhoz tartozó gyűjteményeket (táblákat) is
            // ehhez néhány alap információt be kell állítanunk, lehetővé téve, hogy az adatok lekérdezését biztosítsa a 
            _articleMock = new Mock<DbSet<Article>>(MockBehavior.Strict);
            _articleMock.As<IQueryable<Article>>().Setup(mock => mock.ElementType).Returns(_articleData.ElementType);
            _articleMock.As<IQueryable<Article>>().Setup(mock => mock.Expression).Returns(_articleData.Expression);
            _articleMock.As<IQueryable<Article>>().Setup(mock => mock.Provider).Returns(_articleData.Provider);
            _articleMock.As<IQueryable<Article>>().Setup(mock => mock.GetEnumerator()).Returns(_articleData.GetEnumerator()); // a korábban megadott listát fogjuk 

            // a szimulált entitásmodell csak ezt a két táblát fogja tartalmazni
            _entityMock = new Mock<INewsEntities>(MockBehavior.Strict);
            _entityMock.Setup<DbSet<Article>>(entity => entity.Articles).Returns(_articleMock.Object);
        }

        [TestMethod]
        public void ArticleServiceGetArticles()
        {
            // vezérlő példányosítása
            Service.Models.NewsService service = new Service.Models.NewsService(_entityMock.Object);

            // ellenőrzés januárra
            IEnumerable<Article> articles = service.Articles;
            Assert.AreEqual(1, articles.ToArray().Length);
        }

        [TestMethod]
        public void ArticleServiceGetArticle()
        {
            // vezérlő példányosítása
            Service.Models.NewsService service = new Service.Models.NewsService(_entityMock.Object);

            // ellenőrzés januárra
            Article article = service.GetArticle(1);
            Assert.AreEqual(1, article.Id);
        }

        [TestMethod]
        public void ArticleServiceGetWrongArticle()
        {
            // vezérlő példányosítása
            Service.Models.NewsService service = new Service.Models.NewsService(_entityMock.Object);

            // ellenőrzés januárra
            Article article = service.GetArticle(2);
            Assert.AreEqual(null, article);
        }
    }
}
