using System.IO;

namespace GreatBritainStore.Models
{
    public partial class Product
    {
        /// <summary>
        /// Возвращает абсолютный путь к изображению
        /// </summary>
        public string GetPhoto
        {
            get
            {
                if (Image is null)
                    return null;
                return Directory.GetCurrentDirectory() + @"\" + Image.Trim();
            }
        }
        /// <summary>
        /// Задает цвет фона товара
        /// </summary>
        public string GetColor
        {
            get
            {
                if (IsActive)
                    return "#fff";
                else
                    return "#8C8181";
            }
        }
        /// <summary>
        /// Текстовое представление активности товара
        /// </summary>
        public string GetStatus
        {
            get
            {
                if (IsActive)
                    return "";
                else
                    return "нет в наличии";
            }
        }
        /// <summary>
        /// Возвращает количество дополнительных товаров 
        /// </summary>
        //public string GetCount
        //{
        //    get
        //    {
        //        if (Complects.Count > 0)
        //            return $"({Complects.Count})";
        //        else
        //            return "";
        //    }
        //}
        public string GetInfo
        {
            get
            {
                return $"{Name} - {Price.ToString("c")} рублей.";
            }
        }

    }
}
