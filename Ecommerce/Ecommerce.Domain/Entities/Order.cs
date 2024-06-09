using System;
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain.Entities
{
    public class Order : AuditableBaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
