using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace CookingCraft
{
    public class Achievment
    {
        private const int PixelSize = 320; // Size of the sprite in pixels
        private const int SpritesPerRow = 8; // Number of sprites per row in the sprite sheet
        public string Description { get; set; }
        public int ID { get; set; }
        public bool IsUnlocked { get; set; } = false;

        public ImageSource Sprite { get; set; }
        public Achievment(int id, Game game, ObservableCollection<Achievment> entrys)
        {
            ID = id;
            LoadDescription();
            LoadSprite();

            if (game.AchievmentIDs.Contains(ID))
            {
                IsUnlocked = true;
            }
            PopUp();
            AddEntry(entrys);


        }
        public Achievment()
        {
        }

        public void Unlock()
        {
            IsUnlocked = true;
        }

        public void LoadDescription()
        {
            using (var reader = new StreamReader("Ressources/Achievements.csv"))
            {
                string? line = reader.ReadLine();
                string[] parts = line.Split(';');

                if (line != null && int.Parse(parts[2]) == ID)
                {
                    Description = parts[1];



                }
            }
        }
        public void PopUp()
        {
            if (IsUnlocked)
            {
                MessageBox.Show($"{Description}");
            }

        }
        public void AddEntry(ObservableCollection<Achievment> entrys)
        {
            entrys.Add(this);
        }

        private void LoadSprite()
        {
            var spriteSheet = new BitmapImage(new Uri("Ressources/Sprites/IngredientsSpriteSheet.png", UriKind.Relative));

            int spriteIndex = ID - 1; // Assuming ID starts from 1
            int spriteX = (spriteIndex % SpritesPerRow) * PixelSize;
            int spriteY = (spriteIndex / SpritesPerRow) * PixelSize;
            var spriteRect = new Int32Rect(spriteX, spriteY, PixelSize, PixelSize);
            var croppedSprite = new CroppedBitmap(spriteSheet, spriteRect);
            Sprite = croppedSprite;
            Log.Logger.Information($"Sprite for Achievment with ID {ID} was loaded");

        }
    }
}
