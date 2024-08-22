using Microsoft.AspNetCore.Mvc;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

List<Menu> specialMenu = new List<Menu>
{
    new Menu(1,"Truffle Risotto", "Creamy risotto with white truffle and Parmesan cheese", "Main Course"),
    new Menu(2,"Miso-Glazed Black Cod", "Black cod marinated in miso, served with a side of pickled vegetables", "Main Course"),
    new Menu(3,"Saffron Lobster Bisque", "Rich and creamy lobster bisque infused with saffron", "Starter"),
    new Menu(4,"Foie Gras Terrine", "Foie gras terrine with fig jam and toasted brioche", "Starter"),
    new Menu(5,"Wagyu Beef Tartare", "Hand-chopped Wagyu beef with capers, shallots, and quail egg", "Starter"),
    new Menu(6,"Mango Sorbet", "Refreshing sorbet made from ripe mangoes", "Dessert"),
    new Menu(7,"Chocolate Fondant", "Warm chocolate cake with a molten center, served with vanilla ice cream", "Dessert"),
    new Menu(8,"Matcha Cheesecake", "Japanese-style cheesecake with matcha green tea powder", "Dessert"),
    new Menu(9,"Tuna Tataki", "Seared tuna with a sesame crust, served with ponzu sauce", "Starter"),
    new Menu(10,"Black Truffle Pasta", "Fresh pasta with black truffle, garlic, and Parmesan", "Main Course"),
    new Menu(11,"Caviar Blini", "Buckwheat blinis topped with caviar and crème fraîche", "Starter"),
    new Menu(12,"Szechuan Chili Prawns", "Spicy Szechuan-style prawns with garlic and chili oil", "Main Course"),
    new Menu(13,"Pistachio Baklava", "Layers of phyllo pastry with pistachios and honey syrup", "Dessert"),
    new Menu(14,"Charcoal Grilled Octopus", "Grilled octopus with smoked paprika and lemon", "Main Course"),
    new Menu(15,"French Onion Soup", "Classic French onion soup topped with a Gruyère cheese crouton", "Starter")
};

app.MapGet("/menu", () =>
{
    return Results.Ok(specialMenu);
})
.WithName("GetMenu")
.WithDescription("Get the whole menu")
.WithOpenApi();

app.MapGet("/menu/{id}/ingredients", (int id) =>
{
    return Results.Ok(specialMenu.SingleOrDefault(x => x.Id == id));
})
.WithName("GetMenuItemIngredients")
.WithDescription("Get Ingredients for a specific dish")
.WithOpenApi();

app.MapGet("/menu/{id}/price", (int id) =>
{
    return Results.Ok(new Random().Next(10, 100));
})
.WithName("GetMenuItemPrice")
.WithDescription("Get a specific menu item price")
.WithOpenApi();

app.Run();

public record Menu(int Id, string Name, string Ingredients, string Type);
