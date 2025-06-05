using AutoMapper;
using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;

namespace N5Challenge.Api.Infraestructure.SQL;

public class Repository(AppDbContext context) : IRepository
{
    protected readonly AppDbContext _context = context;
}
