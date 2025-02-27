using System.Windows;

namespace StudentsPayment
{
    public partial class AddEditRoomWindow : Window
    {
        public Rooms Room { get; private set; }

        public AddEditRoomWindow(Rooms room = null)
        {
            InitializeComponent();
            Room = room ?? new Rooms();
            DataContext = Room;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}