﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neodynamic.SDK.Web;
using Microsoft.AspNetCore.Hosting;

namespace WebClientPrint4WebSiteSample.Controllers
{
    public class DemoPrintFilePDFController : Controller
    {

        private readonly IHostingEnvironment _hostEnvironment;


        public DemoPrintFilePDFController(IHostingEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;

        }

        public IActionResult Index()
        {
            ViewData["WCPScript"] = Neodynamic.SDK.Web.WebClientPrint.CreateScript(Url.Action("ProcessRequest", "WebClientPrintAPI", null, Url.ActionContext.HttpContext.Request.Scheme), Url.Action("PrintFile", "DemoPrintFilePDF", null, Url.ActionContext.HttpContext.Request.Scheme), Url.ActionContext.HttpContext.Session.Id);

            return View();
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult PrintFile(string printerName, string trayName, string paperName, string printRotation, string pagesRange, string printAnnotations, string printAsGrayscale, string printInReverseOrder)
        {
            string fileName = Guid.NewGuid().ToString("N");
            string filePath = filePath = "/files/GuidingPrinciplesBusinessHR_EN.pdf";

            PrintFilePDF file = new PrintFilePDF(_hostEnvironment.ContentRootPath + filePath, fileName);
            file.PrintRotation = (PrintRotation)Enum.Parse(typeof(PrintRotation), printRotation); ;
            file.PagesRange = pagesRange;
            file.PrintAnnotations = (printAnnotations == "true");
            file.PrintAsGrayscale = (printAsGrayscale == "true");
            file.PrintInReverseOrder = (printInReverseOrder == "true");

            ClientPrintJob cpj = new ClientPrintJob();
            cpj.PrintFile = file;
            if (printerName == "null")
                cpj.ClientPrinter = new DefaultPrinter();
            else
            {
                if (trayName == "null") trayName = "";
                if (paperName == "null") paperName = "";

                cpj.ClientPrinter = new InstalledPrinter(printerName, true, trayName, paperName);
            }

            return File(cpj.GetContent(), "application/octet-stream");
            
        }

    }
}
