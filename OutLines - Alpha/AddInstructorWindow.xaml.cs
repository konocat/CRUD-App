using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace OutLines___Alpha
{
    /// <summary>
    /// Логика взаимодействия для AddInstructorWindow.xaml
    /// </summary>
    public partial class AddInstructorWindow : Window
    {
        public AddInstructorWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            string фамилия = null;
            string имя = null;
            string отчество = null;
            DateTime? рождение = null;
            DateTime? поступление = null;
            int стаж = 0;
            string пол = null;
            string адрес = null;
            string телефон = null;

            if (int.TryParse(ID.Text, out id) && id > 0)
            {
                фамилия = Фамилия.Text;
                имя = Имя.Text;
                отчество = Отчество.Text;

                if (!string.IsNullOrEmpty(Рождение.Text) && DateTime.TryParse(Рождение.Text, out DateTime dateValue1))
                {
                    рождение = dateValue1;
                }
                else
                {
                    MessageBox.Show("Неверный формат даты рождения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!string.IsNullOrEmpty(Поступление.Text) && DateTime.TryParse(Поступление.Text, out DateTime dateValue2))
                {
                    поступление = dateValue2;
                }
                else
                {
                    MessageBox.Show("Неверный формат даты поступления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (int.TryParse(Стаж.Text, out стаж) && стаж >= 0)
                {
                    пол = Пол.Text;
                    адрес = Адрес.Text;
                    телефон = Телефон.Text;

                    try
                    {
                        using (SqlConnection con = new SqlConnection(@"Server=HOME-PC;Database=Автошкола;Integrated Security=True"))
                        {
                            con.Open();

                            string query = "INSERT INTO Инструкторы VALUES (@ID, @Фамилия, @Имя, @Отчество, @Рождение, @Поступление, @Стаж, @Пол, @Адрес, @Телефон)";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@ID", id);
                                cmd.Parameters.AddWithValue("@Фамилия", фамилия);
                                cmd.Parameters.AddWithValue("@Имя", имя);
                                cmd.Parameters.AddWithValue("@Отчество", отчество);
                                cmd.Parameters.AddWithValue("@Рождение", рождение);
                                cmd.Parameters.AddWithValue("@Поступление", поступление);
                                cmd.Parameters.AddWithValue("@Стаж", стаж);
                                cmd.Parameters.AddWithValue("@Пол", пол);
                                cmd.Parameters.AddWithValue("@Адрес", адрес);
                                cmd.Parameters.AddWithValue("@Телефон", телефон);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Неверный формат стажа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Неверный формат ID", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}