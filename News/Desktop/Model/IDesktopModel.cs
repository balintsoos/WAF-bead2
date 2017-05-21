using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence;

namespace Desktop.Model
{
    public interface IDesktopModel
    {
        IReadOnlyList<Article> Articles { get; }

        Boolean IsUserLoggedIn { get; }

        event EventHandler<ArticleEventArgs> ArticleChanged;

        void CreateArticle(Article article);

        void UpdateArticle(Article article);

        void DeleteArticle(Article article);

        void CreateImage(Int32 articleId, Byte[] imageSmall, Byte[] imageLarge);

        Task LoadAsync();

        Task SaveAsync();

        Task<Boolean> LoginAsync(String userName, String userPassword);

        Task<Boolean> LogoutAsync();
    }
}
