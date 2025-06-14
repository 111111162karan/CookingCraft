using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
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

        public List<Food> boughtFood = new List<Food>();

        public Game game;

        public GameShop(MainGame gameWindow)
        {
            InitializeComponent();
            mainGame = gameWindow;
            CollectionFood = mainGame.entrys; // Get the food collection from the main game
            game = gameWindow.CookingGame;



            foreach (Food food in CollectionFood)
            {
                boughtFood.Add(food);
            }

            Restock();

        }



        public void Restock()
        {
            int counter = 0;

            using (var reader = new StreamReader("Ressources/IngredientNames.csv"))
            {
                string line;
                while ((line = reader.ReadLine()) != null && counter < MaxFoodCount)
                {
                    string[] parts = line.Split(';');
                    int id = int.Parse(parts[0]);

                    // Prüfen, ob dieses ID schon gekauft wurde
                    bool alreadyBought = false;
                    foreach (var food in boughtFood)
                    {
                        if (food.ID == id)
                        {
                            alreadyBought = true;
                            break;
                        }
                    }

                    if (!alreadyBought)
                    {
                        ShopFood[counter] = new Food(id, GameCanvas, CollectionFood, false);
                        counter++;
                    }
                }
            }

            DrawShop();
        }





        private void DrawShop()
        {

            Image1.Source = ShopFood[0].Sprite;
            Image2.Source = ShopFood[1].Sprite;
            Image3.Source = ShopFood[2].Sprite;
            Image4.Source = ShopFood[3].Sprite;
            Image5.Source = ShopFood[4].Sprite;

            Label1.Content = ShopFood[0].Name;
            Label2.Content = ShopFood[1].Name;
            Label3.Content = ShopFood[2].Name;
            Label4.Content = ShopFood[3].Name;
            Label5.Content = ShopFood[4].Name;

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (game.Coins < 5)
            {
                return;
            }



                Button button = sender as Button;

                // Letzte Ziffer im Button-Namen gibt den Slot an (Image1 -> index 0)
                int index = int.Parse(button.Name.Last().ToString()) - 1;
                Food selectedFood = ShopFood[index];

                // Gekauftes Item zur Liste hinzufügen
                boughtFood.Add(new Food(selectedFood.ID, GameCanvas, CollectionFood, true));

                // Neues Item finden, das noch nicht gekauft oder im Shop ist
                using (var reader = new StreamReader("Ressources/IngredientNames.csv"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(';');
                        int id = int.Parse(parts[0]);

                        bool alreadyUsed = false;

                        // Prüfen ob es bereits gekauft wurde
                        foreach (var food in boughtFood)
                        {
                            if (food.ID == id)
                            {
                                alreadyUsed = true;
                                break;
                            }
                        }

                        // Prüfen ob es bereits im Shop ist
                        if (!alreadyUsed)
                        {
                            for (int i = 0; i < ShopFood.Length; i++)
                            {
                                if (ShopFood[i] != null && ShopFood[i].ID == id)
                                {
                                    alreadyUsed = true;
                                    break;
                                }
                            }
                        }

                        if (!alreadyUsed)
                        {
                            // Neues Item setzen
                            ShopFood[index] = new Food(id, GameCanvas, CollectionFood, false);
                            game.Coins -= 5; // Abziehen der Coins für den Kauf
                            mainGame.UpdateCoinLabel();
                            break;
                        }
                    }
                }

                DrawShop();
        }
    }
}