using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieGR8.Helper
{
    public class Authorization
    {
    }
    /*Aqui agregare todas mis reglas de autorización*/
    public class EdadMinima : IAuthorizationRequirement
    {
        public int Edad { get; set; }
        public EdadMinima(int _Edad)
        {
            Edad = _Edad;
        }
    }
    public class EdadMinimaHandler : AuthorizationHandler<EdadMinima>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EdadMinima requirement)
        {
            var authorizationFilterContext = context.Resource as AuthorizationFilterContext;
            if(!context.User.HasClaim(x=>x.Type == "Edad"))
            {
                authorizationFilterContext.Result = new RedirectToActionResult("Index", "Login", null);
                return Task.CompletedTask;
            }

            int edad =  Convert.ToInt32(context.User.FindFirst(x => x.Type == "Edad").Value);
            if(edad >= requirement.Edad)
            {
                context.Succeed(requirement);
            }
            else
            {
                /* authorizationFilterContext.Result = new RedirectToActionResult("Index", "Home", null);
                 context.Succeed(requirement);*/
                context.Fail();
            }
            return Task.CompletedTask;

        }
    }
}
