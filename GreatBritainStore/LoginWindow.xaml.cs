// для работы с моделями вашей БД нужно добавить путь вида
// НазваниеВашегоПриложения.Models
using GreatBritainStore.Models;
using System;
using System.Linq;
using System.Windows;

namespace GreatBritainStore
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            try
            { //загрузка всех пользователей из БД в список
                using (var context = new EnglishStoreEntities())
                {
                    var users = context.User.ToList();
                    //попытка найти пользователя с указанным паролем и логином
                    //если такого пользователя не будет обнаружено то переменная u будет равна null
                    var user = users.FirstOrDefault(p => p.Login == TbLogin.Text && p.Password == TbPass.Password);

                    if (user != null)
                    {
                        // логин и пароль корректные запускаем главную форму приложения
                        var mainWindow = new MainWindow
                        {
                            Owner = this
                        };
                        Hide();
                        mainWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //код кнопки Cancel
        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        // попытка закрыть приложение
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // на экране отображается форма с двумя кнопками
            var result = MessageBox.Show("Вы действительно хотите закрыть приложение?",
            "Выйти", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Cancel)
                e.Cancel = true;
        }
    }
}
