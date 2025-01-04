#region

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using db;
using MySql.Data.MySqlClient;
using System.Net.Mail;

#endregion

namespace server.account
{
    internal class register : RequestHandler
    {
        protected override void HandleRequest()
        {
            if (Query["ignore"] == null || !String.IsNullOrWhiteSpace(Query["entrytag"]))
            {
                using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                    wtr.Write("<Error>WebRegister.invalid_email_address</Error>");
                return;
            }

            if (Query.AllKeys.Length != 6)
            {
                using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                    wtr.Write("<Error>WebRegister.invalid_email_address</Error>");
                return;
            }

            if (!IsValidEmail(Query["newGuid"]))
            {
                using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                    wtr.Write("<Error>WebRegister.invalid_email_address</Error>");
                return;
            }

            using (Database db = new Database())
            {
                byte[] status;
                if (!IsValidEmail(Query["newGUID"]))
                    status = Encoding.UTF8.GetBytes("<Error>WebForgotPasswordDialog.emailError</Error>");
                if (db.HasUuid(Query["guid"]) &&
                    !db.Verify(Query["guid"], "", Program.GameData).IsGuestAccount)
                {
                    if (db.HasUuid(Query["newGUID"]))
                        status = Encoding.UTF8.GetBytes("<Error>Error.emailAlreadyUsed</Error>");
                    else
                    {
                        MySqlCommand cmd = db.CreateQuery();
                        cmd.CommandText =
                            "UPDATE accounts SET uuid=@newUuid, name=@newUuid, password=SHA1(@password), namechosen=1, guest=FALSE WHERE uuid=@uuid, name=@name;";
                        cmd.Parameters.AddWithValue("@uuid", Query["guid"]);
                        cmd.Parameters.AddWithValue("@newUuid", Query["newGUID"]);
                        cmd.Parameters.AddWithValue("@password", Query["newPassword"]);

                        if (cmd.ExecuteNonQuery() > 0)
                            status = Encoding.UTF8.GetBytes("<Success />");
                        else
                            status = Encoding.UTF8.GetBytes("<Error>Error.emailAlreadyUsed</Error>");
                    }
                }
                else
                {
                    Account acc = db.Register(Query["newGUID"], Query["newPassword"], false, Program.GameData);
                    if (acc != null)
                    {
                        if (Program.Settings.GetValue<bool>("verifyEmail"))
                        {
                            MailMessage message = new MailMessage();
                            message.To.Add(Query["newGuid"]);
                            message.IsBodyHtml = true;
                            message.Subject = "Please verify your account.";
                            message.From = new MailAddress(Program.Settings.GetValue<string>("serverEmail", ""));
                            message.Body = "<center>Please verify your email via this <a href=\"" + Program.Settings.GetValue<string>("serverDomain", "localhost") + "/account/validateEmail?authToken=" + acc.AuthToken + "\" target=\"_blank\">link</a>.</center>";
                            Program.SendEmail(message, true);
                        }
                        status = Encoding.UTF8.GetBytes("<Success/>");
                    }
                    else
                        status = Encoding.UTF8.GetBytes("<Error>Error.emailAlreadyUsed</Error>");
                }
                Context.Response.OutputStream.Write(status, 0, status.Length);
            }
        }

        public bool IsValidEmail(string strIn)
        {
            return true;
        }
    }
}