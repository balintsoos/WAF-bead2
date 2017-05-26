using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Persistence;

namespace Service.Models
{
    public interface INewsService
    {
        IEnumerable<Article> Articles { get; }
        IEnumerable<Image> Images { get; }

        Article GetArticle(Int32? articleId);
        Image GetImage(Int32? imageId);

        Boolean AddArticle(Article article);
        Boolean AddImage(Image image);

        Boolean DeleteArticle(Article article);
        Boolean DeleteImage(Image image);

        Boolean EditArticle(Article article);

    }
}