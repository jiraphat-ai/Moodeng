﻿namespace Moodeng.Controllers
{
    internal class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string Picture { get; set; }
    }
}