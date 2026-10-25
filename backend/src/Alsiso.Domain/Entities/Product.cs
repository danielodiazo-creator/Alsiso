using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Domain.Entities
{
    public sealed class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public decimal? Price { get; private set; }

        public Product(
            Guid id,
            string name,
            string description,
            string category,
            decimal? price)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("El identificador es obligatorio.", nameof(id));

           
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Price = price;
        }

        public void UpdateDetails(
        string name,
        string description,
        string category,
        decimal? price)
        {
            ValidateDetails(name, category, price);

            Name = name;
            Description = description;
            Category = category;
            Price = price;
        }

        private static void ValidateDetails(
        string name,
        string category,
        decimal? price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(
                    "El nombre es obligatorio.",
                    nameof(name));

            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException(
                    "La categoría es obligatoria.",
                    nameof(category));

            if (price.HasValue && price.Value < 0)
                throw new ArgumentException(
                    "El precio no puede ser negativo.",
                    nameof(price));
        }


    }



}

