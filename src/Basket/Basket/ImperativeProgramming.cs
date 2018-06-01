using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Basket;
using Newtonsoft.Json;

public static class ImperativeProgramming
{
    public static int CalculateBasketAmount(List<BasketLineArticle> basketTestBasketLineArticles)
    {
        var amountTotal = 0;
        foreach (var basketLineArticle in basketTestBasketLineArticles)
        {
            var id = basketLineArticle.Id;
#if DEBUG
            var article = GetArticleDatabaseMock(basketLineArticle.Id);
#else
            var article = GetArticleDatabase(basketLineArticle.Id);
#endif

            // Calculate amount
            var amount = 0;
            switch (article.Category)
            {
                case "food":
                    amount += article.Price * 100 + article.Price * 12;
                    break;
                case "electronic":
                    amount += article.Price * 100 + article.Price * 20 + 4;
                    break;
                case "desktop":
                    amount += article.Price * 100 + article.Price * 20;
                    break;
            }

            amountTotal += amount * basketLineArticle.Number;
        }

        return amountTotal;
    }

    private static ArticleDatabase ArticleDatabase(string id)
    {
// Retrive article from database
        var codeBase = Assembly.GetExecutingAssembly().CodeBase;
        var uri = new UriBuilder(codeBase);
        var path = Uri.UnescapeDataString(uri.Path);
        var assemblyDirectory = Path.GetDirectoryName(path);
        var jsonPath = Path.Combine(assemblyDirectory, "article-database.json");
        IList<ArticleDatabase> articleDatabases =
            JsonConvert.DeserializeObject<List<ArticleDatabase>>(File.ReadAllText(jsonPath));
        var article = articleDatabases.First(articleDatabase => { return articleDatabase.Id == id; });
        return article;
    }

    public static ArticleDatabase GetArticleDatabaseMock(string id)
    {
        switch (id)
        {
            case "1":
                return new ArticleDatabase
                {
                    Id = "1",
                    Price = 1,
                    Stock =
                        35,
                    Label = "Banana",
                    Category = "food"
                };
            case "2":
                return new ArticleDatabase
                {
                    Id = "2",
                    Price = 500,
                    Stock = 20,
                    Label = "Fridge electrolux",
                    Category = "electronic"
                };
            case "3":
                return new ArticleDatabase
                {
                    Id = "3",
                    Price = 49,
                    Stock =
                        68,
                    Label = "Chair",
                    Category = "desktop"
                };
            default:
                throw new NotImplementedException();
        }
    }
}