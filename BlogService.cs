using System.Collections.Generic;
using System.Linq;

namespace Blog
{
    public class BlogService
    {
        public static IEnumerable<string> NumberOfCommentsPerUser(MyDbContext context)
        {
            return context.BlogComments
                .GroupBy(comments => comments.UserName)
                .Select(commentsByUser => $"{commentsByUser.Key}: {commentsByUser.Count()}")
                .AsEnumerable();
        }

       public static IEnumerable<string> PostsOrderedByLastCommentDate(MyDbContext context)
        {
            return context.BlogPosts
                .SelectMany(Post => Post.Comments
                    .Select(Comment => Comment)
                    .OrderByDescending(Comment => Comment.CreatedDate)
                    .Take(1)
                    .Select(Comment => new{Comment.BlogPostId,Comment.CreatedDate, Comment.Text}))
                    .OrderByDescending(Comment => Comment.CreatedDate)
                    .Select(Comment => $"Post{Comment.BlogPostId}, CreatedDate: {Comment.CreatedDate}, Text: {Comment.Text}")
                .AsEnumerable();
        }
       

        public static IEnumerable<string> NumberOfLastCommentsLeftByUser(MyDbContext context)
        {
            return context.BlogComments.AsEnumerable()
                .GroupBy(comment => comment.BlogPostId)
                .SelectMany(commentsInPost => commentsInPost.Select(comment => new { comment.UserName, comment.CreatedDate }).OrderByDescending(comment => comment.CreatedDate).Take(1))
                .AsEnumerable()
                .GroupBy(lastComments => lastComments.UserName)
                .Select(lastComments => $"{lastComments.Key}:{lastComments.Count()}");
        }
    }
}
