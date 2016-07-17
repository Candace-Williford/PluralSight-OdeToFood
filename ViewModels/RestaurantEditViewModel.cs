using OdeToFood.Entities;
using System.ComponentModel.DataAnnotations;

namespace OdeToFood.ViewModels
{
    //this model contains only the properties I would expect to receive in the http request
    public class RestaurantEditViewModel
    {
        [Required, MaxLength(80)] //data annotations
        public string Name { get; set; }
        public CuisineType Cuisine { get; set; }
    }
}
