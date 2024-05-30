using System;
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain.Entities
{
    public class ProductAttributeMapping : AuditableBaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ProductAttributeId { get; set; }
        public ProductAttribute ProductAttribute { get; set; }
    }
}
