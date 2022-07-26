using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppleTunes.Models.ViewModels
{
    public class SongSelectViewModel
    {
        public List<SelectListItem> SongSelect { get; set; }

        public SongSelectViewModel(List<Song> songs)
        {
            SongSelect = new List<SelectListItem>();

            songs.ForEach(u =>
            {
                SongSelect.Add(new SelectListItem(u.Title, u.Id.ToString()));
            });
        }
    }
}
