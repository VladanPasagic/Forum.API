using Forum.API.DependancyInjection;
using Forum.API.Middleware;
using Forum.Core.Enums;
using Forum.EF;
using Forum.EF.Entities;
using Forum.SIEM.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<AccessControllerMiddleware>();
app.UseMiddleware<WAFMiddlerware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ForumContext>();
    dbContext.Database.EnsureCreated();
    dbContext.Database.Migrate();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    Role[] roles = (Role[])Enum.GetValues(typeof(Role));
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role.ToString()))
        {
            await roleManager.CreateAsync(new IdentityRole(role.ToString()));
        }
    }

    List<Category> categoriesToAdd = [new Category() { Name = "Nauka" }, new Category() { Name = "Kultura" }, new Category() { Name = "Sport" }, new Category() { Name = "Muzika" }];
    foreach (var category in categoriesToAdd)
    {
        if (!(await dbContext.Categories.Where(cat => cat.Name == category.Name).AnyAsync()))
        {
            await dbContext.Categories.AddAsync(category);
        }
    }
    await dbContext.SaveChangesAsync();

    IEnumerable<Category> categories = await dbContext.Categories.ToListAsync();
    foreach (var category in categories)
    {
        if (!(await dbContext.Permissions.Where(per => per.CategoryId == category.Id).AnyAsync()))
        {
            await dbContext.Permissions.AddAsync(new Permission() { CategoryId = category.Id, RequestType = RequestType.POST.ToString() });
            await dbContext.Permissions.AddAsync(new Permission() { CategoryId = category.Id, RequestType = RequestType.PUT.ToString() });
            await dbContext.Permissions.AddAsync(new Permission() { CategoryId = category.Id, RequestType = RequestType.DELETE.ToString() });
        }
    }
    await dbContext.SaveChangesAsync();

    var siemDbContext = scope.ServiceProvider.GetRequiredService<SiemContext>();
    siemDbContext.Database.EnsureCreated();
    siemDbContext.Database.Migrate();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors("MyAllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapIdentityApi<User>();
app.Run();
