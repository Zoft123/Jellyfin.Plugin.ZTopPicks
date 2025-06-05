using System;
using System.Collections.Generic;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using ZTopPicks.Configuration;

namespace ZTopPicks
{
    public class ZTopPicksPlugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        public static ZTopPicksPlugin Instance { get; private set; } = null!;

        public ZTopPicksPlugin(IApplicationPaths appPaths, IXmlSerializer xmlSerializer)
            : base(appPaths, xmlSerializer)
        {
            Instance = this;
        }

        public override string Name => "Zoft's Top Picks";

        public override string Description => "Highlight custom top movie/show picks for the home screen.";

        public override Guid Id => Guid.Parse("45de9c0b-6c86-4709-9fc5-b69bac67535b");

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = "ZTopPicks",
                    EmbeddedResourcePath = typeof(ZTopPicksPlugin).Namespace + ".Web.ZTopPicksPage.html",
                    EnableInMainMenu = false
                }
            };
        }

        // No need to redeclare Configuration property; it's inherited from BasePlugin<T>
    }
}
