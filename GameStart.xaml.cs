using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BondeBridge
{
    /// <summary>
    /// Interaction logic for GameStart.xaml
    /// </summary>
    public partial class GameStart : UserControl
    {
        public GameStart()
        {
            InitializeComponent();
            
        }

        private void start_game_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Content = new GameConfig();
       
        }

        private void quit_game_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
