using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingCraft
{
    public class Achievment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public bool IsUnlocked { get; set; }
        public Achievment(int id)
        {
            ID = id;
            
        }

        public void Unlock()
        {
            IsUnlocked = true;
        }
    }
}
