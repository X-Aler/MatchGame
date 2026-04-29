using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MatchGame
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();

        private TextBlock lastTextBlockClicked;
        private TextBlock lastWrongTextBlockClicked;
        private Stopwatch time;

        private bool findingMath;
        private bool gameComplete;
        private bool gameStart;

        private static Random Random = new Random();

        private TimeSpan bestTime = TimeSpan.MaxValue;

        private List<string> findedMathes = new List<string>();

        private List<string> animeEmoji = new List<string>()
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

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;

            TimeTextBlock.Text = "Select to start!";
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeTextBlock.Text = (time.Elapsed.TotalSeconds).ToString("F2") + 's';

            if (findedMathes.Count == animeEmoji.Count)
            {
                Stop();

                if (time.Elapsed < bestTime)
                {
                    bestTime = time.Elapsed;
                }

                TimeTextBlock.Text += " - Play again?";
                BestTime.Text = bestTime.TotalSeconds.ToString("F2") + " - Best Time";
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

            List<string> gameEmoji = animeEmoji.Concat(animeEmoji).ToList();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (!textBlock.Name.Contains("EmojiText")) continue;

                int index = Random.Next(gameEmoji.Count);
                string nextEmoji = gameEmoji[index];

                textBlock.Text = nextEmoji;
                textBlock.Background = Brushes.Black;
                textBlock.Foreground = Brushes.Black;

                gameEmoji.RemoveAt(index);
            }

            time = Stopwatch.StartNew();

            gameComplete = false;
            findedMathes.Clear();

            timer.Start();

            mathesText.Text = $"Mathes found: {findedMathes.Count}";

            gameStart = true;
        }


        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;

            if (!gameStart)
            {
                SetUpGame();
            }

            if (textBlock == lastTextBlockClicked || gameComplete) return;

            var text = textBlock.Text;

            if (findedMathes.Contains(text)) return;

            if (lastWrongTextBlockClicked != null)
            {
                lastWrongTextBlockClicked.Foreground = Brushes.Black;
                lastWrongTextBlockClicked = null;
            }

            if (findingMath == false)
            {
                textBlock.Foreground = Brushes.White;
                lastTextBlockClicked = textBlock;
                findingMath = true;
            }
            else if (text == lastTextBlockClicked.Text)
            {
                textBlock.Foreground = Brushes.White;

                findedMathes.Add(text);

                mathesText.Text = $"Mathes found: {findedMathes.Count}";

                findingMath = false;

                lastTextBlockClicked = null;
            }
            else
            {
                textBlock.Foreground = Brushes.White;

                lastWrongTextBlockClicked = textBlock;

                lastTextBlockClicked.Foreground = Brushes.Black;

                findingMath = false;
                lastTextBlockClicked = null;
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