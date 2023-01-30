using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Newtonsoft.Json;
using WebApiRssManager_Main.Helpers;

namespace WebApiRssManager_Main.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowsAll")]
    public class RssController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public RssController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpPost]
        public async Task<IActionResult> AddRssFeed(string feedUrl)
        {
            if (string.IsNullOrWhiteSpace(feedUrl))
                return BadRequest("Feed url cannot be null");

            var userId = User.GetId();

            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID not found");

            var channel = new RssChannel(_db);
            channel.CreateChannel(feedUrl, userId);

            if (channel.Channel is null)
                return BadRequest("Channel cannot be created");

            var dto = new ChannelDTO
            {
                Id = channel.Channel.Id,
                Link = channel.Channel.Link,
                Title = channel.Channel.Title,
                Description = channel.Channel.Description,
                LastBuildDate = channel.Channel.LastBuildDate,
                UserId = userId
            };
            var channelJson = JsonConvert.SerializeObject(dto);

            return Ok(channelJson);
        }
        [HttpGet]
        public async Task<IActionResult> GetRssFeeds()
        {            
            var userId = User.GetId();
            var channels = _db.Channels.Where(x => x.UserId == userId).ToList();
            var channelsDTO = new List<ChannelDTO>();

            foreach (var channel in channels)
            {
                var channelDTO = new ChannelDTO
                {
                    Id = channel.Id,
                    Link = channel.Link, 
                    Title = channel.Title,
                    Description = channel.Description,
                    LastBuildDate = channel.LastBuildDate,
                    UserId = channel.UserId
                };
                channelsDTO.Add(channelDTO);
            }
            if (channelsDTO.Count == 0)
                return NotFound("Channels not found");
            var channelJson= JsonConvert.SerializeObject(channelsDTO);

            return Ok(channelJson);
        }
        [HttpGet]
        public async Task<IActionResult> GetUnreadNews(DateTime date)
        {
            throw new NotImplementedException();
        }
        [HttpPut]
        public async Task<IActionResult> SetNewsRead()
        {
            throw new NotImplementedException();
        }
    }
}
