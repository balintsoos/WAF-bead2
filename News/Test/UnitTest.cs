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
        private List<Article> _articleData;
        private List<Image> _imageData;

        private Mock<DbSet<Article>> _articleMock;
        private Mock<DbSet<Image>> _imageMock;

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
            };

            _imageData = new List<Image>
            {
            };

            // entitásmodell inicializációja, amihez le kell generálnunk az adatokhoz tartozó gyűjteményeket (táblákat) is
            // ehhez néhány alap információt be kell állítanunk, lehetővé téve, hogy az adatok lekérdezését biztosítsa a 
            IQueryable<Article> queryableArticleData = _articleData.AsQueryable();
            _articleMock = new Mock<DbSet<Article>>();
            _articleMock.As<IQueryable<Article>>().Setup(mock => mock.ElementType).Returns(queryableArticleData.ElementType);
            _articleMock.As<IQueryable<Article>>().Setup(mock => mock.Expression).Returns(queryableArticleData.Expression);
            _articleMock.As<IQueryable<Article>>().Setup(mock => mock.Provider).Returns(queryableArticleData.Provider);
            _articleMock.As<IQueryable<Article>>().Setup(mock => mock.GetEnumerator()).Returns(_articleData.GetEnumerator()); // a korábban megadott listát fogjuk 

            _articleMock.Setup(mock => mock.Add(It.IsAny<Article>())).Callback<Article>(article => { _articleData.Add(article); }).Returns(_articleData.Last());

            Article deletedArticle = null;

            _articleMock.Setup(mock => mock.Remove(It.IsAny<Article>())).Callback<Article>(article =>
            {
                if (_articleData.Contains(deletedArticle))
                    deletedArticle = article;

                _articleData.Remove(article);
            }).Returns(deletedArticle); // beállítjuk, hogy mi történjen épület törlésekor

            _articleMock.Setup(mock => mock.Include(It.IsAny<String>())).Returns(_articleMock.Object); // azt is be kell állítanunk, hogy tábla csatolása esetén mi történjen

            IQueryable<Image> queryableBuildingImageData = _imageData.AsQueryable();
            _imageMock = new Mock<DbSet<Image>>();
            _imageMock.As<IQueryable<Image>>().Setup(mock => mock.ElementType).Returns(queryableBuildingImageData.ElementType);
            _imageMock.As<IQueryable<Image>>().Setup(mock => mock.Expression).Returns(queryableBuildingImageData.Expression);
            _imageMock.As<IQueryable<Image>>().Setup(mock => mock.Provider).Returns(queryableBuildingImageData.Provider);
            _imageMock.As<IQueryable<Image>>().Setup(mock => mock.GetEnumerator()).Returns(_imageData.GetEnumerator());

            // a szimulált entitásmodell csak ezt a két táblát fogja tartalmazni
            _entityMock = new Mock<INewsEntities>(MockBehavior.Strict);
            _entityMock.Setup<DbSet<Article>>(entity => entity.Articles).Returns(_articleMock.Object);
            _entityMock.Setup<DbSet<Image>>(entity => entity.Images).Returns(_imageMock.Object);

            _entityMock.Setup(x => x.SaveChanges()).Returns(0);
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
        public void ArticleServiceGetImages()
        {
            // vezérlő példányosítása
            Service.Models.NewsService service = new Service.Models.NewsService(_entityMock.Object);

            // ellenőrzés januárra
            IEnumerable<Image> images = service.Images;
            Assert.AreEqual(0, images.ToArray().Length);
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

        [TestMethod]
        public void ArticleServiceGetWrongImage()
        {
            // vezérlő példányosítása
            Service.Models.NewsService service = new Service.Models.NewsService(_entityMock.Object);

            // ellenőrzés januárra
            Image image = service.GetImage(2);
            Assert.AreEqual(null, image);
        }

        [TestMethod]
        public void ArticleServiceAddArticle()
        {
            Service.Models.NewsService service = new Service.Models.NewsService(_entityMock.Object);

            Article article = new Article
            {
                Id = 2,
                Title = "TEST",
                Summary = "TEST",
                Content = "TEST",
                Author = "testadmin",
                PublishedAt = new DateTime(2017, 08, 06),
                isLead = false
            };

            Boolean result = service.AddArticle(article);

            Assert.AreEqual(true, result);
            Assert.AreEqual(2, _articleData.ToArray().Length);
        }

        [TestMethod]
        public void ArticleServiceDeleteArticle()
        {
            Service.Models.NewsService service = new Service.Models.NewsService(_entityMock.Object);

            Article article = _articleData[0];

            Boolean result = service.DeleteArticle(article);

            Assert.AreEqual(true, result);
            Assert.AreEqual(0, _articleData.ToArray().Length);
        }
    }
}
