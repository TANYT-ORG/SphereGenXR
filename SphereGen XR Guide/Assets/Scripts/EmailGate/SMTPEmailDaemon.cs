using UnityEngine;
using System;
using System.Net.Mail;

namespace SphereGen
{
    public class SMTPEmailDaemon : MonoBehaviour
    {
        SmtpClient smtpCLient = new SmtpClient();
        [SerializeField] private string _senderEmail = "your_email@example.com";
        [SerializeField] private string _recipientEmail = "recipient@example.com";
        [SerializeField] private string _subject = "Test Email";
        [SerializeField] private string _body = "";
        private string _secretPassword; //put this in a config file so it can be securely stored!
        string _accessToken;
        [SerializeField] private bool _signedUp = false;
        public UISceneManager UIScript;
       

        private void Start()
        {
            // TODO: Gitignored secret password. Probably not important.
            //PlayerPrefs.DeleteAll(); 
            //_secretPassword = PasswordForSMTP.sgNoReplyPassword;
            CheckIfSignedUp();

        }

        private void Update()
        {
            //if (Input.GetKeyUp(KeyCode.E))
            //{
            //    //Test Case. This is the call you will make though to send an email.
            //    _SendMessage(_senderEmail, _recipientEmail, "", _subject, "", _body);
            //}
        }

        public void UserProvidedEmailAddress(string name, string email)
        {
            string newBody = $"{name} has activated the full SphereGen XR Application. They have provided the following email address: {email}";

            _body = newBody;
        }

        public void SendEmail()
        {
            _SendMessage(_senderEmail, _recipientEmail, "", _subject, "", _body);
            SetSignUp();
        }

        public void SetSignUp()
        {
            _signedUp = true;
            PlayerPrefs.SetInt("Signed_Up", _signedUp ? 1 : 0);
            if (_signedUp == true)
            {
                this.gameObject.SetActive(false);
                UIScript.enabled = true;
                UIScript.POPScript.enabled = true;
            }
        }

        public void CheckIfSignedUp()
        {
           
            _signedUp = PlayerPrefs.GetInt("Signed_Up") == 1 ? true : false;
            if (_signedUp == true)
            {
                this.gameObject.SetActive(false);
                UIScript.enabled = true;
                UIScript.POPScript.enabled = true;
            }


        }

        #region GenericSMTP
        private void _SendMessage(
        string from,
        string toAddress,
        string HTMLBody,
        string subject,
        string ccAddress,
        string TEXTBody,
        string bccAddress = null)
        {
            MailMessage message = null;
            System.IO.Stream stream = null;
            AlternateView view = null;
            try
            {
                string _testDesc = "";
                SmtpClient client = new SmtpClient($"smtp.office365.com");
                string strFromAddress = from;
                string strtoAddress = toAddress;

                client.Credentials = new System.Net.NetworkCredential(_senderEmail, _secretPassword);
                client.Port = 587;
                client.EnableSsl = true;

                message = new MailMessage(strFromAddress, strtoAddress);
                if (!string.IsNullOrEmpty(ccAddress))
                {
                    message.CC.Add(ccAddress);
                }

                //this is remove for only testing purpose
                //message.Bcc.Add("sandeep.kasar@spheregen.com");

                if (!string.IsNullOrEmpty(TEXTBody))
                {
                    message.Body = TEXTBody;
                    message.IsBodyHtml = false;
                }
                else
                {
                    message.Body = HTMLBody;
                    message.IsBodyHtml = true;
                }

                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = subject;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                client.Send(message);

            }
            catch (Exception ex)
            {
                Debug.LogFormat("Error sending email " + ex.Message);
                Debug.LogFormat("Error sending email " + ex.StackTrace);

            }
            finally
            {
                if (message != null)
                    message.Dispose();



                if (stream != null)
                    stream.Dispose();



                if (view != null)
                    view.Dispose();
            }
        }
        #endregion
    }
}
