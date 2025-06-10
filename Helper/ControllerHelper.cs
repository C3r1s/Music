using Microsoft.AspNetCore.Mvc;

namespace Music.Helper;

public static class ControllerHelper
{
    public static string GetName<T>() where T : Controller
    {
        return typeof(T).Name.Replace("Controller", string.Empty);
    }
    
}