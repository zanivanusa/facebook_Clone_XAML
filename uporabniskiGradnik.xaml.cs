using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace socialnoOmrezje
{
    public partial class uporabniskiGradnik : UserControl
    {
        // Custom event declaration
        public event EventHandler<UserDataChangedEventArgs> UserDataChanged;

        // Custom event arguments class
        public class UserDataChangedEventArgs : EventArgs
        {
            public string NewProfilePicturePath { get; set; }
        }

        private string profilePicturePath;
        private BitmapImage prikazSlike;

        public uporabniskiGradnik()
        {
            InitializeComponent();

            // Initialize the profile picture path
            profilePicturePath = Properties.Settings.Default.ProfilePicturePath;

            // Set the initial profile picture
            if (!string.IsNullOrEmpty(profilePicturePath) && File.Exists(profilePicturePath))
                prikazSlike = new BitmapImage(new Uri(profilePicturePath));

            // Set the values from the settings
            imeTextBox.Text = Properties.Settings.Default.Ime;
            priimekTextBox.Text = Properties.Settings.Default.Priimek;
            emailTextBox.Text = Properties.Settings.Default.Email;
            ImagePathTextBox.Text = profilePicturePath;
            ProfileImage.Source = prikazSlike;
        }

        // Method to raise the custom event when profile picture is changed
        private void UpdateProfilePicture(string newImagePath)
        {
            UserDataChangedEventArgs eventArgs = new UserDataChangedEventArgs
            {
                NewProfilePicturePath = newImagePath
            };

            UserDataChanged?.Invoke(this, eventArgs);
        }

        private void ChooseImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = "res";
            if (openFileDialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = openFileDialog.FileName;
                profilePicturePath = openFileDialog.FileName;
                prikazSlike = new BitmapImage(new Uri(profilePicturePath));
                ProfileImage.Source = prikazSlike;
            }
        }

        // Event handler for the button click event
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the updated values from the text boxes
            string ime = imeTextBox.Text;
            string priimek = priimekTextBox.Text;
            string email = emailTextBox.Text;

            // Save the values to settings
            Properties.Settings.Default.Ime = ime;
            Properties.Settings.Default.Priimek = priimek;
            Properties.Settings.Default.Email = email;
            Properties.Settings.Default.ProfilePicturePath = profilePicturePath;
            Properties.Settings.Default.Save();

            // Raise the custom event
            UpdateProfilePicture(profilePicturePath);

            // Optionally, you can also raise events for other data changes
        }

        // Event handler for the Choose Image button click event
        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            ChooseImage();
        }
    }
}
