using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence;

namespace Desktop.Model
{
    public class DesktopModel : IDesktopModel
    {
        private enum DataFlag
        {
            Create,
            Update,
            Delete
        }

        private INewsService _service;
        private List<Article> _articles;
        private Dictionary<Article, DataFlag> _articleFlags;
        private Dictionary<Image, DataFlag> _imageFlags;

        public DesktopModel(INewsService service)
        {
            if (service == null)
                throw new ArgumentNullException("persistence");

            IsUserLoggedIn = false;
            _service = service;
        }

        public Boolean IsUserLoggedIn { get; private set; }

        public IReadOnlyList<Article> Articles
        {
            get { return _articles; }
        }

        public event EventHandler<ArticleEventArgs> ArticleChanged;

        public void CreateArticle(Article article)
        {
            if (article == null)
                throw new ArgumentNullException("article");
            if (_articles.Contains(article))
                throw new ArgumentException("The article is already in the collection.", "article");

            article.Id = (_articles.Count > 0 ? _articles.Max(b => b.Id) : 0) + 1; // generálunk egy új, ideiglenes azonosítót (nem fog átkerülni a szerverre)
            _articleFlags.Add(article, DataFlag.Create);
            _articles.Add(article);
        }

        public void CreateImage(Int32 articleId, Byte[] imageSmall, Byte[] imageLarge)
        {
            Article article = _articles.FirstOrDefault(b => b.Id == articleId);
            if (article == null)
                throw new ArgumentException("The article does not exist.", "articleId");

            // létrehozzuk a képet
            Image image = new Image
            {
                Id = _articles.Max(b => b.Images.Any() ? b.Images.Max(im => im.Id) : 0) + 1,
                ArticleId = articleId,
                ImageSmall = imageSmall,
                ImageLarge = imageLarge
            };

            // hozzáadjuk
            article.Images.Add(image);
            _imageFlags.Add(image, DataFlag.Create);

            // jellezzük a változást
            OnArticleChanged(article.Id);
        }

        public void UpdateArticle(Article article)
        {
            if (article == null)
                throw new ArgumentNullException("article");

            // keresés azonosító alapján
            Article articleToModify = _articles.FirstOrDefault(a => a.Id == article.Id);

            if (articleToModify == null)
                throw new ArgumentException("The article does not exist.", "article");

            // módosítások végrehajtása
            articleToModify.Title = article.Title;
            articleToModify.Summary = article.Summary;
            articleToModify.Content = article.Content;
            articleToModify.PublishedAt = article.PublishedAt;
            articleToModify.Author = article.Author;
            articleToModify.isLead = article.isLead;

            // külön állapottal jelezzük, ha egy adat újonnan hozzávett
            if (_articleFlags.ContainsKey(articleToModify) && _articleFlags[articleToModify] == DataFlag.Create)
            {
                _articleFlags[articleToModify] = DataFlag.Create;
            }
            else
            {
                _articleFlags[articleToModify] = DataFlag.Update;
            }

            // jelezzük a változást
            OnArticleChanged(article.Id);
        }

        public void DeleteArticle(Article article)
        {
            if (article == null)
                throw new ArgumentNullException("article");

            // keresés azonosító alapján
            Article articleToDelete = _articles.FirstOrDefault(b => b.Id == article.Id);

            if (articleToDelete == null)
                throw new ArgumentException("The article does not exist.", "article");

            // külön kezeljük, ha egy adat újonnan hozzávett (ekkor nem kell törölni a szerverről)
            if (_articleFlags.ContainsKey(articleToDelete) && _articleFlags[articleToDelete] == DataFlag.Create)
                _articleFlags.Remove(articleToDelete);
            else
                _articleFlags[articleToDelete] = DataFlag.Delete;

            _articles.Remove(articleToDelete);
        }

        public async Task LoadAsync()
        {
            // adatok
            _articles = (await _service.LoadArticlesAsync()).ToList();

            // állapotjelzések
            _articleFlags = new Dictionary<Article, DataFlag>();
            _imageFlags = new Dictionary<Image, DataFlag>();
        }

        public async Task SaveAsync()
        {
            // épületek
            List<Article> articlesToSave = _articleFlags.Keys.ToList();

            foreach (Article article in articlesToSave)
            {
                Boolean result = true;

                if (article.isLead == true && article.Images.Count == 0)
                {
                    throw new InvalidOperationException("Save failed on lead article '" + article.Title + "': At least 1 image is required!");
                }

                // az állapotjelzőnek megfelelő műveletet végezzük el
                switch (_articleFlags[article])
                {
                    case DataFlag.Create:
                        result = await _service.CreateArticleAsync(article);
                        break;
                    case DataFlag.Delete:
                        result = await _service.DeleteArticleAsync(article);
                        break;
                    case DataFlag.Update:
                        result = await _service.UpdateArticleAsync(article);
                        break;
                }

                if (!result)
                    throw new InvalidOperationException("Operation " + _articleFlags[article] + " failed on article " + article.Id);

                // ha sikeres volt a mentés, akkor törölhetjük az állapotjelzőt
                _articleFlags.Remove(article);
            }

            // képek
            List<Image> imagesToSave = _imageFlags.Keys.ToList();

            foreach (Image image in imagesToSave)
            {
                Boolean result = true;

                switch (_imageFlags[image])
                {
                    case DataFlag.Create:
                        result = await _service.CreateImageAsync(image);
                        break;
                }

                if (!result)
                    throw new InvalidOperationException("Operation " + _imageFlags[image] + " failed on image " + image.Id);

                // ha sikeres volt a mentés, akkor törölhetjük az állapotjelzőt
                _imageFlags.Remove(image);
            }
        }

        public async Task<Boolean> LoginAsync(String userName, String userPassword)
        {
            IsUserLoggedIn = await _service.LoginAsync(userName, userPassword);
            return IsUserLoggedIn;
        }

        public async Task<Boolean> LogoutAsync()
        {
            if (!IsUserLoggedIn)
                return true;

            IsUserLoggedIn = !(await _service.LogoutAsync());

            return IsUserLoggedIn;
        }

        private void OnArticleChanged(Int32 articleId)
        {
            if (ArticleChanged != null)
                ArticleChanged(this, new ArticleEventArgs { ArticleId = articleId });
        }
    }
}
