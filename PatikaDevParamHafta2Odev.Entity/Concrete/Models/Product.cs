using PatikaDevParamHafta2Odev.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatikaDevParamHafta2Odev.Entity.Concrete.Models
{
    public class Product : IEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 60, MinimumLength = 10, ErrorMessage = "Invalid name lenght!")]
        public string Name { get; set; }

        [StringLength(maximumLength: 100, MinimumLength = 10, ErrorMessage = "Invalid description lenght!")]
        public string Description { get; set; }

        [StringLength(maximumLength: 30, MinimumLength = 5, ErrorMessage = "Invalid category name lenght!")]
        public string CategoryName { get; set; } = string.Empty;

        [Range(minimum: 5, maximum: 500, ErrorMessage = "Invalid quantity! Product quantitiy must be between 5 to 500.")]
        public int Quantity { get; set; }

        [Range(minimum: 1, maximum: 99999, ErrorMessage = "Invalid price! Product price must be between 1 to 99999.")]
        public int Price { get; set; }

        [Required]
        public bool SaleStatus { get; set; }

    }
}
