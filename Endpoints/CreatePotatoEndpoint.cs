using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PotatoProject2.Data;
using PotatoProject2.Models;

namespace PotatoProject2.Endpoints.Potatoes;

public class CreatePotatoRequest
{
    public string Description { get; set; } = string.Empty;
}

public class CreatePotatoEndpoint : Endpoint<CreatePotatoRequest>
{
    private readonly AppDbContext _context;

    public CreatePotatoEndpoint(AppDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Post("/api/potato");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
    }


    public override async Task HandleAsync(CreatePotatoRequest req, CancellationToken ct)
    {
        var potato = new Potato { Description = req.Description };
        _context.Potatoes.Add(potato);
        await _context.SaveChangesAsync(ct);
        await HttpContext.Response.WriteAsJsonAsync(potato, cancellationToken: ct);
    }
}
