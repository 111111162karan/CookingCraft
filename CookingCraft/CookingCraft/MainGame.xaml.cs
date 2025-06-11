using System;
using Serilog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CookingCraft
{
    /// <summary>
    /// Interaction logic for MainGame.xaml
    /// </summary>
    public partial class MainGame : Page
    {
        private Point _dragStartPoint;
        private Image? draggedImage = null;
        private Point dragOffset;
        public bool initialised = false;
        public ObservableCollection<Food> entrys { get; set; } // ObservableCollection to hold the food items

        public ObservableCollection<Achievment> AchievementEntries { get; set; } = new ObservableCollection<Achievment>(); // ObservableCollection to hold the food items
        public Game CookingGame { get; set; } // Game object to hold the game state
        public MainGame(Game game)
        {
            InitializeComponent();
            initialised = true;

            CookingGame = game; // Set the game object



            // Set the DataContext of the ListView to the ObservableCollection
            // initialize ListView
            entrys = new ObservableCollection<Food>();
            ListViewGame.ItemsSource = entrys;



            CookingGame.Initialise(entrys, GameCanvas, TextBoxKitchenname, AchievementEntries); // Initialise the game

        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Check if the key pressed is Enter
            if (e.Key == Key.Enter)
            {
                // Unfocus the current element (puts focus on invisible element)
                FocusStealer.Focus();
                Log.Logger.Information("Enter key pressed, unfocused current element.");
            }



        }

        private void TextBoxKitchenname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (initialised)
            {
                CookingGame.KitchenName = TextBoxKitchenname.Text; // Update the kitchen name in the game object
                Log.Logger.Information($"Kitchen name changed to: {CookingGame.KitchenName}");

            }
        }

        private void ButtonShop_Click(object sender, RoutedEventArgs e)
        {
            GameShop ShopWindow = new GameShop(this);
            ShopWindow.Show();
        }

        private void ListViewGame_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            _dragStartPoint = e.GetPosition(null);
        }

        private void ListViewGame_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = _dragStartPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListView listView = sender as ListView;
                if (listView == null) return;

                var item = GetItemAtPoint(listView, e.GetPosition(listView));
                if (item != null)
                {
                    DragDrop.DoDragDrop(listView, item, DragDropEffects.Copy);
                }
            }
        }

        private object GetItemAtPoint(ListView listView, Point point)
        {
            var element = listView.InputHitTest(point) as DependencyObject;
            while (element != null && !(element is ListViewItem))
            {
                element = VisualTreeHelper.GetParent(element);
            }
            return element != null ? listView.ItemContainerGenerator.ItemFromContainer(element) : null;
        }

        private void GameCanvas_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Food)))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void GameCanvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Food)))
            {
                Food droppedFood = (Food)e.Data.GetData(typeof(Food));
                Point dropPosition = e.GetPosition(GameCanvas);

                Image foodImage = new Image
                {
                    Source = droppedFood.Sprite,
                    Width = 64,
                    Height = 64,
                    Tag = droppedFood
                };

                // Position setzen
                Canvas.SetLeft(foodImage, dropPosition.X);
                Canvas.SetTop(foodImage, dropPosition.Y);

                // Events registrieren
                foodImage.MouseLeftButtonDown += FoodImage_MouseLeftButtonDown;
                foodImage.MouseMove += FoodImage_MouseMove;
                foodImage.MouseLeftButtonUp += FoodImage_MouseLeftButtonUp;

                // Kombinierbares Ziel
                foodImage.AllowDrop = true;
                foodImage.Drop += FoodImage_Drop;
                foodImage.DragOver += (s, args) =>
                {
                    args.Effects = DragDropEffects.Move;
                    args.Handled = true;
                };

                // Bild dem Canvas hinzufügen
                GameCanvas.Children.Add(foodImage);
            }
        }

        private void FoodImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            draggedImage = sender as Image;
            dragOffset = e.GetPosition(draggedImage);
            draggedImage.CaptureMouse();


            e.Handled = true;
        }

        private void FoodImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggedImage != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point position = e.GetPosition(GameCanvas);
                Canvas.SetLeft(draggedImage, position.X - dragOffset.X);
                Canvas.SetTop(draggedImage, position.Y - dragOffset.Y);

                e.Handled = true; // wichtig, sonst springt Bild
            }
        }

        private void FoodImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (draggedImage != null)
            {
                draggedImage.ReleaseMouseCapture();

                var draggedFood = draggedImage.Tag as Food;
                if (draggedFood != null)
                {
                    // Position des losgelassenen Bildes
                    Rect draggedRect = new Rect(
                        Canvas.GetLeft(draggedImage),
                        Canvas.GetTop(draggedImage),
                        draggedImage.ActualWidth,
                        draggedImage.ActualHeight);

                    Image? targetImage = null;

                    foreach (UIElement child in GameCanvas.Children)
                    {
                        if (child is Image img && img != draggedImage)
                        {
                            Rect targetRect = new Rect(
                                Canvas.GetLeft(img),
                                Canvas.GetTop(img),
                                img.ActualWidth,
                                img.ActualHeight);

                            if (draggedRect.IntersectsWith(targetRect))
                            {
                                targetImage = img;
                                break;
                            }
                        }
                    }

                    if (targetImage != null)
                    {
                        var food2 = targetImage.Tag as Food;
                        if (food2 != null)
                        {
                            Food? result = draggedFood.Combine(food2, CookingGame);
                            result.CallAchievment(CookingGame, AchievementEntries); // Achievement check
                            if (result != null)
                            {
                                // Kombiniertes Bild erzeugen
                                Image combinedImage = new Image
                                {
                                    Source = result.Sprite,
                                    Width = 64,
                                    Height = 64,
                                    Tag = result
                                };

                                combinedImage.MouseLeftButtonDown += FoodImage_MouseLeftButtonDown;
                                combinedImage.MouseMove += FoodImage_MouseMove;
                                combinedImage.MouseLeftButtonUp += FoodImage_MouseLeftButtonUp;

                                // An Position von Zielbild setzen
                                Canvas.SetLeft(combinedImage, Canvas.GetLeft(targetImage));
                                Canvas.SetTop(combinedImage, Canvas.GetTop(targetImage));

                                GameCanvas.Children.Remove(draggedImage);
                                GameCanvas.Children.Remove(targetImage);
                                GameCanvas.Children.Add(combinedImage);
                            }
                        }
                    }
                }

                draggedImage = null;
                e.Handled = true;
            }
        }
        private void FoodImage_Drop(object sender, DragEventArgs e)
        {
            if (sender is Image targetImage &&
                e.Data.GetDataPresent(typeof(Image)) &&
                e.Data.GetData(typeof(Image)) is Image sourceImage &&
                sourceImage != targetImage)
            {
                var food1 = sourceImage.Tag as Food;
                var food2 = targetImage.Tag as Food;

                if (food1 != null && food2 != null)
                {
                    Food? result = food1.Combine(food2, CookingGame);
                    if (result != null)
                    {
                        // Position des Ziel-Images verwenden
                        double left = Canvas.GetLeft(targetImage);
                        double top = Canvas.GetTop(targetImage);

                        Image combinedImage = new Image
                        {
                            Source = result.Sprite,
                            Width = 64,
                            Height = 64,
                            Tag = result
                        };

                        combinedImage.MouseLeftButtonDown += FoodImage_MouseLeftButtonDown;
                        combinedImage.MouseMove += FoodImage_MouseMove;
                        combinedImage.MouseLeftButtonUp += FoodImage_MouseLeftButtonUp;
                        combinedImage.AllowDrop = true;
                        combinedImage.Drop += FoodImage_Drop;

                        Canvas.SetLeft(combinedImage, left);
                        Canvas.SetTop(combinedImage, top);

                        // Alte Bilder entfernen
                        GameCanvas.Children.Remove(sourceImage);
                        GameCanvas.Children.Remove(targetImage);

                        // Neues Bild hinzufügen
                        GameCanvas.Children.Add(combinedImage);
                    }
                }
            }
        }

        private void ButtonAchievements_Click(object sender, RoutedEventArgs e)
        {
            var achievementsWindow = new Achievements(AchievementEntries);
            achievementsWindow.Show();
        }
    }
}
