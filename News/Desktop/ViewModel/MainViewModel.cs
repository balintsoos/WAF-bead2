using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Persistence;
using Desktop.Model;

namespace Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IDesktopModel _model;
        private ObservableCollection<Article> _articles;
        private Article _selectedArticle;
        private Boolean _isLoaded;

        public ObservableCollection<Article> Articles
        {
            get { return _articles; }
            private set
            {
                if (_articles != value)
                {
                    _articles = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean IsLoaded
        {
            get { return _isLoaded; }
            private set
            {
                if (_isLoaded != value)
                {
                    _isLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public Article SelectedArticle
        {
            get { return _selectedArticle; }
            set
            {
                if (_selectedArticle != value)
                {
                    _selectedArticle = value;
                    OnPropertyChanged();
                }
            }
        }

        public Article EditedArticle { get; private set; }
        public DelegateCommand CreateArticleCommand { get; private set; }
        public DelegateCommand CreateImageCommand { get; private set; }
        public DelegateCommand UpdateArticleCommand { get; private set; }
        public DelegateCommand DeleteArticleCommand { get; private set; }
        public DelegateCommand SaveChangesCommand { get; private set; }
        public DelegateCommand CancelChangesCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        public event EventHandler ExitApplication;
        public event EventHandler ArticleEditingStarted;
        public event EventHandler ArticleEditingFinished;
        public event EventHandler<ArticleEventArgs> ImageEditingStarted;

        public MainViewModel(IDesktopModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            _model = model;
            _model.ArticleChanged += Model_ArticleChanged;
            _isLoaded = false;

            CreateArticleCommand = new DelegateCommand(param =>
            {
                EditedArticle = new Article
                {
                    PublishedAt = DateTime.Now,
                    Author = "admin",
                };

                OnArticleEditingStarted();
            });
            CreateImageCommand = new DelegateCommand(param => OnImageEditingStarted((param as Article).Id));
            UpdateArticleCommand = new DelegateCommand(param => UpdateArticle(param as Article));
            DeleteArticleCommand = new DelegateCommand(param => DeleteArticle(param as Article));
            SaveChangesCommand = new DelegateCommand(param => SaveChanges());
            CancelChangesCommand = new DelegateCommand(param => CancelChanges());
            LoadCommand = new DelegateCommand(param => LoadAsync());
            SaveCommand = new DelegateCommand(param => SaveAsync());
            ExitCommand = new DelegateCommand(param => OnExitApplication());
        }

        private void UpdateArticle(Article article)
        {
            if (article == null)
                return;

            EditedArticle = new Article
            {
                Id = article.Id,
                Title = article.Title,
                Summary = article.Summary,
                Content = article.Content,
                PublishedAt = article.PublishedAt,
                Author = article.Author,
                isLead = article.isLead,
            }; // a szerkesztett épület adatait áttöltjük a kijelöltből

            OnArticleEditingStarted();
        }

        private void DeleteArticle(Article article)
        {
            if (article == null || !Articles.Contains(article))
                return;

            Articles.Remove(article);

            _model.DeleteArticle(article);
        }

        private void SaveChanges()
        {
            // ellenőrzések
            if (String.IsNullOrEmpty(EditedArticle.Title))
            {
                OnMessageApplication("Az cím nincs megadva!");
                return;
            }
            if (EditedArticle.Summary == null)
            {
                OnMessageApplication("Az összefoglaló nincs megadva!");
                return;
            }
            if (EditedArticle.Content == null)
            {
                OnMessageApplication("A tartalom nincs megadva!");
                return;
            }

            // mentés
            if (EditedArticle.Id == 0) // ha új az épület
            {
                _model.CreateArticle(EditedArticle);
                Articles.Add(EditedArticle);
                SelectedArticle = EditedArticle;
            }
            else // ha már létezik az épület
            {
                _model.UpdateArticle(EditedArticle);
            }

            EditedArticle = null;

            OnArticleEditingFinished();
        }

        private void CancelChanges()
        {
            EditedArticle = null;
            OnArticleEditingFinished();
        }

        private async void LoadAsync()
        {
            try
            {
                await _model.LoadAsync();
                Articles = new ObservableCollection<Article>(_model.Articles); // az adatokat egy követett gyűjteménybe helyezzük
                IsLoaded = true;
            }
            catch (ServiceUnavailableException ex)
            {
                OnMessageApplication("A betöltés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }

        private async void SaveAsync()
        {
            try
            {
                await _model.SaveAsync();
                OnMessageApplication("A mentés sikeres!");
            }
            catch (ServiceUnavailableException)
            {
                OnMessageApplication("A mentés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }

        private void Model_ArticleChanged(object sender, ArticleEventArgs e)
        {
            Int32 index = Articles.IndexOf(Articles.FirstOrDefault(article => article.Id == e.ArticleId));
            Articles.RemoveAt(index); // módosítjuk a kollekciót
            Articles.Insert(index, _model.Articles[index]);

            SelectedArticle = Articles[index]; // és az aktuális épületet
        }

        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }

        private void OnArticleEditingStarted()
        {
            if (ArticleEditingStarted != null)
                ArticleEditingStarted(this, EventArgs.Empty);
        }

        private void OnArticleEditingFinished()
        {
            if (ArticleEditingFinished != null)
                ArticleEditingFinished(this, EventArgs.Empty);
        }

        private void OnImageEditingStarted(Int32 articleId)
        {
            if (ImageEditingStarted != null)
                ImageEditingStarted(this, new ArticleEventArgs { ArticleId = articleId });
        }
    }
}
