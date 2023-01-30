using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string PubDate { get; set; }
        public bool IsReaded { get; set; }
        public int ChannelId { get; set; }
        [ForeignKey(nameof(ChannelId))]
        public Channel Channel { get; set; }    
    }
}
