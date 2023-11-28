using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace firsttime.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }


        public string Name { get; set; }
        public string Description { get; set; }

        public int Price { get; set; }

        [ForeignKey("Category")]
        public int? CategoryID { get; set; }

        // Навигационное свойство
        public Categories Category { get; set; }

        public Product() { }
        public Product(string name, string description, int price)
        {
            Name = name;
            Description = description;
            Price = price;
        }


    }
}
