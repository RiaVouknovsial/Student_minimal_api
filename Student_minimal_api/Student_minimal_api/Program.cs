var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();
var students = new List<StudentModel>();

app.MapGet("/students", () => students);
app.MapGet("/students/{id}", (int id) => students.FirstOrDefault(s => s.Id == id));
app.MapPost("/students", (StudentModel student) => {
    // Generate a unique ID for the new student
    student.Id = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1;
    students.Add(student);
    return Results.Created($"/students/{student.Id}", student);
});
app.MapPut("/students/{id}", (int id, StudentModel updatedStudent) => {
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
app.MapDelete("/students/{id}", (int id) => {
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
