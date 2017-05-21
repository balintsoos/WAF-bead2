using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence;

namespace Desktop.Model
{
    public interface INewsService
    {
        Task<IEnumerable<Article>> LoadArticlesAsync();

        Task<Boolean> CreateArticleAsync(Article article);

        Task<Boolean> UpdateArticleAsync(Article article);

        Task<Boolean> DeleteArticleAsync(Article article);

        Task<Boolean> CreateImageAsync(Image image);

        Task<Boolean> LoginAsync(String userName, String userPassword);

        Task<Boolean> LogoutAsync();
    }
}
