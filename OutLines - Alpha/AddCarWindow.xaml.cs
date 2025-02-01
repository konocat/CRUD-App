using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace OutLines___Alpha
{
    /// <summary>
    /// Логика взаимодействия для AddCarWindow.xaml
    /// </summary>
    public partial class AddCarWindow : Window
    {
        public AddCarWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string id = ID.Text;
            string название = null;
            int наработка = 0;
            int пробег = 0;
            int колРемонтов = 0;
            string характеристика = null;
            int кодвод = 0;

            название = Название.Text;

            if (int.TryParse(Наработка.Text, out наработка) && наработка >= 0)
            {
                if (int.TryParse(Пробег.Text, out пробег) && пробег >= 0)
                {
                    if (int.TryParse(КолРемонтов.Text, out колРемонтов) && колРемонтов >= 0)
                    {
                        характеристика = Характеристика.Text;

                        if (int.TryParse(КодВод.Text, out кодвод) && кодвод > 0)
                        {
                            try
                            {
                                using (SqlConnection con = new SqlConnection(@"Server=HOME-PC;Database=Автошкола;Integrated Security=True"))
                                {
                                    con.Open();

                                    string query = "INSERT INTO Автотранспорт VALUES (@ID, @Название, @Наработка, @Пробег, @КолРемонтов, @Характеристика, @КодВод)";
                                    using (SqlCommand cmd = new SqlCommand(query, con))
                                    {
                                        cmd.Parameters.AddWithValue("@ID", id);
                                        cmd.Parameters.AddWithValue("@Название", название);
                                        cmd.Parameters.AddWithValue("@Наработка", наработка);
                                        cmd.Parameters.AddWithValue("@Пробег", пробег);
                                        cmd.Parameters.AddWithValue("@КолРемонтов", колРемонтов);
                                        cmd.Parameters.AddWithValue("@Характеристика", характеристика);
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
                        MessageBox.Show("Неверный формат количества ремонтов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Неверный формат пробега", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Неверный формат наработки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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