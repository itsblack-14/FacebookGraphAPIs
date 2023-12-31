namespace FacebookGraphAPIs.Features.FacebookLogin
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacebookLoginController : ControllerBase
    {
        private readonly IFacebookLoginService _service;

        public FacebookLoginController(IFacebookLoginService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> FacebookLogin(FacebookLoginRequest req)
        {
            try
            {
                var response = await _service.FacebookLogin(req);
                return StatusCode(response.StatusCode,response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
