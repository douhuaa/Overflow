using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestionService.Data;
using QuestionService.Models;

namespace QuestionService.Controllers;

[ApiController]
[Route("[controller]")]
public class TagsController(QuestionDbContext db) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IReadOnlyList<Tag>>> GetTags(string? sort)
	{
		var query = db.Tags.AsQueryable();

		query = query.OrderBy(x => x.Name);

		return await query.ToListAsync();
	}
}
