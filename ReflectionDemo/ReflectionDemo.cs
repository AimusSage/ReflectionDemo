using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace ReflectionDemo
{
    public class ReflectionDemo
    {
        [Fact]
        public void ReflectionDemoTest()
        {
            var solarSystem = new SolarSystem()
            {
                SolarSystemName = "Howdy",
                Worlds = new PlanetarySystem[1]
                {
                    new PlanetarySystem()
                    {
                        Name = "Earth System",
                        Planets = new PlanetaryBody[2] {
                            new PlanetaryBody()
                        {
                            Name = "Planet Earth"
                        }, new PlanetaryBody() {
                            Name = "The Moon"
                        } }
                    }
                }
            };

            
            var result = GetAllObjectsOfType<PlanetaryBody>(solarSystem);

            Assert.True(result.Count == 2);
        }

        private List<TObject> GetAllObjectsOfType<TObject>(object @object)
        {
            var returnableObjects = new List<TObject>();
            if (@object != null)
            {
                if (@object is TObject)
                {
                    returnableObjects.Add((TObject)@object);
                }
                var properties = @object.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(@object);

                    if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string) || prop.PropertyType == typeof(DateTime))
                    {
                        continue;
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                    {
                        if (value is null)
                            return returnableObjects;

                        var enumerable = value as IList;
                        foreach (var item in enumerable)
                        {
                            returnableObjects.AddRange(GetAllObjectsOfType<TObject>(item));
                        }
                    }
                    else
                    {
                        returnableObjects.AddRange(GetAllObjectsOfType<TObject>(value));
                    }

                }
            }
            return returnableObjects;
        }

        private class SolarSystem
        {
            public string SolarSystemName { get; set; }
            public PlanetarySystem[] Worlds { get; set; }

        }
        private class PlanetarySystem
        {
            public string Name { get; set; }

            public PlanetaryBody[] Planets { get; set; }
        }

        private class PlanetaryBody
        {
            public string Name { get; set; }


        }
    }
}
