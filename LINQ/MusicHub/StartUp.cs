using System.Text;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace MusicHub
{
    using System;

    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
       // Console.WriteLine(ExportAlbumsInfo(context,9));
       Console.WriteLine(ExportSongsAboveDuration(context,4));
        }
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumsInfo = context.Producers.First(p => p.Id == producerId)
                .Albums.Select(a=>new
                    {
                        AlbumName = a.Name,
                        ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                        ProducerName = a.Producer.Name,
                        Songs = a.Songs.Select(s=>new
                        {
                            SongName = s.Name,
                            SongPrice = s.Price,
                            SongWriter = s.Writer.Name
                        })
                        .OrderByDescending(s=>s.SongName)
                        .ThenBy(w=>w.SongWriter),
                        AlbumPrice = a.Price
                    }
                   )
                .OrderByDescending(a=>a.AlbumPrice).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var a in albumsInfo)
            {
                sb.AppendLine($"-AlbumName: {a.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {a.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {a.ProducerName}");
                sb.AppendLine($"-Songs:");

                int counter = 1;
                if (a.Songs.Any())
                {
                    foreach (var s in a.Songs)
                    {
                        sb.AppendLine($"---#{counter}");
                        sb.AppendLine($"---SongName: {s.SongName}");
                        sb.AppendLine($"---Price: {s.SongPrice:f2}");
                        sb.AppendLine($"---Writer: {s.SongWriter}");
                        counter++;
                    }   
                }

                sb.AppendLine($"-AlbumPrice: {a.AlbumPrice:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songsInfo = context.Songs.AsEnumerable()
                .Where(s=>s.Duration.TotalSeconds>duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    PerformersNames = s.SongPerformers.
                        Select(p=>$"{p.Performer.FirstName} {p.Performer.LastName}")
                        .OrderBy(n=>n).ToList(),
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c"),
                })
                .OrderBy(s=>s.SongName)
                .ThenBy(w=>w.WriterName)
                .ToList();

                int counter = 1;
            StringBuilder sb = new StringBuilder();

            foreach (var s in songsInfo)
            {
                sb.AppendLine($"-Song #{counter++}");
                sb.AppendLine($"---SongName: {s.SongName}");
                sb.AppendLine($"---Writer: {s.WriterName}");
                if (s.PerformersNames.Any())
                {
                    foreach (var p in s.PerformersNames)
                    {
                    sb.AppendLine($"---Performer: {p}");
                    }

                }
                sb.AppendLine($"---AlbumProducer: {s.AlbumProducer}");
                sb.AppendLine($"---Duration: {s.Duration}");
            }

            return sb.ToString().TrimEnd();
        }

      
    }
}
