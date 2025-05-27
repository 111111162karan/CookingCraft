using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CookingCraft
{
    /// <summary>
    /// Interaction logic for MainGame.xaml
    /// </summary>
    public partial class MainGame : Page
    {
        
        public ObservableCollection<Food> entrys { get; set; } // ObservableCollection to hold the food items
        public Game CookingGame { get; set; } // Game object to hold the game state
        public MainGame(Game game)
        {
            InitializeComponent();

            CookingGame = game; // Set the game object
                                
            

            // Set the DataContext of the ListView to the ObservableCollection
            // initialize ListView
            entrys = new ObservableCollection<Food>();
            ListViewGame.ItemsSource = entrys;

            CookingGame.Initialise(entrys, GameCanvas); // Initialise the game
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Check if the key pressed is Enter
            if (e.Key == Key.Enter)
            {
                // Unfocus the current element (puts focus on invisible element)
                FocusStealer.Focus();
            }

        }
    }
}
