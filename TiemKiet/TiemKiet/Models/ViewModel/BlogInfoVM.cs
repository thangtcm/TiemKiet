using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;
using TiemKiet.Data;

namespace TiemKiet.Models.ViewModel
{
    public class BlogInfoVM
    {
        public int BlogId { get; set; }
        public string Heading { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeatheredImageUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }
        public bool IsRemoved { get; set; }

        public BlogInfoVM() {  }
        public BlogInfoVM(BlogPost model)
        {
            this.BlogId = model.Id;
            this.Heading = model.Heading;
            this.Title = model.Title;
            this.ShortDescription = model.ShortDescription;
            this.FeatheredImageUrl = model.FeatheredImageUrl;
            this.PublishedDate = model.PublishedDate;
            this.Author = model.Author;
            this.Visible = model.Visible;
            this.IsRemoved = model.IsRemoved;
        }
    }
}
