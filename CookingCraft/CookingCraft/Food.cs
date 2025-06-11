using System;
using Serilog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CookingCraft
{
    public class Food
    {
        // Properties

        private const int PixelSize = 320; // Size of the sprite in pixels
        private const int SpritesPerRow = 8; // Number of sprites per row in the sprite sheet

        public string Name { get; set; }
        public int ID { get; set; }
        public ImageSource Sprite { get; set; }
        private Canvas GameCanvas { get; set; }

        public double XPos { get; set; }
        public double YPos { get; set; }

        private ObservableCollection<Food> CollectionFood { get; set; }

        // Constructor
        public Food(int id, Canvas gameCanvas, ObservableCollection<Food> collection, bool bought = false)
        {
            // bought decides whether the Ingredient was bought, combined or available since the start

            ID = id;
            GameCanvas = gameCanvas;
            CollectionFood = collection;

            // Load the sprite and name
            this.LoadSprite();
            this.LoadName();

            if (bought)
            {


                this.AddEntry(CollectionFood);
                Log.Logger.Information($"Food with ID {ID} and Name {Name} was bought and added to the collection.");
            }

        }
        public Food()
        {

        }
        public Food(bool trash = true)
        {

            // Constructor for trash food
            ID = 0;
            Name = "Trash";
            Sprite = new BitmapImage(new Uri("Ressources/Sprites/trash.png", UriKind.Relative));
        }
        // Methods

        public void AddEntry(ObservableCollection<Food> CollectionFood)
        {
            // Add the food to the collection
            CollectionFood.Add(this);
            Log.Logger.Information($"Food with ID {ID} and Name {Name} was added to the collection.");
        }

        // Combine two Food objects into one new Food object
        public Food Combine(Food Ingredient, Game game)
        {
            // Ingredient : Food that this Object gets combined with
            Food food = null;

            // Decide which ID is smaller (becuase the CSV is sorted)
            int smallerID = Math.Min(ID, Ingredient.ID);
            int biggerID = Math.Max(ID, Ingredient.ID);
            Log.Logger.Information($"Combining Food with ID {ID} and Name {Name} with Food with ID {Ingredient.ID} and Name {Ingredient.Name}.");

            using (var reader = new StreamReader("Ressources/Recipes.csv"))
            {
                // Read the file line by line
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Split the line into parts
                    string[] parts = line.Split(';');
                    // Check if the ID matches
                    if (int.Parse(parts[0]) == smallerID && int.Parse(parts[1]) == biggerID)
                    {
                        food = new Food(int.Parse(parts[2]), GameCanvas, CollectionFood);
                      

                        break;
                    }
                }
            }
            // Check if the food was found
            if (food == null)
            {
                // If not found, create trash food
                return new Food(trash: true);
            }

            return food;
        }

        // Calls the Achievment
        public Achievment CallAchievment(Game game, ObservableCollection<Achievment> entrys)
        {
            using (var reader = new StreamReader("Ressources/Achievements.csv"))
            {
                // Read the file line by line
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Split the line into parts
                    string[] parts = line.Split(';');
                    // Check if the ID matches
                    if (int.Parse(parts[0]) == ID)
                    {
                        Achievment achievment = new Achievment(int.Parse(parts[2]), game, entrys);
                        Log.Logger.Information($"Achievement was unlocked for Food with ID: {ID}.");
                        return achievment;
                    }
                }
            }
            return new Achievment(); // Return a default Achievment if none was found




        }

        // Sets the Sprite
        private void LoadSprite()
        {
            var spriteSheet = new BitmapImage(new Uri("Ressources/Sprites/IngredientsSpriteSheet.png", UriKind.Relative));

            int spriteIndex = ID - 1; // Assuming ID starts from 1
            int spriteX = (spriteIndex % SpritesPerRow) * PixelSize;
            int spriteY = (spriteIndex / SpritesPerRow) * PixelSize;
            var spriteRect = new Int32Rect(spriteX, spriteY, PixelSize, PixelSize);
            var croppedSprite = new CroppedBitmap(spriteSheet, spriteRect);
            Sprite = croppedSprite;
            Log.Logger.Information($"Sprite for Food with ID {ID} and Name {Name} was loaded.");

        }

        // Sets the Name 
        private void LoadName()
        {
            using (var reader = new StreamReader("Ressources/IngredientNames.csv", Encoding.UTF8))
            {
                // Read the file line by line
                string? line; ;
                while ((line = reader.ReadLine()) != null)
                {
                    // Split the line into parts
                    string[] parts = line.Split(';');
                    // Check if the ID matches
                    if (int.Parse(parts[0]) == ID)
                    {
                        Name = parts[1];
                        break;
                    }
                }
            }
            Log.Logger.Information($"Name for Food with ID {ID} was set to {Name}.");

        }
    }
}
