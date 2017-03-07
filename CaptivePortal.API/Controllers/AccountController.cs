using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CaptivePortal.API.Models;
using log4net;
using System.IO;
using System.Net.Http.Headers;
using System.Net;
using FP.Radius;
using DBObject;
using System.Web.Script.Serialization;


namespace CaptivePortal.API.Controllers
{
   
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private RegisterDB objRegisterDB = new RegisterDB();
        private ReturnModel ObjReturnModel = new ReturnModel();
        [HttpPost]
        [Route("Register")]
        public HttpResponseMessage Register(RegisterViewModel objRegisterModel)
        {
            try
            {
                Log.Info("enter in register");
                objRegisterDB.CreateNewUser(objRegisterModel.UserName, objRegisterModel.UserPassword, objRegisterModel.Email);

                ObjReturnModel.Id = 1;
                ObjReturnModel.Message = "Sucess";

                JavaScriptSerializer objSerializer = new JavaScriptSerializer();
                return new HttpResponseMessage
                {
                    Content = new StringContent(objSerializer.Serialize(ObjReturnModel))
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw ex;
            }

        }

        [HttpPost]
        [Route("Login")]
        public HttpResponseMessage Login(LoginViewModel objLoginModel)
        {
            try
            {
                var args = new string[4];
                args[0] = "192.168.1.9";
                args[1] = "testing123";
                args[2] = objLoginModel.UserName;
                args[3] = objLoginModel.UserPassword;

                if (args.Length != 4)
                {
                    Authenticate.ShowUsage();
                }

                try
                {
                    Authenticate.Authentication(args).Wait();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                return new HttpResponseMessage()
                {
                    Content = new StringContent("success")
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public HttpResponseMessage GetFile()
        {
            string localFilePath = HttpContext.Current.Server.MapPath("~/App_Data/log.txt");
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "log.txt";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return response;
        }
    }
}
