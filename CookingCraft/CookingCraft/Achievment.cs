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

        public int UnlockID { get; set; }  // ID of the achievement that unlocks this one, if applicable

        public ImageSource Sprite { get; set; }
        public Achievment(int id, Game game, ObservableCollection<Achievment> entries)
        {
            ID = id;
            LoadDescription();
            LoadSprite();

            // Prüfen, ob es vorher schon unlockt war
            bool wasAlreadyUnlocked = game.AchievmentIDs.Contains(ID);

            if (!wasAlreadyUnlocked)
            {
                // Neues Achievement!
                game.AchievmentIDs.Add(ID);

                // Pop-Up auf dem UI-Thread anzeigen
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    MessageBox.Show(
                        $"Erfolg freigeschaltet:\n{Description}",
                        "Achievement unlocked",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    )
                ));
            }

            // Ist unlocked jetzt immer true (alt oder neu)
            IsUnlocked = true;

            // Nur einmalig in die Liste einfügen
            if (!entries.Any(a => a.ID == ID))
            {
                game.Coins += 5;
                entries.Add(this);
            }


        }
        public Achievment(int id, ObservableCollection<Achievment> entrys)
        {
            ID = id;
            LoadDescription();
            LoadSprite();

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
                string? line;
                while((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(";");
                    if (int.Parse(parts[2]) == ID)
                    {
                        Description = parts[1];
                        UnlockID = int.Parse(parts[0]); // Assuming the fourth column is the UnlockID

                    }
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

            int spriteIndex = UnlockID - 1;
            int spriteX = (spriteIndex % SpritesPerRow) * PixelSize;
            int spriteY = (spriteIndex / SpritesPerRow) * PixelSize;
            var spriteRect = new Int32Rect(spriteX, spriteY, PixelSize, PixelSize);
            var croppedSprite = new CroppedBitmap(spriteSheet, spriteRect);
            Sprite = croppedSprite;
            Log.Logger.Information($"Sprite for Achievment with ID {ID} was loaded");

        }
    }
}
