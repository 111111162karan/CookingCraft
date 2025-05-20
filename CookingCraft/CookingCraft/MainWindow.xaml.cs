using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CookingCraft
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Which button gets pressed determines what save is loaded
            Button btn = sender as Button;
            string saveFileName = btn.Name;

            // Load the save
            // TODO: Load the save
            Game game = Game.LoadGame(saveFileName);

            // Hide the grid with the saves
            GridSaves.Visibility = Visibility.Collapsed;

            // Show the main game
            MainFrame.Navigate(new MainGame(game));
            
        }

    }
}