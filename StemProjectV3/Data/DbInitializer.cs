using StemProjectV3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StemProjectV3.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Students.Any())
            {
                return;
            }

            var students = new Student[]
            {
                new Student{ FirstName="Carson", LastName="Alexander", GradDate=DateTime.Parse("2016-06-01")},
                new Student{ FirstName="Meredith", LastName="Alonso", GradDate=DateTime.Parse("2016-09-01")},
                new Student{ FirstName="Arturo", LastName="Anand", GradDate=DateTime.Parse("2017-12-01")},
                new Student{ FirstName="Yan", LastName="Li", GradDate=DateTime.Parse("2017-09-01")}
            };

            foreach (Student s in students)
            {
                context.Students.Add(s);
            }

            context.SaveChanges();

            var projects = new Project[]
            {
                new Project{ProjectID=1001, Name="Solar Powered Cooler for Parked Cars", Abstract="The purpose of the project was to build a solar powered cooling system that keeps hot cars parked in the sun near the ambient outside temperature. A system consisting of a roof mounted solar panel connected to two window mounted fans was created. Preliminary tests on cloudy days have been completed. These tests involved three different trials, one with windows shut, one with windows 1 inch open, and one with the fan system running. A power supply which supplied the same amount of power that the solar panel would have was used to test the system. In each trial the car was heated to 83° F and allowed to cool for 20 minutes, with interior and exterior temperatures being recorded every minute. The end results showed that the car was cooled the fastest and by the greatest amount in the trial with the system running, dropping 7° F in the first 5 minutes and reaching a low of 71° F. The system was marginally more effective at cooling a car than leaving the windows cracked open, which also dropped the temperature 7° F in the first 5 minutes and reached a low of 73° F. This data shows the potential for the systems success. However testing on sunny days must be completed in order to truly evaluate how effective the system is at keeping a car cool.", ProjectDate=DateTime.Parse("2017-09-01")},
                new Project{ProjectID=1002, Name="Cost Efficient Dye-Sensitized Solar Cells", Abstract="ProjectAbstract The original goal of the project was to design and build a solar cell that would result in a better cost to power ratio than $1.00/watt. After research and further consideration, it was determined that the goal should be to design and build a solar cell that produced a power output of over 0 watts in order to yield a power to cost ratio. Safety concerns led the type of solar cell manufactured to be a dye-sensitized solar cell. Three dye-sensitized solar cells were assembled, each using a different catalyst.The first cell used a platinum paste catalyst, the second used a graphite catalyst, and the third used a carbon soot catalyst. Each cell was measured for voltage, amperage, and cost.All three cells were tested under constant light from an incandescent lamp.It was observed that the cell containing the platinum catalyst produced more power than the carbon alternatives.All cells produced a measurable power output and the material cost for each carbon cell was $19.44 while the platinum cell cost $21.34.It was determined that a single layer catalyst counter-electrode can be used in dye - sensitized solar cells to produce a cell for under $20.A cost efficiency of $1.00 per watt, the standard price for photovoltaic power, " +
                    "was not achieved.This may have been due to the quality of manufacturing and the cost of materials for dye - sensitized solar cells when not purchased in bulk.", ProjectDate=DateTime.Parse("2017-06-01")},
                new Project{ProjectID=1003, Name="Compost: Source of Fuel", Abstract="ProjectAbstract Thousands of tons of food scraps are being thrown away each year in the US. The US is also working to develop alternative energy sources to replace carbon based fuels. Through the design and construction of the biogenerator, it would be possible to harness the methane gas from the compost and turn it into clean energy. " +
                    "The energy being stored in food scraps can be used to give power to many people and stop this “garbage” from being thrown away and over filling garbage depots. This would be most beneficial to restaurants where they are obligated to throw away food that are past the expiration date. This would be clean energy because we would be reusing the gases already in the atmosphere instead of putting more pollutants " +
                    "into the atmosphere and the amount waste in the country would be reduced. In this experiment, food scraps will be left long enough to be turned into compost and produce methane gas, which is a gas that can be used for energy.By heating up the food scraps repeatedly, the process of producing methane will speed up the composting stage for the food scraps.", ProjectDate=DateTime.Parse("2016-09-01")},
                new Project{ProjectID=1004, Name="Airfoil Dimpling and Drag Reduction", Abstract="ProjectAbstract Millions of dollars are spent each year on fuel costs for commercial airlines. Aircraft Designers pursue every opportunity to minimize drag on an aircraft because even a 1 % reduction in drag can result in huge savings and better profit for airline companies.This study was conducted in efforts to investigate" +
                    " the implications on lift and drag in implementing dimples, like that of a golf ball, onto the surface of an airfoil.There is reason to believe that doing so can increase an airplane's efficiency by reducing drag and therefore save airlines millions on jet fuel. Scale model airfoils were produced with different dimpling variations, as well as a smooth baseline airfoil to establish a control. The baseline" +
                    " will be used to compare the difference between the dimples and no dimples by testing lift and drag. These airfoils were tested in a wind tunnel and recorded drag using a rake behind the airfoil and surface pressure from 8 different locations on each airfoil. Lift was found by finding the difference between the surface pressure from the top and bottom of the airfoil. Calculating drag is done by using the data" +
                    " collected by the rake and the height of the rake, then multiplying by the frontal area of the airfoil, and using the surface pressures. Implementing dimples had a small, however noticeable difference in lift and drag. These differences can equate to a greater efficiency and significantly lower an aircraft's operating costs if implemented on a real scale. Preliminary results show that implementing dimples on the" +
                    " surface of an airfoil has potential to increase the aerodynamic efficiency of an airfoil.", ProjectDate=DateTime.Parse("2015-09-01")}
            };
            foreach (Project p in projects)
            {
                context.Projects.Add(p);
            }

            var enrollments = new Enrollment[]
            {
                new Enrollment{ StudentID=1, ProjectID=1001},
                new Enrollment{ StudentID=1, ProjectID=1002},
                new Enrollment{ StudentID=1, ProjectID=1003},
                new Enrollment{StudentID=2, ProjectID=1001},
                new Enrollment{StudentID=2, ProjectID=1002},
                new Enrollment{ StudentID=2, ProjectID=1003},
                new Enrollment{StudentID=3, ProjectID=1001},
                new Enrollment{StudentID=3, ProjectID=1002},
                new Enrollment{StudentID=3, ProjectID=1001},
                new Enrollment{StudentID=3, ProjectID=1004}
            };
            foreach (Enrollment e in enrollments)
            {
                context.Enrollments.Add(e);
            }
            context.SaveChanges();

            var mentors = new Mentor[]
            {
                new Mentor{FirstName="Kim", LastName="Abercrombie", Email="kimaber@gmail.com", Discipline="Math", Comment="Very happy" },
                new Mentor{FirstName="Fadi", LastName="Fakhouri", Email="fadi12@yahoo.com", Discipline="Chemistry", Comment="Excited"},
                new Mentor{FirstName="Roger", LastName="Harui", Email="rogerharui@gmail.com", Discipline="Literature", Comment="Competitive"},
                new Mentor{FirstName="Candace", LastName="Kapoor", Email="candace@yahoo.com", Discipline="Biology", Comment="Energetic"}
            };
            foreach(Mentor i in mentors)
            {
                context.Mentors.Add(i);
            }
            context.SaveChanges();

            var projectMentors = new ProjectAssignment[]
            {
                new ProjectAssignment
                {
                    ProjectID = projects.Single(p => p.Name=="Solar Powered Cooler for Parked Cars").ProjectID,
                    MentorID = mentors.Single(m => m.LastName=="Abercrombie").ID
                },
                new ProjectAssignment
                {
                    ProjectID = projects.Single(p => p.Name=="Cost Efficient Dye-Sensitized Solar Cells").ProjectID,
                    MentorID = mentors.Single(m => m.LastName=="Fakhouri").ID
                },
                new ProjectAssignment
                {
                    ProjectID=projects.Single(p=>p.Name=="Compost: Source of Fuel").ProjectID,
                    MentorID = mentors.Single(m => m.LastName=="Harui").ID
                },
                new ProjectAssignment
                {
                    ProjectID=projects.Single(p=>p.Name=="Solar Powered Cooler for Parked Cars").ProjectID,
                    MentorID=mentors.Single(m=>m.LastName=="Kapoor").ID
                }
            };

            foreach(ProjectAssignment p in projectMentors)
            {
                context.ProjectAssignments.Add(p);
            }
            context.SaveChanges();
        }
    }
}
