using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;

namespace SpendSmart.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly SpendSmartDbContext _context;

    public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Expenses()
    {
        var allExpenses = _context.Expenses.ToList();

        var totalExpenses = allExpenses.Sum(x => x.Value);

        ViewBag.Expenses = totalExpenses;

        return View(allExpenses);
    }

    public IActionResult CreateEditExpense(int? Id)
    {
        if(Id is not null)
        {
            // editing
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == Id);
            return View(expenseInDb);
        }

        return View();
    }

    public IActionResult CreateEditExpenseForm(Expense model)
    {


        if (model.Id == 0)
        {
            //neu hinzufügen
            _context.Expenses.Add(model);
        }
        else
        {
            //bearbeiten
            _context.Expenses.Update(model);
        }


            _context.SaveChanges();
        return RedirectToAction("Expenses");
    }

    public IActionResult DeleteExpense(int Id)
    {
        var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == Id);
        _context.Expenses.Remove(expenseInDb);
        _context.SaveChanges();

        return RedirectToAction("Expenses");
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
