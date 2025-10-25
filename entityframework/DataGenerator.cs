using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using entityframework.entities;

namespace entityframework
{
    public class DataGenerator
    {
        public static void Seed(MyBoardsContext context)
        {

            Randomizer.Seed = new Random(8675309);

            var addressGenerator = new Faker<Address>()
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.Street, f => f.Address.StreetName())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
                .RuleFor(a => a.Country, f => f.Address.Country());

            Address address = addressGenerator.Generate();

            var userGenerator = new Faker<User>()
                .RuleFor(u => u.FullName, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Address, f => address);

            var user = userGenerator.Generate(100);

            context.Users.AddRange(user);
            context.SaveChanges();
        }
    }
}