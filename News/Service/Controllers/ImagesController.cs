﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Service.Models;
using Persistence;

namespace Service.Controllers
{
    public class ImagesController : ApiController
    {
        private NewsModel db = new NewsModel();

        private INewsService _newsService = new NewsService(new NewsEntities());

        // GET: api/Images
        public IQueryable<Image> GetImages()
        {
            return db.Images;
        }

        // GET: api/Images/5
        [ResponseType(typeof(Image))]
        public IHttpActionResult GetImage(int id)
        {
            Image image = _newsService.GetImage(id);
            if (image == null)
            {
                return NotFound();
            }

            return Ok(image);
        }

        // PUT: api/Images/5
        [ResponseType(typeof(void))]
        [Authorize]
        public IHttpActionResult PutImage(int id, Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != image.Id)
            {
                return BadRequest();
            }

            db.Entry(image).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Images
        [ResponseType(typeof(Image))]
        [Authorize]
        public IHttpActionResult PostImage(Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _newsService.AddImage(image);

            return Created(Request.RequestUri + image.Id.ToString(), image.Id);
        }

        // DELETE: api/Images/5
        [ResponseType(typeof(Image))]
        [Authorize]
        public IHttpActionResult DeleteImage(int id)
        {
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return NotFound();
            }

            db.Images.Remove(image);
            db.SaveChanges();

            return Ok(image);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ImageExists(int id)
        {
            return db.Images.Count(e => e.Id == id) > 0;
        }
    }
}