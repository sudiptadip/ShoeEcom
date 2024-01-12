// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace shoeEcom.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUniteOfWork _uniteOfWork;

        public IndexModel( UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUniteOfWork uniteOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _uniteOfWork = uniteOfWork;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public List<OrderItem> OrderItem { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            var aplicationUser = _uniteOfWork.ApplicationUser.Get(u => u.Id == userId);

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = aplicationUser.FirstName,
                LastName = aplicationUser.LastName,
                Email = aplicationUser.Email,
                OrderItem = (List<OrderItem>)_uniteOfWork.OrderItem.GetAll(u => u.ApplicationUserId == userId, 
                includeProperty: "Product,ApplicationUser,OrderAddress," +
                "Product.Category,Product.ProductImages")
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            if(Input.OrderItem.Count > 0)
            {
                Input.OrderItem = Input.OrderItem.OrderByDescending(m => m.Id).ToList();
            }
            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
