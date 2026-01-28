using Finance.Api.Data;
using Finance.Api.DTOs.Account;
using Finance.Api.DTOs.Transaction;
using Finance.Api.Models;
using Finance.Api.Services.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal.Finance.Api.Responses;
using System.Collections.Generic;

namespace Finance.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/account")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService service)
        {
            _accountService = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountRequest request)
        {
            var result = await _accountService.CreateAccountAsync(UserId, request);

            return Ok(ApiResponse<object>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _accountService.GetAllAccountAsync(UserId);

            return Ok(ApiResponse<object>.SuccessResponse(result));
        }
    }
}
