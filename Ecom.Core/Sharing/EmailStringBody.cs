using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Sharing
{
    public class EmailStringBody
    {
        public static string Send(string email,string token,string component,string message)
        {
            string encodeToken=Uri.EscapeDataString(token);
            return $@"
<!DOCTYPE html>
<html lang=""ar"" dir=""rtl"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>تأكيد الحساب</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: 'Segoe UI', 'Cairo', 'Tahoma', sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 20px;
            color: #333;
            direction: rtl;
            text-align: right;
        }}
        
        .email-wrapper {{
            max-width: 650px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 20px;
            overflow: hidden;
            box-shadow: 0 25px 50px rgba(0, 0, 0, 0.15);
        }}
        
        .email-header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 40px 30px;
            text-align: center;
            position: relative;
        }}
        
        .email-header::before {{
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: url('data:image/svg+xml,<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 100 100""><circle cx=""20"" cy=""20"" r=""2"" fill=""white"" opacity=""0.1""/><circle cx=""80"" cy=""40"" r=""1"" fill=""white"" opacity=""0.1""/><circle cx=""40"" cy=""80"" r=""1.5"" fill=""white"" opacity=""0.1""/></svg>');
        }}
        
        .logo {{
            width: 80px;
            height: 80px;
            background: rgba(255, 255, 255, 0.2);
            border-radius: 50%;
            margin: 0 auto 20px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 36px;
            color: white;
            font-weight: bold;
            backdrop-filter: blur(10px);
            border: 2px solid rgba(255, 255, 255, 0.3);
        }}
        
        .header-title {{
            color: white;
            font-size: 28px;
            font-weight: 700;
            margin-bottom: 10px;
            text-shadow: 0 2px 4px rgba(0, 0, 0, 0.3);
        }}
        
        .header-subtitle {{
            color: rgba(255, 255, 255, 0.9);
            font-size: 16px;
            font-weight: 400;
        }}
        
        .email-body {{
            padding: 50px 40px;
            background: #ffffff;
        }}
        
        .greeting {{
            font-size: 22px;
            color: #2c3e50;
            margin-bottom: 25px;
            font-weight: 600;
        }}
        
        .message-content {{
            font-size: 17px;
            line-height: 1.8;
            color: #555;
            margin-bottom: 35px;
            text-align: justify;
        }}
        
        .cta-section {{
            text-align: center;
            margin: 40px 0;
        }}
        
        .cta-button {{
            display: inline-block;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 18px 45px;
            font-size: 18px;
            font-weight: 600;
            text-decoration: none;
            border-radius: 50px;
            box-shadow: 0 15px 35px rgba(102, 126, 234, 0.4);
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            position: relative;
            overflow: hidden;
        }}
        
        .cta-button::before {{
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
            transition: left 0.5s;
        }}
        
        .cta-button:hover {{
            transform: translateY(-3px);
            box-shadow: 0 20px 40px rgba(102, 126, 234, 0.6);
        }}
        
        .cta-button:hover::before {{
            left: 100%;
        }}
        
        .security-notice {{
            background: #f8f9fa;
            border: 2px solid #e9ecef;
            border-radius: 15px;
            padding: 25px;
            margin: 30px 0;
            position: relative;
        }}
        
        .security-notice::before {{
            content: '🔒';
            position: absolute;
            top: -15px;
            right: 20px;
            background: #f8f9fa;
            padding: 5px 10px;
            border-radius: 50%;
            border: 2px solid #e9ecef;
        }}
        
        .security-title {{
            font-size: 18px;
            font-weight: 600;
            color: #495057;
            margin-bottom: 10px;
        }}
        
        .security-text {{
            font-size: 15px;
            color: #6c757d;
            line-height: 1.6;
        }}
        
        .divider {{
            height: 2px;
            background: linear-gradient(90deg, transparent, #667eea, transparent);
            margin: 30px 0;
            border-radius: 2px;
        }}
        
        .footer {{
            background: #f8f9fa;
            padding: 30px 40px;
            text-align: center;
            border-top: 1px solid #e9ecef;
        }}
        
        .footer-text {{
            font-size: 14px;
            color: #6c757d;
            line-height: 1.6;
            margin-bottom: 15px;
        }}
        
        .company-info {{
            font-size: 12px;
            color: #adb5bd;
            margin-top: 20px;
        }}
        
        .token-display {{
            background: #f1f3f4;
            border: 2px dashed #d1d5db;
            border-radius: 10px;
            padding: 15px;
            margin: 20px 0;
            font-family: 'Courier New', monospace;
            font-size: 14px;
            color: #374151;
            text-align: center;
            word-break: break-all;
        }}
        
        @media (max-width: 600px) {{
            .email-wrapper {{
                border-radius: 0;
                margin: 0;
            }}
            
            .email-body {{
                padding: 30px 20px;
            }}
            
            .email-header {{
                padding: 30px 20px;
            }}
            
            .header-title {{
                font-size: 24px;
            }}
            
            .cta-button {{
                padding: 15px 35px;
                font-size: 16px;
            }}
            
            .footer {{
                padding: 25px 20px;
            }}
        }}
    </style>
</head>
<body>
    <div class=""email-wrapper"">
        <div class=""email-header"">
            <div class=""logo"">🚀</div>
            <h1 class=""header-title"">مرحباً بك</h1>
            <p class=""header-subtitle"">نحن سعداء لانضمامك إلينا</p>
        </div>
        
        <div class=""email-body"">
            <div class=""greeting"">أهلاً وسهلاً!</div>
            
            <div class=""message-content"">
                {message}
            </div>
            
            <div class=""divider""></div>
            
            <div class=""cta-section"">
                <p style=""margin-bottom: 25px; font-size: 16px; color: #666;"">
                    لإكمال عملية التسجيل، يرجى الضغط على الزر أدناه:
                </p>
                <a class=""cta-button"" href=""http://localhost:4200/account/{component}?email={email}&code={encodeToken}"" target=""_blank"">
                     تأكيد الحساب الآن
                </a>
            </div>
            
            <div class=""security-notice"">
                <div class=""security-title"">ملاحظة أمنية مهمة</div>
                <div class=""security-text"">
                    إذا لم تكن أنت من طلب هذا التأكيد، يرجى تجاهل هذا الإيميل تماماً. 
                    هذا الرابط صالح لمدة محدودة فقط لضمان الأمان.
                </div>
            </div>
            
            <div class=""token-display"">
                <strong>رمز التأكيد:</strong><br>
                {token}
            </div>
        </div>
        
        <div class=""footer"">
            <div class=""footer-text"">
                <strong>هل تحتاج مساعدة؟</strong><br>
                إذا واجهت أي مشكلة في تأكيد حسابك، لا تتردد في التواصل معنا.
            </div>
            
            <div class=""divider"" style=""margin: 20px 0; height: 1px;""></div>
            
            <div class=""company-info"">
                هذا إيميل تلقائي، يرجى عدم الرد عليه مباشرة.<br>
                جميع الحقوق محفوظة © 2025
            </div>
        </div>
    </div>
</body>
</html>";
        }
    }
}
