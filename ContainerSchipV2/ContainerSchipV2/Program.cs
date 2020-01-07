using ContainerSchipV2;
using System;
using System.Collections.Generic;
using System.Linq;
using static ContainerSchipV2.Enum;

namespace LP_Containervervoer_App
{
    class Program
    {
        const int _defaultWeight = 30000;
        const int _defaulShiptMaxWeight = 60000;
        const int _defaultTopLoad = 120000;

        static List<Container> _currentInput;

        static void Main(string[] args)
        {
            _currentInput = new List<Container>()
            {
               new Container(_defaultWeight, ContainerType.Standard)
            };

         

            Ship ship = new Ship(15, 1);
            DisplayShipInformation(ship);

            Console.WriteLine("Loading Ship...");
            ship.LoadShip(_currentInput);
            Console.WriteLine("Loading complete:");
            DisplayShipInformation(ship);

            DisplayLayoutFromList(ship.Layout);
            DisplayNonPlacedContainers(ship.NotPlacedContainers);

            Console.ReadLine();
        }

        

        static void DisplayLayoutFromList(IEnumerable<Place> slots)
        {
            foreach (Place slot in slots)
            {
                Console.WriteLine("---------------------------------------");
                Console.WriteLine($"Position (X, Y): {slot.PlaceXPostion}, {slot.PlaceYPosition}");
                List<Container> containers = slot.containers.ToList();

                for (int i = slot.containers.Count() - 1; i >= 0; i--)
                {
                    Console.WriteLine(containers[i]);
                }
            }
            Console.WriteLine("");
        }

        static void DisplayShipInformation(Ship ship)
        {
            Console.WriteLine("Ship information:");
            Console.WriteLine($"Is Ship Sailable: {ship.AbletoSail}");
            Console.WriteLine($"Reason: {ship.Exception}");
            Console.WriteLine("");
        }

        static void DisplayNonPlacedContainers(IEnumerable<Container> nonContainers)
        {
            Console.WriteLine("Here are the containers that are not placed:");
            Console.WriteLine($"Number of containers input: {_currentInput.Count}, Numer of containers left over: {nonContainers.Count()}");
            foreach (Container container in nonContainers)
            {
                Console.WriteLine(container);
            }
        }

        static List<Container> FillerDataOne()
        {
            List<Container> returnList = new List<Container>()
            {
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Valuable),
                new Container(_defaultWeight, ContainerType.Valuable),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Valuable),
                new Container(_defaultWeight, ContainerType.Valuable),
                new Container(_defaultWeight, ContainerType.Valuable),
                new Container(_defaultWeight, ContainerType.Valuable),
                new Container(_defaultWeight, ContainerType.Valuable),
                new Container(_defaultWeight, ContainerType.Valuable),
                new Container(_defaultWeight, ContainerType.Valuable),
                new Container(_defaultWeight, ContainerType.Valuable),
                new Container(_defaultWeight, ContainerType.Valuable),
                new Container(_defaultWeight, ContainerType.Valuable),
                new Container(_defaultWeight, ContainerType.Valuable),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Cool),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard),
                new Container(_defaultWeight, ContainerType.Standard)
            };

            return returnList;
        }
    }
}
