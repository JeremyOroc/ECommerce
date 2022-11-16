using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;

        public SearchService(IProductsService productsService,
            IOrdersService ordersService, ICustomersService customersService)
        {
            this.productsService = productsService;
            this.ordersService = ordersService;
            this.customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var ordersResult = await ordersService.GetOrdersAsync(customerId);
            var productsResult = await productsService.GetProductsAsync();
            var customerResult = await customersService.GetCustomerAsync(customerId);

            if (ordersResult.IsSuccess)
            {
                foreach (var order in ordersResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ?
                            productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name :
                            "Product information is not available";
                    }
                }


                var result = new
                {
                    Customer = customerResult.IsSuccess ?
                                customerResult.Customer :
                                new { Name = "Customer information is not available" },
                    Orders = ordersResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
