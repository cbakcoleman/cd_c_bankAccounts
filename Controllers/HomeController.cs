using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using cd_c_bankAccounts.Models;

namespace cd_c_bankAccounts.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult RegistrationPage()
        {
            return View("RegistrationPage");
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "The email address you entered is already in use.");

                    return View("RegistrationPage", user);
                }

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);

                _context.Add(user);
                _context.SaveChanges();

                return View("LoginPage");
            }
            else
            {
                return View("RegistrationPage", user);
            }
        }

        [HttpGet("loginpage")]
        public IActionResult LoginPage()
        {
            return View("LoginPage");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.Email);

                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("loginpage", userSubmission);
                }
                var hasher = new PasswordHasher<LoginUser>();

                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);

                if(result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("loginpage", userSubmission);
                }

                HttpContext.Session.SetInt32("loggedInUser", userInDb.UserId);

                return RedirectToAction("transactionspage", new {userId = userInDb.UserId});
            }
            else
            {
                return View("loginpage", userSubmission);
            }
        }

        [HttpGet("transactions/{userId}")]
        public IActionResult TransactionsPage(int userId)
        {
            if(HttpContext.Session.GetInt32("loggedInUser") == null)
            {
                return View("loginpage");
            }

            // User ToDisplay = _context.Users.
            //     FirstOrDefault(u => u.UserId == userId);
            //     if(ToDisplay == null)
            //     {
            //         return RedirectToAction("loginpage");
            //     }
            
            ViewBag.AllTransactions = _context.Transactions.
                Include(u => u.Creator).
                Where(u => u.Creator.UserId == userId).
                OrderByDescending(u => u.CreatedAt).
                ToList();

            

            return View("TransactionsPage");
        }

        [HttpPost("addtransaction")]
        public IActionResult AddTransaction(Transaction transaction)
        {
            if(ModelState.IsValid)
            {
                System.Console.WriteLine("MODEL STATE IS VALID");
                _context.Add(transaction);
                _context.SaveChanges();
                return RedirectToAction("TransactionsPage");
            }
            else
            {
                System.Console.WriteLine("MODEL STATE IS NOT VALID");
                return RedirectToAction("TransactionsPage", transaction);
            }
        }
        

    }
}