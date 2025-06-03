using Serilog;
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
        public Game game { get; set; } // Game object to hold the game state
        public string saveFileName;
        public MainWindow()
        {

            InitializeComponent();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("kitchenlog.log")
                .CreateLogger();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Which button gets pressed determines what save is loaded
            Button btn = sender as Button;
            saveFileName = btn.Name;

            // Load the save
            // TODO: Load the save
            game = Game.Load(saveFileName);



            // Hide the grid with the saves
            GridSaves.Visibility = Visibility.Collapsed;

            // Show the main game
            MainFrame.Navigate(new MainGame(game));
            
            Log.Logger.Information($"Game loaded from save file: {saveFileName}");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            game.Save(saveFileName); // Save the game on close
            Log.Logger.Information($"Game saved to file: {saveFileName}");
            Log.Logger.Information("Window was closed.");
        }
    }
}