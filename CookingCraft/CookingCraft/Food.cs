using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CookingCraft
{
    class Food
    {
        // Properties

        public string Name { get; set; }
        public int ID { get; set; }
        private Canvas Sprite { get; set; }
        private Canvas GameCanvas { get; set; }

        // Constructor
        public Food(int id, Canvas gameCanvas, bool bought = false)
        {
            // bought decides whether the Ingredient was bought, combined or available since the start
            
            ID = id;
            GameCanvas = gameCanvas;

            if (bought)
            {


            }

        }
        public Food()
        {

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
            Food food = new Food();




            return food;
        }

        // Calls the Achievment
        public void CallAchievment()
        {

        }

        // Sets the Sprite
        private void LoadSprite()
        {

        }

        // Sets the Name 
        private void LoadName()
        {

        }

        // Call this if Food is created or placed somewehere
        private void Pop()
        {

        }


    }
}
