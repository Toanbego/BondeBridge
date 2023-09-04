using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using static System.Net.Mime.MediaTypeNames;

namespace BondeBridge
{
    /// <summary>
    /// Interaction logic for GamePlayWindow.xaml
    /// </summary>
    public partial class GamePlayWindow : UserControl
    {

        public List<string> PlayerNames { get; set; }
        public int Rounds { get; set; }

        private List<Player> Players { get; set; } = new List<Player>();

        private List<int> SevenRound { get; set; } = new List<int> {7,6,5,4,3,2,1,2,3,4,5,6,7};
        private List<int> TenRound { get; set; } = new List<int> {10,9,8,7,6,5,4,3,2,1,2,3,4,5,6,7,8,9,10};

        private int CurrentRound { get; set; }

        public GamePlayWindow(List<string> names, int rounds)
        {
            this.PlayerNames = names;
            this.Rounds = rounds;
            this.CurrentRound = 0;

            InitializeComponent();

            // Build game dashboard
            createPlayerClass();
            populatePlayerColumns();
            createScoreColumns();
            maxPredctionsGrid();
            populateCurrentRoundGrid();
            populateTotalScoreColumns();
            createHeaderGrid();
        }

        public void createPlayerClass()
        {
            List<int> initialScore= new List<int>();
            for (int i = 0; i < (Rounds*2)-1; i++) 
            {
                initialScore.Add(0);
            }
       
            foreach (string name in PlayerNames)
            {
                Player newPlayer = new Player(name);
                Players.Add(newPlayer);
            }
            // First player should be dealer in the beginning of the round
            Players[0].isDealer = true;
            Players[Players.Count-1].isLast = true;

        }

        private void populatePlayerColumns()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                // Create TextBlock for player name
                TextBlock textBlock = new TextBlock
                {
                    Text = Players[i].Name,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize=24,
                    FontFamily = new FontFamily("Impact"),
                    
                };
                if (Players[i].isDealer ) { textBlock.Text += " - Dealer"; }
                if (Players[i].isLast) { textBlock.Text += " - Last"; }
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                playerNamesGrid.ColumnDefinitions.Add(col);
                // If not the last element, create and add a vertical line
                if (i <= Players.Count)
                {
                    Border border = new Border
                    {
                        BorderThickness = new Thickness(2),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        Background = new SolidColorBrush(Colors.LightSkyBlue)
                    };
                    Grid.SetColumn(border, i);
                    playerNamesGrid.Children.Add(border);
                }

                // Add to grid cell
                Grid.SetColumn(textBlock, i);
                playerNamesGrid.Children.Add(textBlock);

            }
  
        }

        private void populateTotalScoreColumns()
        {
            Grid grid = totalScoreGrid;
            int row = 0;
            int totalColumnCount = Players.Count;
            for (int c = 0; c < totalColumnCount; c++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                setBorderARoundCell(grid, row, c, "green");
                TextBlock predBlock2 = new TextBlock
                {
                    Text = "0",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 24,
                    FontFamily = new FontFamily("Impact"),
                };
                Grid.SetRow(predBlock2, row);
                Grid.SetColumn(predBlock2, c);
                grid.Children.Add(predBlock2);
            }
            
            



        }

        private void setBorderARoundCell(Grid grid, int row, int col, string colorType)
        {
            
            
            Border border = new Border
            {
                BorderThickness = new Thickness(2),
                BorderBrush = new SolidColorBrush(Colors.Black),
                Background = new SolidColorBrush(Colors.LightSkyBlue)
            };
            if(colorType == "header") { border.Background = new SolidColorBrush(Colors.LightGray); }
            if (colorType == "evenRow") { border.Background = new SolidColorBrush(Colors.White); }
            if (colorType == "oddRow") { border.Background = new SolidColorBrush(Colors.White); }
            if (colorType == "green") { border.Background = new SolidColorBrush(Colors.Green);}
            Grid.SetRow(border, row);
            Grid.SetColumn(border, col);
            grid.Children.Add(border);

        }

        private void createHeaderGrid()
        {
            int row = 0;
            int columnCountPerPlayer = 3;
            int totalColumnCount = columnCountPerPlayer * Players.Count;
            Grid grid = headerGrid;
            for (int c = 0; c < totalColumnCount; c++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int playerIndex = 0; playerIndex < Players.Count; playerIndex++)
            {
                int baseColumn = playerIndex * columnCountPerPlayer;
                // Add column headers

                List<string> headers = new List<string>
                    {
                        "Predictions",
                        "Whists",
                        "Score"
                    };
                for (int x = 0; x < 3; x++)
                {
                    setBorderARoundCell(grid, row, baseColumn + x, "header");
                    TextBlock predBlock2 = new TextBlock
                    {
                        FontSize = 20,
                        Text = headers[x],
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    Grid.SetRow(predBlock2, row);
                    Grid.SetColumn(predBlock2, baseColumn + x);
                    grid.Children.Add(predBlock2);
                }


            }
        }

        private void createScoreColumns()
        {
            int rowCount = (Rounds * 2)-1;
            int columnCountPerPlayer = 3;
            int totalColumnCount = columnCountPerPlayer * Players.Count;
            Grid grid = scoreGrid;


            for (int i = 0; i < rowCount; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            for (int c = 0; c < totalColumnCount; c++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < rowCount; row++)
            {
                for (int playerIndex = 0; playerIndex < Players.Count; playerIndex++)
                {

                    int baseColumn = playerIndex * columnCountPerPlayer;


                    string rowType;
                    if (row % 2 == 0)
                    {
                        rowType = "evenRow";
                    }
                    else 
                    { 
                        rowType = "oddRow"; 
                    }

                    for (int x = 0; x<2; x++)
                    {
                        setBorderARoundCell(grid, row, baseColumn + x, rowType);
                        // Create and add TextBox 1
                        TextBox textBox1 = new TextBox
                        {
                            FontSize = 24,
                            VerticalAlignment = VerticalAlignment.Stretch,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            TextAlignment = TextAlignment.Center,
                            VerticalContentAlignment = VerticalAlignment.Center

                        };
                        Grid.SetRow(textBox1, row);
                        Grid.SetColumn(textBox1, baseColumn + x);
                        grid.Children.Add(textBox1);
                    }
                    // Create and add TextBlock
                    setBorderARoundCell(grid, row, baseColumn + 2, "header");
                    TextBlock textBlock = new TextBlock
                    {
                        Text = "0",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    Grid.SetRow(textBlock, row);
                    Grid.SetColumn(textBlock, baseColumn + 2);
                    grid.Children.Add(textBlock);
                       
                    

                    

                }


            }

        }

        private void maxPredctionsGrid()
        {
            int rowCount = (Rounds * 2) - 1;
            
            Grid grid = maxPredictionGrid;

            

            for (int row = 0; row < rowCount; row++)
            {

                grid.RowDefinitions.Add(new RowDefinition());
                int roundNumber = 0;
                // Create and add TextBlock
                
                if (rowCount == SevenRound.Count)
                {
                    roundNumber = SevenRound[row];
                }
                
                setBorderARoundCell(grid, row, 0, "default");
                TextBlock textBlock = new TextBlock
                {
                    Text = roundNumber.ToString(),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                Grid.SetRow(textBlock, row);
                Grid.SetColumn(textBlock, 0);
                grid.Children.Add(textBlock);
            }
        }

        private void populateCurrentRoundGrid()
        {
            int rowCount = (Rounds * 2) - 1;

            Grid grid = currentRoundGrid;



            for (int row = 0; row < rowCount; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                int roundNumber = 0;
                // Create and add TextBlock

                if (rowCount == SevenRound.Count)
                {
                    roundNumber = SevenRound[row];
                }

                setBorderARoundCell(grid, row, 0, "header");
                TextBlock textBlock = new TextBlock
                {
                    Text = roundNumber.ToString(),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                Grid.SetRow(textBlock, row);
                Grid.SetColumn(textBlock, 0);
                grid.Children.Add(textBlock);
            }
        }


        private void UpdateVisuals()
        {
            // Update Visual
            Grid grid = scoreGrid;
            int rowCount = CurrentRound;

            int playerIdx = 0;
            foreach (var child in grid.Children)
            {
                int i = 3;
                if (child is TextBlock textBlock && Grid.GetRow(textBlock) == CurrentRound)
                {
                    textBlock.Text = Players[playerIdx].currentScore.ToString();
                }
                playerIdx++;
                if (playerIdx == Players.Count) { break; }
            }

            CurrentRound++;
            
            

        }

        private void CalculateScore()
        {
            // Loop through the grid and calculate score for each player
            // Check if all players have added their inputs
            // Ideally a button to go back if a wrong input would be better. Or no button at all
            Grid grid = scoreGrid;
            int rowCount = CurrentRound;



            // Get all variables for calculating the score
            List<string> predInputs = new List<string>();
            foreach (UIElement child in grid.Children)
            {
                if (child is TextBox textBox && Grid.GetRow(textBox) == CurrentRound)
                {
                    string text = textBox.Text;
                    predInputs.Add(text);
                }
            }

            // Calculate and update score
            int playerIdx = 0;
            foreach (var player in Players)
            {
                int pred;
                int trick;

                if (int.TryParse(predInputs[playerIdx], out pred) && int.TryParse(predInputs[playerIdx+1], out trick))
                {
                    player.MyPrediction = pred;
                    player.MyTricks = trick;
                }
                player.CalculateScore(CurrentRound);
                playerIdx = playerIdx + 2;


            }

        }

        private void FinishRound(object sender, RoutedEventArgs e)
        { 
            CalculateScore();

            UpdateVisuals();

            // Change Dealer



            // Check for victory condition
        }
    }
}
