using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; } 
        public string LastBuildDate { get; set; }
        public string UserId { get; set; }  
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }  
        public List<Item> Items { get; set; }
    }
}
