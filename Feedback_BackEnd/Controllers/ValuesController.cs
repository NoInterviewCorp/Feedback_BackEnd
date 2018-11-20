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
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values

        GraphDbConnection client;
        public ValuesController(GraphDbConnection _client)
        {
            this.client = _client;

        }
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()

        {


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
        [HttpPost("Question")]
        public IActionResult QuestionPost([FromBody] Question newQuestion)
        {
            try
            {
                // save 
                client.client.Cypher
                .Merge("(question:Question { QuestionId: {id} })")
                .OnCreate()
                .Set("question = {newQuestion}")
                .WithParams(new
                {
                    id = newQuestion.QuestionId,
                    newQuestion
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
        [HttpPost("LearningPlanRating")]
        public async Task<IActionResult> UpdateRatingAsync([FromBody] LearningPlanFeedBack learningPlanFeedback)
        {
            GiveStarPayload giveStar = new GiveStarPayload { Rating = learningPlanFeedback.Star };
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
                    id = learningPlanFeedback.LearningPlanId,
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
            GiveStarPayload giveStar = new GiveStarPayload { Rating = resourceFeedBack.Star };
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
                    id = resourceFeedBack.ResourceId,
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
            GiveStarPayload LearningPlanSubscriber = new GiveStarPayload { Subscribe = learningPlanFeedback.subscribe };
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
                // .Match((GiveStarPayload sub)=>sub.Subscribe==1)
                .WithParams(new
                {
                    id = learningPlanFeedback.LearningPlanId,
                    // rating=
                })
               .Return<int>("count(g.Subscribe)")
                // .Return (g => Avg(g.As<GiveStarPayload>().Rating))
                .ResultsAsync;
            return Ok(new List<int>(totalsubscriber)[0]);
        }
        [HttpPost("QuestionReport")]
        public async Task<IActionResult> QuestionReportAsync([FromBody] QuestionFeedBack questionFeedBack)
        {
            GiveStarPayload QuestionReport = new GiveStarPayload { ambigous = questionFeedBack.Ambiguity };
            await client.client.Cypher
                .Match("(user:User)", "(qe:Question)")
                .Where((User user) => user.UserId == questionFeedBack.UserId)
                .AndWhere((Question qe) => qe.QuestionId == questionFeedBack.QuestionId)

                .Merge("(user)-[g:Question_Report]->(qe)")
                .OnCreate()
                .Set("g={QuestionReport}")
                .OnMatch()
                .Set("g={QuestionReport}")
                .WithParams(new
                {
                    userreport = questionFeedBack.Ambiguity,
                    QuestionReport
                })
                .ExecuteWithoutResultsAsync();
            var totalReport = await client.client.Cypher
               .Match("(:User)-[g:Question_Report]->(qe:Question {QuestionId:{id}})")
                // .Match((GiveStarPayload sub)=>sub.Subscribe==1)
                .WithParams(new
                {
                    id = questionFeedBack.QuestionId,
                    // rating=
                })
               .Return<int>("count(g.ambigous)")
                // .Return (g => Avg(g.As<GiveStarPayload>().Rating))
                .ResultsAsync;
            return Ok(new List<int>(totalReport)[0]);
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
