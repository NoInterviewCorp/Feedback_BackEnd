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
using Microsoft.AspNetCore.StaticFiles;
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
        [HttpGet]
        public async Task<IActionResult> Get()

        {


            var userResults = await client.client.Cypher
             .Match("(user:User)")
             .Return(user => user.As<User>())
             .ResultsAsync;
            return Ok(userResults);
        }

        //Get user by id
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var userResult = await client.client.Cypher
           .Match("(user:User)")
           .Where((User user) => user.UserId == userId)
           .Return(user => user.As<User>())
           .ResultsAsync;
            return Ok(userResult);
        }
        //Get learningplan by id
        [HttpGet("LearningPlan/{learningPlanId}")]
        public async Task<IActionResult> GetLearningPlanById(int learningplanId)
        {
            var learningPlanResult = await client.client.Cypher
          .Match("(LP:LearningPlan)")
          .Where((LearningPlan LP) => LP.LearningPlanId == learningplanId)
          .Return(LP => LP.As<LearningPlan>())
          .ResultsAsync;
            return Ok(learningPlanResult);

        }
        //Get resource by id
        [HttpGet("Resource/{resourceId}")]
        public async Task<IActionResult> GetResourceById(int resourceId)
        {
            var resourceResult = await client.client.Cypher
          .Match("(Re:Resource)")
          .Where((Resource Re) => Re.ResourceId == resourceId)
          .Return(Re => Re.As<Resource>())
          .ResultsAsync;
            return Ok(resourceResult);

        }
        //Get question by id
        [HttpGet("Question/{questionId}")]
        public async Task<IActionResult> GetQuestionById(int questionId)
        {
            var questionResult = await client.client.Cypher
          .Match("(qe:Question)")
          .Where((Question qe) => qe.QuestionId == questionId)
          .Return(qe => qe.As<Question>())
          .ResultsAsync;
            return Ok(questionResult);

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
        //post learningplan
        [HttpPost("UploadLearningPlan")]
        public IActionResult LPPost([FromBody] LearningPlan newLearningPlan)
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
                    id = newLearningPlan.LearningPlanId,
                    newLearningPlan
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
        //post resources
        [HttpPost("UploadResource")]
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
        //post questions
        [HttpPost("UploadQuestion")]
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
        //Rate a learningplan
        [HttpPost("RatingLearningPlan")]
        public async Task<IActionResult> RatingLearningPlanAsync([FromBody] LearningPlanFeedBack learningPlanFeedback)
        {
            GiveStarPayload giveStar = new GiveStarPayload { Rating = learningPlanFeedback.Star };
            await client.client.Cypher
                .Match("(user:User)", "(lp:LearningPlan)")
                .Where((User user) => user.UserId == learningPlanFeedback.UserId)
                .AndWhere((LearningPlan lp) => lp.LearningPlanId == learningPlanFeedback.LearningPlanId)
                .Merge("(user)-[g:RATING_LP]->(lp)")
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
            var LPqueryAvg = await client.client.Cypher
                .Match("(:User)-[g:RATING_LP]->(lp:LearningPlan {LearningPlanId:{id}})")
                .With("lp,  avg(g.Rating) as avg_rating ")
                .Set("lp.AvgRating = avg_rating")
                .WithParams(new
                {
                    id = learningPlanFeedback.LearningPlanId,
                    // rating=
                })
                .Return<float>("lp.AvgRating")
                .ResultsAsync;
            // .Return (g => Avg(g.As<GiveStarPayload>().Rating))
            return Ok(new List<float>(LPqueryAvg)[0]);
        }
        //Rate a resource
        [HttpPost("RatingResource")]
        public async Task<IActionResult> RatingResourceAsync([FromBody] ResourceFeedBack resourceFeedBack)
        {
            GiveStarPayload giveStar = new GiveStarPayload { Rating = resourceFeedBack.Star };
            await client.client.Cypher
                .Match("(user:User)", "(Re:Resource)")
                .Where((User user) => user.UserId == resourceFeedBack.UserId)
                .AndWhere((Resource Re) => Re.ResourceId == resourceFeedBack.ResourceId)
                .Merge("(user)-[g:RATING_Resource]->(Re)")
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
            var Re_queryAvg = await client.client.Cypher
                .Match("(:User)-[g:RATING_Resource]->(Re:Resource {ResourceId:{id}})")
                .With("Re,  avg(g.Rating) as avg_rating ")
                .Set("Re.AvgRating = avg_rating")
                .WithParams(new
                {
                    id = resourceFeedBack.ResourceId,
                    // rating=
                })
                .Return<float>("Re.AvgRating)")
                // .Return (g => Avg(g.As<GiveStarPayload>().Rating))
                .ResultsAsync;
            return Ok(new List<float>(Re_queryAvg)[0]);
        }
        //subscribe a learning plan
        [HttpPost("SubscriberLearningPlan")]
        public async Task<IActionResult> SubscriberLearningPlanAsync([FromBody] LearningPlanFeedBack learningPlanFeedback)
        {
            GiveStarPayload LearningPlanSubscriber = new GiveStarPayload { Subscribe = learningPlanFeedback.Subscribe };
            await client.client.Cypher
                .Match("(user:User)", "(lp:LearningPlan)")
                .Where((User user) => user.UserId == learningPlanFeedback.UserId)
                .AndWhere((LearningPlan lp) => lp.LearningPlanId == learningPlanFeedback.LearningPlanId)

                .Merge("(user)-[g:Subscribe_LP]->(lp)")
                .OnCreate()
                .Set("g={LearningPlanSubscriber}")
                .OnMatch()
                .Set("g={LearningPlanSubscriber}")
                .WithParams(new
                {
                    usersubscribe = learningPlanFeedback.Subscribe,
                    LearningPlanSubscriber
                })
                .ExecuteWithoutResultsAsync();
            var totalsubscriber = await client.client.Cypher
               .Match("(:User)-[g:Subscribe_LP]->(lp:LearningPlan {LearningPlanId:{id}})")
                // .Match((GiveStarPayload sub)=>sub.Subscribe==1)
                .With("lp,  count(g.Subscribe) as total_subscriber ")
                .Set("lp.Subscriber = total_subscriber")
                .WithParams(new
                {
                    id = learningPlanFeedback.LearningPlanId,
                    // rating=
                })
               .Return<int>("lp.Subscriber")
                // .Return (g => Avg(g.As<GiveStarPayload>().Rating))
                .ResultsAsync;
            return Ok(new List<int>(totalsubscriber)[0]);
        }

        [HttpPost("UnSubscriberLearningPlan")]
        public async Task<IActionResult> UnSubscriberLearningPlanAsync([FromBody] LearningPlanFeedBack learningPlanFeedback)
        {
            GiveStarPayload LearningPlanSubscriber = new GiveStarPayload { Subscribe = learningPlanFeedback.Subscribe };
            await client.client.Cypher
                .Match("(user:User)", "(lp:LearningPlan)")
                .Where((User user) => user.UserId == learningPlanFeedback.UserId)
                .AndWhere((LearningPlan lp) => lp.LearningPlanId == learningPlanFeedback.LearningPlanId)

                .Merge("(user)-[g:Subscribe_LP]->(lp)")
                .Delete("g")
                .ExecuteWithoutResultsAsync();
            var totalsubscriber = await client.client.Cypher
               .Match("(:User)-[g:Subscribe_LP]->(lp:LearningPlan {LearningPlanId:{id}})")
                // .Match((GiveStarPayload sub)=>sub.Subscribe==1)
                .With("lp,  count(g.Subscribe) as total_subscriber ")
                .Set("lp.Subscriber = total_subscriber")
                .WithParams(new
                {
                    id = learningPlanFeedback.LearningPlanId,
                    // rating=
                })
               .Return<int>("lp.Subscriber")
                // .Return (g => Avg(g.As<GiveStarPayload>().Rating))
                .ResultsAsync;
            return Ok(new List<int>(totalsubscriber)[0]);
        }
        //Repost a question
        [HttpPost("ReportQuestion")]
        public async Task<IActionResult> ReportQuestionAsync([FromBody] QuestionFeedBack questionFeedBack)
        {
            GiveStarPayload QuestionReport = new GiveStarPayload { Ambigous = questionFeedBack.Ambiguity };
            await client.client.Cypher
                .Match("(user:User)", "(qe:Question)")
                .Where((User user) => user.UserId == questionFeedBack.UserId)
                .AndWhere((Question qe) => qe.QuestionId == questionFeedBack.QuestionId)

                .Merge("(user)-[g:Report_Question]->(qe)")
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
               .Match("(:User)-[g:Report_Question]->(qe:Question {QuestionId:{id}})")
                // .Match((GiveStarPayload sub)=>sub.Subscribe==1)
                .With("qe,  count(g.ambigous) as total_ambiguity ")
                .Set("qe.Total_Ambiguity = total_ambiguity")
                .WithParams(new
                {
                    id = questionFeedBack.QuestionId,
                    // rating=
                })
               .Return<int>("qe.Total_Ambiguity")
                // .Return (g => Avg(g.As<GiveStarPayload>().Rating))
                .ResultsAsync;
            return Ok(new List<int>(totalReport)[0]);
        }


        // PUT api/values/5
        //update a user details
        [HttpPut("user/{id}")]
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
        // delete a user

        [HttpDelete("user/{id}")]
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
