﻿using Microsoft.AspNetCore.Mvc;
using PierresBakery.Models;
using System.Diagnostics;
using System.Numerics;

namespace PierresBakery.Controllers
{
    public class VendorsController : Controller

    {

        private static List<Vendor> vendors = new();

        [HttpGet]
        public IActionResult Index(int? vendorId, string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                List<Vendor> matchingVendors = vendors.Where(v => v.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();


                if (matchingVendors.Count == 0)
                {
                    ViewBag.Message = "No vendors found matching the search term.";
                }

                return View(matchingVendors);
            }
            if (vendorId.HasValue)
            {
                Vendor vendor = vendors.FirstOrDefault(v => v.VendorId == vendorId);
                if (vendor == null)
                {
                    return NotFound();
                }

                return View("Details", vendor);
            }

            return View(vendors);
        }




        [HttpGet("/vendors/{vendorId}/orders/new")]
        public IActionResult Create(int vendorId)
        {
            Vendor vendor = vendors.FirstOrDefault(v => v.VendorId == vendorId);
            if (vendor == null)
            {
                return NotFound();
            }

            return View();
        }





        [HttpGet("/vendors/new")]
        public IActionResult NewVendor()
        {
            return View();
        }

        [HttpPost("/vendors/new")]
        public IActionResult NewVendor(string name, string description)
        {
            int newVendorId = vendors.Count + 1;
            Vendor newVendor = new Vendor(newVendorId, name, description);
            vendors.Add(newVendor);
            return RedirectToAction("Index", "vendors");
        }

        [HttpPost("/vendors/{vendorId}/orders/new")]
        public IActionResult Create(int vendorId, string title, string description, decimal price, DateTime date)
        {
            Vendor vendor = vendors.FirstOrDefault(v => v.VendorId == vendorId);
            if (vendor == null)
            {
                return NotFound();
            }


            Order newOrder = new Order(title, description, price, date);
            vendor.Orders.Add(newOrder);

            return RedirectToAction("Index", "vendors", new { vendorId = vendor.VendorId });
        }




    }
}
