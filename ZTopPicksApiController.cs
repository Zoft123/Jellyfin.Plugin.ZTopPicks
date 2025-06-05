using System;
using System.Collections.ObjectModel;
using System.Linq;
using Jellyfin.Data.Enums;
using Jellyfin.Database.Implementations.Enums;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Querying;
using Microsoft.AspNetCore.Mvc;


namespace ZTopPicks
{
    [ApiController]
    [Route("ZTopPicks")]
    public class ZTopPicksApiController : ControllerBase
    {
        private readonly ILibraryManager _libraryManager;

        public ZTopPicksApiController(ILibraryManager libraryManager)
        {
            _libraryManager = libraryManager;
        }

        [HttpGet("Movies")]
        public IActionResult GetMovies()
        {
            var movies = _libraryManager.GetItemList(new InternalItemsQuery
            {
                IncludeItemTypes = new[] { BaseItemKind.Movie },
                OrderBy = new[] { (ItemSortBy.SortName, SortOrder.Ascending) }.ToList()
            });

            var result = movies.Select(i => new { i.Id, i.Name }).ToList();
            return Ok(result);
        }

        [HttpGet("Shows")]
        public IActionResult GetShows()
        {
            var shows = _libraryManager.GetItemList(new InternalItemsQuery
            {
                IncludeItemTypes = new[] { BaseItemKind.Series },
                OrderBy = new[] { (ItemSortBy.SortName, SortOrder.Ascending) }.ToList()
            });

            var result = shows.Select(i => new { i.Id, i.Name }).ToList();
            return Ok(result);
        }

        [HttpGet("Settings")]
        public IActionResult GetSettings()
        {
            var config = ZTopPicksPlugin.Instance.Configuration;
            return Ok(new SettingsDto
            {
                TopMovieIds = new ReadOnlyCollection<Guid>(config.TopMovieIds),
                TopShowIds = new ReadOnlyCollection<Guid>(config.TopShowIds)
            });
        }

        [HttpPost("Settings")]
        public IActionResult SaveSettings([FromBody] SettingsDto dto)
        {
            var config = ZTopPicksPlugin.Instance.Configuration;

            config.TopMovieIds.Clear();
            config.TopShowIds.Clear();

            foreach (var id in dto.TopMovieIds)
            {
                config.TopMovieIds.Add(id);
            }
            foreach (var id in dto.TopShowIds)
            {
                config.TopShowIds.Add(id);
            }

            ZTopPicksPlugin.Instance.SaveConfiguration();
            return Ok();
        }
    }

    public class SettingsDto
    {
        public ReadOnlyCollection<Guid> TopMovieIds { get; init; } = new ReadOnlyCollection<Guid>(Array.Empty<Guid>());
        public ReadOnlyCollection<Guid> TopShowIds { get; init; } = new ReadOnlyCollection<Guid>(Array.Empty<Guid>());
    }
}
