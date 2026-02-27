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

        int tenthsOfSecondsElapsed;
        int mathesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;

            SetUpGame();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            TimeTextBlock.Text = (tenthsOfSecondsElapsed / 10f).ToString("0.0s");

            if (mathesFound == 8)
            {
                timer.Stop();
                TimeTextBlock.Text = TimeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            List<string> animeEmoji = new List<string>()
            {
                "💵","💵",
                "🍄","🍄",
                "🐦‍","🐦‍",
                "🔥","🔥",
                "🌑","🌑",
                "🔆","🔆",
                "👮","👮",
                "🦴","🦴",
            };

            Random random = new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name == "TimeTextBlock") continue;

                int index = random.Next(animeEmoji.Count);

                string nextEmoji = animeEmoji[index];

                textBlock.Text = nextEmoji;
                textBlock.Visibility = Visibility.Visible;

                animeEmoji.RemoveAt(index);
            }


            timer.Start();
            tenthsOfSecondsElapsed = 0;
            mathesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMath = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;

            if (findingMath == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMath = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                textBlock.Visibility = Visibility.Hidden;
                findingMath = false;
                mathesFound++;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMath = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mathesFound == 8)
            {
                SetUpGame();
            }
        }

    }
}