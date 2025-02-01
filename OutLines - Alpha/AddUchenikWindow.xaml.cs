using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace OutLines___Alpha
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddUchenikWindow : Window
    {
        public AddUchenikWindow()
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
            string пол = null;
            string адрес = null;
            string город = null;
            string телефон = null;
            int кодвод = 0;

            if (int.TryParse(ID.Text, out id) && id > 0)
            {
                фамилия = Фамилия.Text;
                имя = Имя.Text;
                отчество = Отчество.Text;

                if (!string.IsNullOrEmpty(ГодРож.Text) && DateTime.TryParse(ГодРож.Text, out DateTime dateValue1)){
                    рождение = dateValue1;
                }

                if (!string.IsNullOrEmpty(ГодПос.Text) && DateTime.TryParse(ГодПос.Text, out DateTime dateValue2))
                {
                    поступление = dateValue2;
                }

                пол = Пол.Text;
                адрес = Адрес.Text;
                город = Город.Text;
                телефон = Телефон.Text;

                if (int.TryParse(КодВод.Text, out кодвод) && кодвод > 0)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(@"Server=HOME-PC;Database=Автошкола;Integrated Security=True"))
                        {
                            con.Open();

                            string query = "INSERT INTO Ученики VALUES (@ID, @Фамилия, @Имя, @Отчество, @ДатаРож, @ДатаПост, @Пол, @Адрес, @Город, @Телефон, @КодВод)";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@ID", id);
                                cmd.Parameters.AddWithValue("@Фамилия", фамилия);
                                cmd.Parameters.AddWithValue("@Имя", имя);
                                cmd.Parameters.AddWithValue("@Отчество", отчество);
                                cmd.Parameters.AddWithValue("@ДатаРож", рождение);
                                cmd.Parameters.AddWithValue("@ДатаПост", поступление);
                                cmd.Parameters.AddWithValue("@Пол", пол);
                                cmd.Parameters.AddWithValue("@Адрес", адрес);
                                cmd.Parameters.AddWithValue("@Город", город);
                                cmd.Parameters.AddWithValue("@Телефон", телефон);
                                cmd.Parameters.AddWithValue("@КодВод", кодвод);

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
                    MessageBox.Show("Неверный формат кода водителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
