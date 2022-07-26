using AppleTunes.Models;
using AppleTunes.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleTunes.Controllers
{
    public class UserController : Controller
    {
        private AppleTunesContext _db { get; set; }
        public UserController(AppleTunesContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            UserSelectViewModel vm = new UserSelectViewModel(_db.Users.ToList());
            return View(vm);
        }

        public IActionResult UserMusicLibrary(int? userId)
        {
            SongSelectViewModel sm = new SongSelectViewModel(_db.Songs.ToList());

            ViewBag.Message = sm.SongSelect;

            User user = _db.Users.Include(r => r.Collections.OrderBy(c => c.Song.Title)).ThenInclude(s => s.Song).ThenInclude(s => s.Artist).First(r => r.Id == userId);
            return View(user);

        }

        [HttpPost]
        public IActionResult DelistSong(int userId, int songId)
        {
            User user = _db.Users.First(r => r.Id == userId);
            Song song = _db.Songs.First(z => z.Id == songId);

            Collection collection = _db.Collections.First(e => e.UserId == userId && e.SongId == songId);

            _db.Collections.Remove(collection);
            _db.SaveChanges();
            
            user = _db.Users.Include(r => r.Collections.OrderBy(c => c.Song.Title)).ThenInclude(s => s.Song).ThenInclude(s => s.Artist).First(r => r.Id == userId);
            
            return View("UserMusicLibrary", user);
            
        }

        [HttpPost]
        public IActionResult EnlistSong (int userId, int songId)
        {
            User user = _db.Users.First(r => r.Id == userId);
            Song song = _db.Songs.First(z => z.Id == songId);

            SongSelectViewModel sm = new SongSelectViewModel(_db.Songs.ToList());

            ViewBag.Message = sm.SongSelect;

            Collection collection = new Collection();

            if (!_db.Collections.Any(s => s.UserId == userId && s.SongId == songId))
            {
                collection.Song = song;
                collection.User = user;
                collection.UserId = userId;
                collection.SongId = songId;

                _db.Collections.Add(collection);
                _db.SaveChanges();

                user = _db.Users.Include(r => r.Collections.OrderBy(c => c.Song.Title)).ThenInclude(s => s.Song).ThenInclude(s => s.Artist).First(r => r.Id == userId);

                return View("UserMusicLibrary", user);

            }else
            {
                user = _db.Users.Include(r => r.Collections.OrderBy(c => c.Song.Title)).ThenInclude(s => s.Song).ThenInclude(s => s.Artist).First(r => r.Id == userId);

                return View("UserMusicLibrary", user);

            }


        }



    }
}
