using JournalAPI.Models;
using JournalAPI.Models.Test;
using JournalAPI.Repositories;
using JournalAPI.Services.Journal.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace JournalAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TestsController : ControllerBase
{
    private readonly ITestDataRepository repository;

    public TestsController(ITestDataRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public ActionResult GetAll()
    {
        var result = repository.GetAll();
        return Ok(new TestGetAllResponse(result));
    }

    [HttpGet("{id:int}")]
    [JournalGet<TestDataModel>]
    public ActionResult Get([FromRoute] int id)
    {
        var result = repository.Get(id);

        return result is not null
            ? Ok(new TestGetResponse(result))
            : NotFound(new BaseResponse(false));
    }

    [HttpPut("{id:int}")]
    [JournalUpdate<TestDataModel, TestUpdateRequest>(nameof(id), nameof(request))]
    public ActionResult Update([FromRoute] int id, [FromBody] TestUpdateRequest request)
    {
        var result = repository.Update(id, request);

        return result is not null 
            ? Ok(new TestUpdateResponse(result))
            : NotFound(new BaseResponse(false));
    }

    [HttpPost]
    public ActionResult Create([FromBody] TestCreateRequest request)
    { 
        var result = repository.Create(request);

        return CreatedAtAction(nameof(Get), new { id = result.Id }, new TestCreateResponse(result));
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete([FromRoute] int id)
    {
        var result = repository.Delete(id);

        return result is not null 
            ? Ok(new TestDeleteResponse(result))
            : NotFound(new BaseResponse(false));
    }
}
