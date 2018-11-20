using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace feedBack
{

    public class LearningPlanFeedBack
    {
        public int LearningPlanFeedBackId { get; set; }
        public int LearningPlanId { get; set; }
        public int UserId { get; set; }
        // public double AverageStar { get; set;}
        public int Star { get; set; }
        public int subscribe { get; set; }
        // public int totalsubscriber{get;set;}
    }
    public class LearningPlan
    {
        // public int LearningPlanFeedBackId{get;set;}
        public int LearningPlanId { get; set; }
        public string technology { get; set; }

    }
    public class Resource
    {
        public int ResourceId { get; set; }
        public string RDescription { get; set; }
    }
    public class Question
    {
        public int QuestionId { get; set; }
        public string technology { get; set; }
    }
    public class ResourceFeedBack
    {

        public int ResourceId { get; set; }
        public int UserId { get; set; }
        // public double AverageStar { get; set;}
        public int Star { get; set; }

    }
    public class QuestionFeedBack
    {

        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public int Ambiguity { get; set; }

    }


    public class User
    {
        //change primitive type into class

        //change all user id into string
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //  public int Star {get; set;}
        // public byte[] ProfilePic{get;set;}
        public string Description { get; set; }
        // public bool subscribe {get; set;}        

        // public int totalSubscribe {get;set;}



    }
}

