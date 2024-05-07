using API_EF.Data;
using API_EF.Entities;
using API_EF.Helper;
using API_EF.Service;
using API_EF.Testing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace API_EF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {

        private readonly DataContext _context;
        private readonly ICaches _caches;
       
        //private readonly IMemoryCache _memoryCache;
        List<GameEnties> gamecache;

        public GamesController(DataContext context , ICaches caches)
        {
            _context = context;
            //_memoryCache = memoryCache;
            _caches = caches;
        }

        [HttpGet]
        [Authorize(Policy =TypeSafe.Policy.FullPolicy)]
        public async Task<ActionResult> GetAllGame()
        {
            gamecache =  _caches.GetData<List<GameEnties>>("GameCaches");
            if(gamecache is null)
            {
                gamecache = await _context.GameStoreEntites.ToListAsync();

                _caches.SetData("GameCaches", gamecache, DateTimeOffset.Now.AddSeconds(30));
            }

            return Ok(gamecache);
        }
        [HttpGet("{id}")]
        [Authorize(Roles =TypeSafe.Roles.Admin)]
        public async Task<ActionResult> GetItembyID(int? id)
        {
            var Game = await _context.GameStoreEntites.FindAsync(id);
            if (Game == null) { return NotFound(); }
            return Ok(Game);
        }

      
        [HttpPost]
        public async Task<ActionResult> AddNewGame(GameEnties newGameEntity)
        {
            if (newGameEntity == null) { return BadRequest(); }
            _context.GameStoreEntites.Add(newGameEntity);
            await _context.SaveChangesAsync();
            return Ok(await _context.GameStoreEntites.ToListAsync());
        }
        [HttpPut]
        public async Task<ActionResult> UpDateGameEntities(GameEnties updateGameEntity)
        {
            var game = _context.GameStoreEntites.Find(updateGameEntity.GameId);
            if (game == null) { return NotFound(); }
            game.Name = updateGameEntity.Name;
            game.Genres = updateGameEntity.Genres;
            game.Title = updateGameEntity.Title;

            await _context.SaveChangesAsync();
            return Ok(await _context.GameStoreEntites.ToListAsync());
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteGame(int? id)
        {
            var game = await _context.GameStoreEntites.FindAsync(id);
            if (game == null) { return NotFound(); };
            _context.GameStoreEntites.Remove(game);
            await _context.SaveChangesAsync();
            return Ok(await _context.GameStoreEntites.ToListAsync());
        }

        private async Task<List<GameEnties>> GetGameEntityCaches(List<GameEnties> games)
        {
            List<GameEnties> gameCaches = new List<GameEnties>();

            foreach(var game in games)
            {
                gameCaches.Add(game);
            }
             return  gameCaches;
        }
    }
}
