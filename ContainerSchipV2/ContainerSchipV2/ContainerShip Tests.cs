using ContainerSchipV2;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ContainerSchipV2.Enum;

namespace ContainerSchipTests
{
    class ContainerShip_Tests
    {
        int Length = 5;
        int Width = 5;
        int containerWeight = 5000;
        int randomNumber = 100;

        [Test]
        public void InstantiateShip()
        {
            //Arrange
            int testLength = 0;
            int testWidth = 0;

            //Act
            Ship bestShip = new Ship(Width, Length);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.Catch(() => { Ship ship = new Ship(testWidth, testLength); });

                Assert.AreEqual(Length, bestShip.Length);
                Assert.AreEqual(Width, bestShip.Width);
            });
        }

        [Test]
        public void CorrectNumberOfSlots()
        {
            //Arrange
            int expected = Width * Length;

            //Act
            Ship ship = new Ship(Width, Length);

            //Assert
            Assert.AreEqual(expected, ship.Layout.Count());
        }

        [Test]
        public void CoolContainerOnlyInFrontRow()
        {
            //Arrange
            Container coolContainer = new Container(containerWeight, ContainerType.Cool);
            Ship ship = new Ship(1, 2);
            List<Container> containers = new List<Container>()
            {
                new Container(containerWeight, ContainerType.Standard),
                new Container(containerWeight, ContainerType.Standard),
                new Container(containerWeight, ContainerType.Standard),
                new Container(containerWeight, ContainerType.Standard),
                new Container(containerWeight, ContainerType.Standard),
                coolContainer
            };

            //Act
            ship.LoadShip(containers);
            Place PlaceCoolContainer = ship.Layout.First(slot => slot.containers.Any(c => c.Type == ContainerType.Cool));

            //Assert
            Assert.IsTrue(PlaceCoolContainer.PlaceYPosition == 0);
        }

        [Test]
        public void ValuableContainerOnlyOnTop()
        {
            List<Container> containers = new List<Container>();
            for (int i = 0; i < randomNumber; i++)
            {
                containers.Add(new Container(containerWeight, ContainerType.Valuable));
            }

            for (int i = 0; i < randomNumber; i++)
            {
                containers.Add(new Container(containerWeight, ContainerType.Standard));
            }

            Ship ship = new Ship(Width, Length);
            ship.LoadShip(containers);

            Assert.Multiple(() =>
            {
                foreach (Place place in ship.Layout)
                {
                    if (place.containers.Any(c => c.Type == ContainerType.Valuable))
                    {
                        int indexOfValuable = place.containers.ToList()
                                                .IndexOf(place.containers.ToList()
                                                .Where(c => c.Type == ContainerType.Valuable)
                                                .FirstOrDefault());

                        int heigthOfSlot = place.containers.Count();

                        Assert.AreEqual(heigthOfSlot, indexOfValuable + 1);
                    }
                }
            });
        }

        [Test]
        public void PlaceFiveContainers()
        {
            int NumberOfContainers = 5;
            List<Container> Containers = new List<Container>();
            for (int i = 0; i < NumberOfContainers; i++)
            {
                Containers.Add(new Container(containerWeight, ContainerType.Standard));
            }

            Ship ship = new Ship(Width, Length);
            ship.LoadShip(Containers);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(NumberOfContainers, CountPlacedContainersByProperty(Containers));
                Assert.AreEqual(NumberOfContainers, CountAmountOfContainersInSlots(ship.Layout));
            });
        }

        [Test]
        public void TwoValuablesCanNotBePlacedOnTopOfEachOther()
        {
            int expectedNumberOfPlaced = 1;

            List<Container> valuables = new List<Container>()
            {
                new Container(containerWeight, ContainerType.Valuable),
                new Container(containerWeight, ContainerType.Valuable)
            };

            Ship ship = new Ship(1, 1);
            ship.LoadShip(valuables);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedNumberOfPlaced, CountAmountOfContainersInSlots(ship.Layout));
                Assert.AreEqual(expectedNumberOfPlaced, CountPlacedContainersByProperty(valuables));
            });
        }

        [Test]
        public void ThreeValuableCanNotBePlacedInFrontEachOther()
        {
            int numberOfValubles = 3;
            int expectedNumberOfPlacedValuables = 2;

            List<Container> valuables = new List<Container>();
            for (int i = 0; i < numberOfValubles; i++)
            {
                valuables.Add(new Container(containerWeight, ContainerType.Valuable));
            }

            Ship ship = new Ship(1, numberOfValubles);

            ship.LoadShip(valuables);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedNumberOfPlacedValuables, CountPlacedContainersByProperty(valuables));
                Assert.AreEqual(expectedNumberOfPlacedValuables, CountAmountOfContainersInSlots(ship.Layout));
            });
        }

        [Test]
        public void RunLoadFunctionTwoTimes()
        {
            List<Container> firstSeaContainers = new List<Container>()
            {
                new Container(containerWeight, ContainerType.Standard),
                new Container(containerWeight, ContainerType.Standard)
            };
            List<Container> secondSeaContainers = new List<Container>()
            {
                new Container(containerWeight, ContainerType.Standard),
                new Container(containerWeight, ContainerType.Standard)
            };

            Ship ship = new Ship(Width, Length);
            ship.LoadShip(firstSeaContainers);
            ship.LoadShip(secondSeaContainers);

            List<Container> combinedList = new List<Container>();
            combinedList.AddRange(firstSeaContainers);
            combinedList.AddRange(secondSeaContainers);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(combinedList.Count(), CountPlacedContainersByProperty(combinedList));
                Assert.AreEqual(combinedList.Count(), CountAmountOfContainersInSlots(ship.Layout));
            });
        }

        private int CountAmountOfContainersInSlots(IEnumerable<Place> places)
        {
            int numberOfPlacedByCountingContainersInSlots = 0;
            foreach (Place place in places)
            {
                foreach (Container cont in place.containers)
                {
                    numberOfPlacedByCountingContainersInSlots++;
                }
            }
            return numberOfPlacedByCountingContainersInSlots;
        }

        private int CountPlacedContainersByProperty(IEnumerable<Container> containers)
        {
            int returnValue = 0;
            foreach (Container cont in containers)
            {
                if (cont.Placed)
                {
                    returnValue++;
                }
            }
            return returnValue;
        }

    }
}
