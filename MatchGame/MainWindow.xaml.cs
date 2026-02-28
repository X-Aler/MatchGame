using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MatchGame
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        TextBlock lastTextBlockClicked;
        TextBlock lastWrongBlockClicked;
        Stopwatch time;

        bool findingMath;
        int mathesFound;
        bool gameComplete;

        TimeSpan bestTime = TimeSpan.MaxValue;

        double timeToComplete = 15;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;

            SetUpGame();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeTextBlock.Text = (timeToComplete - time.Elapsed.TotalSeconds).ToString("F2") + 's';

            if (mathesFound == 8)
            {
                Stop();

                if (time.Elapsed < bestTime)
                {
                    bestTime = time.Elapsed;
                }

                TimeTextBlock.Text = TimeTextBlock.Text + " - Play again?";
                BestTime.Text = bestTime.TotalSeconds.ToString("F2") + " - Best Time";
            }

            if (time.Elapsed.TotalSeconds >= timeToComplete)
            {
                Stop();

                TimeTextBlock.Text = "Time gone - Play again?";
            }
        }

        private void Stop()
        {
            timer.Stop();

            time.Stop();

            gameComplete = true;
        }

        private void SetUpGame()
        {
            List<string> animeEmoji = new List<string>()
            {
                "💵",
                "🍄",
                "🐦‍",
                "🔥",
                "🌑",
                "🔆",
                "👮",
                "🦴"
            };

            List<string> gameEmoji = animeEmoji.Concat(animeEmoji).ToList();

            Random random = new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (!textBlock.Name.Contains("EmojiText")) continue;

                int index = random.Next(gameEmoji.Count);
                string nextEmoji = gameEmoji[index];

                textBlock.Text = nextEmoji;
                textBlock.Visibility = Visibility.Visible;
                textBlock.Background = Brushes.White;

                gameEmoji.RemoveAt(index);
            }

            time = Stopwatch.StartNew();

            gameComplete = false;
            mathesFound = 0;

            timer.Start();
        }


        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;

            if (textBlock == lastTextBlockClicked || gameComplete) return;

            if (lastWrongBlockClicked != null)
            {
                lastWrongBlockClicked.Background = Brushes.White;
            }

            if (findingMath == false)
            {
                if (lastTextBlockClicked != null)
                {
                    lastTextBlockClicked.Background = Brushes.White;
                }

                textBlock.Background = Brushes.Green;
                lastTextBlockClicked = textBlock;
                findingMath = true;
            }
            else if (textBlock.Text == lastTextBlockClicked?.Text)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked.Visibility = Visibility.Hidden;

                findingMath = false;
                mathesFound++;
                lastTextBlockClicked = null;
            }
            else
            {
                textBlock.Background = Brushes.Red;
                lastWrongBlockClicked = textBlock;

                findingMath = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (gameComplete)
            {
                SetUpGame();
            }
        }

    }
}