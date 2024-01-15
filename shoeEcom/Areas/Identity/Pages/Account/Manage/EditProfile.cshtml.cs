using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace shoeEcom.Areas.Identity.Pages.Account.Manage
{
    public class EditProfileModel : PageModel
    {
        private readonly IUniteOfWork _uniteOfWork;
        public EditProfileModel(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public void OnGet(string userId)
        {
            ApplicationUser = _uniteOfWork.ApplicationUser.Get(u => u.Id == userId);
        }

        public IActionResult OnPost()
        {
            var currentUser = _uniteOfWork.ApplicationUser.Get(u => u.Id == ApplicationUser.Id);

            if(currentUser == null)
            {
                return BadRequest();
            }


            currentUser.FirstName = ApplicationUser.FirstName ?? currentUser.FirstName;
            currentUser.LastName = ApplicationUser.LastName ?? currentUser.LastName;
            currentUser.PhoneNumber = ApplicationUser.PhoneNumber ?? currentUser.PhoneNumber;
            currentUser.City = ApplicationUser.City ?? currentUser.City;
            currentUser.Country = ApplicationUser.Country ?? currentUser.Country;
            currentUser.State = ApplicationUser.State ?? currentUser.State;
            currentUser.Address = ApplicationUser.Address ?? currentUser.Address;
            currentUser.PostalCode = ApplicationUser.PostalCode ?? currentUser.PostalCode;


            _uniteOfWork.ApplicationUser.Update(currentUser);
            _uniteOfWork.Save();
            TempData["success"] = $"Your profile has been updated {ApplicationUser.FirstName}";
            return RedirectToPage("Index", "Manage");
        }
    }
}
