using System;
using Mango;

namespace PostilApp.Models
{
    public class PostilInfoModel
    {
        public Id _id { get; set; }
        public string Title { get; set; }
        public Id ProjectId { get; set; }
        public DateTime CreateTime { get; set; }
        public Id CreateUser { get; set; }
        public string[] Tags { get; set; }
        public bool IsPublic { get; set; }
        public Id FileId { get; set; }
        public float[] CameraMatrix { get; set; }
        public string ImageUrl { get; set; }
        public string FirstTag { get; set; }
        public string LastTime { get; set; }
        public string PostilUser { get; set; }
    }
}