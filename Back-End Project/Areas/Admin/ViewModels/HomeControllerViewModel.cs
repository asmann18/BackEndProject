using Back_End_Project.Models;

namespace Back_End_Project.Areas.Admin.ViewModels
{
    public class HomeControllerViewModel
    {
        public List<Course> courses { get; set; }
        public List<Event> events { get; set; }
        public List<Blog> blogs { get; set; }

        public HomeControllerViewModel(List<Course> Courses,List<Event> Events,List<Blog> Blogs)
        {
            courses = Courses;
            events = Events;    
            blogs = Blogs;  
        }
       
    }
}
