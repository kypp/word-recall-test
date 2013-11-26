using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Xml.XPath;

using Controls;

namespace BadanieWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum Phase
        {
            Poczatek,
            Wstep,
            Zadanie1_Opis,
            Zadanie1_ListaSlow,
            Zadanie1_Gra,
            Przerwa,
            Zadanie2_Opis,
            Zadanie2_ListaSlow,
            Zadanie2_Gra,
            Zadanie3_Opis,
            Zadanie3_Gra,
            Zadanie4_Opis,
            Koniec
        }
        private Phase CurrentPhase;
        private int IleSlow;
        private int ktoreSlowo;
        
        private DispatcherTimer timer;
        private TimeSpan countdown;

        private Evaluation evaluation;

        public void StartTimer(TimeSpan timeSpan, Action onFinished )
        {
            timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 1)};
            countdown = timeSpan;
            timer.Tick += (sender, args) =>
                {
                    var tc = CurrentControl as IDowncountable;
                    if (countdown.TotalSeconds > 0)
                    {
                        countdown -= TimeSpan.FromSeconds(1);
                        tc.Countdown = countdown;
                    }
                    else
                    {
                        tc.Countdown = TimeSpan.Zero;
                        timer.Stop();
                        onFinished();
                    }

                };
            timer.Start();
        }

        private void MakeWordList(IEnumerable<string[]> lista, int iloscslow)
        {
            int maxlen = lista.Take(iloscslow)
                              .Select((s) => s[0].Length)
                              .Max();

            string pary =
                lista.Take(iloscslow)
                      .Select((s) => s[0] + (new string(' ', maxlen - s[0].Length+5)) + "-\t" + s[1])
                      .Aggregate((s1, s2) => s1 + "\n" + s2);
            var tc = new TekstComponent(pary, "Dalej");
            Fade(tc);
            tc.ButtonClicked += NextPhase;
        }

        private void StartGame(IEnumerable<string[]> lista, int iloscslow)
        {
            Random rand = new Random();
            currentSlowa = lista
                .Take(iloscslow)
                .OrderBy((x) => rand.Next())
                .ToList();
            ktoreSlowo = 0;
            var tc = new TekstComponent(currentSlowa[0][0], "Dalej", currentSlowa[0][1]);
            Fade(tc);
            tc.ButtonClicked += NextPhase;
        }

        private void PlayGame(Phase currPhase, Action onFinished)
        {
            var tc = (CurrentControl as TekstComponent);
            if (!tc.Validate())
            {
                CurrentPhase = currPhase;
                NextPhase(null, null);
                return;
            }
            if (ktoreSlowo == IleSlow - 1)
            {
                onFinished();
            }
            else
            {
                ktoreSlowo++;
                var tc2 = new TekstComponent(currentSlowa[ktoreSlowo][0], "Dalej", currentSlowa[ktoreSlowo][1]);
                Fade(tc2);
                tc2.ButtonClicked += NextPhase;
            }
        }


        private void NextPhase(object o, EventArgs e)
        {
            switch (CurrentPhase)
            {
                case Phase.Poczatek:
                    {
                        evaluation = new Evaluation {typBadania = (CurrentControl as WyborBadaniaControl).Wynik};
                        var tc = MakeTekst("Wstep", null);
                        Fade(tc);
                        CurrentPhase = Phase.Wstep;
                        tc.ButtonClicked += NextPhase;
                    }
                    break;
                case Phase.Wstep:
                    {
                        TekstComponent tc;
                        switch (evaluation.typBadania)
                        {
                            case WyborBadaniaControl.TypBadania.Badanie5:
                                CurrentPhase = Phase.Zadanie1_Opis;
                                IleSlow = 5;
                                break;
                            case WyborBadaniaControl.TypBadania.Badanie10:
                                CurrentPhase = Phase.Zadanie1_Opis;
                                IleSlow = 10;
                                break;
                            case WyborBadaniaControl.TypBadania.Badanie15:
                                CurrentPhase = Phase.Zadanie1_Opis;
                                IleSlow = 15;
                                break;
                            case WyborBadaniaControl.TypBadania.BadanieK:
                                CurrentPhase = Phase.Zadanie2_Opis;
                                IleSlow = 15;
                                break;
                        }
                        Fade(tc = MakeTekst("Zadanie1", IleSlow));
                        tc.ButtonClicked += NextPhase;
                    }
                    break;
                case Phase.Zadanie1_Opis:
                    {
                        MakeWordList(slowa1, IleSlow);
                        CurrentPhase = Phase.Zadanie1_ListaSlow;
                    }
                    break;
                case Phase.Zadanie1_ListaSlow:
                    {
                        StartGame(slowa1, IleSlow);
                        CurrentPhase = Phase.Zadanie1_Gra;
                    }
                    break;
                case Phase.Zadanie1_Gra:
                    {
                        PlayGame(Phase.Zadanie1_Opis, () =>
                            {
                                var tc2 = MakeTekst("Przerwa", null);
                                Fade(tc2);
                                CurrentPhase = Phase.Przerwa;
                                StartTimer(new TimeSpan(0, 2, 0), () => NextPhase(o, e));
                            });
                    }
                    break;
                case Phase.Przerwa:
                    {
                        TekstComponent tc;
                        IleSlow = 15;
                        Fade(tc = MakeTekst("Zadanie2", 15));
                        tc.ButtonClicked += NextPhase;
                        CurrentPhase = Phase.Zadanie2_Opis;
                    }
                    break;
                case Phase.Zadanie2_Opis:
                    {
                        MakeWordList(slowa2, 15);
                        evaluation.StartTimeEval();
                        CurrentPhase = Phase.Zadanie2_ListaSlow;
                    }
                    break;
                case Phase.Zadanie2_ListaSlow:
                    {
                        StartGame(slowa2, 15);
                        CurrentPhase = Phase.Zadanie2_Gra;
                    }
                    break;
                case Phase.Zadanie2_Gra:
                    {
                        PlayGame(Phase.Zadanie2_Opis, () =>
                            {
                                var tc = MakeTekst("Zadanie3", null);
                                Fade(tc);
                                tc.ButtonClicked += NextPhase;
                                CurrentPhase = Phase.Zadanie3_Opis;
                                evaluation.EndTimeEval();
                            });
                    }
                    break;
                case Phase.Zadanie3_Opis:
                    {
                        StartTimer(new TimeSpan(0,2,0), () =>
                            {
                                var tc = MakeTekst("Zadanie4", evaluation.typBadania == WyborBadaniaControl.TypBadania.BadanieK?1:2);
                                Fade(tc);
                                CurrentPhase = Phase.Zadanie4_Opis;
                                tc.ButtonClicked += NextPhase;
                            });
                        CurrentPhase = Phase.Zadanie3_Gra;
                        NextPhase(o, e);
                    }
                    break;
                case Phase.Zadanie3_Gra:
                    {
                        Random rand = new Random();
                        int min = int.Parse(xnav.SelectSingleNode("TekstyBadania/Zadanie3/Min/text()").Value);
                        int max = int.Parse(xnav.SelectSingleNode("TekstyBadania/Zadanie3/Max/text()").Value);

                        string[] sgns = {"-", "+", "*"};

                        string query = String.Format("{0} \t {1} \t {2} \t =", rand.Next(min, max), sgns[rand.Next(sgns.Length)], rand.Next(min, max));

                        var tc = new TekstComponent(query, "Dalej", "0");
                        Fade(tc);
                        tc.ButtonClicked += NextPhase;
                    }
                    break;
                case Phase.Zadanie4_Opis:
                    {
                        var rand = new Random();
                        var tc = new KoncowyTest(slowa2.OrderBy((st) => rand.Next()).ToList());
                        StartTimer(new TimeSpan(0, 2, 0), () =>
                        {
                            var tkon = MakeTekst("Koniec", null);
                            Fade(tkon);
                            evaluation.CorrectFraction = tc.GetPercentage();
                            evaluation.SaveToFile("raport");
                            CurrentPhase = Phase.Koniec;
                        });
                        Fade(tc);
                    }
                    break;
                case Phase.Koniec:
                    break;
            }
        }


        public bool IsMaximized { get; private set; }

        private UIElement CurrentControl;
        private XPathDocument xdoc ;
        private XPathNavigator xnav;

        public List<String[]> slowa1, slowa2, currentSlowa;

        private string GetText(string xpath)
        {
            return xnav.SelectSingleNode("/TekstyBadania/" + xpath + "/text()").Value;
        }

        private TekstComponent MakeTekst(string nazwa, int? replacement)
        {
            var t1 = GetText("/" + nazwa + "/Tekst");
            var t2 = GetText("/" + nazwa + "/Button");
            if (replacement != null)
                t1 = t1.Replace("$", replacement.ToString());
            return new TekstComponent(t1, t2);
        }

        private void Fade(UIElement FadeToControl)
        {
            CurrentControl.IsEnabled = false;
            // Create a storyboard to contain the animations.
            var storyboard = new Storyboard();
            var storyboard2 = new Storyboard();

            // Create a DoubleAnimation to fade the not selected option control
            var animationIn = new DoubleAnimation();
            var animationOut = new DoubleAnimation();

            animationIn.From = 1.0;
            animationIn.To = 0.0;
            animationOut.From = 0.0;
            animationOut.To = 1.0;
            animationIn.Duration =  new Duration(TimeSpan.FromMilliseconds(500));
            animationOut.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            
            animationIn.Completed += (x, y) =>
                {
                    CurrentControl.IsEnabled = true;
                    MainGrid.Children.Remove(CurrentControl);
                    MainGrid.Children.Add(FadeToControl);
                    //CurrentControl.Visibility = Visibility.Hidden;
                    FadeToControl.Visibility = Visibility.Visible;
                    storyboard2.Begin(this);
                };
            // Configure the animation to target de property Opacity
            Storyboard.SetTarget(animationIn, CurrentControl);
            Storyboard.SetTargetProperty(animationIn, new PropertyPath(Control.OpacityProperty));
            Storyboard.SetTarget(animationOut, FadeToControl);
            Storyboard.SetTargetProperty(animationOut, new PropertyPath(Control.OpacityProperty));

            // Add the animation to the storyboard
            storyboard.Children.Add(animationIn);
            storyboard2.Children.Add(animationOut);

            // Begin the storyboard
            storyboard.Begin(this);
            CurrentControl = FadeToControl;

            FadeToControl.Focus();
            /*
            if (FadeToControl is TekstComponent)
            {
                var tc = FadeToControl as TekstComponent;
                tc.InputBox.Focusable = true;
                FocusManager.SetFocusedElement(this, tc.InputBox);
                Keyboard.Focus(InputBox);
            }*/
        }

        private void WczytajSlowa()
        {
            var s1 = GetText("Slowa/Lista1")
                .Replace(" ", "")
                .Replace("\t", "")
                .Split("\n".ToArray(), StringSplitOptions.RemoveEmptyEntries)
                .Where((x) => x.Length > 5);

            var s2 = GetText("Slowa/Lista2")
                .Replace(" ", "")
                .Replace("\t", "")
                .Split("\n".ToArray(), StringSplitOptions.RemoveEmptyEntries)
                .Where((x) => x.Length > 5);

            foreach (var s in s1)
                slowa1.Add(s.Replace("\r","").Split('-'));
            foreach (var s in s2)
                slowa2.Add(s.Replace("\r","").Split('-'));
        }

        public MainWindow()
        {
            xdoc = new XPathDocument("Teksty.xml");
            xnav = xdoc.CreateNavigator();
            InitializeComponent();
            slowa1 = new List<string[]>();
            slowa2 = new List<string[]>();
            WczytajSlowa();
            CurrentPhase = Phase.Poczatek;
            var wbc = new WyborBadaniaControl();
            CurrentControl = wbc;
            MainGrid.Children.Add(wbc);
            wbc.ButtonClicked += NextPhase; /* (sender, args) =>
                {
                    
                };*/


        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                if (!IsMaximized)
                {
                    WindowState = WindowState.Maximized;
                    WindowStyle = WindowStyle.None;
                }
                else
                {
                    WindowState = WindowState.Normal;
                    WindowStyle = WindowStyle.ThreeDBorderWindow;
                }
                IsMaximized = !IsMaximized;
            }
        }



    }
}
