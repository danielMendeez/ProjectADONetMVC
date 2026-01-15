using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectADONetMVC.DAL;
using ProjectADONetMVC.Models;

namespace ProjectADONetMVC.Controllers
{
    public class ProductController : Controller
    {
        Product_DAL _productDAL = new Product_DAL();

        // GET: Product
        public ActionResult Index()
        {
            var productList = _productDAL.GetAllProducts();

            if(productList.Count == 0) 
            {
                TempData["InfoMessage"] = "Currently products no avalible.";
            }

            return View(productList);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Producto producto)
        {
            bool existProduct = false;
            bool isInserted = false;
            try
            {
                if (ModelState.IsValid)
                {
                    existProduct = _productDAL.VerifyExistProduct(producto.Nombre);
                    if(existProduct)
                    {
                        TempData["ErrorMessage"] = "Product is already available.";
                    }
                    else
                    {
                        isInserted = _productDAL.InsertProduct(producto);
                        if (isInserted)
                        {
                            TempData["SuccessMessage"] = "Product details saved successfully.";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Unable to save the product details.";
                        } 
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
            
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _productDAL.GetProductByID(id).FirstOrDefault();
            if(product == null)
            {
                TempData["InfoMessage"] = "Product not avalible.";
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost, ActionName("Edit")]
        public ActionResult UpdateProduct(Producto producto)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    bool isUpdated = _productDAL.UpdateProduct(producto);
                    if(isUpdated)
                    {
                        TempData["SuccessMessage"] = "Product details updated successfully.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to update the product details.";
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
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
