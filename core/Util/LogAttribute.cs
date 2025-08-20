using core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

public class LogAttribute : Attribute, IActionFilter
{
    //private ILoginService _loginService;
    public LogAttribute()
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        //var actionName = context.RouteData.Values["action"].ToString();
        //var token = context.HttpContext.Request.Headers["Authorization"].ToString();
        //if (actionName != "InserirBuyer" && actionName != "Logar" && actionName != "Login" && actionName != "LogarPontoVenda")
        //{
        //    _loginService = (ILoginService)context.HttpContext.RequestServices.GetService(typeof(ILoginService));
        //    if (!_loginService.ValidaToken(token.Replace("Bearer ", "")))
        //    {
        //        //context.Result = new OkObjectResult(new { codigo = 403, mensagem = "Token inválido" });
        //    }
        //    else
        //    {
        //        var aut = _loginService.ConsultaAutenticacaoToken($" Token='{token.Replace("Bearer ", "")}'");
        //        var actions = _loginService.ConsultarMetodos().ToList();

        //        var list = actions.Where(x => x.Action == actionName && x.Categoria == aut.Tipo).FirstOrDefault();
        //        if (list == null)
        //        {
        //            //context.Result = new OkObjectResult(new { codigo = 401, mensagem = "Não autorizado" });
        //        }
        //    }
        //}
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        //Codigo  : depois que a action executa 
    }
}