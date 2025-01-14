﻿using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace spaCommerce.Models.Blogs
{
    public partial class AddBlogCommentModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Blog.Comments.CommentText")]
        public string CommentText { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}