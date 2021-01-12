using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace CancerRegistry.Identity
{
    public class BulgarianLanguageIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError() 
            => new IdentityError { Code = nameof(DefaultError), Description = $"Възникна неизвестна грешка." };

        public override IdentityError PasswordMismatch() 
            => new IdentityError { Code = nameof(PasswordMismatch), Description = "Грешна парола." }; 
        
        public override IdentityError InvalidToken()
            => new IdentityError { Code = nameof(InvalidToken), Description = "Невалиден токен." }; 
        
        public override IdentityError LoginAlreadyAssociated() 
            => new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Вече съществува потребител с този вход." };
        
        public override IdentityError InvalidUserName(string userName) 
            => new IdentityError { Code = nameof(InvalidUserName), Description = $"Потребителското име '{userName}' е невалидно, може да съдържа само букви или цифри." };
        
        public override IdentityError InvalidEmail(string email)
            => new IdentityError { Code = nameof(InvalidEmail), Description = $"Имейлът '{email}' е невалиден." };
        
        public override IdentityError DuplicateUserName(string userName) 
            => new IdentityError { Code = nameof(DuplicateUserName), Description = $"Потребителското име '{userName}' е вече заето." };
        
        public override IdentityError DuplicateEmail(string email) 
            => new IdentityError { Code = nameof(DuplicateEmail), Description = $"Имейлът '{email}' е вече зает." };
        
        public override IdentityError InvalidRoleName(string role) 
            => new IdentityError { Code = nameof(InvalidRoleName), Description = $"Роля с име '{role}' е невалидна." };
        
        public override IdentityError DuplicateRoleName(string role) 
            => new IdentityError { Code = nameof(DuplicateRoleName), Description = $"Роля с име '{role}' е вече заета." };
        
        public override IdentityError UserAlreadyInRole(string role) 
            => new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"Потребителят вече принадлежи към роля '{role}'." };
        
        public override IdentityError UserNotInRole(string role) 
            => new IdentityError { Code = nameof(UserNotInRole), Description = $"Потребителят не в роля с име '{role}'." };
        
        public override IdentityError PasswordTooShort(int length) 
            => new IdentityError { Code = nameof(PasswordTooShort), Description = $"Паролата трябва да съдържа поне {length} символа." };
        
        public override IdentityError PasswordRequiresNonAlphanumeric() 
            => new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Паролата трябва да съдържа поне един символ." };
        
        public override IdentityError PasswordRequiresDigit() 
            => new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Паролата трябва да съдържа поне една цифра." };
        
        public override IdentityError PasswordRequiresLower() 
            => new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Паролата трябва да съдържа поне една главна буква." };
        
        public override IdentityError PasswordRequiresUpper() 
            => new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Паролата трябва да съдържа поне една малка буква." };
    }
}
