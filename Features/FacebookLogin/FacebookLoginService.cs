namespace FacebookGraphAPIs.Features.FacebookLogin
{
    public interface IFacebookLoginService
    {
        Task<FacebookLoginResponse> FacebookLogin (FacebookLoginRequest request);
    }

    public class FacebookLoginService : IFacebookLoginService
    {
        private readonly FacebookAuthContext _context; 
        private readonly IConfiguration _configuration;

        public FacebookLoginService(FacebookAuthContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<FacebookLoginResponse> FacebookLogin(FacebookLoginRequest req)
        {
            try
            {
                //Get Account Infos from Facebook
                string baseUrl = "https://graph.facebook.com/";
                baseUrl += "me?fields=id,name,email&access_token=" + req.Access_token;

                HttpClient client = new HttpClient();
                var getJob = await client.GetAsync(baseUrl);

                if(!getJob.IsSuccessStatusCode)
                {
                    return new FacebookLoginResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Fail"
                    };
                }

                var userData = JsonConvert.DeserializeObject<FacebookUserData>(
                    await getJob.Content.ReadAsStringAsync());

                if(userData == null)
                {
                    return new FacebookLoginResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "User Data Extraction Fail"
                    };
                }

                //Login or registration
                var existUser = await _context.User.Where(x => x.Email == userData.email
                                && x.FacebookId == userData.id && x.IsActive == true).FirstOrDefaultAsync();

                if(existUser == null)//Registraion
                {
                    var userToAdd = new User
                    {
                        Id = Guid.NewGuid(),
                        Name = userData.name,
                        Email = userData.email,
                        FacebookId = userData.id,
                        CreatedDate = DateTime.Now,
                        CreatedBy = Guid.NewGuid(),//temp
                        IsActive = true,
                    };
                    _context.User.Add(userToAdd);
                    await _context.SaveChangesAsync();

                    userToAdd.CreatedBy = userToAdd.Id;
                    await _context.SaveChangesAsync();

                    return new FacebookLoginResponse
                    {
                        Id = userToAdd.Id,
                        Name = userToAdd.Name,
                        Email = userToAdd.Email,
                        FacebookId = userToAdd.FacebookId,
                        Token = "Get Token From Token Service xD",
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Success",
                        Ref = userToAdd.Id.ToString()
                    };
                }
                else //Login
                {
                    return new FacebookLoginResponse
                    {
                        Id = existUser.Id,
                        Name = existUser.Name,
                        Email = existUser.Email,
                        FacebookId = existUser.FacebookId,
                        Token = "Get Token From Token Service xD",
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Success",
                        Ref = existUser.Id.ToString()
                    };
                }
            }
            catch (Exception e)
            {
                return new FacebookLoginResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = e.Message,
                };
            }
        }
    }
}
