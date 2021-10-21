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

namespace PolygonCircleEditor
{
    /// <summary>
    /// Interaction logic for PickSizeDialog.xaml
    /// </summary>
    public partial class PickSizeDialog : Window
    {
        public uint Size { get; private set; }

        public PickSizeDialog(uint size)
        {
            Size = size;
            InitializeComponent();
            SizeTextBox.Text = Size.ToString();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Size = uint.Parse(SizeTextBox.Text);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e) =>
            DialogResult = false;
    }
}
