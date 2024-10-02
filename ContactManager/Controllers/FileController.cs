using ContactManager.Data;
using ContactManager.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ContactManager.Controllers
{
    // This class maps the CSV columns to the Person model properties
    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            // Mapping CSV column names to Person model properties
            Map(m => m.Name).Name("Name");
            Map(m => m.DateOfBirth).Name("DateOfBirth");
            Map(m => m.Married).Name("Married");
            Map(m => m.Phone).Name("Phone");
            Map(m => m.Salary).Name("Salary");
        }
    }

    public class FileController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor to inject the database context
        public FileController(AppDbContext context)
        {
            _context = context;
        }

        // Action method to display the file upload form
        public IActionResult UploadFile()
        {
            return View();
        }

        // Action method to process the uploaded CSV file
        public async Task<IActionResult> UploadCSV(IFormFile file)
        {
            // If no file is uploaded or file is empty, return an error
            if (file == null || file.Length == 0)
                return BadRequest("File not selected");

            // Reading the CSV file
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Registering the mapping for the Person class
                csv.Context.RegisterClassMap<PersonMap>();

                // Retrieving all records from the CSV and converting them into a list of Person objects
                var records = csv.GetRecords<Person>().ToList();

                // Adding the list of Person records to the database
                _context.Persons.AddRange(records);
                await _context.SaveChangesAsync(); // Saving the changes to the database
            }

            // Redirecting to the Index action of the Home controller after successful upload
            return RedirectToAction("Index", "Home");
        }
    }
}
