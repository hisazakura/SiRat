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

namespace SiRat
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Popup : Window
    {
        public Popup()
        {
            InitializeComponent();
        }

        public Popup(string content)
        {
            InitializeComponent();
            ContentText.Text = content;
        }

        public Popup(string title, string content)
        {
            InitializeComponent();
            Title = title;
            ContentText.Text = content;
        }

        public Button AddButton(string text)
        {
            Button button = new() { Content = text, Margin = new Thickness(8, 0, 0, 0) };
            ButtonList.Children.Add(button);
            return button;
        }

        public DockPanel GetContentContainer()
        {
            return ContentContainer;
        }
    }
}
