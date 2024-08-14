using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Operations;
using ConsoleCalculator.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddTransient<IOperation, AddOperation>();
builder.Services.AddTransient<IOperation, SubtractOperation>();
builder.Services.AddTransient<IOperation, MultiplyOperation>();
builder.Services.AddTransient<IOperation, DivideOperation>();

builder.Services.AddTransient<ITokenizer, Tokenizer>();
builder.Services.AddTransient<IParser, Parser>();
builder.Services.AddTransient<IEvaluator, Evaluator>();
builder.Services.AddSingleton<ICalculator, Calculator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
