using Mango;

namespace PostilApp.Models
{
    public class PictureModel
    {
        public Id PostilId { get; set; }

        public string ImageUrl { get; set; }

        public int SelectedPicIndex { get; set; }
    }
}