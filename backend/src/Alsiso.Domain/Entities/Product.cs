using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Domain.Entities
{
    public sealed class Product
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string Category { get; }
        public decimal? Price { get; }

        public Product(
            Guid id,
            string name,
            string description,
            string category,
            decimal? price)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("El identificador es obligatorio.", nameof(id));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre es obligatorio.", nameof(name));

            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("La categoría es obligatoria.", nameof(category));

            if (price.HasValue && price.Value < 0)
                throw new ArgumentException("El precio no puede ser negativo.", nameof(price));

            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Price = price;
        }
    }



}

