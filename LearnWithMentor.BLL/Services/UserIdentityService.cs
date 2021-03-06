﻿using System;
using System.Security.Claims;
using System.Web;
using LearnWithMentorBLL.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace LearnWithMentorBLL.Services
{
    public class UserIdentityService:  IUserIdentityService
    {
        private readonly IHttpContextAccessor _accessor;

        public UserIdentityService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public int GetUserId()
        {
            var identity = _accessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return -1;
            }
            return int.Parse(identity.FindFirst("Id").Value);
        }

        public string GetUserRole()
        {
            var identity = _accessor.HttpContext.User.Identity as ClaimsIdentity;
            return identity == null ? "" : identity.FindFirst(identity.RoleClaimType).Value;
        }
    }
}
