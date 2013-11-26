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
    /// Interaction logic for WyborBadaniaControl.xaml
    /// </summary>
    public partial class WyborBadaniaControl : UserControl
    {
        public enum TypBadania {Badanie5=5, Badanie10=10, Badanie15=15, BadanieK=0};

        public TypBadania Wynik = TypBadania.Badanie5;
        
        public event EventHandler ButtonClicked;

        public WyborBadaniaControl()
        {
            InitializeComponent();
            Radio5.Checked += (sender, args) => { Wynik = TypBadania.Badanie5; };
            Radio10.Checked += (sender, args) => { Wynik = TypBadania.Badanie10; };
            Radio15.Checked += (sender, args) => { Wynik = TypBadania.Badanie15; };
            RadioK.Checked += (sender, args) => { Wynik = TypBadania.BadanieK; };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ButtonClicked != null) ButtonClicked(sender, e);
        }

    }
}
