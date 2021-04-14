using Microsoft.AspNetCore.Mvc;
using ServiceManager.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceManager.Web.ViewComponents
{
    [ViewComponent]
    public class LastServiceUpdateViewComponent : ViewComponent
    {
        private readonly ServiceManagerContext _context;

        public LastServiceUpdateViewComponent(ServiceManagerContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(
        int maxPriority, bool isDone)
        {
            var items = await GetItemsAsync(maxPriority, isDone);
            return View(items);
        }
        private Task<List<string>> GetItemsAsync(int maxPriority, bool isDone)
        {
            return Task.Run<List<string>>(() =>
            {
                var names = new List<string>();
                names.Add("First");
                names.Add("Second");
                names.Add("Third");
                names.Add("Fourth");
                return names;
            });
        }

    }
}
