using Microsoft.EntityFrameworkCore;
using RecipeBook.Api.Data;
using RecipeBook.Api.Entities;

namespace RecipeBook.Api.Services
{
    public interface IRecipeFeedbackService
    {
        Task<string> AddCommentAsync(string commentContent, int recipeId);
        Task<string> AddLikeAsync(int recipeId);
        Task<List<Comment>> GetCommentsAsync(int recipeId);
        Task<List<Like>> GetLikesAsync(int recipeId);
        Task<bool> DeleteCommentAsync(int recipeId);
        Task<bool> DeleteLikeAsync(int recipeId);
    }
    public class RecipeFeedbackService : IRecipeFeedbackService
    {
        private readonly AppDbContext _context;
        private readonly IUserContextService _userContextService;

        public RecipeFeedbackService(AppDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        public async Task<string> AddCommentAsync(string commentContent, int recipeId)
        {
            if (commentContent == null)
            {
                throw new Exception("Empty comment");
            }

            var comment = new Comment
            {
                Content = commentContent,
                UserId = _userContextService.GetUserId(),
                RecipeId = recipeId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return $"{commentContent} added to destinated recipe.";
        }

        public async Task<List<Comment>> GetCommentsAsync(int recipeId)
        {
            return await _context.Comments
                .Where(c => c.RecipeId == recipeId)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<bool> DeleteCommentAsync(int recipeId)
        {
            var userId = _userContextService.GetUserId();
            var comment = await _context.Comments.FindAsync(userId,recipeId);
            if (comment == null)
            {
                throw new Exception("Comment not found.");
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> AddLikeAsync(int recipeId)
        {
            var userId = _userContextService.GetUserId();
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.RecipeId == recipeId);
            if (existingLike != null)
            {
                throw new Exception("You already liked this recipe");
            }

            var like = new Like
            {
                UserId = userId,
                RecipeId = recipeId
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
            return "Recipe liked";
        }

        public async Task<List<Like>> GetLikesAsync(int recipeId)
        {
            return await _context.Likes
                .Where(l => l.RecipeId == recipeId)
                .Include(l => l.User)
                .ToListAsync();
        }

        public async Task<bool> DeleteLikeAsync(int recipeId)
        {
            var userId = _userContextService.GetUserId();
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.RecipeId == recipeId);
            if (like == null)
            {
                throw new Exception("Like not found.");
            }
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
