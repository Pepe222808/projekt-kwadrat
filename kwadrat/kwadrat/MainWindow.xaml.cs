using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace kwadrat
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        double podstawa_G;
        double podstawa_D;
        double srodek_ciezkosci = 0.5;


        private void txtBok_TextChanged(object sender, TextChangedEventArgs e)
        {
            double bok;
            if(double.TryParse(txtBok.Text, out bok) && bok >=0)
            { 
                txtPole.Text = Math.Pow(bok, 2.0).ToString();
                txtObwod.Text = (4*bok).ToString();
                lblKomunikat.Content = string.Empty; 
            }
            else
            {
                lblKomunikat.Content = "Wpisz liczbe dodatnia";
                wyczysc();
            }
            rectangle2.Points.Clear();
        }
        private void btnWyczysc_Click(object sender, RoutedEventArgs e)
        {
            wyczysc();
            lblKomunikat.Content = "Wpisz wymiar boku";
        }

        private void btnRysuj_Click(object sender, RoutedEventArgs e)
        {
            double bok;
            if(double.TryParse(txtBok.Text,out bok) && bok <=380)
            {
                rectangle2.Points = new PointCollection() { new Point(0, 0), new Point(bok, 0), new Point(bok, bok), new Point(0, bok) };
                SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFromString(cmbKolory.Text);
                rectangle2.Stroke = color;
                rectangle2.Fill = color;
                rectangle2.Opacity = (cbPrzezroczysty.IsChecked.Value) ? 0.5 : 1;
                rectangle2.Height = bok;
                rectangle2.Width = bok;
            }
            else
            {
                lblKomunikat.Content = "Brak danych lub zbyt duży bok";
                rectangle2.Points.Clear() ;
            }
        }

        private void rbPokaz_Checked(object sender, RoutedEventArgs e)
        {
            rectangle2.Visibility = Visibility.Visible;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            rectangle2.Visibility = Visibility.Hidden;
        }
        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            double bok = licz_bok();
            txtBok.Text = (bok - 1).ToString();
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            double bok = licz_bok();
            txtBok.Text = (bok + 1).ToString();
        }

        private void btnGora_Click(object sender, RoutedEventArgs e)
        {
            if (rectangle2.Points.Count != 0)
            {
                licz_podstawy();
                double bok = licz_bok();

                if (podstawa_G <= podstawa_D && podstawa_G > 1)
                {
                    zmien_podstawy("gora", 1);
                    srodek_ciezkosci = (podstawa_G + 2 * podstawa_D) / (3 * (podstawa_G + podstawa_D));
                }
                else if (podstawa_D < bok)
                {
                    zmien_podstawy("dol", 1);
                }
                nowe_dane();
            }
        }
        private void btnDol_Click(object sender, RoutedEventArgs e)
        {
            if (rectangle2.Points.Count != 0)
            {
                licz_podstawy();
                double bok = licz_bok();

                if (podstawa_D <= podstawa_G && podstawa_D > 1)
                {
                    zmien_podstawy("dol", -1); 
                    srodek_ciezkosci = (podstawa_D + 2 * podstawa_G) / (3 * (podstawa_G + podstawa_D));
                }
                else if (podstawa_G < bok)
                {
                    zmien_podstawy("gora", -1); 
                }
            }
            nowe_dane();
        }
        private void wyczysc()
        {
            txtBok.Text = String.Empty;
            txtPole.Text = String.Empty;
            txtObwod.Text = String.Empty;
        }

        private double licz_bok() 
        {
            double bok;
            double.TryParse(txtBok.Text, out bok);
            return bok;
        }


        private void licz_podstawy()
        {
            podstawa_G = rectangle2.Points[1].X - rectangle2.Points[0].X;
            podstawa_D = rectangle2.Points[2].X - rectangle2.Points[3].X;
            lblKomunikat.Content = podstawa_G.ToString() + podstawa_D.ToString();
        }

        private void zmien_podstawy(string podstawa, int i)
        {
            if (podstawa == "gora")
            {
                rectangle2.Points[0] = new Point((rectangle2.Points[0].X + i), rectangle2.Points[0].Y);
                rectangle2.Points[1] = new Point((rectangle2.Points[1].X + (i * -1)), rectangle2.Points[1].Y);
            }
            if (podstawa == "dol")
            {
                rectangle2.Points[2] = new Point((rectangle2.Points[2].X + i), rectangle2.Points[2].Y);
                rectangle2.Points[3] = new Point((rectangle2.Points[3].X + (i * -1)), rectangle2.Points[3].Y);
            }
        }

        private void nowe_dane()
        {
            double bok = licz_bok();
            txtPole.Text = ((podstawa_G + podstawa_D) * bok / 2).ToString();
            txtObwod.Text = ((podstawa_D + podstawa_G) + ramiona(bok)).ToString();
        }

        private double ramiona(double bok)
        {
            return Math.Sqrt(
                Math.Pow(Math.Abs((podstawa_G - podstawa_D)/2), 2) 
                + 
                Math.Pow(bok, 2)) * 2;
        }

        private void sldKat_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double bok = licz_bok();
            int kat = (int)sldKat.Value;
            txtKat.Text = kat.ToString();
            RotateTransform rotateTransform = new RotateTransform(kat);
            rectangle2.RenderTransform = rotateTransform;
            rectangle2.RenderTransformOrigin = new Point(0.5, srodek_ciezkosci);    
        }
        private void walidacja_liczb(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
