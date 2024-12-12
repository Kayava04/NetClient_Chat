using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetClient_Chat.Controls
{
    public partial class Message : UserControl
    {
        public string Text
        {
            get => messageText.Text;
            set
            {
                messageText.Text = value;
            }
        }

        private bool _isMyMessage;
        public bool IsMyMessage
        {
            get => _isMyMessage;
            set
            {
                if (value)
                {
                    messageBox.Background = rectanglePosition.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    messageBox.HorizontalAlignment = rectanglePosition.HorizontalAlignment = HorizontalAlignment.Right;
                    messageBox.Margin = new Thickness(0,4,10,0);
                    rectanglePosition.Margin = new Thickness(0,4,10.15,0);
                }
                else
                {
                    messageBox.Background = rectanglePosition.Fill = new SolidColorBrush(Color.FromArgb(255,0,255,0));
                    messageBox.HorizontalAlignment = rectanglePosition.HorizontalAlignment = HorizontalAlignment.Left;
                    messageBox.Margin = new Thickness(10,4,0,0);
                    rectanglePosition.Margin = new Thickness(10.15,4,0,0);
                }
                
                _isMyMessage = value;
            }
        }

        public Message()
        {
            InitializeComponent();
        }

        public Message(string username, string text, bool isMine = true)
        {
            InitializeComponent();

            Username.Text = username;
            Text = text;
            IsMyMessage = isMine;
        }
    }
}