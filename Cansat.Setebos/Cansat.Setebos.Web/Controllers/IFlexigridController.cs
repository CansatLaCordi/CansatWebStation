using System;
namespace Cansat.Setebos.Web.Controllers
{
    interface IFlexigridController
    {
        System.Web.Mvc.ActionResult Flexigrid(Cansat.Setebos.Web.Models.FlexigridModel model);
    }
}
