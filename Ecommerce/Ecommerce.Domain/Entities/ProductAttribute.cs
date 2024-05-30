using System;
using System.Collections.Generic;
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain.Entities
{
    /// <summary>
    /// Represents a product attribute
    /// </summary>
    public class ProductAttribute : AuditableBaseEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Has many product attribute mappings
        /// </summary>
        public ICollection<ProductAttributeMapping> ProductAttributeMappings { get; set; }
    }
}
