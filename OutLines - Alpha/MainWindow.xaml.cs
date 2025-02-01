using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace OutLines___Alpha
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        string tableName;
        private string connectionString = "Data Source=HOME-PC;Initial Catalog=Автошкола;Integrated Security=True";
        public MainWindow()
        {
            InitializeComponent();
            dgData.IsReadOnly = true;
        }

    private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            switch (this.WindowState) {
                case WindowState.Normal:
                    this.WindowState = WindowState.Maximized;
                    break; 
                case WindowState.Maximized:
                    this.WindowState= WindowState.Normal;
                    break;
            }
        }
        void Login_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            box.Text = " ";
            box.Foreground = Brushes.Black;
            box.GotFocus -= Login_GotFocus;

        }
        void Login_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box.Text.Trim().Equals(string.Empty))
            {
                box.Text = " Логин";
                box.Foreground = Brushes.LightGray;
                box.GotFocus += Login_GotFocus;
            }
        }
        void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            box.Text = " ";
            box.Foreground = Brushes.Black;
            box.GotFocus -= Password_GotFocus;
        }
        void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box.Text.Trim().Equals(string.Empty))
            {
                box.Text = " Пароль";
                box.Foreground = Brushes.LightGray;
                box.GotFocus += Password_GotFocus;
            }
        }
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(@"Server=HOME-PC;Database=Автошкола;Integrated Security=True");
                con.Open();
                string get_data = "SELECT * FROM Вход WHERE Логин = @username AND Пароль = @password";
                SqlCommand cmd = new SqlCommand(get_data, con);

                cmd.Parameters.AddWithValue("@username", Login.Text.Trim());
                cmd.Parameters.AddWithValue("@password", Password.Text.Trim());
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

                if (count > 0) {
                    LoginWindow.Visibility = Visibility.Hidden;
                    OutLinesWindow.Visibility = Visibility.Visible;
                }
            }
            catch
            {
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box.Text.Trim().Equals(string.Empty))
            {
                box.Text = "     Поиск...";
                box.Foreground = Brushes.LightGray;
                box.GotFocus += SearchTextBox_GotFocus;
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            box.Text = "     ";
            box.Foreground = Brushes.Black;
            box.GotFocus -= SearchTextBox_GotFocus;
        }
        private void rb_Checked(object sender, RoutedEventArgs e)
        {
            dgData.ItemsSource = null;
            dgData.Columns.Clear();
            RadioButton rb = (RadioButton)sender;
            if (rb.Name == "rbOption1")
            {
                tableName = "Автотранспорт";
                LoadDataGrid($"SELECT * FROM {tableName}");
            }
            else if (rb.Name == "rbOption2")
            {
                tableName = "Инструкторы";
                LoadDataGrid($"SELECT * FROM {tableName}");
            }
            else if (rb.Name == "rbOption3")
            {
                tableName = "Маршруты";
                LoadDataGrid($"SELECT * FROM {tableName}");
            }
            else if (rb.Name == "rbOption4")
            {
                tableName = "Ученики";
                LoadDataGrid($"SELECT * FROM {tableName}");
            }
            errorlabel.Content = "";
        }
        private void LoadDataGrid(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        dgData.ItemsSource = dataTable.DefaultView;

                        foreach (DataColumn column in dataTable.Columns)
                        {
                            DataGridTextColumn textColumn = new DataGridTextColumn();
                            textColumn.Header = column.ColumnName;
                            textColumn.Binding = new Binding(column.ColumnName);
                            dgData.Columns.Add(textColumn);
                        }
                    }
                }
            }
            errorlabel.Content = "";
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dgData.SelectedItem;
            if (selectedRow != null)
            {
                string id = selectedRow["Id"].ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        string query = $"DELETE FROM {tableName} WHERE ID = @Id";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            //command.Parameters.AddWithValue("@table", tableName);
                            command.Parameters.AddWithValue("@Id", id);
                            command.ExecuteNonQuery();
                            errorlabel.Content = "";
                        }
                    }
                    catch
                    {
                        errorlabel.Content = "Ошибка удаления! Возможно удаляете данные с зависимостями";
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tableName == "Ученики")
            {
                AddUchenikWindow addWindow = new AddUchenikWindow();
                addWindow.Show();
            } else if(tableName == "Инструкторы") {
                AddInstructorWindow addWindow = new AddInstructorWindow();
                addWindow.Show();
            }
            else if (tableName == "Маршруты")
            {
                AddMarshrutWindow addWindow = new AddMarshrutWindow();
                addWindow.Show();
            }
            else if (tableName == "Автотранспорт")
            {
                AddCarWindow addWindow = new AddCarWindow();
                addWindow.Show();
            }
        }


    }
}
