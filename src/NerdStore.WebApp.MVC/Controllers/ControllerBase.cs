using Microsoft.AspNetCore.Mvc;
using System;

namespace NerdStore.WebApp.MVC.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected Guid ClienteId = Guid.Parse("CD23E2B3-15FF-4FF5-AC44-A69E093569FD");
    }
}
