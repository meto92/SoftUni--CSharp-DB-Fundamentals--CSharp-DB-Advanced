namespace FastFood.DataProcessor.Dto.Export
{
    public class EmployeeOrdersDto
    {
        public string Name { get; set; }

        public OrderDto[] Orders { get; set; }

        public decimal TotalMade { get; set; }
    }
}