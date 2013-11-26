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
    /// Interaction logic for KoncowyTest.xaml
    /// </summary>
    public partial class KoncowyTest : UserControl, IDowncountable
    {
        public Label[] labels;
        public TextBox[] textboxes;

        private List<string[]> slowa;

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

        public float GetPercentage()
        {
            float sum = 0;
            for (int i = 0; i < 15; i++)
            {
                if (String.Compare(textboxes[i].Text.Trim(), slowa[i][1].Trim(), StringComparison.OrdinalIgnoreCase) == 0)
                    sum += 1.0f;
            }
            return sum/15.0f;
        }

        public KoncowyTest(List<string[]> slowa)
        {
            InitializeComponent();
            labels = new Label[15];
            textboxes = new TextBox[15];
            this.slowa = slowa;
            for (int i = 0; i < 15; i++)
            {
                labels[i] = new Label();
                textboxes[i] = new TextBox();
                labels[i].Content = slowa[i][0];
                labels[i].Margin = new Thickness(0,5,0,5);
                textboxes[i].Margin = new Thickness(0, 5, 0, 5);
                textboxes[i].MinWidth = 50;
                DockPanel dock = new DockPanel();
                dock.MinWidth = 200;
                var sep = new Separator();
                sep.MinWidth = 50;
                sep.Visibility = Visibility.Hidden;
                ;
                dock.Children.Add(labels[i]);
                dock.Children.Add(sep);
                dock.Children.Add(textboxes[i]);

                StackPanel.Children.Add(dock);
            }
        }
    }
}
