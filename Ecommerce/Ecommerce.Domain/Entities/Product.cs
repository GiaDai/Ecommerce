using Ecommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Domain.Entities
{
    public class Product : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        /// <summary>
        /// Has many product attribute mappings
        /// </summary>
        public ICollection<ProductAttributeMapping> ProductAttributeMappings { get; set; }
    }
}
