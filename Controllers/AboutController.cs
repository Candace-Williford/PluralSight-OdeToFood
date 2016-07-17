using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood.Controllers
{
    //attribute based routing
    [Route("[controller]/[action]")] //tells asp.net to use the name of this controller

    public class AboutController
    {
        //[Route("")]
        public string Phone()
        {
            return "+1-555-555-5555";
        }

        //[Route("[action]")]
        public string Country()
        {
            return "USA";
        }
    }
}
