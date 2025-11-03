using System;
using System.Collections.Generic;

namespace Dishapi.DAL.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}