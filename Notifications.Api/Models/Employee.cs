﻿namespace Notifications.Api.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public DateOnly DOJ { get; set; }
        public bool IsActive { get; set; }
    }
}
