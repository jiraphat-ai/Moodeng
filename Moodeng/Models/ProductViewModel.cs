using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Moodeng.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกหมวดหมู่")]
        [Display(Name = "หมวดหมู่")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "กรุณาระบุชื่อสินค้า")]
        [StringLength(255)]
        [Display(Name = "ชื่อสินค้า")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "กรุณาระบุราคา")]
        [Range(0.01, double.MaxValue, ErrorMessage = "ราคาต้องมากกว่า 0")]
        [Display(Name = "ราคา")]
        public decimal Price { get; set; }

        [StringLength(300)]
        [Display(Name = "รายละเอียด")]
        public string Description { get; set; }

        public string Picture { get; set; }

        public string RetailId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
} 