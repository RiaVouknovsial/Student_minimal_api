using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Student_minimal_api.Auth;
using Student_minimal_api.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();     //добавляем в ручном режиме
builder.Services.AddSwaggerGen();               //добавляем в ручном режиме

//добавляем код для ауторизации и идентификации
builder.Services.AddSingleton<ITokenService>(new TokenService());
builder.Services.AddSingleton<IUserRepository>(new UserRepository());
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddTransient<GetRequestLoggingMiddleware>();
builder.Services.AddTransient<PostRequestLoggingMiddleware>();
builder.Services.AddTransient<RequestLoggingMiddleware>();


var app = builder.Build();

//app.UseRequestLogging_1();
//app.UseRequestLogging_2();
//app.UseRequestLogging_3();
app.UseMiddleware<HeaderValidationMiddleware>();
//app.UseMiddleware<GetRequestLoggingMiddleware>();
//app.UseMiddleware<PostRequestLoggingMiddleware>();
//app.UseMiddleware<RequestLoggingMiddleware>();

//добавляем использование ауторизации и идентификации
app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
app.UseSwagger();       //добавляем в ручном режиме
app.UseSwaggerUI();     //добавляем в ручном режиме
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
//    c.RoutePrefix = "swagger"; // Это уберет добавление /index.html в URL
//});

//для возврата токенап
app.MapGet("/login", [AllowAnonymous] async (HttpContext context,
    ITokenService tokenService, IUserRepository userRepository) => {
        UserDto userModel = new()
        {
            UserName = context.Request.Query["username"],
            Password = context.Request.Query["password"]
        };
        var userDto = userRepository.GetUser(userModel);
        if (userDto == null) return Results.Unauthorized();
        var token = tokenService.BuildToken(builder.Configuration["Jwt:Key"],
            builder.Configuration["Jwt:Issuer"], userDto);
        return Results.Ok(token);
    });

//app.UseHttpsRedirection();
var students = new List<StudentModel>();

//app.MapGet("/students", () => students);
//app.MapGet("/students/{id}", (int id) => students.FirstOrDefault(s => s.Id == id));
//app.MapPost("/students", (StudentModel student) => {
//    // Generate a unique ID for the new student
//    student.Id = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1;
//    students.Add(student);
//    return Results.Created($"/students/{student.Id}", student);
//});
//app.MapPut("/students/{id}", (int id, StudentModel updatedStudent) => {
//    var existingStudent = students.FirstOrDefault(s => s.Id == id);
//    if (existingStudent == null)
//    {
//        throw new Exception("Student not found");
//    }
//    // Update the existing student with the new data
//    existingStudent.Team = updatedStudent.Team;
//    existingStudent.TableNumber = updatedStudent.TableNumber;
//    existingStudent.LastName = updatedStudent.LastName;
//    existingStudent.FirstName = updatedStudent.FirstName;
//    existingStudent.SecondName = updatedStudent.SecondName;
//    return Results.Ok();
//});
//app.MapDelete("/students/{id}",  (int id) => {
//    var existingStudent = students.FirstOrDefault(s => s.Id == id);
//    if (existingStudent == null)
//    {
//        throw new Exception("Student not found");
//    }
//    students.Remove(existingStudent);
//    return Results.Ok();
//});

//изменяем верхний код, для проверки использования token при авторизации
app.MapGet("/students", [Authorize] () => students);
app.MapGet("/students/{id}", [Authorize] (int id) => students.FirstOrDefault(s => s.Id == id));
app.MapPost("/students", [Authorize] (StudentModel student) => {
    // Generate a unique ID for the new student
    student.Id = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1;
    students.Add(student);
    return Results.Created($"/students/{student.Id}", student);
});
app.MapPut("/students/{id}", [Authorize] (int id, StudentModel updatedStudent) => {
    var existingStudent = students.FirstOrDefault(s => s.Id == id);
    if (existingStudent == null)
    {
        throw new Exception("Student not found");
    }
    // Update the existing student with the new data
    existingStudent.Team = updatedStudent.Team;
    existingStudent.TableNumber = updatedStudent.TableNumber;
    existingStudent.LastName = updatedStudent.LastName;
    existingStudent.FirstName = updatedStudent.FirstName;
    existingStudent.SecondName = updatedStudent.SecondName;
    return Results.Ok();
});
app.MapDelete("/students/{id}", [Authorize] (int id) => {
    var existingStudent = students.FirstOrDefault(s => s.Id == id);
    if (existingStudent == null)
    {
        throw new Exception("Student not found");
    }
    students.Remove(existingStudent);
    return Results.Ok();
});

app.Run();

public class StudentModel
{
    public int Id { get; set; }
    public int Team { get; set; }
    public int TableNumber { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
}
