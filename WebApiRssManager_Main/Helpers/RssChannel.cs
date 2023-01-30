using Models;
using System.Xml.Linq;

namespace WebApiRssManager_Main.Helpers
{
    public class RssChannel
    {
        private readonly ApplicationDbContext _db;
        private XDocument doc;
        public RssChannel(ApplicationDbContext db)
        {
            _db = db;
        }
        public Channel Channel { get; set; }    
        public List<Item> Items { get; set; }

        public Channel CreateChannel(string url, string userId)
        {
            doc = XDocument.Load(url);
            string? title = (from c in doc.Root.Descendants("channel")
                            select c.Element("title").Value).FirstOrDefault();
            string? link = (from c in doc.Root.Descendants("channel")
                             select c.Element("link").Value).FirstOrDefault();
            string? description = (from c in doc.Root.Descendants("channel")
                            select c.Element("description").Value).FirstOrDefault();

            string? lastBuildDate;
            try
            {
                lastBuildDate = (from c in doc.Root.Descendants("channel")
                                         select c.Element("lastBuildDate").Value).FirstOrDefault();
            }
            catch (Exception)
            {

                lastBuildDate = "Not Available";
            }

            var newChannel = new Channel { Title = title, Link = link, Description = description, LastBuildDate = lastBuildDate, UserId = userId };

            Channel = newChannel;

            _db.Channels.Add(newChannel);
            _db.SaveChanges();

            return newChannel;
        }
        public List<Item> CreateItems(string url, int channelId )
        {
            doc = XDocument.Load(url);
            List<XElement> itemsXML = doc.Root.Element("channel").Elements("item").ToList();
            List<Item> items = new List<Item>();

            if (itemsXML.Count > 0)
            {
                foreach (var item in itemsXML)
                {
                    string? title = item.Element("title").Value;             
                    string? link = item.Element("link").Value;
                    string? description = item.Element("description").Value.Replace("src=\"//", "src=\"http://");
                    string? pubDate = item.Element("pubDate").Value;

                    items.Add(new Item { Title = title, Link = link, Description = description, PubDate = pubDate, ChannelId = channelId });
                }
            }
            Items = items;
            _db.Items.AddRange(items);
            _db.SaveChanges();

            return items;
        }
    }
}
