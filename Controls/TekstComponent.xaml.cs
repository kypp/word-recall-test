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

namespace Controls
{
    /// <summary>
    /// Interaction logic for TekstComponent.xaml
    /// </summary>
    public partial class TekstComponent : UserControl, IDowncountable
    {
        public string IntroText { get; set; }
        public string ButtonText { get; set; }
        public string InputText { get; set; }

        public string ExpectedString { get; set; }

        private TimeSpan _countdown;
        public TimeSpan Countdown
        {
            get { return _countdown; }
            set
            {
                _countdown = value;
                countdownLabel.Content = value.TotalSeconds > 0 ? _countdown.ToString() : "";
            }
        }

        public bool Validate()
        {
            int i = String.Compare(InputText.Trim(), ExpectedString.Trim(), StringComparison.OrdinalIgnoreCase);
            return i==0;
        }

        public event EventHandler ButtonClicked;

        public TekstComponent(string text, string buttonText)
        {
            IntroText = text;
            ButtonText = buttonText;
            InputText = "";
            InitializeComponent();
            InputBox.Visibility = Visibility.Hidden;
            if (buttonText.Contains("brak"))
            {
                Przycisk.Visibility = Visibility.Hidden;
                Przycisk.IsEnabled = false;
            }
            InputBox.Focus();
        }

        public TekstComponent(string text, string buttonText, string expectedString) : this(text, buttonText)
        {
            ExpectedString = expectedString;
            InputBox.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ButtonClicked != null) ButtonClicked(sender, e);
        }

        private void Grid_KeyDown_1(object sender, KeyEventArgs e)
        {
        }

        private void ThisControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (InputBox.Visibility == Visibility.Visible)
                InputBox.Focus();
            else
                Przycisk.Focus();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            InputText = InputBox.Text;
            if (e.Key == Key.Enter && ButtonClicked != null)
                ButtonClicked(sender, e);
        }
    }
}
