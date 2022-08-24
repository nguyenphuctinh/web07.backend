using MISA.Web07.GD.NPTINH.BL;
using MISA.Web07.GD.NPTINH.BL.RoomBL;
using MISA.Web07.GD.NPTINH.BL.SubjectBL;
using MISA.Web07.GD.NPTINH.DL;
using MISA.Web07.GD.NPTINH.DL.RoomDL;
using MISA.Web07.GD.NPTINH.DL.SubjectDL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddScoped<ITeacherDL, TeacherDL>();
builder.Services.AddScoped<IGroupDL, GroupDL>();
builder.Services.AddScoped<ISubjectDL, SubjectDL>();
builder.Services.AddScoped<IRoomDL, RoomDL>();
builder.Services.AddScoped<ITeacherBL, TeacherBL>();
builder.Services.AddScoped<IGroupBL, GroupBL>();
builder.Services.AddScoped<ISubjectBL, SubjectBL>();
builder.Services.AddScoped<IRoomBL, RoomBL>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
