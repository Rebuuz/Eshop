﻿

using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ContactInformationRepo(UserContext userContext) : BaseRepo<ContactInformationEntity>(userContext)
{
    private readonly UserContext _userContext = userContext;

}