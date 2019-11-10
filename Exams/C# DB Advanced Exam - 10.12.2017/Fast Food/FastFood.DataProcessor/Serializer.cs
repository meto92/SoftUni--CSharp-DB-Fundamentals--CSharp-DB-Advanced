using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Export;
using FastFood.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
	public class Serializer
	{
		public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
		{
            OrderType orderTypeEnum = OrderType.ForHere;

            Enum.TryParse(orderType, out orderTypeEnum);

            OrderDto[] orders = context.Employees
                .Include(e => e.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.Item)
                .Where(e => e.Name == employeeName)
                .FirstOrDefault()
                .Orders
                .Where(o => o.Type == orderTypeEnum)
                .AsQueryable()
                .ProjectTo<OrderDto>()
                .OrderByDescending(o => o.TotalPrice)
                .ThenByDescending(o => o.Items.Length)
                .ToArray();

            EmployeeOrdersDto dto = new EmployeeOrdersDto
            {
                Name = employeeName,
                Orders = orders,
                TotalMade = orders.Sum(o => o.TotalPrice)
            };

            string json = JsonConvert.SerializeObject(dto, Newtonsoft.Json.Formatting.Indented);

            return json;
		}

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            string[] categoryNames = categoriesString.Split(',').ToArray();

            //CategoryDto[] categoriesStatistics = context.Items
            //    .Where(i => categoryNames.Contains(i.Category.Name))
            //    .GroupBy(i => i.Category.Name)
            //    .Select(g => new CategoryDto
            //    {
            //        Name = g.Key,
            //        MostPopularItem = g.Select(i => new MostPopularItemDto
            //        {
            //            Name = i.Name,
            //            TimesSold = i.OrderItems.Sum(oi => oi.Quantity),
            //            TotalMade = i.OrderItems.Sum(oi => oi.Quantity * i.Price)
            //        })
            //            .OrderByDescending(i => i.TotalMade)
            //            .ThenByDescending(i => i.TimesSold)
            //            .FirstOrDefault()
            //    })
            //    .OrderByDescending(c => c.MostPopularItem.TotalMade)
            //    .ThenByDescending(c => c.MostPopularItem.TimesSold)
            //    .ToArray();

            CategoryDto[] categoriesStatistics = context.Categories
                .Where(c => categoryNames.Contains(c.Name))
                .Select(c => new CategoryDto
                {
                    Name = c.Name,
                    MostPopularItem = c.Items.Select(i => new MostPopularItemDto
                    {
                        Name = i.Name,
                        TimesSold = i.OrderItems.Sum(oi => oi.Quantity),
                        TotalMade = i.OrderItems.Sum(oi => oi.Quantity * oi.Item.Price)
                    })
                        .OrderByDescending(i => i.TotalMade)
                        .ThenByDescending(i => i.TimesSold)
                        .FirstOrDefault()
                })
                .OrderByDescending(c => c.MostPopularItem.TotalMade)
                .ThenByDescending(c => c.MostPopularItem.TimesSold)
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Categories");
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryDto[]), root);
            XmlSerializerNamespaces namespaces =
                new XmlSerializerNamespaces(new[]
                {
                    XmlQualifiedName.Empty
                });

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, categoriesStatistics, namespaces);
                
                return writer.ToString();
            }
        }
    }
}