using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderManager.Models
{
    public class OrderView
    {
        public int Id { get; set; }
        public string Number { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Provider Id")]
        public int ProviderId { get; set; }

        [Display(Name = "Provider")]
        public string ProviderName { get; set; }
        public List<OrderItem> ItemList { get; set; }

        public OrderView()
        {

        }

        public OrderView(Order order)
        {
            Id = order.Id;
            Number = order.Number;
            Date = order.Date;
            ProviderId = order.ProviderId;
        }

        public OrderView(Order order, Provider provider, List<OrderItem> items)
        {
            Id = order.Id;
            Number = order.Number;
            Date = order.Date;
            ProviderId = order.ProviderId;
            ProviderName = provider.Name;
            ItemList = items;
        }
    }
}
