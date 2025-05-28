using System;
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

        private const int PixelSize = 32; // Size of the sprite in pixels
        private const int SpritesPerRow = 8; // Number of sprites per row in the sprite sheet

        public string Name { get; set; }
        public int ID { get; set; }
        private ImageSource Sprite { get; set; }
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
            Sprite = new BitmapImage(new Uri("Ressources/Sprites/Trash.png"));
        }
        // Methods

        public void AddEntry(ObservableCollection<Food> CollectionFood)
        {
            // Add the food to the collection
            CollectionFood.Add(this);
        }

        // Combine two Food objects into one new Food object
        public Food Combine(Food Ingredient)
        {
            // Ingredient : Food that this Object gets combined with
            Food food = null;

            // Decide which ID is smaller (becuase the CSV is sorted)
            int smallerID = Math.Min(ID, Ingredient.ID);
            int biggerID = Math.Max(ID, Ingredient.ID);


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
                        food = new Food(int.Parse(parts[3]), GameCanvas, CollectionFood);
                        food.CallAchievment();
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
        public void CallAchievment()
        {
            // TODO:
            // Popup Message
            // Put into List of Achievments






        }

        // Sets the Sprite
        private void LoadSprite()
        {
            var spriteSheet = new BitmapImage(new Uri("Ressources/Sprites/IngredientsSpriteSheet.png"));

            int spriteIndex = ID - 1; // Assuming ID starts from 1
            int spriteX = (spriteIndex % SpritesPerRow) * PixelSize;
            int spriteY = (spriteIndex / SpritesPerRow) * PixelSize;
            var spriteRect = new Int32Rect(spriteX, spriteY, PixelSize, PixelSize);
            var croppedSprite = new CroppedBitmap(spriteSheet, spriteRect);
            Sprite = croppedSprite;

        }

        // Sets the Name 
        private void LoadName()
        {
            using (var reader = new StreamReader("Ressources/IngredientNames.csv"))
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

        }
    }
}
