using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleApp1
{
    [RoutePrefix("api/print")]
    public class PrintController : ApiController

    {
        [HttpPost]
        [Route("receipt")]
        public dynamic receipt(SaleModel student)
        {
            return new int[] { 12, 13, 14, 15 };
        }
    }
    public class Student
    {
        public string name;
    }
}
