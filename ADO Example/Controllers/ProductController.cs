using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADO_Example.Data;
using ADO_Example.Models;

namespace ADO_Example.Controllers
{
    public class ProductController : Controller
    {
        Product_DAL _productDal = new Product_DAL();

        // GET ALL: Product
        public ActionResult Index()
        {
            var productList = _productDal.GetAllProducts();

            if(productList.Count == 0)
            {
                TempData["InfoMessage"] = "Currently products are not available in the Database.";
            }
            return View(productList);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Create Product view is from this function
        public ActionResult Create()
        {
            return View();
        }

        // POST: Create product action method to insert data into DB
        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                bool IsInserted = false;

                if (ModelState.IsValid)
                {
                    IsInserted = _productDal.InsertProduct(product);

                    if (IsInserted)
                    {
                        TempData["SuccessMessage"] = "Product details saved successfully !";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Product is already available/unable to save product details.";
                    }              
                }
                return RedirectToAction("Index");                   // If success or fail - redirect to Index view 
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();                                      // If error is caught - redirect to Create view.
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {   
            var products = _productDal.GetProductByID(id).FirstOrDefault();

            if(products == null)
            {
                TempData["InfoMessage"] = "Product not available with ID " + id.ToString();
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // POST: Update Product
        [HttpPost, ActionName("Edit")]
        public ActionResult UpdateProduct(Product product)
        {
            try
            {
                bool isUpdated = false;

                if (ModelState.IsValid)
                {
                    isUpdated = _productDal.UpdateProduct(product);

                    if (isUpdated)
                    {
                        TempData["SuccessMessage"] = "Product details updated successfully !";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Product is already available/unable to update product details.";
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();                                  // If failed to update - redirect to Edit view
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
