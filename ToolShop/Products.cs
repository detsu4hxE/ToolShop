//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToolShop
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            this.OrderProducts = new HashSet<OrderProducts>();
        }

        public string correctImage
        {
            get
            {
                string path = (Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + $"/Resources/");
                if (String.IsNullOrEmpty(Image) || String.IsNullOrWhiteSpace(Image) || Image == null)
                {
                    return path + "default_picture.png";
                }
                else
                {
                    return path + Image;
                }
            }
        }

        public string productTypeName
        {
            get
            {
                return App.Context.ProductTypes.Where(pt => pt.ID == ProductTypeID).First().Title;
            }
        }
        
        public string shortDescription
        {
            get
            {
                if (Description.Length > 25)
                {
                    return (Description.Substring(0, 25) + "...").ToString();
                }
                return Description;
            }
        }
        public string infoOrEditButtonContent
        {
            get
            {
                if (App.CurrentUser == null)
                {
                    return "Подробнее";
                }
                if (App.CurrentUser.RoleID == 1)
                {
                    return "Изменить";
                }
                return "Подробнее";
            }
        }
        public string addOrDeleteButtonContent
        {
            get
            {
                if (App.CurrentUser == null)
                {
                    return "Добавить в корзину";
                }
                if (App.CurrentUser.RoleID == 1)
                {
                    return "Удалить";
                }
                return "Добавить в корзину";
            }
        }
        public string addOrDeleteButtonVisibility
        {
            get
            {
                if (App.CurrentUser != null)
                {
                    return "Visible";
                }
                else
                {
                    return "Collapsed";
                }
            }
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProductTypeID { get; set; }
        public decimal Price { get; set; }
        public int AmountInStock { get; set; }
        public int ManufacturerID { get; set; }
        public int SupplierID { get; set; }
        public string Image { get; set; }
    
        public virtual Manufacturers Manufacturers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProducts> OrderProducts { get; set; }
        public virtual ProductTypes ProductTypes { get; set; }
        public virtual Suppliers Suppliers { get; set; }
    }
}
