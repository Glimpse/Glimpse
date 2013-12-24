using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcMusicStore.Controllers
{
    public class ModelBindingTestsController : Controller
    {
        public ActionResult Index()
        {
            return View("Index", new ComplexType());
        }

        [HttpPost]
        public ActionResult SimplePost(string firstname, string lastname)
        {
            return View("Index", new ComplexType());
        }

        [HttpPost]
        public ActionResult ComplexPost(ComplexType complexType, string additionalSimpleValue)
        {
            return View("Index", new ComplexType());
        }

        [HttpPost]
        public JsonResult PostItem(AjaxPostedData item)
        {
            return new JsonResult { Data = new { PostedItem = item } };
        }

        public class AjaxPostedData
        {
            public string MySimpleValue { get; set; }
            public Dictionary<string, string> MyStringDictionary { get; set; }
            public Dictionary<string, MyComplexObject> MyComplexObjectDictionary { get; set; }
            public MyComplexObject MyComplexObject { get; set; }
            public string[] MyStringArray { get; set; }
        }

        public class MyComplexObject
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public class ComplexType
        {
            public ComplexType()
            {
                this.Name = "Foo";
                this.Another = new SecondLevelComplexType();
            }

            public string Name { get; set; }
            public SecondLevelComplexType Another { get; set; }
        }

        public class SecondLevelComplexType
        {
            public SecondLevelComplexType()
            {
                this.Name = "Bar";
                this.Values = new ThirdLevelComplexType(this.Name);
            }

            public string Name { get; set; }
            public ThirdLevelComplexType Values { get; set; }
        }

        public class ThirdLevelComplexType
        {
            public ThirdLevelComplexType(string valueFor)
            {
                this.Value1 = "Value 1 for " + valueFor;
                this.Value2 = "Value 2 for " + valueFor;
            }

            public string Value1 { get; set; }
            public string Value2 { get; set; }
        }
    }
}
