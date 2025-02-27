using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;

namespace StudentsPayment
{
    public partial class RoomsPage : Page
    {
        private databaseEntities _context;

        public RoomsPage()
        {
            InitializeComponent();
            _context = new databaseEntities();
            LoadData();
        }

        private void LoadData()
        {
            _context.Rooms.Load();
            RoomsDataGrid.ItemsSource = _context.Rooms.Local;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditWindow = new AddEditRoomWindow();
            if (addEditWindow.ShowDialog() == true)
            {
                _context.Rooms.Add(addEditWindow.Room);
                _context.SaveChanges();
                LoadData();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = RoomsDataGrid.SelectedItem as Rooms;
            if (selectedRoom != null)
            {
                var addEditWindow = new AddEditRoomWindow(selectedRoom);
                if (addEditWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = RoomsDataGrid.SelectedItem as Rooms;
            if (selectedRoom != null)
            {
                _context.Rooms.Remove(selectedRoom);
                _context.SaveChanges();
                LoadData();
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}