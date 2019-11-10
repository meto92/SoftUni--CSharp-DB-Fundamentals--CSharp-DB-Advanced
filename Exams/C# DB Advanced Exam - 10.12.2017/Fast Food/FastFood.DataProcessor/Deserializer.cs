using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
	public static class Deserializer
	{
		private const string FailureMessage = "Invalid data format.";
		private const string SuccessMessage = "Record {0} successfully imported.";

        public static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
		{
            EmployeeDto[] employees = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);

            StringBuilder result = new StringBuilder();

            List<EmployeeDto> validEmployees = new List<EmployeeDto>();

            Dictionary<string, Position> positions = employees
                .Select(e => e.Position)
                .Distinct()
                .ToDictionary(p => p, p => new Position { Name = p });

            foreach (EmployeeDto employee in employees)
            {
                bool isValid = IsValid(employee);

                if (!isValid)
                {
                    result.AppendLine(FailureMessage);

                    continue;
                }

                validEmployees.Add(employee);

                result.AppendLine(string.Format(SuccessMessage, employee.Name));
            }

            context.Employees.AddRange(validEmployees
                .Select(dto => new Employee
                {
                    Name = dto.Name,
                    Age = dto.Age,
                    Position = positions[dto.Position]
                }));

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

		public static string ImportItems(FastFoodDbContext context, string jsonString)
		{
			ItemDto[] items = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);

            StringBuilder result = new StringBuilder();

            List<ItemDto> validItems = new List<ItemDto>();

            Dictionary<string, Category> categories = items
                .Select(i => i.Category)
                .Distinct()
                .ToDictionary(c => c, c => new Category { Name = c });

            foreach (ItemDto item in items)
            {
                bool isValid = IsValid(item);

                if (!isValid || validItems.Any(i => i.Name == item.Name))
                {
                    result.AppendLine(FailureMessage);

                    continue;
                }

                validItems.Add(item);

                result.AppendLine(string.Format(SuccessMessage, item.Name));
            }

            context.Items.AddRange(validItems
                .Select(dto => new Item
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Category = categories[dto.Category]
                }));

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

		public static string ImportOrders(FastFoodDbContext context, string xmlString)
		{
            XmlRootAttribute root = new XmlRootAttribute("Orders");
            XmlSerializer serializer = new XmlSerializer(typeof(OrderDto[]), root);

            OrderDto[] orders = null;

            using (StringReader reader = new StringReader(xmlString))
            {
                 orders = (OrderDto[]) serializer.Deserialize(reader);
            }

            StringBuilder result = new StringBuilder();

            Dictionary<string, Item> itemsCache = orders
                .SelectMany(o => o.Items)
                .Select(i => i.Name)
                .Distinct()
                .ToDictionary(i => i, i => context.Items
                    .FirstOrDefault(item => item.Name == i));

            Dictionary<string, Employee> employeesCache = orders
                .Select(o => o.Employee)
                .Distinct()
                .ToDictionary(e => e, e => context.Employees
                    .FirstOrDefault(employee => employee.Name == e));

            List<Order> validOrders = new List<Order>();

            foreach (OrderDto order in orders)
            {
                bool employeeExists = employeesCache[order.Employee] != null;

                bool allItemsExist = order.Items
                    .All(i => itemsCache[i.Name] != null);

                bool allItemsHaveValidQuantity = order.Items.All(i => i.Quantity > 0);

                bool allItemsHaveValidPrice = order.Items
                    .Select(i => itemsCache[i.Name])
                    .Where(i => i != null)
                    .All(i => i.Price >= 0.01m);

                if (!employeeExists || 
                    !allItemsExist || 
                    !allItemsHaveValidQuantity ||
                    !allItemsHaveValidPrice)
                {
                    result.AppendLine(FailureMessage);

                    continue;
                }

                Enum.TryParse(order.Type, out OrderType type);

                validOrders.Add(new Order
                {
                    Customer = order.Customer,
                    Employee = employeesCache[order.Employee],
                    DateTime = order.DateTime,
                    Type = type,
                    OrderItems = order.Items
                        .Select(i => new OrderItem
                        {
                            Item = itemsCache[i.Name],
                            Quantity = i.Quantity
                        })
                        .ToArray()
                });

                result.AppendLine($"Order for {order.Customer} on {order.Date} added");
            }

            context.Orders.AddRange(validOrders);

            context.SaveChanges();

            return result.ToString();
        }
	}
}