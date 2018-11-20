using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using feedBack.Services;
using feedBack;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Web.Http;
namespace feedBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values

        GraphDbConnection client;
        public ValuesController(GraphDbConnection _client)
        {
            this.client = _client;

        }
        [HttpPost("LearningPlanRating")]
        public async Task<IActionResult> UpdateRatingAsync([FromBody] LearningPlanFeedBack learningPlanFeedback)
        {
            GiveStarPayload giveStar = new GiveStarPayload{Rating=learningPlanFeedback.Star};
            await client.client.Cypher
                .Match("(user:User)", "(lp:LearningPlan)")
                .Where((User user) => user.UserId == learningPlanFeedback.UserId)
                .AndWhere((LearningPlan lp) => lp.LearningPlanId == learningPlanFeedback.LearningPlanId)
                .Merge("(user)-[g:LP_RATING]->(lp)")
                .OnCreate()
                .Set("g={giveStar}")
                .OnMatch()
                .Set("g={giveStar}")
                .WithParams(new
                {
                    userRating = learningPlanFeedback.Star,
                    giveStar
                })
                .ExecuteWithoutResultsAsync();
            var queryAvg = await client.client.Cypher
                .Match("(:User)-[g:LP_RATING]->(lp:LearningPlan {LearningPlanId:{id}})")
                .WithParams(new
                {
                        id= learningPlanFeedback.LearningPlanId,
                        // rating=
                })
                .Return<float>("avg(g.Rating)")
                // .Return (g => Avg(g.As<GiveStarPayload>().Rating))
                .ResultsAsync;
            return Ok(new List<float>(queryAvg)[0]);
        }
        [HttpPost("ResourceRating")]
        public async Task<IActionResult> UpdateResourceRatingAsync([FromBody] ResourceFeedBack resourceFeedBack)
        {
            GiveStarPayload giveStar = new GiveStarPayload{Rating=resourceFeedBack.Star};
            await client.client.Cypher
                .Match("(user:User)", "(Re:Resource)")
                .Where((User user) => user.UserId == resourceFeedBack.UserId)
                .AndWhere((Resource Re) => Re.ResourceId == resourceFeedBack.ResourceId)
                .Merge("(user)-[g:Resource_RATING]->(Re)")
                .OnCreate()
                .Set("g={giveStar}")
                .OnMatch()
                .Set("g={giveStar}")
                .WithParams(new
                {
                    userRating = resourceFeedBack.Star,
                    giveStar
                })
                .ExecuteWithoutResultsAsync();
            var queryAvg = await client.client.Cypher
                .Match("(:User)-[g:Resource_RATING]->(Re:Resource {ResourceId:{id}})")
                .WithParams(new
                {
                        id= resourceFeedBack.ResourceId,
                        // rating=
                })
                .Return<float>("avg(g.Rating)")
                // .Return (g => Avg(g.As<GiveStarPayload>().Rating))
                .ResultsAsync;
            return Ok(new List<float>(queryAvg)[0]);
        }
         [HttpPost("LearningPlanSubscriber")]
        public async Task<IActionResult> UpdateSubscriberAsync([FromBody] LearningPlanFeedBack learningPlanFeedback)
        {
            GiveStarPayload LearningPlanSubscriber = new GiveStarPayload{Subscribe=learningPlanFeedback.subscribe};
            await client.client.Cypher
                .Match("(user:User)", "(lp:LearningPlan)")
                .Where((User user) => user.UserId == learningPlanFeedback.UserId)
                .AndWhere((LearningPlan lp) => lp.LearningPlanId == learningPlanFeedback.LearningPlanId)
                .Merge("(user)-[g:LP_Subscribe]->(lp)")
                .OnCreate()
                .Set("g={LearningPlanSubscriber}")
                .OnMatch()
                .Set("g={LearningPlanSubscriber}")
                .WithParams(new
                {
                    usersubscribe = learningPlanFeedback.subscribe,
                    LearningPlanSubscriber
                })
                .ExecuteWithoutResultsAsync();
            var totalsubscriber = await client.client.Cypher
                .Match("(:User)-[g:LP_Subscribe]->(lp:LearningPlan {LearningPlanId:{id}})")
                .WithParams(new
                {
                        id= learningPlanFeedback.LearningPlanId,
                        // rating=
                })
                .Return<int>("count(g.Subscribe")
                // .Return (g => Avg(g.As<GiveStarPayload>().Rating))
                .ResultsAsync;
            return Ok(new List<int>(totalsubscriber)[0]);
        }
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()

        {


            /*    IEnumerable<string> results = client.client.Cypher
                .Match("(p:Person)-[:ACTED_IN]->(m:Movie {title: 'Top Gun'})")
                .Return<string>("p.name")
                   .Results;*/
            var results1 = client.client.Cypher
             .Match("(user:User)")
             .Return(user => user.As<User>())
             .Results;
            return Ok(results1);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var results = client.client.Cypher
           .Match("(user:User)")
           .Where((User user) => user.UserId == id)
           .Return(user => user.As<User>())
           .Results;
            return Ok(results);
        }

        // POST api/values
        [HttpPost("UserNode")]
        public IActionResult UserPost([FromBody] User newUser)
        {
            try
            {
                // save 
                client.client.Cypher
                .Merge("(user:User { UserId: {id} })")
                .OnCreate()
                .Set("user = {newUser}")
                .WithParams(new
                {
                    id = newUser.UserId,
                    newUser
                })
              .ExecuteWithoutResults();
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
            //newUser =new  List<User>();


        }
        [HttpPost("LearningPlan")]
        public IActionResult LPPost([FromBody] LearningPlan newLP)
        {
            try
            {
                // save 
                client.client.Cypher
                .Merge("(LP:LearningPlan { LearningPlanId: {id} })")
                .OnCreate()
                .Set("LP = {newLP}")
                .WithParams(new
                {
                    id = newLP.LearningPlanId,
                    newLP
                })
              .ExecuteWithoutResults();
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
            //newUser =new  List<User>();


        }

 [HttpPost("Resource")]
        public IActionResult ResourcePost([FromBody] Resource newResource)
        {
            try
            {
                // save 
                client.client.Cypher
                .Merge("(resource:Resource { ResourceId: {id} })")
                .OnCreate()
                .Set("resource = {newResource}")
                .WithParams(new
                {
                    id = newResource.ResourceId,
                    newResource
                })
              .ExecuteWithoutResults();
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
            //newUser =new  List<User>();


        }

    
        [HttpPost("Uploads")]

        public async Task<IActionResult> Uploads(IFormFileCollection files)
        {

            long size = files.Sum(f => f.Length);
            try
            {
                foreach (var formFile in files)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./ProfilePic/Image", formFile.FileName);
                    var stream = new FileStream(filePath, FileMode.Create);
                    await formFile.CopyToAsync(stream);


                }
                return Ok(new { count = files.Count });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User newUser)
        {
            try
            {
                client.client.Cypher
              .Match("(user:User)")
              .Where((User user) => user.UserId == id)
               .Set("user = {newUser}")
                    .WithParams(new
                    {
                        newUser
                    })
               .ExecuteWithoutResults();
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            client.client.Cypher
           .Match("(user:User)")
           .Where((User user) => user.UserId == id)
           .Delete("user")

           .ExecuteWithoutResults();
        }
    }
}
