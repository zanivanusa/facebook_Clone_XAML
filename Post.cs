using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace socialnoOmrezje
{
    public class Post
    {
        private string _content;
        private ImageSource _image;
        private List<string> _taggedFriends;

        public string Title { get; set; }

        public void UpdatePost(string title, string content, ImageSource image, List<string> taggedFriends)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Title = title;
            }

            if (!string.IsNullOrEmpty(content))
            {
                Content = content;
            }

            if (image != null)
            {
                Image = image;
            }

            if (taggedFriends != null)
            {
                TaggedFriends = taggedFriends;
            }
        }


        public string Content
        {
            get => _content;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Content cannot be empty");
                _content = value;
            }
        }
        public ImageSource Image
        {
            get => _image;
            set//pozneje naredi da ce ne bo slike pac ne bo slike
            {
                if (value == null) throw new ArgumentException("Image cannot be null");
                _image = value;
            }
        }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public List<string> TaggedFriends
        {
            get => _taggedFriends;
            set 
            {
                _taggedFriends = value;
            }
        }
    }

}
