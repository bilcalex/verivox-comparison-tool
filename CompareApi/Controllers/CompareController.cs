using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompareApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CompareApi.Controllers
{
    [Route("api/Compare")]
    [ApiController]
    public class CompareController : ControllerBase
    {

        private readonly CompareContext _context;

        //Basic Tariff constants
        public const int baseMonthlyCost = 5; // euro/month
        public const double consumptionCost = 22; // cents/KWh

        //Packaged Tariff constants
        public const int costUnderDiscountLimit = 800; // euro/year
        public const int discountLimit = 4000; //KWh/year
        public const double additionalCosts = 30; // cents/KWh

        public CompareController(CompareContext context)
        {
            _context = context;

            if (_context.ProductItems.Count() == 0)
            {
                // Create new ProductItems
                _context.ProductItems.Add(new ProductItem { Type = ProductType.Basic });
                _context.ProductItems.Add(new ProductItem { Type = ProductType.Packaged });
                _context.SaveChanges();
            }
        }

        public double ComputeAnnualCosts(ProductItem productItem)
        {
            double result = 0;
            switch (productItem.Type)
            {
                case ProductType.Basic:
                    result = baseMonthlyCost * 12 + ((double)consumptionCost / 100) * productItem.Consumption;
                    break;
                case ProductType.Packaged:
                    result = productItem.Consumption <= discountLimit ?
                                costUnderDiscountLimit :
                                costUnderDiscountLimit + ((double)additionalCosts / 100) * (productItem.Consumption - discountLimit);
                    break;
            }

            return result;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductItem>>> GetCompareItems()
        {
            return await _context.ProductItems.ToListAsync();
        }



        // GET api/values/4500
        [HttpGet("{annualConsumption}")]
        public async Task<ActionResult<IEnumerable<ProductItem>>> GetCompareItems(int annualConsumption)
        {
            var productItems = await _context.ProductItems.ToListAsync();

            if (productItems == null)
            {
                return NotFound();
            }
            else
            {
                foreach (ProductItem item in productItems)
                {
                    item.Consumption = annualConsumption;
                    item.AnnualCost = ComputeAnnualCosts(item);
                }
            }

            return productItems.OrderBy(pi => pi.AnnualCost).ToList();
        }
    }
}
