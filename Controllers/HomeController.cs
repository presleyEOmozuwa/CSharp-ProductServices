using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductServices.Models;

namespace ProductServices.Controllers {
    public class HomeController : Controller {
        public HomeContext dbContext;
        public HomeController (HomeContext context) {
            dbContext = context;
        }



/////////************  LIST OF COMMODITIES /  FORM VIEW /  COMMODITY FORM SUBMISSION OR POST DATA / GET A SINGLE COMMODITY OBJECT FROM DATABASE //////////////*********************//////////

        [HttpGet ("")]
        public IActionResult Index () {
            List<Commodity> Commodities = dbContext.Commodities.Include (p => p.Categories).ThenInclude (u => u.category).ToList ();
            ViewBag.Commodities = Commodities;
            return View ();
        }

        [HttpGet ("newCommodity")]
        public IActionResult NewCommodity () {
            return View ();
        }

        [HttpPost ("newCommodity/create")]
        public IActionResult Create (Commodity newCommodity) {
            if (ModelState.IsValid) {
                dbContext.Commodities.Add (newCommodity);
                dbContext.SaveChanges ();
                return RedirectToAction ("Index");
            } else {
                return View ("NewCommodity");
            }
        }

        [HttpGet ("showCommodity/{CommodityId}")]
        public IActionResult ShowCommodity (int CommodityId) {
            Commodity getOneCommodity = dbContext.Commodities.Include( c => c.Categories).ThenInclude( b => b.category).FirstOrDefault( c => c.CommodityId == CommodityId);
            
            List<Category> allCategories = dbContext.Categories.Include (u => u.Commodities).ThenInclude (p => p.commodity).ToList ();
            ViewBag.allCategories = allCategories;
            return View (getOneCommodity);
        }

        
    
    //////////*********/ DELETE COMMODITY / EDIT COMMODITY //////////////******** */
        
        [HttpGet ("deleteCommodity/{CommodityId}")]
        public IActionResult DeleteCommodity (int CommodityId) {
            Commodity commodity = dbContext.Commodities.FirstOrDefault (c => c.CommodityId == CommodityId);

            dbContext.Commodities.Remove (commodity);
            dbContext.SaveChanges ();
            return RedirectToAction ("Index");
        }

        [HttpGet ("editCommodity/{CommodityId}")]
        public IActionResult EditCommodity (int CommodityId) {
            Commodity editCommodity = dbContext.Commodities.FirstOrDefault (c => c.CommodityId == CommodityId);
            return View (editCommodity);
        }

        [HttpPost ("editCommodity/update/{CommodityId}")]
        public IActionResult UpdateCommodity (Commodity updateCommodity, int CommodityId) {
            Commodity dbCommodity = dbContext.Commodities.FirstOrDefault (c => c.CommodityId == CommodityId);

            dbCommodity.Name = updateCommodity.Name;
            dbCommodity.Price = updateCommodity.Price;
            dbCommodity.Quantity = updateCommodity.Quantity;
            dbCommodity.Description = updateCommodity.Description;
            dbCommodity.UpdatedAt = DateTime.Now;
            dbContext.SaveChanges ();
            return RedirectToAction ("Index");
        }


///////////// ADDING CATEGORY TO COMMODITY OBJECT ********** /////////  ADDING CATEGORY TO COMMODITY USING THE DROP DOWN LIST /////////////

        [HttpPost("show/addCategory")]
        public IActionResult AddCategory(Brand newBrand)
        {
            if(ModelState.IsValid)
            {   

                Commodity brandCommodity = dbContext.Commodities
                .FirstOrDefault(p => p.CommodityId == newBrand.CommodityId); 
                
                Category brandCategory = dbContext.Categories
                .FirstOrDefault(c => c.CategoryId == newBrand.CategoryId);
                
                Brand brand = new Brand();
                brand.CommodityId = newBrand.CommodityId;
                brand.CategoryId = newBrand.CategoryId;
                brand.commodity = brandCommodity;
                brand.category = brandCategory;
                dbContext.Add(brand);
                dbContext.SaveChanges();
                return Redirect($"/showCommodity/{newBrand.CommodityId}");
            }
            else
            {
                Commodity commodity = dbContext.Commodities
                .FirstOrDefault(p => p.CommodityId == newBrand.CommodityId);
                ModelState.AddModelError("AssociationProduct", "Could not add product");
                return View("ShowCommodity", commodity);
            }
        }

        

/////////************  LIST OF CATEGORIES /  FORM VIEW /  FORM DATA SUBMISSION TO THE BACK END / GET A SINGLE CATEGORY OBJECT FROM DATABASE //////////////*********************//////////
        
        [HttpGet ("allCategories")]
        public IActionResult AllCategories () {
            List<Category> allCategories = dbContext.Categories.Include (u => u.Commodities).ThenInclude (p => p.commodity).ToList ();
            ViewBag.allCategories = allCategories;
            return View ();
        }

        [HttpGet ("show/newCategory")]
        public IActionResult NewCategory () {
            return View ();
        }

        [HttpPost ("createCategory")]
        public IActionResult NewCategory (Category newCategory) {
            if (ModelState.IsValid) {
                dbContext.Categories.Add (newCategory);
                dbContext.SaveChanges ();
                return RedirectToAction ("AllCategories");
            } else {
                return View ("NewCategory");
            }
        }

        [HttpGet ("showCategory/{CategoryId}")]
        public IActionResult ShowCategory (int CategoryId) {
            
            Category getCategory = dbContext.Categories.Include( c => c.Commodities).ThenInclude( b => b.commodity).FirstOrDefault( c => c.CategoryId == CategoryId);

            List<Commodity> allCommodities = dbContext.Commodities.Include (p => p.Categories).ThenInclude (u => u.category).ToList ();
            ViewBag.allCommodities = allCommodities;
            return View (getCategory);
        }



//////////*********/ DELETE CATEGORY OBJECT / EDIT CATEGORY //////////////******** */
    
        [HttpGet("deleteCategory/{CategoryId}")]
        public IActionResult DeleteCategory( int CategoryId )
        {
            Category category = dbContext.Categories.FirstOrDefault(c => c.CategoryId == CategoryId);

            dbContext.Categories.Remove(category);
            dbContext.SaveChanges();
            return RedirectToAction("AllCategories");
        }

        [HttpGet("editCategory/{CategoryId}")]
        public IActionResult EditCategory( int CategoryId )
        {
            Category editCategory = dbContext.Categories.FirstOrDefault(c => c.CategoryId == CategoryId);
            return View(editCategory);
        }

        [HttpPost("editCategory/update/{CategoryId}")]
        public IActionResult UpdateCategory( Category updateCategory, int CategoryId )
        {
            Category dbCategory = dbContext.Categories.FirstOrDefault( c => c.CategoryId == CategoryId);

            dbCategory.CategoryName = updateCategory.CategoryName;
            dbCategory.UpdatedAt = DateTime.Now;
            dbContext.SaveChanges();
            return RedirectToAction("AllCategories");
        }


        [HttpPost("show/addCommodity")]
        public IActionResult AddCommodity(Brand newBrand)
        {
            if(ModelState.IsValid)
            {   
                
                Category brandCategory = dbContext.Categories
                .FirstOrDefault(c => c.CategoryId == newBrand.CategoryId);
                
                Commodity brandCommodity = dbContext.Commodities
                .FirstOrDefault(p => p.CommodityId == newBrand.CommodityId); 
                
                
                Brand brand = new Brand();
                brand.CommodityId = newBrand.CommodityId;
                brand.CategoryId = newBrand.CategoryId;
                brand.commodity = brandCommodity;
                brand.category = brandCategory;
                dbContext.Add(brand);
                dbContext.SaveChanges();
                return Redirect($"/showCategory/{newBrand.CategoryId}");
            }
            else
            {
                Category category = dbContext.Categories
                .FirstOrDefault(p => p.CategoryId == newBrand.CategoryId);
                ModelState.AddModelError("AssociationProduct", "Could not add product");
                return View("ShowCategory", category);
            }
        }



////////////***************************** */    //////////////////////////////////    
                /// THE END OF BUSINESS LOGIC
        
  //////////////////////////////////********************************** */      
        
        
        

        
        public IActionResult Privacy () 
        {
            return View ();
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error () {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}