using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Ganss.Xss;
using Microsoft.Security.Application;

namespace XSSUnitTestMVC.Controllers
{
    public static class Css
    {
        public static string A { get; set; } = "A";
    }
    public class HomeController : Controller
    {
        private List<Abc> abc;
        private static string _intput;
        private static int _count;
         [HttpPost]
         [ValidateInput(false)]
         public ActionResult Submit(FormCollection form)
         {
             _intput = form["SValue"];
             //var sValue = form["SValue"];
             
             // var htmlEncode = HttpUtility.HtmlEncode(sValue);
             // var antiXSS = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(sValue);
             //
             // ViewBag.sValueS = sValue;
             // ViewBag.htmlEncodeS = htmlEncode;
             // ViewBag.antiXSSS = antiXSS;
             
             //return View(_intput);
             return RedirectToAction("Index");
         }
        
        public ActionResult Index()
        {
            if (_count == 0)
            {
                _count++;
            }
            else
            {
                var sValue = _intput;
                
                //_intput = EscapeHtmlBr(_intput);
                var htmlEncode = "";
                if (sValue != null && sValue.Contains("&quot;"))
                {
                    htmlEncode = HttpUtility.HtmlEncode(sValue);
                    htmlEncode = htmlEncode.Replace("&amp;quot;", "&quot;");
                }
                else
                {
                    htmlEncode  = HttpUtility.HtmlEncode(sValue);
                }
                
                
                
                var antiXSS = HttpUtility.UrlEncode(sValue);
                //var antiXSSUpdated = UnescapeHtmlBr(antiXSS);
                
                var sanitizer = new HtmlSanitizer();
                //sanitizer.AllowedTags.Add("tr");
                //var formatter = AngleSharp.Xhtml.XhtmlMarkupFormatter.Instance;
                var htmlSanitizer = sanitizer.Sanitize(sValue);

                //sValue = sValue.Replace("2839 6333 <br />", "2839 6333<br />");
                var customSanitizer = Common.HtmlSanitizer.SanitizeHtml(sValue);
                customSanitizer = HttpUtility.HtmlDecode(customSanitizer);
                //var customSanitizer = Regex.Replace(Common.HtmlSanitizer.SanitizeHtml(sValue), @"&nbsp;", "");
                //var customSanitizer = Common.HtmlSanitizer.SanitizeHtml(sValue.Replace("2839 6333 ", "2839 6333"));
               

                var compareOGvscustomSanitizer = sValue.Equals(customSanitizer) ? "true" : "false";
            
                // ViewBag.sValue = sValue;
                ViewBag.htmlEncode = htmlEncode;
                ViewBag.antiXSS = antiXSS;
                ViewBag.htmlSanitizer = htmlSanitizer;
                ViewBag.customerSanitizer = customSanitizer;

                ViewBag.compareOGvscustomSanitizer = compareOGvscustomSanitizer;
                //ViewData["sValue"] = sValue;
            }
            
            return View();
        }
        const string BrMarker = @"|tr|";

        private static string UnescapeHtmlBr(string result)
        {
            result = result.Replace(BrMarker, "</tr>");

            return result;
        }

        private static string EscapeHtmlBr(string input)
        {
            input = input.Replace("<tr", BrMarker);
            input = input.Replace("</ tr>", BrMarker);
            input = input.Replace("</tr>", BrMarker);

            return input;
        }

        public ActionResult About()
        {
            var sValue = "<b>Your email address can not be found.</b>";

                var htmlEncode = HttpUtility.HtmlEncode(sValue);
            var antiXSS = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(sValue);
            
            ViewBag.Message1 = sValue;
            ViewBag.Message2 = htmlEncode;
            ViewBag.Message3 = antiXSS;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }

    public class Abc
    {
        public string OG { get; set; }
        public string HTMLEncode { get; set; }
        public string AntiXSS { get; set; }
    }
}