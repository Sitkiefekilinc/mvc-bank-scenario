using EEZBankServer.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EEZBankServer.Extensions
{
    public static class EEZBankHtmlExtensions
    {
        public static IHtmlContent UsersForm(this IHtmlHelper helper)
        {
            var labelFormAd = new TagBuilder("label");
            labelFormAd.Attributes.Add("for", "UserName");
            labelFormAd.InnerHtml.Append("Adınız:");

            var textboxForm = new TagBuilder("input");
            textboxForm.Attributes.Add("type", "text");
            textboxForm.Attributes.Add("name", "UserName");
            textboxForm.Attributes.Add("placeHolder", "Adınızı giriniz");

            var labelFormSoyad = new TagBuilder("label");
            labelFormSoyad.Attributes.Add("for", "UserSurname");
            labelFormSoyad.InnerHtml.Append("Soyadınız:");

            var textboxFormSoyad = new TagBuilder("input");
            textboxFormSoyad.Attributes.Add("type", "text");
            textboxFormSoyad.Attributes.Add("name", "UserSurname");
            textboxFormSoyad.Attributes.Add("placeHolder", "Soyadınızı giriniz");

            var emailForm = new TagBuilder("label");
            emailForm.Attributes.Add("for", "UserEmail");
            emailForm.InnerHtml.Append("E-Posta Adresiniz:");

            var emailInputForm = new TagBuilder("input");
            emailInputForm.Attributes.Add("type", "email");
            emailInputForm.Attributes.Add("name", "UserEmail");
            emailInputForm.Attributes.Add("placeHolder", "E-Posta adresinizi giriniz");

            var passwordForm = new TagBuilder("label");
            passwordForm.Attributes.Add("for", "UserPassword");
            passwordForm.InnerHtml.Append("Şifreniz:");

            var passwordInputForm = new TagBuilder("input");
            passwordInputForm.Attributes.Add("type", "password");
            passwordInputForm.Attributes.Add("name", "UserPassword");
            passwordInputForm.Attributes.Add("placeHolder", "Şifrenizi giriniz");

            var passwordAgainForm = new TagBuilder("label");
            passwordAgainForm.Attributes.Add("for", "UserPasswordAgain");
            passwordAgainForm.InnerHtml.Append("Şifreniz Tekrar:");

            var passwordAgainInputForm = new TagBuilder("input");
            passwordAgainInputForm.Attributes.Add("type", "password");
            passwordAgainInputForm.Attributes.Add("name", "UserPasswordAgain");
            passwordAgainInputForm.Attributes.Add("placeHolder", "Şifrenizi tekrar giriniz");

            var submitButton = new TagBuilder("button");
            submitButton.Attributes.Add("type", "submit");
            submitButton.AddCssClass("btn btn-primary");
            submitButton.InnerHtml.Append("Kayıt ol");

            var span = new TagBuilder("span");
            span.Attributes.Add("asp-validation-for", "UserName");


            var divForm = new TagBuilder("div");
            divForm.AddCssClass("form-group");
            divForm.InnerHtml.AppendHtml(labelFormAd);
            divForm.InnerHtml.AppendHtml(textboxForm);
            divForm.InnerHtml.AppendHtml("<br>");
            divForm.InnerHtml.AppendHtml(labelFormSoyad);
            divForm.InnerHtml.AppendHtml(textboxFormSoyad);
            divForm.InnerHtml.AppendHtml("<br>");
            divForm.InnerHtml.AppendHtml(emailForm);
            divForm.InnerHtml.AppendHtml(emailInputForm);
            divForm.InnerHtml.AppendHtml("<br>");
            divForm.InnerHtml.AppendHtml(passwordForm);
            divForm.InnerHtml.AppendHtml(passwordInputForm);
            divForm.InnerHtml.AppendHtml("<br>");
            divForm.InnerHtml.AppendHtml(passwordAgainForm);
            divForm.InnerHtml.AppendHtml(passwordAgainInputForm);
            divForm.InnerHtml.AppendHtml("<br>");
            divForm.InnerHtml.AppendHtml(submitButton);

            return divForm;
        }

    }
}
