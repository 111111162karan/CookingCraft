using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CookingCraft
{
    public class Game
    {
        public List<int> IngredientIDs { get; set; } = new List<int>();
        public List<int> AchievmentIDs { get; set; } = new List<int>();
        public int Coins { get; set; }
        public string KitchenName { get; set; } = "CookiesKitchen";
        private static string GetSavePath(string SaveName) => $"Saves/{SaveName}.json";


        public Game(List<int> ingredients, int coins) 
        {
            IngredientIDs = ingredients;
            Coins = coins;

        }
        public Game()
        {
            
        }
        public void Save(string FileName)
        {

            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

            using (var writer = new StreamWriter(GetSavePath(FileName)))
            {
                writer.Write(json);

            }
            
        }
        public static Game Load(string FileName)
        {
            Game game = new Game();
            using (var reader = new StreamReader(GetSavePath(FileName)))
            {
                var json = reader.ReadToEnd();
                game = JsonSerializer.Deserialize<Game>(json);
            }
            return game;
        }
        public void Initialise(ObservableCollection<Food> Entrys, Canvas GameCanvas)
        {
            // Load the ingredients from the IDs
            foreach (int id in IngredientIDs)
            {
                Food food = new Food(id, GameCanvas,Entrys, bought:true);

            }

        }
    }
}
