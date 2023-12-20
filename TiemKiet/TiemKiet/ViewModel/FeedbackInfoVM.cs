using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using TiemKiet.Models;

namespace TiemKiet.ViewModel
{
    public class FeedbackInfoVM
    {
        public int FeedbackId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ICollection<IFormFile> formFiles { get; set; }
        public ICollection<string> ImageLst { get; set; }
        public FeedbackInfoVM() { }
        public FeedbackInfoVM(Feedback model)
        {
            this.FeedbackId = model.Id;
            this.Title = model.Title;
            this.Content = model.Content;
        }
    }
}
