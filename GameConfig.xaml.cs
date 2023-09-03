using System;
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

namespace BondeBridge
{
    /// <summary>
    /// Interaction logic for GameConfig.xaml
    /// </summary>
    public partial class GameConfig : UserControl
    {

        public class GameConfigAttr
        {
            public List<string> PlayerNames { get; set; } = new List<string>();

            public int Rounds { get; set; }
        }

        GameConfigAttr configAttr = new GameConfigAttr();

        private bool isGameTypeSelected = false;

        public GameConfig()
        {
            InitializeComponent();

            
            

        }
        

        private void numPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            
            if (selectedItem != null)
            {
                string content = selectedItem.Content.ToString();
                int numPlayers;

                if (int.TryParse(content, out numPlayers))
                {
                    
                    playerList.Children.Clear();

                    for (int i = 0; i < numPlayers; i++)
                    {

                        TextBlock playerText = new TextBlock();
                        playerText.HorizontalAlignment = HorizontalAlignment.Center;
                        playerText.FontSize = 24;
                        playerText.FontStyle = FontStyles.Italic;
                        playerText.FontFamily = new FontFamily("Impact");
                        playerText.Text = $"Player {i+1}: ";


                        TextBox playerName = new TextBox();
                        playerName.HorizontalAlignment = HorizontalAlignment.Stretch;
                        playerName.VerticalAlignment = VerticalAlignment.Center;
                        playerName.Background = Brushes.LightSkyBlue;
                        playerName.Width = 100;
                        playerName.Margin = new Thickness(30, 0, 0, 0);
                        playerName.TextChanged += TextBox_TextChanged;

                        var panel = new StackPanel { Orientation = Orientation.Horizontal };
                        panel.Children.Add(playerText);
                        panel.Children.Add(playerName);

                        playerList.Children.Add(panel);
                    }     
     
                }
     
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckToEnableSubmit();
        }

        private void CheckToEnableSubmit()
        {
            if (isGameTypeSelected)
            {
                foreach (StackPanel panel in playerList.Children)
                {
                    var textBox = panel.Children[1] as TextBox;
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        startGame.IsEnabled = false;
                        return;
                    }

                }
                
                startGame.IsEnabled = true;
            }
        }

        private void StartGameBtn(object sender, RoutedEventArgs e)
        {
            // Add selected game properties to attributes
            foreach(var name in playerList.Children)
            {
                // If it's a panel, go deeper
                if (name is Panel panel)
                {
                    foreach (var innerChild in panel.Children)
                    {
                        if (innerChild is TextBox textBox)
                        {
                            configAttr.PlayerNames.Add(textBox.Text);
                        }
                    }
                }
            }

            // Set window to full size
            Window window = Window.GetWindow(this);
            if (window != null)
            {
                window.WindowState = WindowState.Maximized;
            }

            //GameMonitoring gameConfiguration = new GameMonitoring(configAttr.PlayerNames, configAttr.Rounds);
             
            GamePlayWindow gameView = new GamePlayWindow(configAttr.PlayerNames, configAttr.Rounds);
            //gameView.PlayerNames = configAttr.PlayerNames;
            //gameView.Rounds = configAttr.Rounds;
            window.Content = gameView;
            
            //window.Content = new GamePlayWindow();

        }

        public void SevenRoundBtn(object sender, RoutedEventArgs e)
        {
            configAttr.Rounds = 7;

            isGameTypeSelected = true;
            
            // Highlight this button
            SevenRound.Background = Brushes.LightSkyBlue;

            // Remove highlight from the other button
            TenRound.Background = Brushes.LightBlue;

            CheckToEnableSubmit();
        }
        private void TenRoundBtn(object sender, RoutedEventArgs e)
        {
            configAttr.Rounds = 10;

            isGameTypeSelected = true;

            // Highlight this button
            TenRound.Background = Brushes.LightSkyBlue;

            // Remove highlight from the other button
            SevenRound.Background = Brushes.LightBlue;

            CheckToEnableSubmit();
        }
    }
}
