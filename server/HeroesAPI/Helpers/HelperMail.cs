namespace HeroesAPI.Helpers
{
    public class MailLink
    {
        public string Link { get; set; }
        public string LinkText { get; set; }
    }

    public class HelperMail
    {
        private string HeroesAppBaseMail = @"<!DOCTYPE html>
<html
    lang=""en""
    xmlns=""http://www.w3.org/1999/xhtml""
    xmlns:o=""urn:schemas-microsoft-com:office:office""
>
    <head>
        <meta charset=""UTF-8"" />
        <meta name=""viewport"" content=""width=device-width,initial-scale=1"" />
        <meta name=""x-apple-disable-message-reformatting"" />
        <title></title>
        <!--[if mso]>
            <noscript>
                <xml>
                    <o:OfficeDocumentSettings>
                        <o:PixelsPerInch>96</o:PixelsPerInch>
                    </o:OfficeDocumentSettings>
                </xml>
            </noscript>
        <![endif]-->
        <style>
            table,
            td,
            div,
            h1,
            p {
                font-family: Arial, sans-serif;
            }
        </style>
    </head>
    <body style=""margin: 0; padding: 0"">
        <table
            role=""presentation""
            style=""
                width: 100%;
                border-collapse: collapse;
                border: 0;
                border-spacing: 0;
                background: #ffffff;
            ""
        >
            <tr>
                <td align=""center"" style=""padding: 0"">
                    <table
                        role=""presentation""
                        style=""
                            width: 602px;
                            border-collapse: collapse;
                            border: 1px solid #cccccc;
                            border-spacing: 0;
                            text-align: left;
                        ""
                    >
                        <tr>
                            <td
                                align=""center""
                                style=""
                                    padding: 40px 0 30px 0;
                                    background: #70bbd9;
                                ""
                            >
                            <!-- MoodReboot Logo -->
                                <img
                                    src=""%MOODREBOOT_IMAGE%""
                                    alt=""""
                                    width=""200""
                                    style=""height: auto; display: block""
                                />
                            </td>
                        </tr>
                        <tr>
                            <td style=""padding: 36px 30px 42px 30px"">
                                <table
                                    role=""presentation""
                                    style=""
                                        width: 100%;
                                        border-collapse: collapse;
                                        border: 0;
                                        border-spacing: 0;
                                    ""
                                >
                                    <tr>
                                        <td
                                            style=""
                                                padding: 0 0 36px 0;
                                                color: #153643;
                                            ""
                                        >
                                            <h1
                                                style=""
                                                    font-size: 24px;
                                                    margin: 0 0 20px 0;
                                                    font-family: Arial,
                                                        sans-serif;
                                                ""
                                            >
                                                %SUBJECT%
                                            </h1>
                                            <p
                                                style=""
                                                    margin: 0 0 12px 0;
                                                    font-size: 16px;
                                                    line-height: 24px;
                                                    font-family: Arial,
                                                        sans-serif;
                                                ""
                                            >
                                                %BODY%
                                            </p>
                                            <div
                                                style=""
                                                    display: flex;
                                                    justify-content: center;
                                                    gap: 10px;
                                                ""
                                            >
                                                %LINKS%
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style=""padding: 30px; background: #ee4c50"">
                                <table
                                    role=""presentation""
                                    style=""
                                        width: 100%;
                                        border-collapse: collapse;
                                        border: 0;
                                        border-spacing: 0;
                                        font-size: 9px;
                                        font-family: Arial, sans-serif;
                                    ""
                                >
                                    <tr>
                                        <td
                                            style=""padding: 0; width: 50%""
                                            align=""left""
                                        >
                                            <p
                                                style=""
                                                    margin: 0;
                                                    font-size: 14px;
                                                    line-height: 16px;
                                                    font-family: Arial,
                                                        sans-serif;
                                                    color: #ffffff;
                                                ""
                                            >
                                                <a
                                                    href=""%MOODREBOOTLINK""
                                                    style=""
                                                        color: #ffffff;
                                                        text-decoration: underline;
                                                    ""
                                                    >&reg; MoodReboot, Spain 2023</a
                                                >
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </body>
</html>
";

        public string BuildMailTemplate(string asunto, string mensaje, string baseUrl, List<MailLink>? links = null)
        {
            string nuevoEmail = this.HeroesAppBaseMail;
            nuevoEmail = nuevoEmail.Replace("%SUBJECT%", asunto);
            nuevoEmail = nuevoEmail.Replace("%BODY%", mensaje);
            nuevoEmail = nuevoEmail.Replace("%MOODREBOOTLINK%", baseUrl);
            nuevoEmail = nuevoEmail.Replace("%MOODREBOOT_IMAGE%", "https://live.staticflickr.com/65535/52772156860_2cdcd949cb_m.jpg");

            string linksHtml = "";

            if (links != null)
            {
                foreach (MailLink link in links)
                {
                    linksHtml += $@"                                                
                            <p
                                style=""
                                    margin: 0;
                                    font-size: 16px;
                                    line-height: 24px;
                                    font-family: Arial,
                                        sans-serif;
                                ""
                            >
                                <a
                                    href=""{link.Link}""
                                    style=""
                                        color: #ee4c50;
                                        text-decoration: underline;
                                    ""
                                    >{link.LinkText}</a
                                >
                            </p>";
                }
            }

            nuevoEmail = nuevoEmail.Replace("%LINKS%", linksHtml);
            return nuevoEmail;
        }
    }
}
