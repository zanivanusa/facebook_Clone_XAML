using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace socialnoOmrezje
{
    public partial class AddPostWindow : Window
    {
       
        public ICommand ChooseImageCommand => new RelayCommand(ChooseImage);
        public ICommand SaveCommand => new RelayCommand(Save);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        private string _imagePath { get; set; }

        private ViewModel _viewModel;

        private BitmapImage prikazSlike;

        public AddPostWindow(ViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = this;
        }

        private void ChooseImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = "res";
            if (openFileDialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = "res/" + openFileDialog.SafeFileName;
                _imagePath = "res/" + openFileDialog.SafeFileName;
            }

            prikazSlike = new BitmapImage(new Uri(_imagePath, UriKind.Relative));

        }

        private void Save()
        {

            if ( string.IsNullOrEmpty(TitleTextBox.Text) || string.IsNullOrEmpty(ContentTextBox.Text))
            {
              //  MessageBox.Show("invalid inputs try again");
               // Save();
            }

            // Create a new Post object and populate its properties
            var post = new Post
            {

                Title = TitleTextBox.Text,
                Content = ContentTextBox.Text,//v poti samo od res naprej
                Image = prikazSlike,
                Author = Properties.Settings.Default.Ime,
            };

            _viewModel.Posts.Add(post);

            // Close the AddPostWindow
            Close();
        }

        private void Cancel()
        {
            // Close the AddPostWindow without saving
            Close();
        }
    }
}
