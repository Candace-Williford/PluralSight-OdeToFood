using Microsoft.AspNetCore.Mvc;
using OdeToFood.ViewModels;
using OdeToFood.Services;
using OdeToFood.Entities;

namespace OdeToFood.Controllers
{
    public class HomeController : Controller
    {
        private IGreeter _greeter;
        private IRestaurantData _restaurantData;

        public HomeController(IRestaurantData restaurantData, IGreeter greeter)
        {
            _restaurantData = restaurantData;
            _greeter = greeter;
        }

        public ViewResult Index()
        {
            var model = new HomePageViewModel();
            model.Restaurants = _restaurantData.GetAll();
            model.CurrentGreeting = _greeter.GetGreeting();

            return View(model);
        }

        [HttpGet] //helps asp.net mvc differentiate which one should be called
        public ViewResult Create()
        {
            return View(); //don't have to pass it a model because you're creating a new one
        }

        [HttpPost]
        public IActionResult Create(RestaurantEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                var restaurant = new Restaurant();
                restaurant.Name = model.Name;
                restaurant.Cuisine = model.Cuisine;

                _restaurantData.Add(restaurant);

                return RedirectToAction("Details", new { id = restaurant.Id });
            }
            else
            {
                return View();
            }
        }

        public IActionResult Details(int id) //parameter name must match route
        {
            var model = _restaurantData.Get(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
    }
}