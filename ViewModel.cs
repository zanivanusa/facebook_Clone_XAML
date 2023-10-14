using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace socialnoOmrezje
{
    public class ViewModel : INotifyPropertyChanged
    {

        private AddPostWindow _addPostWindow;
        private EditWindowView _editPostWindow;

        private bool isDefaultView = true;

        public bool IsDefaultView
        {
            get { return isDefaultView; }
            set
            {
                if (isDefaultView != value)
                {
                    isDefaultView = value;
                    OnPropertyChanged(nameof(IsDefaultView));
                }
            }
        }
        public ViewModel()
        {
            Posts = new ObservableCollection<Post>();

            StaticneObjave();//zgeneriramo objave

            Friends = new ObservableCollection<Friend>
        {
            new Friend { Name = "Ana", Image = new BitmapImage(new Uri("res/Ana.jpg", UriKind.Relative)) },
            new Friend { Name = "Teo", Image = new BitmapImage(new Uri("res/Teo.jpg", UriKind.Relative)) },
            new Friend { Name = "Bobo", Image = new BitmapImage(new Uri("res/Bob.jpg", UriKind.Relative)) }
            };

            //moramo prej nastavit kak pogledamo tab o meni, da nalozimo?
            Ime = Properties.Settings.Default.Ime;
            Priimek = Properties.Settings.Default.Priimek;
            Email = Properties.Settings.Default.Email;
            OnPropertyChanged(nameof(Ime));


            // _addPostWindow = new AddPostWindow();

            AddPostCommand = new RelayCommand(AddPost);
            RemovePostCommand = new RelayCommand(RemovePost, CanRemovePost);
            EditPostCommand = new RelayCommand(EditPost, CanEditPost);

            AddFriendCommand = new RelayCommand(AddFriend);
            RemoveFriendCommand = new RelayCommand(RemoveFriend, CanRemoveFriend);
            EditFriendCommand = new RelayCommand(EditFriend, CanEditFriend);

            AlternativniPogledCommand = new RelayCommand(AlternativniPogled);
            NavadenPogledCommand = new RelayCommand(NavadenPogled);

            SaveCommand = new RelayCommand(SaveSettings);


            NavadenPogled(); // Set the default layout

        }


        //saving  O meni

        private string ime;
        public string Ime
        {
            get { return ime; }
            set
            {
                ime = value;
                OnPropertyChanged();
            }
        }

        private string priimek;
        public string Priimek
        {
            get { return priimek; }
            set
            {
                priimek = value;
                OnPropertyChanged();
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        private string profilePicturePath;
        public string ProfilePicturePath
        {
            get { return profilePicturePath; }
            set
            {
                profilePicturePath = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        private void SaveSettings()
        {
            // Retrieve the values from the properties in the view model
            string ime = Ime;
            string priimek = Priimek;
            string email = Email;

            // Update the settings with the new values
            Properties.Settings.Default.Ime = ime;
            Properties.Settings.Default.Priimek = priimek;
            Properties.Settings.Default.Email = email;
            // Save the settings
            Properties.Settings.Default.Save();
        }




        #region friends
        public ICommand AddFriendCommand { get; set; }
        public RelayCommand RemoveFriendCommand { get; private set; }
        public RelayCommand EditFriendCommand { get; private set; }



        private ObservableCollection<Friend> _friends;
        public ObservableCollection<Friend> Friends
        {
            get => _friends;
            set
            {
                _friends = value;
                OnPropertyChanged();
            }
        }

        private Friend _selectedFriend;
        public Friend SelectedFriend
        {

            get => _selectedFriend;
            set
            {
                _selectedFriend = value;
                OnPropertyChanged(nameof(SelectedFriend));
            }
        }

        public bool CanEditFriend()
        {
            // Disable the EditFriend command if no friend is selected
            return SelectedFriend != null;
        }

        public void EditFriend()
        {
            if (SelectedFriend == null) return;

            // Modify the selected friend
            SelectedFriend.Name = "Modified Friend";
            SelectedFriend.Image = new BitmapImage(new Uri("res/sprememba.jpg", UriKind.Relative));

            // Raise property changed events for the modified properties
            OnPropertyChanged(nameof(SelectedFriend));
            OnPropertyChanged(nameof(Friends));
        }

        public bool CanRemoveFriend()
        {
            // Disable the RemoveFriend command if no friend is selected
            return SelectedFriend != null;
        }

        public void RemoveFriend()
        {
            // Remove the currently selected friend from the Friends collection
            Friends.Remove(SelectedFriend);
        }

        public void AddFriend()
        {

            // Define the name and image of the new friend
            string name = "TROLLER";
            BitmapImage image = new BitmapImage(new Uri("res/eidt.png", UriKind.Relative));

            // Create a new friend
            var friend = new Friend
            {
                Name = name,
                Image = image
            };

            // Add the new friend to the Friends collection
            Friends.Add(friend);
        }


        #endregion


        #region Posts

        //za zdaj samo delete in insert, iz nekega razloga negre ce naredim

        public ICommand AddPostCommand { get; set; }
        public RelayCommand EditPostCommand { get; private set; }

        public RelayCommand RemovePostCommand { get; private set; }

        private ObservableCollection<Post> _posts;

        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set
            {
                _posts = value;
                OnPropertyChanged();
            }
        }

        private Post _selectedPost;
        public Post SelectedPost
        {
            get { return _selectedPost; }
            set
            {
                _selectedPost = value;
                OnPropertyChanged(nameof(SelectedPost));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public void EditPost()
        {
            if (SelectedPost == null) return;

            _editPostWindow = new EditWindowView(this, SelectedPost);
            // Pass the selected post to the EditWindowView

            _editPostWindow.ShowDialog();

            //se ne posodbi????

            OnPropertyChanged(nameof(SelectedPost));
            OnPropertyChanged(nameof(Posts));

        }
        // CanEditPost command can execute handler
        public bool CanEditPost()
        {
            // Disable the EditPost command if no post is selected
            return SelectedPost != null;
        }
        public void RemovePost()
        {
            // Remove the currently selected post from the Posts collection
            Posts.Remove(SelectedPost);
        }

        // CanRemovePost command can execute handler
        public bool CanRemovePost()
        {
            // Disable the RemovePost command if no post is selected
            return SelectedPost != null;
        }




        private void AddPost()
        {
            _addPostWindow = new AddPostWindow(this);
           _addPostWindow.ShowDialog();
        }

        public void AddPostToCollection(Post post)
        {
            Posts.Add(post);
        }

        private void AddPostWindow_Closing(object sender, CancelEventArgs e)
        {
            // Set the DialogResult of the AddPostWindow based on the user action (Save or Cancel)
            var addPostWindow = (AddPostWindow)sender;
            if (addPostWindow.DialogResult == null)
            {
                // The window was closed without clicking the Save or Cancel buttons
                // Set the DialogResult to false to indicate cancellation
                addPostWindow.DialogResult = false;
            }
        }






        public void StaticneObjave()
        {
            // Create some sample posts
            var post1 = new Post
            {
                Title = "telefon spet pokvarjen",
                Content = "prve dne je blo vse vredik, nevem kaj mu je zdaj, kličem ano pa teota in se ne zglasita...",
                Image = new BitmapImage(new Uri("res/postImage1.jpg", UriKind.Relative)),
                Author = "Bob",
                Date = new DateTime(2023, 3, 23),
                TaggedFriends = new List<string> { "Ana", "Teo" }
            };
            var post2 = new Post
            {
                Title = "pozabil sem na uro",
                Content = "shit meni se mudi.",
                Image = new BitmapImage(new Uri("res/postImage2.jpg", UriKind.Relative)),
                Author = "Ana",
                Date = new DateTime(2023, 3, 22),
                TaggedFriends = new List<string> { "" }
            };
            var post3 = new Post
            {
                Title = "mislim da sem nekaj zahakljal",
                Content = "verjetno sem fasal corono.",
                Image = new BitmapImage(new Uri("res/postImage3.jpg", UriKind.Relative)),
                Author = "zan",
                Date = new DateTime(2023, 3, 23),
                TaggedFriends = new List<string> { "" }
            };

            Posts.Add(post1);
            Posts.Add(post2);
            Posts.Add(post3);
        }
        #endregion


        #region pogled    


        public RelayCommand AlternativniPogledCommand { get; private set; }
        public RelayCommand NavadenPogledCommand { get; private set; }
        public alternativniPogled AlternativeLayoutControl { get; set; }

        private UIElement currentLayout;

        public UIElement CurrentLayout
        {
            get { return currentLayout; }
            set
            {
                if (currentLayout != value)
                {
                    currentLayout = value;
                    OnPropertyChanged(nameof(CurrentLayout));
                }
            }
        }
        private void AlternativniPogled()
        {
            AlternativeLayoutControl = new alternativniPogled();
            CurrentLayout = AlternativeLayoutControl;
            IsDefaultView = false;
        }

        private void NavadenPogled()
        {
            //CurrentLayout = new DefaultLayoutControl();
            IsDefaultView = true;
        }

        #endregion

    }
}