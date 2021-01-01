using System;

#nullable disable

namespace ReportingTool.Data
{
    public partial class Arrival
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime When { get; set; }
    }
}
