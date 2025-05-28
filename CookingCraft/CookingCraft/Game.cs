using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
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


        public Game(List<int> ingredients, int coins, string kitchenname) 
        {
            IngredientIDs = ingredients;
            Coins = coins;
            KitchenName = kitchenname;

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
                if(json == "")
                {
                    return new Game(null, 0, "CookiesKitchen"); // Return a new game if the file is empty or not found
                }
                game = JsonSerializer.Deserialize<Game>(json);
            }
            return game;
        }
        public void Initialise(ObservableCollection<Food> Entrys, Canvas GameCanvas, TextBox TextBoxKitchen)
        {
            

            

            // Set the kitchen name in the TextBox
            TextBoxKitchen.Text = KitchenName;



            // Load the ingredients from the IDs
            if (IngredientIDs != null)
            {
                foreach (int id in IngredientIDs)
                {
                    Food food = new Food(id, GameCanvas, Entrys, bought: true);

                }
            }
            


        }
    }
}
