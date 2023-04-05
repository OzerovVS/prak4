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
using System.Windows.Shapes;

namespace prak4_
{
    /// <summary>
    /// Логика взаимодействия для dialog.xaml
    /// </summary>
    public partial class dialog : Window
    {
        public dialog()
        {
            InitializeComponent();
            tb.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tb.Text == "")
            {
                MessageBox.Show("Поле не заполнено");
                return;
            }
            if (Use.naz.Contains(tb.Text.ToString()))
            {
                MessageBox.Show("Такой тип уже есть");
                return;
            }
            Use.naz.Add(tb.Text);
            Jsonka.Ser("naz.json", Use.naz);
            Hide();
            new MainWindow().Show();
        }
    }
}
