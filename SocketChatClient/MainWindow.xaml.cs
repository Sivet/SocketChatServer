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

namespace SocketChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConnectionController connection = new ConnectionController("127.0.0.1", 11000);
        public MainWindow()
        {
            InitializeComponent();
            
            connection.Conect();
            connection.AddCompletedEvent += GetFromServer;
            connection.StartRecieveFromServerThread();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            WriteToTextblock("To: " + textBox.Text);
            connection.sendToServer(textBox.Text);
            textBox.Clear();
            textBox.Focus();
        }
        private void GetFromServer(string message)
        {
            WriteToTextblock(message);
        }
        public void WriteToTextblock(string message)
        {
            try
            {
                this.Dispatcher.Invoke(new Action(() => { textBlock.Text += message + "\n"; }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("A handled exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }
        
        
    }
}
