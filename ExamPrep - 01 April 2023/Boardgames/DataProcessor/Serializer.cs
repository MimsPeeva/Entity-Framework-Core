using Boardgames.Data.Models.Enums;
using Boardgames.DataProcessor;
using Boardgames.DataProcessor.ExportDto;
using Boardgames.Utilities;

namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var creators = context.Creators
                .Where(c => c.Boardgames.Count > 0)
                .Select(c => new ExportCreatorsDto
                {
                    BoardgamesCount = c.Boardgames.Count,
                    CreatorName = c.FirstName + " " + c.LastName,
                    Boardgames = c.Boardgames
                        .Select(b => new ExportBoardgamesDto
                        {
                            BoardgameName = b.Name,
                            BoardgameYearPublished = b.YearPublished,
                        })
                        .OrderBy(b => b.BoardgameName)
                        .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.CreatorName)
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();

            return xmlHelper.Serialize<ExportCreatorsDto[]>(creators, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => s.BoardgamesSellers
                    .Any(bg => bg.Boardgame.YearPublished >= year &&
                               bg.Boardgame.Rating <= rating))
                .Select(s => new
                {
                    s.Name,
                    s.Website,
                    Boardgames = s.BoardgamesSellers
                        .Where(bgs => bgs.Boardgame.YearPublished >= year &&
                                      bgs.Boardgame.Rating <= rating)
                        .Select(bgs => new
                        {
                            Name = bgs.Boardgame.Name,
                            Rating = bgs.Boardgame.Rating,
                            Mechanics = bgs.Boardgame.Mechanics,
                            Category = bgs.Boardgame.CategoryType.ToString()
                        })
                        .OrderByDescending(bgs => bgs.Rating)
                        .ThenBy(bgs => bgs.Name)
                        .ToList()
                })
                .OrderByDescending(s => s.Boardgames.Count)
                .ThenBy(s => s.Name)
                .Take(5)
                .ToList();

            return JsonConvert.SerializeObject(sellers, Formatting.Indented);
        }
    }
}