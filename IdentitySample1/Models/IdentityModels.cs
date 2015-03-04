using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;

namespace IdentitySample1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class MyUser : IdentityUser<long, MyLogin, MyUserRole, MyClaim>
    {
        public string ActivationToken { get; set; }

        public string PasswordAnswer { get; set; }

        public string PasswordQuestion { get; set; }
    }

    public class MyUserRole : IdentityUserRole<long> { }
    public class MyRole : IdentityRole<long, MyUserRole> { }

    public class MyClaim : IdentityUserClaim<long> { }

    public class MyLogin : IdentityUserLogin<long> { }
    public class ApplicationDbContext : IdentityDbContext<MyUser, MyRole, long, MyLogin, MyUserRole, MyClaim>
    {
        // It will go and check for connectionstring in web.config. then it creates a localdb with that name.
        // If this CS is not present in web.config then it is stored somewhere.
        // If this CS is present in web.config with which is addressing sql server... and even if that CS is wrong.. this is going to create database with that name
        public ApplicationDbContext()
            : base("MagicDbToSql")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Map Entities to their tables.
            modelBuilder.Entity<MyUser>().ToTable("User");
            modelBuilder.Entity<MyUserRole>().ToTable("UserRole");
            modelBuilder.Entity<MyRole>().ToTable("Role");
            modelBuilder.Entity<MyClaim>().ToTable("UserClaim");
            modelBuilder.Entity<MyLogin>().ToTable("UserLogin");

            // Set AutoIncrement-Properties
            modelBuilder.Entity<MyUser>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MyRole>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MyClaim>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


        }
    }

    public class MyPasswordHasher : PasswordHasher
    {
        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return hashedPassword.Equals(HashPassword(providedPassword)) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
        public override string HashPassword(string password)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var textToHash = Encoding.Default.GetBytes(password);
            var result = sha1.ComputeHash(textToHash);
            var resultText = Convert.ToBase64String(result);
            resultText = resultText.Replace("+", "-");
            return resultText;
        }
    }
}