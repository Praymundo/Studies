using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NancyTest
{
    public class GetPage : Nancy.NancyModule
    {
        public GetPage()
        {
            Get["/"] = x =>
            {
                return "Hello!";
            };

            Get["/{name}"] = x => { return "Name: " + x.name; };

            Get["/json"] = x =>
            {
                try
                {
                    return GetById(1);
                    return Negotiate.NegotiationContext.Cookies.ToArray();
                    // Response.Context.Response = new Nancy.Response();
                    // Response.Context.Response.ContentType = "text/json";
                    return "{Status:1}";
                }
                catch (Exception ex)
                {
                    return "{Status:\"" + ex.Message + "\"}";
                }
                
            };
        }

        private object GetById(int id)
        {
            // fake a return
            return new { Id = id, Title = "Site Admin", Level = 2 };
        }
    }
}
