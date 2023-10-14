using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace socialnoOmrezje
{
    public class DateTimeConverter : IValueConverter
    {

        /// <summary>
        /// nisem uspel implementirat, pise (collection pred date)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                // Change the format string to the desired format
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out DateTime dateTime))
                {
                    return dateTime;
                }
            }
            return value;
        }
    }



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void PostListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Get the selected post
            Post selectedPost = (Post)postListView.SelectedItem;

            // Display the post content in a MessageBox
            MessageBox.Show(selectedPost.Content);
        }


        private void friendsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Get the selected post
            Friend selectedFriend = (Friend)friendsListView.SelectedItem;

            // Display the post content in a MessageBox
            MessageBox.Show(selectedFriend.Name);
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private ViewModel _viewModel;


        public MainWindow()
        {
            InitializeComponent();

            // Instantiate a new ViewModel and set it as the DataContext
            _viewModel = new ViewModel();
            DataContext = _viewModel;
          

        }
    }
}


