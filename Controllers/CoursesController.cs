using Figmadesign.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Figmadesign.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            var courses = new List<Course>
            {
                new Course { Title = "Course 1", Description = "Description for Course 1" },
                new Course { Title = "Course 2", Description = "Description for Course 2" },
                new Course { Title = "Course 3", Description = "Description for Course 3" }
            };

            return View(courses);
        }
    }
}
