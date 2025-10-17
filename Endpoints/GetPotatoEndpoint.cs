using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PotatoProject2.Data;
using PotatoProject2.Models;

namespace PotatoProject2.Endpoints.Potatoes;

public class GetPotatoesEndpoint : EndpointWithoutRequest
{
    private readonly AppDbContext _context;

    public GetPotatoesEndpoint(AppDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/api/potatoes");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var potatoes = await _context.Potatoes
            .OrderByDescending(p => p.Id)
            .ToListAsync(ct);

        HttpContext.Response.StatusCode = 200;
        await HttpContext.Response.WriteAsJsonAsync(potatoes, cancellationToken: ct);
    }
}
