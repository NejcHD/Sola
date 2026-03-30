using ARHI_VAJAZAse2.Data;
using ARHI_VAJAZAse2.DTOs;
using ARHI_VAJAZAse2.Modeli;

var builder = WebApplication.CreateBuilder(args);

// DODAJ LOGGING
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<LibraryContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<LibraryContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// BOOK ENDPOINTS

// STARI NAČIN - direktno z Modeli
app.MapGet("/api/books", (LibraryContext context) => {
    return Results.Ok(context.Books.ToList());
});

app.MapGet("/api/books/{id}", (int id, LibraryContext context) => {
    var book = context.Books.Find(id);
    return book != null ? Results.Ok(book) : Results.NotFound();
});

// STARI NAČIN - direktno z Model (manj varno)
app.MapPost("/api/books", (Book book, LibraryContext context) => {
    context.Books.Add(book);
    context.SaveChanges();
    return Results.Created($"/api/books/{book.Id}", book);
});

// NOVI NAČIN - z DTO (bolj varno)
app.MapPost("/api/books/dto", (CreateBookDto bookDto, LibraryContext context, ILogger<Program> logger) => {
    try
    {
        logger.LogInformation("Dodajanje nove knjige: {Title}", bookDto.Title);

        var book = new Book
        {
            Title = bookDto.Title,
            Year = bookDto.Year,
            Genra = bookDto.Genra,
            Language = "Slovenščina",
           
        };

        context.Books.Add(book);
        context.SaveChanges();

        logger.LogInformation("Knjiga uspešno dodana z ID: {BookId}", book.Id);
        return Results.Created($"/api/books/{book.Id}", book);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Napaka pri dodajanju knjige: {Title}", bookDto.Title);
        return Results.Problem($"Napaka: {ex.Message}");
    }
});

app.MapPut("/api/books/{id}/dto", (int id, UpdateBookDto bookDto, LibraryContext context, ILogger<Program> logger) => {
    try
    {
        logger.LogInformation("Posodabljanje knjige z ID: {BookId}", id);

        var book = context.Books.Find(id);
        if (book == null)
        {
            logger.LogWarning("Knjiga z ID {BookId} ne obstaja", id);
            return Results.NotFound();
        }

        book.Title = bookDto.Title;
        book.Year = bookDto.Year;
        book.Genra = bookDto.Genra;

        context.SaveChanges();

        logger.LogInformation("Knjiga z ID {BookId} uspešno posodobljena", id);
        return Results.Ok(book);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Napaka pri posodabljanju knjige z ID: {BookId}", id);
        return Results.Problem($"Napaka: {ex.Message}");
    }
});

// SEARCH endpoint
app.MapGet("/api/books/search", (string? title, string? genra, LibraryContext context) => {
    var query = context.Books.AsQueryable();

    if (!string.IsNullOrEmpty(title))
        query = query.Where(b => b.Title.Contains(title));

    if (!string.IsNullOrEmpty(genra))
        query = query.Where(b => b.Genra.Contains(genra));

    return Results.Ok(query.ToList());
});





app.MapPut("/api/books/{id}", (int id, Book updatedBook, LibraryContext context) => {
    var book = context.Books.Find(id);
    if (book == null) return Results.NotFound();

    book.Title = updatedBook.Title;
    book.Year = updatedBook.Year;
    book.Genra = updatedBook.Genra;
    book.Language = updatedBook.Language;

    context.SaveChanges();
    return Results.Ok(book);
});

app.MapDelete("/api/books/{id}", (int id, LibraryContext context) => {
    var book = context.Books.Find(id);
    if (book == null) return Results.NotFound();

    context.Books.Remove(book);
    context.SaveChanges();
    return Results.NoContent();
});

// AUTHOR ENDPOINTS

// STARI NAČIN
app.MapGet("/api/authors", (LibraryContext context) => {
    return Results.Ok(context.Author.ToList());
});

app.MapGet("/api/authors/{id}", (int id, LibraryContext context) => {
    var author = context.Author.Find(id);
    return author != null ? Results.Ok(author) : Results.NotFound();
});

// STARI NAČIN
app.MapPost("/api/authors", (Author author, LibraryContext context) => {
    context.Author.Add(author);
    context.SaveChanges();
    return Results.Created($"/api/authors/{author.Id}", author);
});

// NOVI NAČIN - z DTO
app.MapPost("/api/authors/dto", (CreateAuthorDto authorDto, LibraryContext context) => {
    var author = new Author
    {
        Name = authorDto.Name,
        LastName = authorDto.LastName,
        Country = authorDto.Country
    };

    context.Author.Add(author);
    context.SaveChanges();
    return Results.Created($"/api/authors/{author.Id}", author);
});

app.MapPut("/api/authors/{id}", (int id, Author updatedAuthor, LibraryContext context) => {
    var author = context.Author.Find(id);
    if (author == null) return Results.NotFound();

    author.Name = updatedAuthor.Name;
    author.LastName = updatedAuthor.LastName;
    author.Country = updatedAuthor.Country;

    context.SaveChanges();
    return Results.Ok(author);
});

app.MapDelete("/api/authors/{id}", (int id, LibraryContext context) => {
    var author = context.Author.Find(id);
    if (author == null) return Results.NotFound();

    context.Author.Remove(author);
    context.SaveChanges();
    return Results.NoContent();
});

// RENTAL ENDPOINTS

// STARI NAČIN
app.MapGet("/api/rentals", (LibraryContext context) => {
    return Results.Ok(context.Rentals.ToList());
});

app.MapGet("/api/rentals/{id}", (int id, LibraryContext context) => {
    var rental = context.Rentals.Find(id);
    return rental != null ? Results.Ok(rental) : Results.NotFound();
});

// STARI NAČIN
app.MapPost("/api/rentals", (Rental rental, LibraryContext context) => {
    context.Rentals.Add(rental);
    context.SaveChanges();
    return Results.Created($"/api/rentals/{rental.Id}", rental);
});

// NOVI NAČIN - z DTO
app.MapPost("/api/rentals/dto", (CreateRentalDto rentalDto, LibraryContext context) => {
    var rental = new Rental
    {
        RentalDate = rentalDto.RentalDate,
        ReturnDate = rentalDto.ReturnDate,
        BookId = rentalDto.BookId,
        AuthorId = rentalDto.AuthorId
    };

    context.Rentals.Add(rental);
    context.SaveChanges();
    return Results.Created($"/api/rentals/{rental.Id}", rental);
});

app.MapPut("/api/rentals/{id}", (int id, Rental updatedRental, LibraryContext context) => {
    var rental = context.Rentals.Find(id);
    if (rental == null) return Results.NotFound();

    rental.RentalDate = updatedRental.RentalDate;
    rental.ReturnDate = updatedRental.ReturnDate;
    rental.BookId = updatedRental.BookId;
    rental.AuthorId = updatedRental.AuthorId;

    context.SaveChanges();
    return Results.Ok(rental);
});

app.MapDelete("/api/rentals/{id}", (int id, LibraryContext context) => {
    var rental = context.Rentals.Find(id);
    if (rental == null) return Results.NotFound();

    context.Rentals.Remove(rental);
    context.SaveChanges();
    return Results.NoContent();
});

app.Run();
