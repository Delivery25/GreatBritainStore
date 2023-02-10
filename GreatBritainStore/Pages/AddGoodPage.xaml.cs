using GreatBritainStore.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GreatBritainStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddGoodPage.xaml
    /// </summary>
    public partial class AddGoodPage
    {
        //текущий товар
        private Product _currentGood = new Product();
        // путь к файлу
        private string _filePath;
        // название текущей главной фотографии
        private string _photoName;
        // текущая папка приложения
        private static string _currentDirectory = Directory.GetCurrentDirectory() + @"\";
        // передача в AddGoodPage товара
        public AddGoodPage(Product selectedGood)
        {
            InitializeComponent();
            // если передано null, то мы добавляем новый товар
            if (selectedGood != null)
            {
                _currentGood = selectedGood;
                TextBoxGoodId.Visibility = Visibility.Hidden;
                TextBlockGoodId.Visibility = Visibility.Hidden;
                var x = selectedGood.ID;
                // загрузка комплементарных товаров
                // List<Complect> additional = context.Complects.Where(p =>
                //p.MainGoodId == selectedGood.ID).ToList();
                var goods = new List<Product>();
                //foreach (Complect item in additional)
                //{
                //    goods.Add(item.Good1);
                //}
                //ListViewAdditional.ItemsSource = goods;
                _filePath = _currentDirectory + _currentGood.Image.Trim().Trim();
            }
            DataContext = _currentGood;
            _photoName = _currentGood.Image.Trim();
            //загрузка производителей
            using (var context = new EnglishStoreEntities())
            {
                ComboDeveloper.ItemsSource = context.Manufacturer.ToList();
            }
        }
        // проверка полей
        private StringBuilder CheckFields()
        {
            var message = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentGood.Name))
                message.AppendLine("Поле название пустое");

            if (_currentGood.Manufacturer == null)
                message.AppendLine("Выберите производителя");

            if (_currentGood.Price < 0)
                message.AppendLine("Цена не может быть отрицательной");

            if (!string.IsNullOrWhiteSpace(TextBoxHeight.Text))
            {
                if (!double.TryParse(TextBoxHeight.Text, out var x))
                    message.AppendLine("Высота только число");
                else if (x < 0)
                {
                    message.AppendLine("Высота не может быть отрицательной");
                }
            }

            if (!string.IsNullOrWhiteSpace(TextBoxLength.Text))
            {
                if (!double.TryParse(TextBoxLength.Text, out var x))
                    message.AppendLine("Длина только число");
                else if (x < 0)
                {
                    message.AppendLine("Длина не может быть отрицательной");
                }
            }

            if (!string.IsNullOrWhiteSpace(TextBoxWidth.Text))

            {
                if (!double.TryParse(TextBoxWidth.Text, out var x))
                    message.AppendLine("Ширина только число");
                else if (x < 0)

                {
                    message.AppendLine("Ширина не может быть отрицательной");

                }

            }
            if (!string.IsNullOrWhiteSpace(TextBoxWeight.Text))

            {
                if (!double.TryParse(TextBoxWeight.Text, out var x))
                    message.AppendLine("Вес только число");
                else if (x < 0)
                {
                    message.AppendLine("Вес не может быть отрицательным");
                }
            }

            if (string.IsNullOrWhiteSpace(_photoName))
                message.AppendLine("фото не выбрано пустое");

            return message;

        }
        // сохранение
        private void BtnSaveClick(object sender, RoutedEventArgs e)

        {
            var error = CheckFields();
            // если ошибки есть, то выводим ошибки в MessageBox
            // и прерываем выполнение 
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString());
                return;
            }

            // проверка полей прошла успешно
            using (var context = new EnglishStoreEntities())
            {
                if (_currentGood.ID == 0)
                {
                    // добавление нового товара
                    // формируем новое название файла картинки,
                    // так как в папке может быть файл с тем же именем
                    var photo = ChangePhotoName();
                    // путь куда нужно скопировать файл
                    var dest = _currentDirectory + photo;
                    File.Copy(_filePath, dest);
                    _currentGood.Image = photo;
                    // добавляем товар в БД
                    context.Product.Add(_currentGood);
                }
                try
                {
                    if (_filePath != null)
                    {
                        var photo = ChangePhotoName();
                        var dest = _currentDirectory + photo;
                        File.Copy(_filePath, dest);
                        _currentGood.Image = photo;
                    }
                    // Сохраняем изменения в БД
                    context.SaveChanges();
                    MessageBox.Show("Запись Изменена");
                    // Возвращаемся на предыдущую форму
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        // открытие этой же страницы
        // в качестве параметра передаем выделенный товар в комплементарных товарах
        //private void ListViewAdditionalPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (ListViewAdditional.SelectedItems.Count > 0)
        //    {
        //        Product good = (sender as ListView).SelectedItem as Product;
        //        Manager.MainFrame.Navigate(new AddGoodPage(good));
        //    }
        //}
        // открытие окна редактирования комплементарных товаров
        //private void BtnEditAdditionalClick(object sender, RoutedEventArgs e)
        //{
        //    if (_currentGood.ID != 0)
        //    {
        //        Manager.MainFrame.Navigate(new AdditionalGoodsPage(_currentGood));
        //    }
        //}
        // Событие визуализации страницы
        // после визуализации окна данные в listView подгружаются снова
        private void PageIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // загрузка комплементарных товаров
            //List<Complect> additional = context.Complects.Where(p => p.MainGoodId == _currentGood.ID).ToList();
            //List<Product> goods = new List<Product>();
            //foreach (Complect item in additional)
            //{
            //    goods.Add(item.Good1);
            //}
            //ListViewAdditional.ItemsSource = goods;
        }
        // загрузка фото
        private void BtnLoadClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //Диалог открытия файла
                var op = new OpenFileDialog
                {
                    Title = "Select a picture",
                    Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif) | *.gif"
                };

                // диалог вернет true, если файл был открыт
                if (op.ShowDialog() == true)
                {
                    // проверка размера файла
                    // по условию файл дожен быть не более 2Мб.
                    var fileInfo = new FileInfo(op.FileName);
                    if (fileInfo.Length > (1024 * 1024 * 2))
                    {
                        // размер файла меньше 2Мб. Поэтому выбрасывается новое исключение
                        throw new Exception("Размер файла должен быть меньше 2Мб");
                    }
                    ImagePhoto.Source = new BitmapImage(new Uri(op.FileName));
                    _photoName = op.SafeFileName;
                    _filePath = op.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                _filePath = null;
            }
        }
        //подбор имени файла
        string ChangePhotoName()
        {
            var x = _currentDirectory + _photoName;
            var photoName = _photoName;
            var i = 0;
            if (File.Exists(x))
            {
                while (File.Exists(x))
                {
                    i++;
                    x = _currentDirectory + i + photoName;
                }
                photoName = i + photoName;
            }
            return photoName;
        }
    }
}
