﻿namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;
    using BlogSystem.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private const int PostsPerPageDefaultValue = 5;

        private readonly IRepository<BlogPost> blogPostsData;
        private readonly IRepository<Page> pagesData;

        public HomeController(IRepository<BlogPost> blogPosts, IRepository<Page> pages)
        {
            this.blogPostsData = blogPosts;
            this.pagesData = pages;
        }

        public ActionResult Index(int page = 1, int perPage = PostsPerPageDefaultValue)
        {
            var pagesCount = (int)Math.Ceiling(this.blogPostsData.All().Count() / (decimal)perPage);

            var posts = this.blogPostsData
                .All()
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedOn)
                .Project().To<BlogPostAnnotationViewModel>()
                .Skip(perPage * (page - 1))
                .Take(perPage);

            var model = new IndexViewModel
                            {
                                Posts = posts.ToList(),
                                CurrentPage = page,
                                PagesCount = pagesCount,
                            };

            return this.View(model);
        }

        /// <summary>
        /// Gets the robots.txt file.
        /// </summary>
        /// <returns>Returns a robots.txt file.</returns>
        [HttpGet]
        [OutputCache(Duration = 3600)]
        public FileResult RobotsTxt()
        {
            var robotsTxtContent = new StringBuilder();
            robotsTxtContent.AppendLine("User-Agent: *");
            robotsTxtContent.AppendLine("Allow: /");

            return this.File(Encoding.ASCII.GetBytes(robotsTxtContent.ToString()), "text/plain");
        }

        [ChildActionOnly]
        [OutputCache(Duration = 6 * 10 * 60)]
        public ActionResult Menu()
        {
            var menuItems = this.pagesData
                .All()
                .Where(p => !p.IsDeleted)
                .Project().To<MenuItemViewModel>()
                .ToList();

            return this.PartialView("_Menu", menuItems);
        }
    }
}