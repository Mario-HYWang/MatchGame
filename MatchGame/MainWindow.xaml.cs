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

namespace MatchGame
{
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer = new DispatcherTimer();
        private int _tenthsOfSecondsElapsed;
        private int _matchesFound;

        private TextBlock _lastTextBlockClicked;
        private bool _findingMatch = false;

        public MainWindow()
        {
            InitializeComponent();

            _timer.Interval = TimeSpan.FromSeconds(.1);
            _timer.Tick += Timer_Tick;

            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _tenthsOfSecondsElapsed++;
            TimeTextBlock.Text = (_tenthsOfSecondsElapsed / 10f).ToString("0.0s");
            if (_matchesFound == 8)
            {
                _timer.Stop();
                TimeTextBlock.Text = TimeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            var animalEmojis = new List<string>()
            {
                "🐙","🐙",
                "🐂","🐂",
                "🐶","🐶",
                "🐱","🐱",
                "🐯","🐯",
                "🐟","🐟",
                "🦌","🦌",
                "🐵","🐵",
            };

            var random = new Random();
            
            foreach (var textBlock in MainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "TimeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    var index = random.Next(animalEmojis.Count);
                    var nextEmoji = animalEmojis[index];
                    textBlock.Text = nextEmoji;
                    animalEmojis.RemoveAt(index);
                }
            }

            _timer.Start();
            _tenthsOfSecondsElapsed = 0;
            _matchesFound = 0;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (!_findingMatch)
            {
                textBlock.Visibility = Visibility.Hidden;
                _lastTextBlockClicked = textBlock;
                _findingMatch = true;
            }
            else if (textBlock.Text == _lastTextBlockClicked.Text)
            {
                _matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                _findingMatch = false;
            }
            else
            {
                _lastTextBlockClicked.Visibility = Visibility.Visible;
                _findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
