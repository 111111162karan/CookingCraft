using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
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
    /// Interaktionslogik für Achievements.xaml
    /// </summary>
    public partial class Achievements : Window
    {

        public Achievements(ObservableCollection<Achievment> entrys)
        {
            InitializeComponent();

            // Set the DataContext of the ListView to the provided ObservableCollection
            ListViewAchievements.ItemsSource = entrys;
        }
    }
}