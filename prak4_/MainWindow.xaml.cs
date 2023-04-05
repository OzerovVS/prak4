using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using static System.Net.WebRequestMethods;

namespace prak4_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int count = 0;
        public static string text = "";
        private static List<User> us = new List<User>();
        private static List<User> uses = new List<User>();
        public static List<string> naz = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            us = Jsonka.Des<List<User>>("us.json") ?? new List<User>();
            count = Jsonka.Read();
            cena.Content = count.ToString();
            if (us.Count != 0)
            {
                Use.naz = Jsonka.Des<List<string>>("naz.json") ?? new List<string>();
                cb2.ItemsSource = Use.naz;
                Vib((DateTime)datka.SelectedDate);
                dg.ItemsSource = uses;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(tb3.Text) > 0)
                {
                    us.Add(new User(datka.SelectedDate, tb1.Text.ToString(), cb2.SelectedItem.ToString(), Convert.ToInt32(tb3.Text), true));
                    uses.Add(new User(datka.SelectedDate, tb1.Text.ToString(), cb2.SelectedItem.ToString(), Convert.ToInt32(tb3.Text), true));
                }
                else
                {
                    us.Add(new User(datka.SelectedDate, tb1.Text.ToString(), cb2.SelectedItem.ToString(), Convert.ToInt32(tb3.Text), false));
                    uses.Add(new User(datka.SelectedDate, tb1.Text.ToString(), cb2.SelectedItem.ToString(), Convert.ToInt32(tb3.Text), false));
                }
                count += Convert.ToInt32(tb3.Text);
                dg.ItemsSource = null;
                dg.ItemsSource = uses;
                cena.Content = count.ToString();
                Jsonka.Ser("us.json", us);
                Jsonka.Write(count);
            }
            catch
            {
                MessageBox.Show("Произошла ошибка ввода");
            }
        }

        void Vib(DateTime time)
        {
            uses.Clear();
            foreach (User user in us)
            {
                if (time == user.data)
                    uses.Add(user);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new dialog().Show();
            Close();
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((dg.SelectedItem) != null)
            {
                tb1.Text = ((User)dg.SelectedItem).Name.ToString();
                cb2.SelectedItem= ((User)dg.SelectedItem).Type;
                tb3.Text = ((User)dg.SelectedItem).money.ToString();
            }
        }

        private void datka_CalendarClosed(object sender, RoutedEventArgs e)
        {
            dg.ItemsSource = null;
            tb1.Text = null;
            cb2.SelectedIndex = -1;
            tb3.Text = null;
            Vib((DateTime)datka.SelectedDate);
            dg.ItemsSource = uses;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            count -= Convert.ToInt32(((User)dg.SelectedItem).money);
            uses.Remove((User)dg.SelectedItem);
            us.Remove((User)dg.SelectedItem);
            Jsonka.Ser("us.json", us);
            Jsonka.Write(count);
            dg.ItemsSource = null;
            dg.ItemsSource = uses;
            cena.Content = count.ToString();

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            int j = 0;
            foreach (User s in us)
            {
                if (s == dg.SelectedItem)
                {
                    j = Convert.ToInt32(((User)dg.SelectedItem).money);
                    foreach (User w in uses)
                    {
                        if (w == s)
                        {
                            w.Name = tb1.Text.ToString();
                            w.Type = cb2.SelectedItem.ToString();
                            w.money = Convert.ToInt32(tb3.Text);
                            break;
                        }
                    }
                    s.Name = tb1.Text.ToString();
                    s.Type = cb2.SelectedItem.ToString();
                    s.money = Convert.ToInt32(tb3.Text);
                    break;
                }
            }
            count += -(j - Convert.ToInt32(tb3.Text));
            Jsonka.Ser("us.json", us);
            Jsonka.Write(count);
            dg.ItemsSource = null;
            dg.ItemsSource = uses;
            cena.Content = count.ToString();
        }
    }
    class User
    {
        public string Name { get; set; }
        public string Type { get; set; }
        private int Money;
        public int money
        {
            get { return Money; }
            set
            {
                Money = Math.Abs(value);
            }
        }
        public bool isincome { get; set; }
        public DateTime? data;

        public User(DateTime? selectedDate, string text1, string v, int text2, bool isin)
        {
            data = selectedDate;
            Name = text1;
            Type = v;
            money = text2;
            isincome = isin;
        }
    }
    class Use
    {
        public static List<string> naz = new List<string>();
    }
    class Jsonka
    {
        public static void Ser<T>(string path, T ato)
        {
            System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(ato));
        }
        public static T Des<T>(string path)
        {
            if (!System.IO.File.Exists(path))
                System.IO.File.Create(path);
            return JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(path));
        }
        public static void Write(int a)
        {
            System.IO.File.WriteAllText("cena.json", $"let a = {a}");
        }
        public static int Read()
        {
            string s = System.IO.File.ReadAllText("cena.json");
            if (s == null || s == "")
                return 0;
            string h = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '=' && s[i + 1] == ' ')
                {
                    for (int j = i + 2; j < s.Length; j++)
                    {
                        h += s[j];
                    }
                    break;
                }
            }
            return Convert.ToInt32(h);
        }
    }
}
