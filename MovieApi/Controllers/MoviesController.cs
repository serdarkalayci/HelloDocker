using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieApi.MongoDatabase;
using MongoDB.Driver;
using MongoDB.Bson;
using MovieApi.Models;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMongoCollection<Movie> _movies;

        public MoviesController(IMongoDatabaseSettings mongoDatabaseSettings)
        {
            var client = new MongoClient(Environment.GetEnvironmentVariable("CONNSTR") ?? mongoDatabaseSettings.ConnectionString);
            var database = client.GetDatabase (Environment.GetEnvironmentVariable("DBNAME") ?? mongoDatabaseSettings.DatabaseName);
            _movies = database.GetCollection<Movie>(Environment.GetEnvironmentVariable("COLLNAME") ?? mongoDatabaseSettings.MoviesCollectionName);
        }

        [HttpGet]
        public ActionResult<List<Movie>> Get()
        {
            // Read all movie data from mongo
            List<Movie> movies = _movies.Find(m => true).ToList();

            // Return full list
            return movies;
        }

        [HttpGet("{id:length(24)}", Name = "GetMovie")]
        public ActionResult<Movie> Get(string id)
        {
            // Read all movie data from mongo
            Movie movie = _movies.Find(m => m.Id == id).FirstOrDefault();

            // Return movie
            return movie;
        }

    }
}