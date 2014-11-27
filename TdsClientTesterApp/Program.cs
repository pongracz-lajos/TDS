﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Client;
using TestDataSeeding.Model;
using TestDataSeeding.SerializedStorage;

namespace TdsClientTesterApp
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlStorageClient xml = new XmlStorageClient();

            //EntityStructure esA = new EntityStructure();
            //esA.Name = "TableA";
            //esA.Attributes.Add("TableA_ID", "int");
            //esA.Attributes.Add("TableB_ID", "int");
            //esA.Attributes.Add("TableC_ID", "int");
            //esA.Attributes.Add("name", "string");
            //esA.PrimaryKeys.Add("TableA_ID");
            //esA.ForeignKeys.Add("TableB_ID", new EntityForeignKey("TableB", "TableB_ID"));
            //esA.ForeignKeys.Add("TableC_ID", new EntityForeignKey("TableC", "TableC_ID"));

            //EntityStructure esB = new EntityStructure();
            //esB.Name = "TableB";
            //esB.Attributes.Add("TableB_ID", "int");
            //esB.Attributes.Add("TableD_ID", "int");
            //esB.Attributes.Add("name", "string");
            //esB.PrimaryKeys.Add("TableB_ID");
            //esB.ForeignKeys.Add("TableD_ID", new EntityForeignKey("TableD", "TableD_ID"));
            //esB.BelongsToMany.Add("TableF");

            //EntityStructure esC = new EntityStructure();
            //esC.Name = "TableC";
            //esC.Attributes.Add("TableC_ID", "int");
            //esC.Attributes.Add("TableD_ID", "int");
            //esC.Attributes.Add("name", "string");
            //esC.PrimaryKeys.Add("TableC_ID");
            //esC.ForeignKeys.Add("TableD_ID", new EntityForeignKey("TableD", "TableD_ID"));

            //EntityStructure esD = new EntityStructure();
            //esD.Name = "TableD";
            //esD.Attributes.Add("TableD_ID", "int");
            //esD.Attributes.Add("name", "string");
            //esD.PrimaryKeys.Add("TableD_ID");

            //EntityStructure esE = new EntityStructure();
            //esE.Name = "TableE";
            //esE.Attributes.Add("TableE_ID", "int");
            //esE.Attributes.Add("TableG_ID", "int");
            //esE.Attributes.Add("name", "string");
            //esE.PrimaryKeys.Add("TableE_ID");
            //esE.ForeignKeys.Add("TableG_ID", new EntityForeignKey("TableG", "TableG_ID"));

            //EntityStructure esF = new EntityStructure();
            //esF.Name = "TableF";
            //esF.Attributes.Add("TableB_ID", "int");
            //esF.Attributes.Add("TableE_ID", "int");
            //esF.Attributes.Add("name", "string");
            //esF.PrimaryKeys.Add("TableB_ID");
            //esF.PrimaryKeys.Add("TableE_ID");
            //esF.ForeignKeys.Add("TableB_ID", new EntityForeignKey("TableB", "TableB_ID"));
            //esF.ForeignKeys.Add("TableE_ID", new EntityForeignKey("TableE", "TableE_ID"));

            //EntityStructure esG = new EntityStructure();
            //esG.Name = "TableG";
            //esG.Attributes.Add("TableG_ID", "int");
            //esG.Attributes.Add("TableB_ID", "int");
            //esG.Attributes.Add("name", "string");
            //esG.PrimaryKeys.Add("TableG_ID");
            //esG.ForeignKeys.Add("TableB_ID", new EntityForeignKey("TableB", "TableB_ID"));

            //EntityStructures escollection = new EntityStructures();
            //escollection.Add(esA);
            //escollection.Add(esB);
            //escollection.Add(esC);
            //escollection.Add(esD);
            //escollection.Add(esE);
            //escollection.Add(esF);
            //escollection.Add(esG);

            //xml.SaveEntityStructures(escollection, "D:\\TDS");

            //var a = xml.GetEntityStructures("D:\\TDS");
            //foreach (var b in a.Structures)
            //{
            //    Console.WriteLine(b);
            //}
            //Console.WriteLine("Sikeres mentes!");


            TdsClient tds = new TdsClient(ConfigurationManager.AppSettings["TdsStoragePath"]);

            var entities = new List<EntityWithKey>();
            var entity = new EntityWithKey("TableA", new List<string> { "1" });
            entities.Add(entity);
            //entity = new EntityWithKey("TableA", new List<string> { "2" });
            //entities.Add(entity);

            tds.SaveEntity(entities, "D:\\TDS");
            Console.WriteLine("Mentes sikeres!");
            //// Meglepo, de mukodik!


            //tds.LoadEntity(entities);
            //Console.WriteLine("Visszaallitas sikeres!");
            //// Ez vegkepp meglepo, de mukodik, es elsore!


            Console.ReadLine();
        }
    }
}
