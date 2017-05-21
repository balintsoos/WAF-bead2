using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Persistence;

namespace Desktop.Model
{
    public class NewsService : INewsService
    {
        private HttpClient _client;

        public NewsService(String baseAddress)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseAddress);
        }

        public async Task<IEnumerable<Article>> LoadArticlesAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/Articles/");

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<Article> articles = await response.Content.ReadAsAsync<IEnumerable<Article>>();

                    return articles;
                }
                else
                {
                    throw new ServiceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new ServiceUnavailableException(ex);
            }
        }

        public async Task<Boolean> CreateArticleAsync(Article article)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/Articles/", article);
                article.Id = (await response.Content.ReadAsAsync<Article>()).Id;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new ServiceUnavailableException(ex);
            }
        }

        public async Task<Boolean> UpdateArticleAsync(Article article)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/Articles/", article);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new ServiceUnavailableException(ex);
            }
        }

        public async Task<Boolean> DeleteArticleAsync(Article article)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("api/Articles/" + article.Id);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new ServiceUnavailableException(ex);
            }
        }

        public async Task<Boolean> CreateImageAsync(Image image)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/Images/", image); // elküldjük a képet
                if (response.IsSuccessStatusCode)
                {
                    image.Id = await response.Content.ReadAsAsync<Int32>(); // a válaszüzenetben megkapjuk az azonosítót
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new ServiceUnavailableException(ex);
            }
        }

        public async Task<Boolean> LoginAsync(String userName, String userPassword)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/account/login/" + userName + "/" + userPassword);
                return response.IsSuccessStatusCode; // a művelet eredménye megadja a bejelentkezés sikeressségét
            }
            catch (Exception ex)
            {
                throw new ServiceUnavailableException(ex);
            }
        }

        public async Task<Boolean> LogoutAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/account/logout");
                return !response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new ServiceUnavailableException(ex);
            }
        }
    }
}
