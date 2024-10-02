using ContactManager.Data;  // Importing the Data namespace for accessing the AppDbContext
using ContactManager.Models;  // Importing the Models namespace for accessing the Person model
using Microsoft.AspNetCore.Mvc;  // Importing the ASP.NET MVC framework for creating controllers
using Microsoft.EntityFrameworkCore;  // Importing the Entity Framework Core library for database interactions
using System.Diagnostics;  // Importing for tracing and error handling

namespace ContactManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;  // Logger to log information and errors
        private readonly AppDbContext _context;  // AppDbContext instance to interact with the database

        // Constructor to inject the logger and database context
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Action method to display a list of persons in the view
        public async Task<IActionResult> Index()
        {
            var persons = await _context.Persons.ToListAsync();  // Fetch all persons from the database
            return View(persons);  // Pass the list of persons to the view
        }

        // Action method to handle updating a person record
        [HttpPost]
        public JsonResult UpdatePerson(int id, Person updatedPerson)
        {
            try
            {
                var person = _context.Persons.Find(id);  // Find the person in the database by ID
                if (person == null)  // If the person is not found, return an error response
                    return Json(new { success = false, errorMessage = "Person not found." });

                // Update the person with new values
                person.Name = updatedPerson.Name;
                person.DateOfBirth = updatedPerson.DateOfBirth;
                person.Married = updatedPerson.Married;
                person.Phone = updatedPerson.Phone;
                person.Salary = updatedPerson.Salary;

                _context.SaveChanges();  // Save the changes to the database

                return Json(new { success = true });  // Return a success response
            }
            catch (Exception ex)
            {
                // If there's an error, return a failure response with the error message
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

        // Action method to handle deleting a person record
        [HttpDelete]
        public JsonResult DeletePerson(int id)
        {
            try
            {
                var person = _context.Persons.Find(id);  // Find the person in the database by ID
                if (person == null)  // If the person is not found, return an error response
                    return Json(new { success = false, errorMessage = "Person not found." });

                _context.Persons.Remove(person);  // Remove the person from the database
                _context.SaveChanges();  // Save the changes to the database

                return Json(new { success = true });  // Return a success response
            }
            catch (Exception ex)
            {
                // If there's an error, return a failure response with the error message
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

        // Action method for handling and displaying error information
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Return the error view with the request ID or trace identifier
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
