using GraphQL.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestGraphQL
{
    public class GraphQlSchema
    {
        public  GraphQL<VendorsEntities> GraphQL;
        public  GraphQLSchema<VendorsEntities> GraphQLSchemaObj;
        public GraphQlSchema()
        {
            var dbcon = new VendorsEntities();
            GraphQLSchemaObj =  GraphQL<VendorsEntities>.CreateDefaultSchema(() => dbcon,false);
            GraphQLSchemaObj.AddScalar(new { year = 0, month = 0, day = 0 }, ymd => new DateTime(ymd.year, ymd.month, ymd.day));

           GraphQLSchemaObj.AddType<User>().AddAllFields();
            GraphQLSchemaObj.AddType<Manuscript>().AddAllFields();
            GraphQLSchemaObj.AddType<Journal>().AddAllFields();

           
            GraphQLSchemaObj.AddField("Manuscript", new { ManuscriptId =0}, (db, arg) =>
             GetManuscript(db, arg.ManuscriptId));
            GraphQLSchemaObj.AddField("User", new { UserId = 0 }, (db, arg) => GetUser(db, arg.UserId));
            GraphQLSchemaObj.AddListField("Manuscripts", new { last = default(int?) },(db, arg) => GetManuscripts(db,arg.last));


    //        GraphQLSchemaObj.AddMutation("AddUser",
    //                new { UserName = "", UserMail = "" },
    //                (db, args) =>
    //                {
    //                    var user = new User() { UserName = args.UserName, UserMail = args.UserMail };
    //                    db.Users.Add(user);
    //                    db.SaveChanges();
    //                    return user;
    //                },
    //(db, args, user) => user);

            GraphQLSchemaObj.Complete();
            //var xx=GraphQLSchema.ToString();
            GraphQL = new GraphQL<VendorsEntities>(GraphQLSchemaObj);


        }

        Manuscript GetManuscript(VendorsEntities db, int ManuscriptId)
        {
           
            // var xc= db.Manuscripts.Where(i => i.ManuscriptId == ManuscriptId).Select(x=>new ManuscriptT() { ManuscriptId=x.ManuscriptId}).First();
            var xc = db.Manuscripts.Include("User").Single(i => i.ManuscriptId == ManuscriptId);

            return xc;
        }
        User GetUser(VendorsEntities db, int UserId)
        {

            // var xc= db.Manuscripts.Where(i => i.ManuscriptId == ManuscriptId).Select(x=>new ManuscriptT() { ManuscriptId=x.ManuscriptId}).First();
            return db.Users.Include("Journals").Include("Journals.Manuscripts").Single(i => i.UserId == UserId);
            
        }
        IQueryable<Manuscript> GetManuscripts(VendorsEntities db, int? last)
        {
            var r = db.Manuscripts.OrderBy(i => i.ManuscriptId);
            if(last.HasValue)
               return r.Take(last.Value).AsQueryable();
            else
            return r.AsQueryable();

            // var xc= db.Manuscripts.Where(i => i.ManuscriptId == ManuscriptId).Select(x=>new ManuscriptT() { ManuscriptId=x.ManuscriptId}).First();
           

        }
    }
}