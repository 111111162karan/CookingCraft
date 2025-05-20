using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CookingCraft
{
    public class Game
    {
        public List<Food> Ingredients { get; set; } // List to hold the ingredients
        public int Coins { get; set; } // Coins the player has


        public Game(List<Food> ingredients, int coins) 
        {
            Ingredients = ingredients;
            Coins = coins;

        }
        public Game()
        {
            
        }
        public static Game LoadGame(string FileName)
        {
            Game game = new Game();
            return game;
        }
    }
}
