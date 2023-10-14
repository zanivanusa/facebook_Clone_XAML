using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace socialnoOmrezje
{
    public partial class EditWindowView : Window
    {
        public ICommand ChooseImageCommand => new RelayCommand(ChooseImage);
        public ICommand EditCommand => new RelayCommand(Edit);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        private Post _selectedPost; // Define the SelectedPost property
        private ViewModel _viewModel;
        private BitmapImage prikazSlike;

         
        public EditWindowView(ViewModel viewModel, Post selectedPost)
        {
            InitializeComponent();
            _viewModel = viewModel;

            _selectedPost = selectedPost; // Assign the selectedPost to the SelectedPost property
            DataContext = this;

            // Set the initial values in the TextBox controls
            TitleTextBox.Text = _selectedPost.Title;
            ContentTextBox.Text = _selectedPost.Content;
            ImagePathTextBox.Text = _selectedPost.Image.ToString();
            prikazSlike = (BitmapImage)_selectedPost.Image;
        }


        private void ChooseImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = "res";
            if (openFileDialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = "res/" + openFileDialog.SafeFileName;
                MessageBox.Show(ImagePathTextBox.Text);
            }

            prikazSlike = new BitmapImage(new Uri(ImagePathTextBox.Text, UriKind.Relative));

        }


        private void Edit()
        {
            // Retrieve the selected post from your ViewModel or wherever it's stored

            // Update the properties of the selected post
            _selectedPost.Title = TitleTextBox.Text;
            _selectedPost.Content = ContentTextBox.Text;
            _selectedPost.Image = prikazSlike;

            // Perform the necessary actions with the updated post (e.g., save it to the database, update the collection, etc.)
            // Notify the ViewModel that the selected post has changed
            _viewModel.OnPropertyChanged(nameof(_viewModel.SelectedPost));
            _viewModel.OnPropertyChanged(nameof(_viewModel.Posts));

            // Close the EditWindowView
            Close();
        }

        private void Cancel()
        {
            // Close the EditWindowView without saving
            Close();
        }
    }
}
