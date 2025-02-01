using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace OutLines___Alpha
{
    /// <summary>
    /// Логика взаимодействия для AddMarshrutWindow.xaml
    /// </summary>
    public partial class AddMarshrutWindow : Window
    {
        public AddMarshrutWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            string название = null;
            string транспорт = null;
            int колПерекрестков = 0;
            DateTime? график = null;

            if (int.TryParse(ID.Text, out id) && id > 0)
            {
                название = Название.Text;
                транспорт = Транспорт.Text;

                if (int.TryParse(КолПерекрестков.Text, out колПерекрестков) && колПерекрестков >= 0)
                {
                    if (!string.IsNullOrEmpty(График.Text) && DateTime.TryParse(График.Text, out DateTime dateValue))
                    {
                        график = dateValue;

                        try
                        {
                            using (SqlConnection con = new SqlConnection(@"Server=HOME-PC;Database=Автошкола;Integrated Security=True"))
                            {
                                con.Open();

                                string query = "INSERT INTO Маршруты VALUES (@ID, @Название, @Транспорт, @КолПерекрестков, @График)";
                                using (SqlCommand cmd = new SqlCommand(query, con))
                                {
                                    cmd.Parameters.AddWithValue("@ID", id);
                                    cmd.Parameters.AddWithValue("@Название", название);
                                    cmd.Parameters.AddWithValue("@Транспорт", транспорт);
                                    cmd.Parameters.AddWithValue("@КолПерекрестков", колПерекрестков);
                                    cmd.Parameters.AddWithValue("@График", график);

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
                        MessageBox.Show("Неверный формат графика", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Неверный формат количества перекрестков", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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