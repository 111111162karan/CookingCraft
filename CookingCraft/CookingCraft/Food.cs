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
        public Food(string name, int id, Canvas sprite, Canvas gameCanvas)
        {
            Name = name;
            ID = id;
            Sprite = sprite;
            GameCanvas = gameCanvas;
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



    }
}
