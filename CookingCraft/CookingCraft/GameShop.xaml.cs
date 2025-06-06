using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace CookingCraft
{
    /// <summary>
    /// Interaktionslogik für GameShop.xaml
    /// </summary>
    public partial class GameShop : Window
    {
        MainGame mainGame { get; set; }
        private ObservableCollection<Food> CollectionFood { get; set; }
        private Canvas GameCanvas { get; set; }

        private const int MaxFoodCount = 5; // Maximum number of food items in the shop

        private Food[] ShopFood { get; set; } = new Food[MaxFoodCount]; // Array to hold the food items in the shop, 5 is the maximum number of items in the shop

        private const int AvailableFoodID = 30; // The last ID that can be bought 

        public GameShop(MainGame gameWindow)
        {
            InitializeComponent();
            mainGame = gameWindow;
            CollectionFood = mainGame.entrys; // Get the food collection from the main game
            Restock();

        }




        public void Restock()
        {
            List<Food> boughtFood = new List<Food>();


            foreach (Food food in CollectionFood)
            {
                boughtFood.Add(food);
            }

            bool breaker = false; // Flag to break out of the loop when the maximum food count is reached
            int counter = 0;
            using (var reader = new StreamReader("Ressources/IngredientNames"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    if (breaker) break; // Break the loop if the flag is set to true
                    foreach (Food food in boughtFood)
                    {
                        if ($"{food.ID}" != parts[0])
                        {
                            if (counter <= MaxFoodCount)
                            {
                                ShopFood[counter] = new Food(int.Parse(parts[0]), GameCanvas, CollectionFood, false);
                                counter++;
                            }
                            else
                            {
                                breaker = true; // Set the flag to true to break out of the loop
                                break;
                            }
                        }
                    }
                }
            }



        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = int.Parse(button.Name);

            

        }
    }
}
