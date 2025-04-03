using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Api.Services;

namespace RecipeBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeFeedbacksService : ControllerBase
    {
        private readonly IRecipeFeedbackService _recipeFeedbackService;
        public RecipeFeedbacksService(IRecipeFeedbackService recipeFeedbackService, IUserContextService userContextService)
        {
            _recipeFeedbackService = recipeFeedbackService;
        }
        [HttpGet]
        [Route("comments")]
        public async Task<IActionResult> GetComments(int recipeId)
        {
            var comments = await _recipeFeedbackService.GetCommentsAsync(recipeId);
            return Ok(comments);
        }

        [HttpPost]
        [Route("comments")]
        public async Task<IActionResult> PostComment(string commentContent, int recipeId)
        {
            var comment = await _recipeFeedbackService.AddCommentAsync(commentContent, recipeId);
            return Ok(comment);
        }

        [HttpGet]
        [Route("likes")]
        public async Task<IActionResult> GetLikes(int recipeId)
        {
            var likes = await _recipeFeedbackService.GetLikesAsync(recipeId);
            return Ok(likes);
        }

        [HttpPost]
        [Route("likes")]
        public async Task<IActionResult> PostLike(int recipeId)
        {
            var like = await _recipeFeedbackService.AddLikeAsync(recipeId);
            return Ok(like);
        }

    }
}
