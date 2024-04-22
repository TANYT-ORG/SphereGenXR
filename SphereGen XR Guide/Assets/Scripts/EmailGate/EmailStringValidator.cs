using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;
using SphereGen;

[RequireComponent(typeof(SMTPEmailDaemon))]
public class EmailStringValidator : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_InputField emailInputField;
    public TextMeshProUGUI messageForUser;

    private void Start()
    {
        // Attach a listener to the input field's end edit event
        emailInputField.onEndEdit.AddListener(ValidateEmail);
    }

    private void ValidateEmail(string email)
    {
        // Define a regex pattern for email validation
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        bool isEmailValid = Regex.IsMatch(email, emailPattern);

        // Update the validation text based on the result
        if (isEmailValid && ValidateName())
        {
            GetComponent<SMTPEmailDaemon>().UserProvidedEmailAddress(nameInputField.text, emailInputField.text);
            GetComponent<SMTPEmailDaemon>().SendEmail();
            Debug.LogFormat("Email input success.");
        }
        else if(!isEmailValid)
        {
            messageForUser.text = "Please provide an email address using this format 'email@example.com'";
        }
        else if(!ValidateName())
        {
            messageForUser.text = "Please provide a name.";
        }
        else if(!ValidateName() && !isEmailValid)
        {
            messageForUser.text = "Please provide a name and a valid email address. Email addresses should be in a similar format to 'email@example.com' ";
        }

    }

    private bool ValidateName()
    {
        string name = nameInputField.text;
        bool isNameValid = name.Length >= 2 && Regex.IsMatch(name, @"^[a-zA-Z0-9\s]+$");
        return isNameValid;
    }

    private void OnDestroy()
    {
        emailInputField.onEndEdit.RemoveListener(ValidateEmail);
    }
}