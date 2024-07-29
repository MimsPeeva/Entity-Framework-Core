using Boardgames.Data.Models;
using Boardgames.Data.Models.Enums;
using Boardgames.DataProcessor.ImportDto;
using Boardgames.Utilities;
using Newtonsoft.Json;
using static Boardgames.Data.Models.Enums.CategoryType;
namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Boardgames.Data;
   
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            var creatorDtos = XmlHelper
                .Deserialize<ImportCreatorsDto[]>(xmlString, "Creators");

            StringBuilder sb = new StringBuilder();
            List<Creator> creators = new List<Creator>();

            foreach (var creatorDto in creatorDtos)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Creator creator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName
                };

                foreach (var gameDto in creatorDto.Boardgames)
                {
                    if (!IsValid(gameDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    creator.Boardgames.Add(new Boardgame()
                    {
                        Name = gameDto.Name,
                        Rating = gameDto.Rating,
                        YearPublished = gameDto.YearPublished,
                        CategoryType = (CategoryType)gameDto.CategoryType,
                        Mechanics = gameDto.Mechanics
                    });
                }
                creators.Add(creator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, creator.FirstName,
                    creator.LastName, creator.Boardgames.Count()));
            }

            context.Creators.AddRange(creators);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ICollection<Seller> sellersToImport = new List<Seller>();
            var boardgameIds = context.Boardgames
                .Select(x => x.Id)
                .ToArray();
            ImportSellersDto[] sellersDto = JsonConvert.DeserializeObject<ImportSellersDto[]>(jsonString);
            foreach (ImportSellersDto dto in sellersDto)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Seller seller = new Seller()
                {
                    Name = dto.Name,
                    Address = dto.Address,
                    Country = dto.Country,
                    Website = dto.Website
                };
                foreach (var id in dto.BoardgamesId.Distinct())
                {
                    if (!boardgameIds.Contains(id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    BoardgameSeller boardgameSeller = new BoardgameSeller()
                    {
                        Seller = seller,
                        BoardgameId = id,
                    };
                    seller.BoardgamesSellers.Add(boardgameSeller);
                }
                sellersToImport.Add(seller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count()));
            }
            context.Sellers.AddRange(sellersToImport);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
