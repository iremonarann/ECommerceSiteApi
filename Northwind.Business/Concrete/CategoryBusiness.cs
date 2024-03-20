using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Northwind.Business.Abstract;
using Northwind.Business.Data.Contexts;
using Northwind.Business.Dtos;

namespace Northwind.Business.Concrete;

public class CategoryBusiness : ICategoryService
{
    private readonly NorthwindContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CategoryBusiness> _logger;

    public CategoryBusiness(NorthwindContext context, IMapper mapper, IConfiguration configuration, ILogger<CategoryBusiness> logger)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var data = await _context.Categories.ToListAsync();

        var connStr = _configuration.GetConnectionString("NorthwindConnection");
        _logger.LogCritical(connStr);

        return _mapper.Map<IEnumerable<CategoryDto>>(data);
    }
}
